using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class GClass0:MapObject
    {
        public string texturePath;
        public string ramp;
        public Vector3 velocity;
        public float lifetimeFirst;
        public float lifetimeSecond;
        public float periodFirst;
        public float periodSecond;
        public float scaleFirst;
        public float scaleSecond;
        public float frameCount;
        public float frameRateFirst;
        public float frameRateSecond;
        public float stripCount;
    }
}