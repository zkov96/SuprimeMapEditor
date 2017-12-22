using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MyTearrain.BrushMasks;
using MyTearrain.HeightBrush;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace MyTearrain
{
    public class TerrainManager : MonoBehaviour
    {
//        Vector2 meshCount = new Vector2(size.x/maxChankSize, size.z/maxChankSize);
//        Vector2 meshSize = new Vector2(size.x/Mathf.Floor(meshCount.x),size.z/Mathf.Floor(meshCount.y));

        [SerializeField] private GameObject myTearrainGO;
        [SerializeField] private Material terrainBrushMaterial;
        [SerializeField] private Material terrainBrushMaterial_Instanced;
        [SerializeField] private Material terrainLightingMaterial;
        [SerializeField] private Material mainLayerMaterial;
        [SerializeField] private Material otherLayerMaterial;

        [SerializeField] private MyTerrain[,] terrains;
        [SerializeField] private List<GroundLayer> groundLayers;

        [SerializeField] private int selectedTerrain;

        public Rect terrainRect;
        public Vector2 terrainCount;

        public float this[int x, int y]
        {
            get { return terrains[x / (MyTerrain.maxChankSize - 1), y / (MyTerrain.maxChankSize - 1)][x % (MyTerrain.maxChankSize - 1), y % (MyTerrain.maxChankSize - 1)]; }
            set
            {
                int chunkX = x / (MyTerrain.maxChankSize - 1);
                int chunkY = y / (MyTerrain.maxChankSize - 1);

                int locX = x % (MyTerrain.maxChankSize - 1);
                int locY = y % (MyTerrain.maxChankSize - 1);

                if (chunkX>0 && locX==0)
                {
                    terrains[chunkX-1, chunkY][MyTerrain.maxChankSize - 1, locY] = value;
                }

                if (chunkY>0 && locY==0)
                {
                    terrains[chunkX, chunkY-1][locX, MyTerrain.maxChankSize - 1] = value;
                }

                if (chunkX>0 && locX == 0 && chunkY>0 && locY == 0)
                {
                    terrains[chunkX-1, chunkY-1][MyTerrain.maxChankSize - 1, MyTerrain.maxChankSize - 1] = value;
                }

                if (chunkX < terrainCount.x && chunkY < terrainCount.y)
                {
                    terrains[chunkX, chunkY][locX, locY] = value;
                }
            }
        }

        private void Start()
        {
        }

        public void Init(float[,] heights, Vector3 size)
        {
            if (terrains != null)
            {
                for (int k = 0; k < terrainCount.y; k++)
                {
                    for (int i = 0; i < terrainCount.x; i++)
                    {
                        Destroy(terrains[i, k].gameObject);
                    }
                }
            }
            groundLayers = new List<GroundLayer>();


            terrainRect = Rect.MinMaxRect(0, 0, size.x, size.z);
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.z; j++)
                {
                    heights[i, j] = heights[i, j] * size.y;
                }
            }

            selectedTerrain = -1;
            terrainCount = new Vector2(Mathf.Ceil(size.x / (MyTerrain.maxChankSize - 1)), Mathf.Ceil(size.z / (MyTerrain.maxChankSize - 1)));
            terrains = new MyTerrain[(int) terrainCount.x, (int) terrainCount.y];
            for (int k = 0; k < terrainCount.y; k++)
            {
                for (int i = 0; i < terrainCount.x; i++)
                {
                    terrains[i, k] = CreateTerrainChunk(heights, new Vector2(i, k));
                }
            }

            InitDefaultMaterial();
            groundLayers.Add(new GroundLayer(mainLayerMaterial, null));
            ApplyLayer(groundLayers[0]);
        }

        private MyTerrain CreateTerrainChunk(float[,] heights, float x, float y)
        {
            return CreateTerrainChunk(heights, new Vector2(x, y));
        }

        private MyTerrain CreateTerrainChunk(float[,] heights, Vector2 pos)
        {
            Rect rect = RectUtils.OverlapRect(new Rect(pos.x * (MyTerrain.maxChankSize - 1), pos.y * (MyTerrain.maxChankSize - 1), MyTerrain.maxChankSize, MyTerrain.maxChankSize), terrainRect);

            MyTerrain tmpMyTerrain = Instantiate(myTearrainGO).GetComponent<MyTerrain>();
            tmpMyTerrain.gameObject.transform.SetParent(transform, false);
            tmpMyTerrain.gameObject.transform.position += new Vector3(pos.x * MyTerrain.maxChankSize - pos.x, 0, pos.y * MyTerrain.maxChankSize - pos.y);
            tmpMyTerrain.Init(heights, rect);
            return tmpMyTerrain;
        }

        private void InitDefaultMaterial()
        {
            for (int j = 0; j < terrainCount.y; j++)
            {
                for (int i = 0; i < terrainCount.x; i++)
                {
                    terrains[i, j].materials = new[] {terrainBrushMaterial};
                }
            }

            terrainBrushMaterial_Instanced = terrains[0, 0].materials[0];
        }

        public void SetBrushPosTo(Vector2 pos)
        {
            terrainBrushMaterial_Instanced.SetVector("_Position", new Vector4(pos.x, pos.y, 0, 0));
        }

        public Vector3 WorldToTerrainPoint(Vector3 worldPos)
        {
            return worldPos - this.transform.position;
        }

        public GroundLayer AddLayer(Texture2D albedo, float albedoTiling = 1, Texture2D normal = null, float normalTiling = 1)
        {
            GroundLayer tmp = new GroundLayer(otherLayerMaterial, albedo, albedoTiling, normal, normalTiling);
            groundLayers.Add(tmp);
            ApplyLayer(tmp);
            return tmp;
        }

        public GroundLayer GetLayer(int id)
        {
            return groundLayers[id];
        }

        private void ApplyLayer(int id)
        {
            for (int j = 0; j < terrainCount.y; j++)
            {
                for (int i = 0; i < terrainCount.x; i++)
                {
                    terrains[i, j].materials.ToList().Add(groundLayers[id].groundMaterial);
                }
            }
            groundLayers[id].Apply();
        }

        private void ApplyLayer(GroundLayer groundLayer)
        {
            Material[] tmp = new Material[terrains[0, 0].materials.Length + 1];

            for (int i = 0; i < terrains[0, 0].materials.Length; i++)
            {
                tmp[i] = terrains[0, 0].materials[i];
            }
            tmp[terrains[0, 0].materials.Length] = groundLayer.groundMaterial;

            for (int j = 0; j < terrainCount.y; j++)
            {
                for (int i = 0; i < terrainCount.x; i++)
                {
                    terrains[i, j].materials = tmp;
                }
            }
            groundLayer.Apply();
        }

        public void ApplyHeightBrush(TerrainBrushParameters parameters, Type brush, BaseBrushSettings settings)
        {
            Rect newRect = Rect.MinMaxRect(
                parameters.rect.xMin > terrainRect.xMin ? Mathf.RoundToInt(parameters.rect.xMin) : Mathf.RoundToInt(terrainRect.xMin),
                parameters.rect.yMin > terrainRect.yMin ? Mathf.RoundToInt(parameters.rect.yMin) : Mathf.RoundToInt(terrainRect.yMin),
                parameters.rect.xMax < terrainRect.xMax ? Mathf.RoundToInt(parameters.rect.xMax) : Mathf.RoundToInt(terrainRect.xMax),
                parameters.rect.yMax < terrainRect.yMax ? Mathf.RoundToInt(parameters.rect.yMax) : Mathf.RoundToInt(terrainRect.yMax));

            brush.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(method => Attribute.IsDefined(method, typeof(StaticHeightFuncAttribute))).ToList()[0]
                .Invoke(null, new object[] {new BaseTerrainBrushParameters(this, newRect, parameters.mask), settings});

            ApplyHeights(newRect);
        }

        public void ApplyHeights(Rect chengedRect)
        {
            for (int j = 0; j < terrainCount.y; j++)
            {
                for (int i = 0; i < terrainCount.x; i++)
                {
                    if (terrains[i, j].rect.Overlaps(chengedRect))
                    {
                        terrains[i, j].Apply();
                    }
                }
            }
        }
    }

    public class TerrainBrushParameters
    {
        public Rect rect;
        public MethodInfo mask;

        public TerrainBrushParameters(Rect rect, MethodInfo mask)
        {
            this.rect = rect;
            this.mask = mask;
        }
    }
}