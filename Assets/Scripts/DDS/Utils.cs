using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace DDS
{
    public class Utils
    {
        public static int Color565to888(int color)
        {
            byte rb = (byte) ((color >> 11) & 0x1f);
            byte gb = (byte) ((color >> 5) & 0x3F);
            byte bb = (byte) ((color >> 0) & 0x1f);
            byte r = (byte) (rb * 0xFF / 0x1F);
            byte g = (byte) (gb * 0xFF / 0x3F);
            byte b = (byte) (bb * 0xFF / 0x1F);

            return (((int) r)) | (((int) g) << 8) | (((int) b << 16));
        }

        public static Color32 ColorRGBtoColor32(int color)
        {
            byte[] colorBytes = BitConverter.GetBytes(color);
            return new Color32(colorBytes[0], colorBytes[1], colorBytes[2], 0xff);
        }

        public static Color32 ColorRGBAtoColor32(int color)
        {
            byte[] colorBytes = BitConverter.GetBytes(color);
            return new Color32(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]);
        }

        public static int MixColors(int color0, int color1, int mul1, int mul2, int div)
        {
            float r0 = (color0 >> 11) & 31;
            float g0 = (color0 >> 5) & 63;
            float b0 = color0 & 31;
            float r1 = (color1 >> 11) & 31;
            float g1 = (color1 >> 5) & 63;
            float b1 = color1 & 31;
            float r = (r0 * mul1 + r1 * mul2) / div;
            float g = (g0 * mul1 + g1 * mul2) / div;
            float b = (b0 * mul1 + b1 * mul2) / div;
            return ((int) Mathf.Round(r) << 11) | ((int) Mathf.Round(g) << 5) | ((int) Mathf.Round(b));
        }
    }

    public class DataUtils
    {
        public int ReadInt(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(int);
            return BitConverter.ToInt32(ddsBytes, pointer - sizeof(int));
        }

        public long ReadLong(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(long);
            return BitConverter.ToInt64(ddsBytes, pointer - sizeof(long));
        }

        public short ReadShort(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(short);
            return BitConverter.ToInt16(ddsBytes, pointer - sizeof(short));
        }

        public uint ReadUInt(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(uint);
            return BitConverter.ToUInt32(ddsBytes, pointer - sizeof(uint));
        }

        public byte ReadByte(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(byte);
            return ddsBytes[pointer - sizeof(byte)];
        }

        public float ReadFloat(byte[] ddsBytes, ref int pointer)
        {
            pointer += sizeof(float);
            return BitConverter.ToSingle(ddsBytes, pointer - sizeof(float));
        }

        public string ReadString(byte[] ddsBytes, ref int pointer, int length = -1, Encoding encoding = null)
        {
            if (length == -1)
            {
                List<byte> bytes = new List<byte>();
                int tmp = ReadByte(ddsBytes, ref pointer);
                while (tmp != 0)
                {
                    bytes.Add((byte) tmp);
                    tmp = ReadByte(ddsBytes, ref pointer);
                }
                return encoding.GetString(bytes.ToArray());
            }
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            byte[] byteBuffer = new byte[length * encoding.GetByteCount("a")];
            pointer += byteBuffer.Length;
            Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

            return encoding.GetString(byteBuffer);
        }

        public byte[] ReadBytes(byte[] ddsBytes, ref int pointer, int length)
        {
            byte[] byteBuffer = new byte[length];
            pointer += byteBuffer.Length;
            Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

            return byteBuffer;
        }

        public int[] ReadInts(byte[] ddsBytes, ref int pointer, int length)
        {
            byte[] byteBuffer = new byte[length * sizeof(int)];
            pointer += byteBuffer.Length;
            Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);
            return Enumerable.Range(0, byteBuffer.Length / 4)
                .Select(i => BitConverter.ToInt32(byteBuffer, i * 4))
                .ToArray();
        }

        public float[] ReadFloats(byte[] ddsBytes, ref int pointer, int length)
        {
            byte[] byteBuffer = new byte[length * sizeof(float)];
            pointer += byteBuffer.Length;
            Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

            return Enumerable.Range(0, byteBuffer.Length / 4)
                .Select(i => BitConverter.ToSingle(byteBuffer, i * 4))
                .ToArray();
        }

        public short[] ReadShorts(byte[] ddsBytes, ref int pointer, int length)
        {
            byte[] byteBuffer = new byte[length * sizeof(short)];
            pointer += byteBuffer.Length;
            Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

            short[] shorts = new short[length];
            for (int i = 0; i < byteBuffer.Length; i += sizeof(short))
            {
                shorts[i / sizeof(short)] = (short) ((byteBuffer[i] << 8) + byteBuffer[i + 1]);
            }

            return Enumerable.Range(0, byteBuffer.Length / 2)
                .Select(i => BitConverter.ToInt16(byteBuffer, i * 2))
                .ToArray();
        }

        public Vector2 ReadVector2(byte[] ddsBytes, ref int pointer)
        {
            return new Vector2(ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer));
        }

        public Vector3 ReadVector3(byte[] ddsBytes, ref int pointer)
        {
            return new Vector3(ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer));
        }

        public Vector4 ReadVector4(byte[] ddsBytes, ref int pointer)
        {
            return new Vector4(ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer), ReadFloat(ddsBytes, ref pointer));
        }
    }
}
