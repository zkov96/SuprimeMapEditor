using System;
using MyTearrain.BrushMasks;
using System.Reflection;
using UnityEngine;

namespace MyTearrain.HeightBrush
{
    public class BaseTerrainBrushParameters
    {
        public readonly TerrainManager terrainManager;
        public readonly Rect rect;
        public readonly MethodInfo mask;

        public BaseTerrainBrushParameters(TerrainManager terrainManager, Rect rect, MethodInfo mask)
        {
            this.terrainManager = terrainManager;
            this.rect = rect;
            this.mask = mask;
        }
    }
}