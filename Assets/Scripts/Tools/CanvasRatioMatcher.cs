using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    [ExecuteInEditMode]
    public class CanvasRatioMatcher : MonoBehaviour
    {
        [SerializeField]
        private CanvasScaler _canvasScaler;

        [SerializeField]
        private RectTransform _canvasRect;

        [SerializeField]
        private float _minAspectRatio = 0.5625f;

        public void SetMatchWidthOrHeight()
        {
            Rect rect = _canvasRect.rect;
            float aspectRatio = rect.width / rect.height;
            _canvasScaler.matchWidthOrHeight = aspectRatio > _minAspectRatio ? 1.0f : 0.0f;
        }

        private void Start()
        {
            SetMatchWidthOrHeight();
        }

#if UNITY_EDITOR
        private void Update()
        {
            SetMatchWidthOrHeight();
        }
#endif

        private void OnRectTransformDimensionsChange()
        {
            SetMatchWidthOrHeight();
        }
    }
}