using System;
using System.Collections.Generic;
using System.Threading;
using Core.Logger;
using Core.MVP;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.WindowManager
{
    public class WindowManager : IWindowManager, IDisposable
    {
        private readonly IDualLogger _dualLogger;
        private readonly Dictionary<string, IWindowFactory> _windowFactoriesByViewName = new();
        private readonly Dictionary<string, List<IWindowPresenter>> _windowPresentersByViewName = new();
        private readonly Dictionary<IModel, IWindowPresenter> _windowPresentersByModel = new();

        private IObjectResolver _objectResolver = null;

        public bool IsInitialized { get; private set; } = false;

        [Inject]
        public WindowManager(IDualLogger dualLogger)
        {
            _dualLogger = dualLogger;
        }

        public void SetContainer(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => _objectResolver != null, cancellationToken: cancellation);
            RegisterWindowFactories();
            IsInitialized = true;
        }

        public void Dispose()
        {
            foreach (IWindowPresenter windowPresenter in _windowPresentersByModel.Values)
            {
                windowPresenter.Deinit();
            }

            _windowPresentersByModel.Clear();
            _windowPresentersByViewName.Clear();
            _windowFactoriesByViewName.Clear();
        }

        public async UniTask ShowWindowAsync<TView, TModel>(
            string viewName,
            WindowShowDelegate<TModel> beforeShow = null,
            WindowShowDelegate<TModel> alreadyShown = null)
            where TView : IView
            where TModel : IModel
        {
            if (_windowFactoriesByViewName.TryGetValue(viewName, out IWindowFactory windowFactory) == false)
            {
                _dualLogger.Mandatory.LogError($"Window factory with ViewName {viewName} not found");
                return;
            }

            if (_windowPresentersByViewName.TryGetValue(viewName, out List<IWindowPresenter> windowPresenters))
            {
                IWindowPresenter shownWindowPresenter = null;

                foreach (IWindowPresenter windowPresenter in windowPresenters)
                {
                    if (windowPresenter.IsShown)
                    {
                        shownWindowPresenter = windowPresenter;
                        break;
                    }
                }

                bool hasShownWindow = shownWindowPresenter != null;
                if (hasShownWindow)
                {
                    if (windowFactory.IsAllowMultipleInstances)
                    {
                        await AddNewWindow<TView, TModel>(windowFactory, windowPresenters, beforeShow);
                    }
                    else
                    {
                        var presenter = (IWindowPresenter<TView, TModel>) shownWindowPresenter;
                        alreadyShown?.Invoke(presenter.Model);
                    }
                }
                else if (windowPresenters.Count > 0)
                {
                    var presenter = (IWindowPresenter<TView, TModel>) windowPresenters[0];

                    ShowWindow(presenter, beforeShow);
                }
                else
                {
                    await AddNewWindow<TView, TModel>(windowFactory, windowPresenters, beforeShow);
                }
            }
            else
            {
                var windowPresenter = (IWindowPresenter<TView, TModel>) await CreateWindow(windowFactory);

                var presenters = new List<IWindowPresenter> {windowPresenter};
                _windowPresentersByViewName.Add(viewName, presenters);
                _windowPresentersByModel.Add(windowPresenter.Model, windowPresenter);
                ShowWindow(windowPresenter, beforeShow);
            }
        }

        public void HideWindow(IModel model)
        {
            if (_windowPresentersByModel.TryGetValue(model, out IWindowPresenter windowPresenter) == false)
            {
                _dualLogger.Mandatory.LogError(
                    $"Trying to hide a window by {nameof(model)} model that had never been shown before");

                return;
            }

            windowPresenter.SetShown(false);
        }

        private void RegisterWindowFactories()
        {
            var windowFactories = _objectResolver.Resolve<IEnumerable<IWindowFactory>>();

            foreach (IWindowFactory windowFactory in windowFactories)
            {
                _windowFactoriesByViewName.TryAdd(windowFactory.ViewName, windowFactory);
            }
        }

        private void ShowWindow<TView, TModel>(
            IWindowPresenter<TView, TModel> presenter,
            WindowShowDelegate<TModel> beforeShow = null)
            where TView : IView
            where TModel : IModel
        {
            beforeShow?.Invoke(presenter.Model);
            presenter.SetShown(true);
        }

        private async UniTask AddNewWindow<TView, TModel>(
            IWindowFactory windowFactory,
            List<IWindowPresenter> windowPresenters,
            WindowShowDelegate<TModel> beforeShow = null)
            where TView : IView
            where TModel : IModel
        {
            var windowPresenter = (IWindowPresenter<TView, TModel>) await CreateWindow(windowFactory);

            windowPresenters.Add(windowPresenter);
            _windowPresentersByModel.Add(windowPresenter.Model, windowPresenter);

            ShowWindow(windowPresenter, beforeShow);
        }

        private async UniTask<IWindowPresenter> CreateWindow(IWindowFactory windowFactory)
        {
            IWindowPresenter windowPresenter = await windowFactory.CreateAsync();

            return windowPresenter;
        }
    }
}