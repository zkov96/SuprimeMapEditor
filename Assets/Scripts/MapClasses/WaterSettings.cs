using System;

namespace MapClasses
{
    [Serializable]
    public class WaterSettings
    {
        public bool hasWater;
        public float waterElevationInv;
        public float waterElevationDeep;
        public float waterElevationAbyss;
    }
}