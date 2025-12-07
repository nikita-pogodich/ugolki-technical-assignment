using System;
using UnityEngine;

namespace Views
{
    public class WindowFadeAnimationView : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private FadeAnimationView _fadeAnimationView;

        public void Init()
        {
            _fadeAnimationView.SetShown(false);
        }

        public void SetShown(bool isShown)
        {
            if (isShown)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        public void FadeIn(Action onComplete = null)
        {
            _canvas.enabled = true;
            _fadeAnimationView.FadeIn(onComplete);
        }

        public void FadeOut(Action onComplete = null)
        {
            _fadeAnimationView.FadeOut(OnFadeOutComplete);
            return;

            void OnFadeOutComplete()
            {
                _canvas.enabled = false;
                onComplete?.Invoke();
            }
        }
    }
}