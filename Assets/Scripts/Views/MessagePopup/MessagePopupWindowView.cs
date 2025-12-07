using System;
using Core.LocalizationManager;
using R3;
using TMPro;
using UnityEngine;
using VContainer;
using ViewInterfaces;

namespace Views.MessagePopup
{
    public class MessagePopupWindowView : BaseWindowView, IMessagePopupView
    {
        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private FadeAnimationView _fadeAnimation;

        private ILocalizationManager _localizationManager;

        [Inject]
        public void InjectDependencies(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _fadeAnimation.SetShown(false);
        }

        public void SetMessage(string messageLocalizationKey)
        {
            if (string.IsNullOrEmpty(messageLocalizationKey) == true)
            {
                return;
            }

            var messageText = _localizationManager.GetText(messageLocalizationKey);
            _messageText.text = messageText;
        }

        public void FadeIn(Action onComplete)
        {
            _fadeAnimation.FadeIn(onComplete);
        }

        public void FadeOut(Action onComplete)
        {
            _fadeAnimation.FadeOut(onComplete);
        }
    }
}