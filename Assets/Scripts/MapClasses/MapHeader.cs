using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class MapHeader
    {
        public string containerName;
        public int int0;
        public int int1;
        public int int2;
        public Vector2 mapSize;
        public int int3;
        public short sh4;
        public DDSTexture preview;
        public int version;
    }
}