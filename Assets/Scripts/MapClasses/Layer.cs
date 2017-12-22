
using System;
using System.IO;
using System.Text;
using UnityEngine;
using Utils.SCMapExtentions;

namespace MapClasses
{
    [Serializable]
    public class Layer
    {
        public const string GAMEPATH = "F:/Unity/SuprimeMapEditor/Assets/GameData";
        public DDSTexture albedo;
        public DDSTexture normal;
        
        public string albedoPath;
        public string normalPath;
        public float tiling_albedo;
        public float tiling_normal;

        public void Prepare()
        {
            if (File.Exists(GAMEPATH + albedoPath))
            {
                byte[] tmp = File.ReadAllBytes(GAMEPATH + albedoPath);
                albedo = new DDSTexture(tmp);
                albedo.ToTexture2D();
            }
            if (File.Exists(GAMEPATH + normalPath))
            {
                byte[] tmp = File.ReadAllBytes(GAMEPATH + normalPath);
                normal = new DDSTexture(tmp);
                normal.ToTexture2D();
            }
//            Debug.Log("");
        }
    }
}