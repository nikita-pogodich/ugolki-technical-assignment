using Core.ModelProvider;
using Core.MVP;
using Core.PresenterProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using VContainer;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowFactory : IWindowFactory
    {
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IPresenterProvider _presenterProvider;
        private readonly IModelProvider _modelProvider;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.MainMenu;

        [Inject]
        public MainMenuWindowFactory(
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings,
            IPresenterProvider presenterProvider,
            IModelProvider modelProvider)
        {
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
            _presenterProvider = presenterProvider;
            _modelProvider = modelProvider;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = await _modelProvider.GetAsync<IMainMenuModel>();

            var mainMenuWindowView = await _windowViewProvider.GetAsync<IMainMenuWindowView>(ViewName, WindowType.Main);

            var mainMenuWindowPresenter = _presenterProvider.Get<
                IMainMenuWindowPresenter,
                IMainMenuWindowView,
                IMainMenuModel>(mainMenuWindowView, model);

            return mainMenuWindowPresenter;
        }
    }
}