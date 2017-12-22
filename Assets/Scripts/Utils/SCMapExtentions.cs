using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapClasses;
using UnityEngine;

namespace Utils
{
    namespace SCMapExtentions
    {
        public static class LowLevelExtentions
        {
            public static void SkipBytes(this SCMap scMap, int length)
            {
                scMap.fs.Position += length;
            }
            
            public static int ReadInt(this SCMap scMap)
            {
                int length = sizeof(int);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return BitConverter.ToInt32(byteBuffer, 0);
            }

            public static long ReadLong(this SCMap scMap)
            {
                int length = sizeof(long);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return BitConverter.ToInt64(byteBuffer, 0);
            }

            public static short ReadShort(this SCMap scMap)
            {
                int length = sizeof(short);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return BitConverter.ToInt16(byteBuffer, 0);
            }

            public static uint ReadUInt(this SCMap scMap)
            {
                int length = sizeof(long);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return BitConverter.ToUInt32(byteBuffer, 0);
            }

            public static byte ReadByte(this SCMap scMap)
            {
                int length = sizeof(byte);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return byteBuffer[0];
            }

            public static float ReadFloat(this SCMap scMap)
            {
                int length = sizeof(float);
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return BitConverter.ToSingle(byteBuffer, 0);
            }

            public static string ReadString(this SCMap scMap, int length = -1, Encoding encoding = null)
            {
                if (encoding == null)
                {
                    encoding = Encoding.ASCII;
                }
                if (length == -1)
                {
                    List<byte> bytes = new List<byte>();
                    int tmp = scMap.fs.ReadByte();
                    while (tmp != 0)
                    {
                        bytes.Add((byte) tmp);
                        tmp = scMap.fs.ReadByte();
                    }
                    return encoding.GetString(bytes.ToArray());
                }
                byte[] byteBuffer = new byte[length * encoding.GetByteCount("a")];
                int countRead = scMap.fs.Read(byteBuffer, 0, byteBuffer.Length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }

                return encoding.GetString(byteBuffer);
            }

            public static byte[] ReadBytes(this SCMap scMap, int length)
            {
                byte[] byteBuffer = new byte[length];
                int countRead = scMap.fs.Read(byteBuffer, 0, byteBuffer.Length);
                if (countRead != length)
                {
                    Debug.LogWarning("Read bytes less lenght");
                }
                return byteBuffer;
            }

            public static float[] ReadFloats(this SCMap scMap, int length)
            {
                byte[] byteBuffer = new byte[length * sizeof(float)];
                int countRead = scMap.fs.Read(byteBuffer, 0, byteBuffer.Length);
                if (countRead != length * sizeof(float))
                {
                    Debug.LogWarning("Read bytes less lenght");
                }

                return Enumerable.Range(0, byteBuffer.Length / 4)
                    .Select(i => BitConverter.ToSingle(byteBuffer, i * 4))
                    .ToArray();
            }
            
            public static int[] ReadInts(this SCMap scMap, int length)
            {
                byte[] byteBuffer = new byte[length * sizeof(float)];
                int countRead = scMap.fs.Read(byteBuffer, 0, byteBuffer.Length);
                if (countRead != length * sizeof(float))
                {
                    Debug.LogWarning("Read bytes less lenght");
                }

                return Enumerable.Range(0, byteBuffer.Length / 4)
                    .Select(i => BitConverter.ToInt32(byteBuffer, i * 4))
                    .ToArray();
            }

            public static short[] ReadShorts(this SCMap scMap, int length)
            {
                byte[] byteBuffer = new byte[length * sizeof(short)];
                int countRead = scMap.fs.Read(byteBuffer, 0, byteBuffer.Length);
                if (countRead != length * sizeof(short))
                {
                    Debug.LogWarning("Read bytes less lenght");
                }

                short[] shorts = new short[length];
                for (int i = 0; i < byteBuffer.Length; i += sizeof(short))
                {
                    shorts[i / sizeof(short)] = (short) ((byteBuffer[i] << 8) + byteBuffer[i + 1]);
                }

                return Enumerable.Range(0, byteBuffer.Length / 2)
                    .Select(i => BitConverter.ToInt16(byteBuffer, i * 2))
                    .ToArray();
            }
        }

        public static class SimpleExtentions
        {
            public static Vector2 ReadVector2(this SCMap scMap)
            {
                return new Vector2(scMap.ReadFloat(), scMap.ReadFloat());
            }

            public static Vector2 ReadVector2i(this SCMap scMap)
            {
                return new Vector2(scMap.ReadInt(), scMap.ReadInt());
            }

            public static Vector3 ReadVector3(this SCMap scMap)
            {
                return new Vector3(scMap.ReadFloat(), scMap.ReadFloat(), scMap.ReadFloat());
            }

            public static Vector4 ReadVector4(this SCMap scMap)
            {
                return new Vector4(scMap.ReadFloat(), scMap.ReadFloat(), scMap.ReadFloat(), scMap.ReadFloat());
            }

            public static Color ReadColorRGB(this SCMap scMap)
            {
                Vector3 vec3 = scMap.ReadVector3() * 100 / 255;
                return new Color(vec3.x, vec3.y, vec3.z);
            }

            public static Color ReadColorRGBA(this SCMap scMap)
            {
                Vector4 vec4 = scMap.ReadVector4() * 100 / 255;
                return new Color(vec4.x, vec4.y, vec4.z, vec4.w);
            }
        }

        public static class DDSExtentions
        {
            public static DDSTexture ReadDDS(this SCMap scMap)
            {
                int length = scMap.ReadInt();
                byte[] ddsBytes = scMap.ReadBytes(length);
                DDSTexture ddsTexture = new DDSTexture(ddsBytes);
                return ddsTexture;
            }
        }
        
        public static class MapHeaderExtentions
        {
            public static MapHeader ReadMapHeader(this SCMap scMap)
            {
                MapHeader mapHeader = new MapHeader();
                mapHeader.containerName = scMap.ReadString(4);
                mapHeader.int0 = scMap.ReadInt();
                mapHeader.int1 = scMap.ReadInt();
                mapHeader.int2 = scMap.ReadInt();
                mapHeader.mapSize = scMap.ReadVector2();
                mapHeader.int3 = scMap.ReadInt();
                mapHeader.sh4 = scMap.ReadShort();
                mapHeader.preview = scMap.ReadDDS();
                mapHeader.version = scMap.ReadInt();
                return mapHeader;
            }
        }

        public static class TerrainExtentions
        {
            public static SCTerrain ReadSCTearrain(this SCMap scMap)
            {
                SCTerrain terrain = new SCTerrain();
                terrain.size = scMap.ReadVector2i();
                terrain.scaleFactor = scMap.ReadFloat();
                terrain.heights = scMap.ReadShorts(((int) terrain.size.x + 1) * ((int) terrain.size.y + 1));
                terrain.strTTerrain = scMap.ReadString();
                return terrain;
            }
            
            public static float[,] ReadTerrainData(this SCMap scMap, Vector2 size)
            {
                short[] heightMap = scMap.ReadShorts(((int) size.x + 1) * ((int) size.y + 1));
                float[,] heights = new float[(int) size.x + 1, (int) size.y + 1];
                for (int i = 0; i < size.x + 1; i++)
                {
                    for (int j = 0; j < size.y + 1; j++)
                    {
                        heights[j, i] = heightMap[i + (((int) size.y + 1) - j - 1) * ((int) size.x + 1)] / 65535f;
                    }
                }
                return heights;
            }
        }

        public static class LightingSettingsExtentions
        {
            public static LightingSettings ReadLightingSettings(this SCMap scMap)
            {
                LightingSettings lightingSettings = new LightingSettings();
                lightingSettings.bgTexturePath = scMap.ReadString();
                lightingSettings.skyCubeMapPath = scMap.ReadString();
                lightingSettings.envCubeMap = scMap.ReadString();
                lightingSettings.lightingMultiplier = scMap.ReadFloat();
                lightingSettings.sunDirection = scMap.ReadVector3();
                lightingSettings.sunAmbience = scMap.ReadColorRGB();
                lightingSettings.sunColor = scMap.ReadColorRGB();
                lightingSettings.shadowColor = scMap.ReadColorRGB();
                lightingSettings.specularColor = scMap.ReadColorRGBA();
                lightingSettings.bloom = scMap.ReadFloat();
                return lightingSettings;
            }
        }
        
        public static class FogSettingsExtentions
        {
            public static FogSettings ReadFogSettings(this SCMap scMap)
            {
                FogSettings fogSettings = new FogSettings();
                fogSettings.fogColor = scMap.ReadColorRGB();
                fogSettings.fogStart = scMap.ReadFloat();
                fogSettings.fogEnd = scMap.ReadFloat();
                return fogSettings;
            }
        }
        
        public static class WaterSettingsExtentions
        {
            public static WaterSettings ReadWaterSettings(this SCMap scMap)
            {
                WaterSettings waterSettings = new WaterSettings();
                waterSettings.hasWater = scMap.ReadByte() == 1;
                //ReadWater
                if (waterSettings.hasWater)
                {
                    waterSettings.waterElevationInv = scMap.ReadFloat();
                    waterSettings.waterElevationDeep = scMap.ReadFloat();
                    waterSettings.waterElevationAbyss = scMap.ReadFloat();
                }
                else
                {
                    //Skip & setDefault
                    scMap.ReadVector3();
                    waterSettings.waterElevationInv = 17.5f;
                    waterSettings.waterElevationDeep = 15f;
                    waterSettings.waterElevationAbyss = 2.5f;
                }
                return waterSettings;
            }
        }

        public static class WaveExtentions
        {
            public static Wave ReadWave(this SCMap scMap)
            {
                Wave wave = new Wave();
                wave.float_0 = scMap.ReadFloat();
                wave.float_1 = scMap.ReadFloat();
                wave.texturePath = scMap.ReadString();
                return wave;
            }
        }

        public static class GClass0Extentions
        {
            public static GClass0 ReadGClass0(this SCMap scMap)
            {
                GClass0 gClass0 = new GClass0();
                gClass0.texturePath = scMap.ReadString();
                gClass0.ramp = scMap.ReadString();
                gClass0.position = scMap.ReadVector3();
                float rotY = scMap.ReadFloat();
                gClass0.rotation = Quaternion.Euler(0, rotY, 0);
                gClass0.velocity = scMap.ReadVector3();
                gClass0.lifetimeFirst = scMap.ReadFloat();
                gClass0.lifetimeSecond = scMap.ReadFloat();
                gClass0.periodFirst = scMap.ReadFloat();
                gClass0.periodSecond = scMap.ReadFloat();
                gClass0.scaleFirst = scMap.ReadFloat();
                gClass0.scaleSecond = scMap.ReadFloat();
                gClass0.frameCount = scMap.ReadFloat();
                gClass0.frameRateFirst = scMap.ReadFloat();
                gClass0.frameRateSecond = scMap.ReadFloat();
                gClass0.stripCount = scMap.ReadFloat();
                return gClass0;
            }
        }

        public static class DecalExtentions
        {
            public static Layer ReadGClass69(this SCMap scMap)
            {
                Layer layer = new Layer();
                layer.albedoPath = scMap.ReadString();
                layer.normalPath = scMap.ReadString();
                layer.tiling_albedo = scMap.ReadFloat();
                layer.tiling_normal = scMap.ReadFloat();
                return layer;
            }
        }

        public static class GClass1Extentions
        {
            public static GClass1 ReadDecal(this SCMap scMap)
            {
                GClass1 gClass1 = new GClass1();
                gClass1.unk0 = scMap.ReadInt();
                gClass1.gEnum7 = scMap.ReadInt();

                int texturesCount = scMap.ReadInt();
                gClass1.texturePaths = new string[texturesCount];
                for (int i = 0; i < texturesCount; i++)
                {
                    int strLenght = scMap.ReadInt();
                    gClass1.texturePaths[i] = scMap.ReadString(strLenght);
                }

                gClass1.scale = scMap.ReadVector3();
                gClass1.position = scMap.ReadVector3();
                Vector3 rot = scMap.ReadVector3();
                gClass1.rotation = Quaternion.Euler(rot);
                gClass1.cutOffLOD = scMap.ReadFloat();
                gClass1.nearCutOffLOD = scMap.ReadFloat();
                gClass1.id = scMap.ReadInt();

                return gClass1;
            }
        }

        public static class GClass70Extentions
        {
            public static GClass70 ReadGClass70(this SCMap scMap)
            {
                GClass70 gClass70 = new GClass70();
                gClass70.int_0 = scMap.ReadInt();
                gClass70.group = scMap.ReadString();
                int lenght = scMap.ReadInt();
                gClass70.int_1 = scMap.ReadInts(lenght);
                return gClass70;
            }
        }

        public static class PropExtentions
        {
            public static Prop ReadProp(this SCMap scMap)
            {
                Prop prop = new Prop();
                prop.string_0 = scMap.ReadString();
                prop.position = scMap.ReadVector3();
                prop.vector3_2 = scMap.ReadVector3();
                prop.vector3_3 = scMap.ReadVector3();
                prop.rotation = Quaternion.identity;
                prop.unk0 = scMap.ReadVector3();
                return prop;
            }
        }
    }
}