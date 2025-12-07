using Core.MVP;
using Core.PresenterProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Features.UgolkiLogic;
using Settings;
using VContainer;
using ViewInterfaces;

namespace Features.GameHUDWindow
{
    public class GameHUDWindowFactory : IWindowFactory
    {
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IPresenterProvider _presenterProvider;
        private readonly IUgolkiModel _ugolkiModel;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.GameHUD;

        [Inject]
        public GameHUDWindowFactory(
            ILocalSettings localSettings,
            IWindowViewProvider windowViewProvider,
            IPresenterProvider presenterProvider,
            IUgolkiModel ugolkiModel)
        {
            _localSettings = localSettings;
            _windowViewProvider = windowViewProvider;
            _presenterProvider = presenterProvider;
            _ugolkiModel = ugolkiModel;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            IGameHUDWindowView ugolkiGameWindowView = await _windowViewProvider.GetAsync<IGameHUDWindowView>(
                ViewName,
                WindowType.Overlay);

            IGameHUDWindowPresenter ugolkiGameHUDViewPresenter = _presenterProvider.Get<
                IGameHUDWindowPresenter,
                IGameHUDWindowView,
                IUgolkiModel>(
                ugolkiGameWindowView,
                _ugolkiModel);

            return ugolkiGameHUDViewPresenter;
        }
    }
}