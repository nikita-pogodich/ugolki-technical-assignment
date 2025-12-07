using Core.ModelProvider;
using Core.MVP;
using Core.PresenterProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using VContainer;
using ViewInterfaces;

namespace Features.GameResultPopup
{
    public class GameResultPopupWindowFactory : IWindowFactory
    {
        private readonly ILocalSettings _localSettings;
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly IPresenterProvider _presenterProvider;
        private readonly IModelProvider _modelProvider;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.GameResultPopup;

        [Inject]
        public GameResultPopupWindowFactory(
            ILocalSettings localSettings,
            IWindowViewProvider windowViewProvider,
            IPresenterProvider presenterProvider,
            IModelProvider modelProvider)
        {
            _localSettings = localSettings;
            _windowViewProvider = windowViewProvider;
            _presenterProvider = presenterProvider;
            _modelProvider = modelProvider;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = await _modelProvider.GetAsync<IGameResultPopupModel>();

            var gameResultPopupWindowView = await _windowViewProvider.GetAsync<IGameResultPopupWindowView>(
                ViewName,
                WindowType.Popup);

            var gameResultPopupWindowPresenter = _presenterProvider.Get<
                IGameResultPopupWindowPresenter,
                IGameResultPopupWindowView,
                IGameResultPopupModel>(
                gameResultPopupWindowView,
                model);

            return gameResultPopupWindowPresenter;
        }
    }
}