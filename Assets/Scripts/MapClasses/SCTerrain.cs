using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class SCTerrain
    {
        public Vector2 size;
        public float scaleFactor;
        public short[] heights;
        public string strTTerrain;
    }
}