using Core.MVP;
using Core.PresenterProvider;
using Core.ViewProvider;
using Core.WorldObjectManager;
using Cysharp.Threading.Tasks;
using Settings;
using VContainer;

namespace Features.UgolkiLogic
{
    public class UgolkiBoardFactory : IWorldObjectFactory
    {
        private readonly ILocalSettings _localSettings;
        private readonly IViewProvider _viewProvider;
        private readonly IPresenterProvider _presenterProvider;

        public string ViewName => _localSettings.ViewNames.UgolkiBoard;

        [Inject]
        public UgolkiBoardFactory(
            ILocalSettings localSettings,
            IViewProvider viewProvider,
            IPresenterProvider presenterProvider)
        {
            _localSettings = localSettings;
            _viewProvider = viewProvider;
            _presenterProvider = presenterProvider;
        }

        public async UniTask<IWorldObjectPresenter> CreateAsync<TPresenter, TView, TModel>(TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel
        {
            var ugolkiBoardView = await _viewProvider.GetAsync<TView>(ViewName);

            var ugolkiBoardPresenter = _presenterProvider.Get<TPresenter, TView, TModel>(
                ugolkiBoardView,
                model);

            return ugolkiBoardPresenter;
        }

        public void Release<TView, TModel>(IWorldObjectPresenter<TView, TModel> presenter)
            where TView : IWorldObjectView
            where TModel : IModel
        {
            TView worldObjectView = presenter.View;
            presenter.Deinit();
            _viewProvider.Release(worldObjectView.ViewName, worldObjectView);
        }
    }
}