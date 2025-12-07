using System;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class FadeAnimationView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _visible;

        [SerializeField]
        private float _fadeDuration = 0.2f;

        [SerializeField]
        private float _stayShownDuration;

        private Sequence _fadeAnimation;
        private bool _isFadingIn;
        private bool _isFadingOut;

        public void SetShown(bool isShown)
        {
            _visible.alpha = isShown ? 1.0f : 0.0f;
            _visible.interactable = isShown;
            _visible.blocksRaycasts = isShown;
        }

        public void FadeIn(Action onComplete = null)
        {
            if (_isFadingIn)
            {
                return;
            }

            _visible.interactable = true;
            _visible.blocksRaycasts = true;
            Fade(endAlpha: 1.0f, onCompleteDelay: _stayShownDuration, Ease.InQuad, onComplete);

            _isFadingIn = true;
        }

        public void FadeOut(Action onComplete = null)
        {
            if (_isFadingOut)
            {
                return;
            }

            _visible.interactable = false;
            _visible.blocksRaycasts = false;
            Fade(endAlpha: 0.0f, onCompleteDelay: 0.0f, Ease.OutQuad, onComplete);
        }

        private void Fade(float endAlpha, float onCompleteDelay, Ease ease, Action onComplete)
        {
            _fadeAnimation?.Kill();
            _fadeAnimation = DOTween.Sequence();

            Tween fadeTween = _visible.DOFade(endAlpha, _fadeDuration).SetEase(ease);
            _fadeAnimation
                .Append(fadeTween)
                .AppendInterval(onCompleteDelay);

            _fadeAnimation.OnComplete(OnFadeComplete);
            return;

            void OnFadeComplete()
            {
                _isFadingIn = false;
                _isFadingOut = false;
                onComplete?.Invoke();
            }
        }
    }
}