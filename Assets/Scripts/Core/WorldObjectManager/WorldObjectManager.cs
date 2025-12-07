using System;
using System.Collections.Generic;
using System.Threading;
using Core.Logger;
using Core.MVP;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.WorldObjectManager
{
    public class WorldObjectManager : IWorldObjectManager, IDisposable
    {
        private readonly IDualLogger _dualLogger;
        private readonly Dictionary<string, IWorldObjectFactory> _worldObjectFactoriesByViewName = new();
        private readonly Dictionary<IModel, IPresenter> _presentersByModel = new();

        private IObjectResolver _objectResolver = null;

        public bool IsInitialized { get; private set; } = false;

        [Inject]
        public WorldObjectManager(IDualLogger dualLogger)
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
            RegisterFactories();
            await UniTask.CompletedTask;
            IsInitialized = true;
        }

        public void Dispose()
        {
            foreach (IPresenter presenter in _presentersByModel.Values)
            {
                presenter.Deinit();
            }

            _worldObjectFactoriesByViewName.Clear();
            _presentersByModel.Clear();
        }

        public async UniTask CreateWorldObjectAsync<TPresenter, TView, TModel>(string viewName, TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel
        {
            if (_worldObjectFactoriesByViewName.TryGetValue(
                    viewName,
                    out IWorldObjectFactory worldObjectFactory) == false)
            {
                _dualLogger.Mandatory.LogError($"WorldObjectFactory by ViewName {viewName} not found");
                await UniTask.CompletedTask;
                return;
            }

            IWorldObjectPresenter presenter = await worldObjectFactory.CreateAsync<TPresenter, TView, TModel>(model);
            if (presenter is not IWorldObjectPresenter<TView, TModel> worldObjectPresenter)
            {
                _dualLogger.Mandatory.LogError(
                    $"Cannot cast {nameof(presenter)} into {nameof(IWorldObjectPresenter<TView, TModel>)}");

                await UniTask.CompletedTask;
                return;
            }

            _presentersByModel.Add(worldObjectPresenter.Model, worldObjectPresenter);
            worldObjectPresenter.SetShown(true);
        }

        public void ReleaseWorldObject<TView, TModel>(TModel model)
            where TView : IWorldObjectView
            where TModel : IModel
        {
            if (_presentersByModel.TryGetValue(model, out IPresenter presenter) == false)
            {
                _dualLogger.Mandatory.LogError($"World Object Presenter by model {nameof(model)} not found");
                return;
            }

            if (presenter is not IWorldObjectPresenter<TView, TModel> worldObjectPresenter)
            {
                return;
            }

            string viewName = worldObjectPresenter.View.ViewName;
            if (_worldObjectFactoriesByViewName.TryGetValue(
                    viewName,
                    out IWorldObjectFactory worldObjectFactory) == false)
            {
                _dualLogger.Mandatory.LogError($"WorldObjectFactory by ViewName {viewName} not found");
                return;
            }

            worldObjectPresenter.SetShown(false);
            worldObjectFactory.Release(worldObjectPresenter);
        }

        private void RegisterFactories()
        {
            var worldObjectFactories = _objectResolver.Resolve<IEnumerable<IWorldObjectFactory>>();

            foreach (IWorldObjectFactory worldObjectFactory in worldObjectFactories)
            {
                _worldObjectFactoriesByViewName.TryAdd(worldObjectFactory.ViewName, worldObjectFactory);
            }
        }
    }
}