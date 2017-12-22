using System;
using JetBrains.Annotations;
using UnityEngine;

namespace MyTearrain
{
    [Serializable]
    public class GroundLayer
    {
        public Material groundMaterial;
        public Texture2D groundTexture_albedo;
        public Texture2D groundTexture_normal;
        public float albedoTiling;
        public float normalTiling;

        public GroundLayer([NotNull] Material groundMaterial, Texture2D groundTexture_albedo, float albedoTiling=1, Texture2D groundTexture_normal = null, float normalTiling=1)
        {
            Init(groundMaterial, groundTexture_albedo);
        }

        public void Init([NotNull] Material groundMaterial, Texture2D groundTexture_albedo, float albedoTiling=1, Texture2D groundTexture_normal = null, float normalTiling=1)
        {
            this.groundTexture_albedo = Texture2D.blackTexture;

            this.groundTexture_normal = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
            this.groundTexture_normal.SetPixel(0, 0, Color.blue);
            this.groundTexture_normal.filterMode = FilterMode.Point;
            this.groundTexture_normal.anisoLevel = 0;

            this.albedoTiling = albedoTiling;
            this.normalTiling = normalTiling;

            this.groundMaterial = Material.Instantiate(groundMaterial);

            if (groundTexture_albedo != null)
            {
                this.groundTexture_albedo = groundTexture_albedo;
            }

            if (groundTexture_normal != null)
            {
                this.groundTexture_normal = groundTexture_normal;
            }

            groundMaterial.SetTexture("_Albedo", this.groundTexture_albedo);
            groundMaterial.SetTextureScale("_Albedo", new Vector2(this.albedoTiling, this.albedoTiling));
            groundMaterial.SetTexture("_Normal", this.groundTexture_normal);
            groundMaterial.SetTextureScale("_Normal", new Vector2(this.normalTiling, this.normalTiling));

        }

        public void SetTexture([NotNull] Texture2D groundTexture_albedo, Texture2D groundTexture_normal = null)
        {
            this.groundTexture_albedo = groundTexture_albedo;

            if (groundTexture_normal != null)
            {
                this.groundTexture_normal = groundTexture_normal;
            }

            groundMaterial.SetTexture("_Albedo", this.groundTexture_albedo);
            groundMaterial.SetTexture("_Normal", this.groundTexture_normal);
        }

        public void SetTiling(float albedoTiling, float normalTiling)
        {
            this.albedoTiling = albedoTiling;
            this.normalTiling = normalTiling;
            groundMaterial.SetTextureScale("_Albedo", new Vector2(this.albedoTiling, this.albedoTiling));
            groundMaterial.SetTextureScale("_Normal", new Vector2(this.normalTiling, this.normalTiling));
        }

        public void Apply()
        {
            groundTexture_albedo.Apply();
        }
    }
}