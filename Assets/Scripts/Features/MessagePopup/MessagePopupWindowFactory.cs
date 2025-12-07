using Core.ModelProvider;
using Core.MVP;
using Core.PresenterProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using VContainer;
using ViewInterfaces;

namespace Features.MessagePopup
{
    public class MessagePopupWindowFactory : IWindowFactory
    {
        private readonly ILocalSettings _localSettings;
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly IPresenterProvider _presenterProvider;
        private readonly IModelProvider _modelProvider;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.MessagePopup;

        [Inject]
        public MessagePopupWindowFactory(
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
            var model = await _modelProvider.GetAsync<IMessagePopupModel>();

            var messagePopupView = await _windowViewProvider.GetAsync<IMessagePopupView>(ViewName, WindowType.Popup);

            var messagePopupViewPresenter = _presenterProvider.Get<
                IMessagePopupWindowPresenter,
                IMessagePopupView,
                IMessagePopupModel>(messagePopupView, model);

            return messagePopupViewPresenter;
        }
    }
}