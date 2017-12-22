using UnityEngine;

namespace Utils
{
    public static class RectUtils
    {
        public static Rect OverlapRect(Rect rect1, Rect rect2)
        {

            if (!rect1.Overlaps(rect2))
            {
                return Rect.zero;
            }
            
            return Rect.MinMaxRect(
                Mathf.Max(rect1.xMin, rect2.xMin),
                Mathf.Max(rect1.yMin, rect2.yMin),
                Mathf.Min(rect1.xMax, rect2.xMax),
                Mathf.Min(rect1.yMax, rect2.yMax)
                );
        }
    }
}