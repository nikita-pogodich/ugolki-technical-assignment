using System;
using R3;
using UnityEngine;
using ViewInterfaces;

namespace Views.LoadingScreen
{
    public class LoadingScreenView : BaseWindowView, ILoadingScreenView
    {
        [SerializeField]
        private FadeAnimationView _fadeAnimationView;

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _fadeAnimationView.SetShown(false);
        }

        public void FadeIn(Action onComplete = null)
        {
            SetCanvasEnabled(true);
            _fadeAnimationView.FadeIn(onComplete);
        }

        public void FadeOut(Action onComplete = null)
        {
            _fadeAnimationView.FadeOut(OnFadeOutComplete);
            return;

            void OnFadeOutComplete()
            {
                SetCanvasEnabled(false);
                onComplete?.Invoke();
            }
        }
    }
}