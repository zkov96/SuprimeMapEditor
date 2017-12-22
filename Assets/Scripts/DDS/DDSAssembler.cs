using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DDS
{
    public class DDSAssembler
    {
        delegate Color32[][] DecompressorDelegate(byte[] blockData);

        private static Dictionary<string, Type> assemblers;

        static DDSAssembler()
        {
            assemblers = new Dictionary<string, Type>();
            Assembly assembly = Assembly.GetAssembly(typeof(DDSAssembler));
            Type[] types = assembly.GetTypes().Where(item =>
                    Attribute.GetCustomAttribute(item, typeof(DDSAssemblerAttribute)) != null
//                item.GetCustomAttribute(typeof(DDSAssemblerAttribute))
            ).ToArray();
            foreach (Type type in types)
            {
                DDSAssemblerAttribute attr = (DDSAssemblerAttribute) Attribute.GetCustomAttribute(type, typeof(DDSAssemblerAttribute));
                assemblers.Add(attr.type, type);
            }
        }

        public static byte[] Compressor()
        {
            throw new Exception(String.Format("TODO"));
        }

        public static Color32[] Decompressor(DDS_HEADER ddsHeader, byte[] ddsData)
        {
            Color32[] colors = new Color32[ddsHeader.dwWidth * ddsHeader.dwHeight];

            string dwFourCC = ddsHeader.ddspf.dwFourCC;
            if (!ddsHeader.ddspf.dwFlags.ToList().Contains(DDS_PIXELFORMAT.DWFlags.DDPF_FOURCC))
            {
                dwFourCC = "";
            }

            if (!assemblers.ContainsKey(dwFourCC))
            {
                throw new Exception(String.Format("Image has unsupported compression: `{0}`", ddsHeader.ddspf.dwFourCC));
            }

            DDSAssemblerAttribute ddsAssemblerAttribute = (Attribute.GetCustomAttribute(assemblers[dwFourCC], typeof(DDSAssemblerAttribute)) as DDSAssemblerAttribute);
            MethodInfo[] methodInfos = assemblers[dwFourCC].GetMethods(BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public);

            if (methodInfos.Length==0)
            {
                throw new Exception(String.Format("Class have not static methods {0} for {1} assembler. Fix it!", assemblers[dwFourCC].FullName, dwFourCC!=""?dwFourCC:"UNCOMPRESS"));
            }

            MethodInfo methodInfo=null;
            foreach (var method in methodInfos)
            {
                Attribute attr = Attribute.GetCustomAttribute(method, typeof(DDSDecompressorAttribute));
                if (attr != null)
                {
                    methodInfo = method;
                    break;
                }
            }

            if (methodInfo==null)
            {
                throw new Exception(String.Format("Class have not static decompress method {0} for {1} assembler. Fix it!", assemblers[dwFourCC].FullName, dwFourCC!=""?dwFourCC:"UNCOMPRESS"));
            }

            int blockWidth = ddsAssemblerAttribute.width;
            int blockHeight = ddsAssemblerAttribute.height;
            int blockDataLenght = ddsAssemblerAttribute.dataLength;

            int textureWidth = ddsHeader.dwWidth / blockWidth;
            int textureHeight = ddsHeader.dwHeight / blockHeight;
            int textureSize = textureWidth * textureHeight;

            byte[] dxtBytes = new byte[textureSize * blockDataLenght];
            Buffer.BlockCopy(ddsData, 0, dxtBytes, 0, dxtBytes.Length);

            for (int block_index = 0; block_index < textureSize; block_index++)
            {
                int block_x = block_index % textureWidth;
                int block_y = block_index / textureWidth;

                byte[] blockData = new byte[blockDataLenght];
                Array.Copy(dxtBytes, block_index * blockDataLenght, blockData, 0, blockDataLenght);

                Color32[][] blockColors = methodInfo.Invoke(null, new object[] {blockData}) as Color32[][];
//                Color32[][] blockColors = DXT1Assembler.Decompressor(blockData);

                for (int i = 0; i < blockWidth; i++)
                {
                    for (int j = 0; j < blockHeight; j++)
                    {
                        int x = i + block_x * blockWidth;
                        int y = j + block_y * blockHeight;
                        try
                        {
                            colors[x + (ddsHeader.dwHeight - y - 1) * ddsHeader.dwWidth] = blockColors[j][i];
                        }
                        catch (Exception e)
                        {
                            throw new Exception(String.Format("Max=({0},{1}), BlockCoord=({2},{3}), LocPixCoord=({4},{5}), GlobPixCoord=({6},{7}), PixInd=({8})",
                                ddsHeader.dwWidth, ddsHeader.dwHeight,
                                block_x, block_y,
                                i, j,
                                x, y,
                                x + y * ddsHeader.dwWidth
                            ));
                        }
                    }
                }
            }

            return colors;
            return null;
        }
    }
}