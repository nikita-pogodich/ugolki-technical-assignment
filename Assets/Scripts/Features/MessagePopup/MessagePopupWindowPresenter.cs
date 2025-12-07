using R3;
using ViewInterfaces;

namespace Features.MessagePopup
{
    public class MessagePopupWindowPresenter :
        BaseWindowPresenter<IMessagePopupView, IMessagePopupModel>,
        IMessagePopupWindowPresenter
    {
        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            Model.MessageLocalizationKey.Subscribe(OnMessageLocalizationKeyChanged).AddTo(ref disposableBuilder);
            OnMessageLocalizationKeyChanged(Model.MessageLocalizationKey.CurrentValue);
        }

        protected override void OnShow()
        {
            View.SetMessage(Model.MessageLocalizationKey.Value);
            View.FadeIn(OnFadeInComplete);
        }

        private void OnMessageLocalizationKeyChanged(string messageLocalizationKey)
        {
            if (IsShown == false)
            {
                return;
            }

            View.SetMessage(messageLocalizationKey);
            View.FadeIn(OnFadeInComplete);
        }

        private void OnFadeInComplete()
        {
            View.FadeOut(OnFadeOutComplete);
        }

        private void OnFadeOutComplete()
        {
            SetShown(false);
        }
    }
}