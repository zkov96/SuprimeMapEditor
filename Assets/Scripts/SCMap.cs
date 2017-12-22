using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MapClasses;
using UnityEngine;
using Utils.SCMapExtentions;

[Serializable]
public class SCMap
{
    public FileInfo scMapFile;
    public FileStream fs;

    public MapHeader header;

    public SCTerrain terrain;

    public LightingSettings lightingSettings;

    public FogSettings fogSettings;

    public WaterSettings waterSettings;
    
    public float[] float_8;
    public string waterTexture;
    public string waterTextureRamp;
    public float[] float_9;

    public List<Wave> waves;
    public List<GClass0> gClass0;
    public bool hasGClass0;

    public string unk0;
    
    public List<Layer> layers;

    public int int_9;
    public int int_10;

    public List<GClass1> gClass1s;
    public List<GClass70> gClass70s;

    public int unk1;
    public int unk2;

    public int count3;
    public byte[] numArray3;

    public int unk3;

    public byte[] numArray2;

    public int unk4;
    public int toSkip0;
    public int toSkip1;

    public byte[] byte_0;

    public int unk5;

    public List<Prop> props;


    private Dictionary<Type, Action<byte[], Type>> typeConstructor;

    public SCMap()
    {
        waves = new List<Wave>();
        gClass0 = new List<GClass0>();
        layers = new List<Layer>();
        gClass1s = new List<GClass1>();
        gClass70s = new List<GClass70>();
        props = new List<Prop>();
    }

    public bool OpenSCMap(string filePath)
    {
        scMapFile = new FileInfo(filePath.Replace("\\", "/"));
        return scMapFile.Exists;
    }

    public bool ParseSCMapFile(string filePath = "")
    {
        if (filePath != "")
        {
            OpenSCMap(filePath);
        }

        fs = scMapFile.Open(FileMode.Open, FileAccess.Read);

        header = this.ReadMapHeader();

        terrain = this.ReadSCTearrain();
        
        lightingSettings = this.ReadLightingSettings();

        fogSettings = this.ReadFogSettings();

        waterSettings = this.ReadWaterSettings();
        
        float_8 = this.ReadFloats(20);
        waterTexture = this.ReadString();
        waterTextureRamp = this.ReadString();
        float_9 = this.ReadFloats(4);

        //Read Waves
        for (int i = 0; i < 4; i++)
        {
            Wave wave = this.ReadWave();
            waves.Add(wave);
        }

        //Read GClass0
        int gClass0Count = this.ReadInt();
        for (int i = 0; i < gClass0Count; i++)
        {
            GClass0 tmpGClass0 = this.ReadGClass0();
            gClass0.Add(tmpGClass0);
        }
        hasGClass0 = !waterSettings.hasWater || gClass0Count > 0;


        unk0 = this.ReadString();

        //Read GClass69
        int gClass69Count = this.ReadInt();
        for (int i = 0; i < gClass69Count; i++)
        {
            Layer tmpLayer = this.ReadGClass69();
            layers.Add(tmpLayer);
        }


        int_9 = this.ReadInt();
        int_10 = this.ReadInt();


        //Read gClass1
        int gClass1Count = this.ReadInt();
        for (int i = 0; i < gClass1Count; i++)
        {
            GClass1 gClass1 = this.ReadDecal();
            gClass1s.Add(gClass1);
        }

        //Read GClass70

        int gClass70Count = this.ReadInt();
        for (int i = 0; i < gClass70Count; i++)
        {
            GClass70 gClass70 = this.ReadGClass70();
            gClass70s.Add(gClass70);
        }

        unk1 = this.ReadInt();
        unk2 = this.ReadInt();

        //Read UnknowArray

        count3 = this.ReadInt();
        int lenght3 = this.ReadInt();
        numArray3 = this.ReadBytes(lenght3);// texture terrain colors?
        for (int i = 1; i < count3; i++)
        {
            lenght3 = this.ReadInt();
            this.SkipBytes(lenght3);
        }

        unk3 = this.ReadInt();

        int lenght2 = this.ReadInt();
        numArray2 = this.ReadBytes(lenght2);// texture?

        unk4 = this.ReadInt();

        toSkip0 = this.ReadInt();
        this.SkipBytes(toSkip0);

        toSkip1 = Mathf.RoundToInt(terrain.size.x / 2 * terrain.size.y / 2);
        this.SkipBytes(toSkip1);
        this.SkipBytes(toSkip1);
        this.SkipBytes(toSkip1);

        byte_0 = this.ReadBytes((int) (terrain.size.x * terrain.size.y));

        if (header.version <= 52)
        {
            unk5 = this.ReadShort();
        }
        
        //Read Prop

        int propCount = this.ReadInt();
        for (int i = 0; i < propCount; i++)
        {
            Prop prop = this.ReadProp();
            props.Add(prop);
        }
        
        fs.Close();

        return true;
    }


    public enum Endian
    {
        Big,
        Little
    }
}