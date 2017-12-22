using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class LightingSettings
    {
        public string bgTexturePath;
        public string skyCubeMapPath;
        public string envCubeMap;
        public float lightingMultiplier;
        public Vector3 sunDirection;
        public Color sunAmbience;
        public Color sunColor;
        public Color shadowColor;
        public Color specularColor;
        public float bloom;
    }
}