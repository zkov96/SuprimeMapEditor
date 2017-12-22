using System;
using System.Linq;
using System.Reflection;
using DDS;
using MyTearrain.BrushMasks;

namespace MyTearrain.HeightBrush
{
    [HeightBrushAttribute(HeightBrushAttribute.HeightBrushType.Static, typeof(TestBrushSettings))]
    public class TestHeightBrush : BaseHeightBrush
    {
        [StaticHeightFunc]
        private static float[,] ApplyBrush(BaseTerrainBrushParameters parameters, TestBrushSettings settings)
        {
            float[,] outputHeights = new float[(int)parameters.rect.width, (int)parameters.rect.height];
            
            for (int i = (int)parameters.rect.xMin; i < parameters.rect.xMax; i++)
            {
                for (int j = (int)parameters.rect.yMin; j < parameters.rect.yMax; j++)
                {
                    parameters.terrainManager[i, j] =
                        parameters.terrainManager[i, j] +
                        (float)parameters.mask.Invoke(
                            null,
                            new object[]
                            {
                                (float)(i-parameters.rect.xMin)/parameters.rect.width,
                                (float)(j-parameters.rect.yMin)/parameters.rect.height
                            });
                }
            }
            return outputHeights;
            
        }
    }

    public class TestBrushSettings : BaseBrushSettings
    {
    }
}