using System;
using Core.MVP;

namespace ViewInterfaces
{
    public interface ILoadingScreenView : IWindowView
    {
        void FadeIn(Action onComplete = null);
        void FadeOut(Action onComplete = null);
    }
}