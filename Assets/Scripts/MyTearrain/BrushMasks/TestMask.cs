using UnityEngine;
using UnityEngineInternal;

namespace MyTearrain.BrushMasks
{
    [MaskAttribute]
    public class TestMask : BaseMask
    {
        private static float max = (Vector2.one * 0.5f).magnitude;
        private static float min = 0f;

        [StaticMaskFunkAttribute]
        public static float GetValue(float i, float j)
        {
            Vector2 tmp = new Vector2(i, j) - Vector2.one * 0.5f;
            Vector2 posRA = new Vector2(tmp.magnitude, Mathf.Atan2(tmp.y, tmp.x));
            float output = 1-Mathf.Pow(Mathf.Clamp(posRA.x, 0, 0.5f) * 2,2);

            return output;
        }
    }
}