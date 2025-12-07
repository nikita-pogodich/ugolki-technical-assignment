using System;
using Core.MVP;

namespace ViewInterfaces
{
    public interface IMessagePopupView : IWindowView
    {
        void SetMessage(string messageLocalizationKey);
        void FadeIn(Action onComplete);
        void FadeOut(Action onComplete);
    }
}