using UnityEngine;

namespace Core.UI.Utils
{
    /// <summary>
    /// https://github.com/howtungtung/UnitySafeAreaHelper
    /// </summary>
    public class SafeAreaHelper : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        private Rect _lastSafeArea;

        private void Awake()
        {
            if (_lastSafeArea != Screen.safeArea)
            {
                _lastSafeArea = Screen.safeArea;
                Refresh();
            }
        }

        private void Refresh()
        {
            var anchorMin = _lastSafeArea.position;
            var anchorMax = _lastSafeArea.position + _lastSafeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }
    }
}