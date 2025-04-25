using UnityEngine;

namespace Core.UI.Utils
{
    public static class ScrollViewVisibility
    {
        private static readonly Vector3[] _corners = new Vector3[4];

        public static bool IsElementVisible(RectTransform element, RectTransform viewport)
        {
            element.GetWorldCorners(_corners);
            Rect elementRect = GetWorldRect(_corners);

            viewport.GetWorldCorners(_corners);
            Rect viewportRect = GetWorldRect(_corners);

            return elementRect.Overlaps(viewportRect);
        }

        private static Rect GetWorldRect(Vector3[] corners)
        {
            Vector2 min = corners[0];
            Vector2 max = corners[2];
            return new Rect(min, max - min);
        }
    }
}