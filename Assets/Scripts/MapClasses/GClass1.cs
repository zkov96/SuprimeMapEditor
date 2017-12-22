using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class GClass1:MapObject
    {
        public int unk0;
        public GEnum7 genum7_0;
        public string[] texturePaths;
        public Vector3 scale;
        public float cutOffLOD;
        public float nearCutOffLOD;

        public int gEnum7;
        
        public GClass1()
        {
            this.texturePaths = new string[2];
        }
    }

    public enum GEnum7
    {
        
    }
}