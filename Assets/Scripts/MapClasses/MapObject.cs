using System;
using UnityEngine;

namespace MapClasses
{
    [Serializable]
    public class MapObject
    {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
    }
}