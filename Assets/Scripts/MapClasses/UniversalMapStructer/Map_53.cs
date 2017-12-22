using System.Collections.Generic;

namespace MapClasses.UniversalMapStructer
{
    [Map(53)]
    public class Map_53
    {
        public MapHeader header;
        public SCTerrain terrain;
        public LightingSettings lightingSettings;
        public FogSettings fogSettings;
        public WaterSettings waterSettings;
        
        [SizeField(20)]
        public float[] float_8;
        
        public string waterTexture;
        public string waterTextureRamp;
        
        [SizeField(4)]
        public float[] float_9;
        
        [SizeField(4)]
        public Wave[] waves;

        public int gClass0Count;
        [SizeField("gClass0Count")]
        public GClass0[] gClass0;
        
//        public bool hasGClass0;

        public string unk0;
    
        [SizeField(4)]
        public Layer[] layers;

        public int int_9;
        public int int_10;

        public int gClass1Count;
        [SizeField("gClass1Count")]
        public GClass1[] gClass1s;

        public int gClass70Count;
        [SizeField("gClass70Count")]
        public GClass70[] gClass70s;

        public int unk1;
        public int unk2;

        public int count3;

        public int lenght3;
        [SizeField("lenght3")]
        public byte[] numArray3;

        public int[] skip;

        public int unk3;

        private int lenght2;
        [SizeField("lenght2")]
        public byte[] numArray2;

        public int unk4;
        public int toSkip0;
        public int toSkip1;

        [SizeField(4)]
        public byte[] byte_0;

        public int unk5;

        [SizeField(4)]
        public Prop[] props;
        
    }
}