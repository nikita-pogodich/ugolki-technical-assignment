using System.Threading;
using Core.Logger;
using Core.MVP;
using Core.ResourcesManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Views;

namespace Core.ViewProvider
{
    public class ViewProvider : IViewProvider
    {
        private readonly IDualLogger _dualLogger;
        private readonly IResourcesManager _resourcesManager;
        private IObjectResolver _objectResolver = null;

        public bool IsInitialized { get; private set; } = false;

        [Inject]
        public ViewProvider(IDualLogger dualLogger, IResourcesManager resourcesManager)
        {
            _dualLogger = dualLogger;
            _resourcesManager = resourcesManager;
        }

        public void SetContainer(IObjectResolver viewObjectResolver)
        {
            _objectResolver = viewObjectResolver;
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => _objectResolver != null, cancellationToken: cancellation);

            IsInitialized = true;
        }

        public TView Get<TView>(string resourceKey) where TView : IView
        {
            GameObject resource = _resourcesManager.Instantiate(resourceKey);
            TView view = resource.GetComponent<TView>();

            _objectResolver.Inject(view);
            view.Init(resourceKey);

            return view;
        }

        public async UniTask<TView> GetAsync<TView>(string resourceKey) where TView : IView
        {
            GameObject resource = await _resourcesManager.InstantiateAsync(resourceKey);
            TView view = resource.GetComponent<TView>();

            _objectResolver.Inject(view);
            view.Init(resourceKey);

            return view;
        }

        public void Release<TView>(string resourceKey, TView view) where TView : IView
        {
            if (view == null)
            {
                _dualLogger.Mandatory.LogError("Trying to release null view");
                return;
            }

            if (view is not BaseView baseView)
            {
                _dualLogger.Mandatory.LogError(
                    $"Trying to release {view.GetType()}. {nameof(ViewProvider)} can release only {nameof(BaseView)}");
                return;
            }

            if (baseView == null)
            {
                return;
            }

            _resourcesManager.ReleaseGameObject(resourceKey, baseView.gameObject);
        }
    }
}