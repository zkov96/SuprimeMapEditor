using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MapClasses;
using MyTearrain;
using MyTearrain.BrushMasks;
using MyTearrain.HeightBrush;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Starter : MonoBehaviour
{
    [SerializeField] private string filePath = "";
    [SerializeField] private bool parse;
    [SerializeField] private RawImage dds1Image;
    [SerializeField] private Terrain mapTerrain;

    [SerializeField] private Texture2D texture;
    [SerializeField] private TerrainManager terrainManager;


    [SerializeField] private SCMap scMap;

    [SerializeField] private int r = 50;


    private void Start()
    {
        scMap = new SCMap();
        TerrainRaycaster.instance.onPointerDownAction += OnPointerDown;
        TerrainRaycaster.instance.onPointerMoveAction += OnPointerMove;
    }

    private void OnDestroy()
    {
        TerrainRaycaster.instance.onPointerDownAction -= OnPointerDown;
    }

    private void Update()
    {
        if (parse)
        {
            parse = false;
            scMap.ParseSCMapFile(filePath);
            texture = (Texture2D) scMap.header.preview;
            texture.anisoLevel = 0;
            texture.filterMode = FilterMode.Point;

            float[,] heights = new float[(int) scMap.terrain.size.x + 1, (int) scMap.terrain.size.y + 1];
            for (int i = 0; i < scMap.terrain.size.x + 1; i++)
            {
                for (int j = 0; j < scMap.terrain.size.y + 1; j++)
                {
//                    Debug.LogFormat("i={0},j={1}:{2}", i, j, i + ((scMap.heightMapSizeY+1) - j - 1) * (scMap.heightMapSizeX+1));
                    heights[j, i] = scMap.terrain.heights[i + (((int) scMap.terrain.size.y + 1) - j - 1) * ((int) scMap.terrain.size.x + 1)] / 65535f;
//                    heights[j, i] = scMap.heightMap[i + j * ((int)scMap.terrainSize.x + 1)] / 65535f;
                }
            }
//            mapTerrain.terrainData.size = new Vector3(((int)scMap.terrainSize.x + 1), 513, ((int)scMap.terrainSize.y + 1));
//            mapTerrain.terrainData.SetHeights(0, 0, heights);
//            mapTerrain.ApplyDelayedHeightmapModification();

//            terrainManager.Init(heights, new Vector2(0, 0), new Vector2(254, 254), 513);
            terrainManager.Init(heights, new Vector3(scMap.terrain.size.x, 513, scMap.terrain.size.y));

            scMap.layers[0].Prepare();
            terrainManager.GetLayer(0).SetTexture((Texture2D) scMap.layers[0].albedo, (Texture2D) scMap.layers[0].normal);
            terrainManager.GetLayer(0).SetTiling(1/scMap.layers[0].tiling_albedo, 1/scMap.layers[0].tiling_normal);
            terrainManager.GetLayer(0).Apply();
            
//            for (int i = 1; i < scMap.layers.Count; i++)
//            {
//                try
//                {
//                    scMap.layers[i].Prepare();
//                    Layer layer = scMap.layers[i];
//                    terrainManager.AddLayer((Texture2D) layer.albedo,1/layer.tiling_albedo, (Texture2D) layer.normal, 1/layer.tiling_normal);
//                }
//                catch (Exception e)
//                {
//                    
//                }
//            }

            texture = (Texture2D) scMap.layers[0].normal;
            
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath+"/../test.png",bytes);

            
        }
    }

    private void OnPointerDown(Vector3 pos)
    {
        Vector2 pos2 = new Vector2(pos.x,pos.z);
        
        if (pos.x < 0)
        {
            return;
        }
        
        if (terrainManager)
        {
            MethodInfo maskMethod = typeof(TestMask).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(item => Attribute.IsDefined(item, typeof(StaticMaskFunkAttribute))).ToList()[0];
            TerrainBrushParameters parameters = new TerrainBrushParameters(Rect.MinMaxRect(
                (pos2.x-r)>0?pos2.x-r:terrainManager.terrainRect.xMin,
                (pos2.y-r)>0?pos2.y-r:terrainManager.terrainRect.yMin,
                (pos2.x+r)>0?pos2.x+r:terrainManager.terrainRect.xMax,
                (pos2.y+r)>0?pos2.y+r:terrainManager.terrainRect.yMax), maskMethod);
            
            terrainManager.ApplyHeightBrush(parameters, typeof(TestHeightBrush), null);
        }
    }

    private void OnPointerMove(Vector3 pos1, Vector3 pos2)
    {
        OnPointerDown(pos2);
    }
}