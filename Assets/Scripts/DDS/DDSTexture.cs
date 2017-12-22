using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using DDS;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

[Serializable]
public class DDSTexture
{
    private const int DDS_HEADER_SIZE = 128;

    public Texture2D texture2d = null;


    private int length;
    private byte[] ddsBytes;
    private int pointer = 0;

    private string headerName;
    public DDS_HEADER ddsHeader;
    public byte[] ddsData;

    int rShift;
    int gShift;
    int bShift;
    int aShift;
    private byte[] imageBytes;

    delegate Color32[] Decompressor();

    public DDSTexture()
    {
    }

    public DDSTexture(byte[] ddsBytes, int length = -1)
    {
        this.ddsBytes = ddsBytes;
        if (length <= 0)
        {
            this.length = ddsBytes.Length;
        }
        pointer = 0;
        headerName = ReadString(4);
        ddsHeader.dwSize = ReadInt();
        ddsHeader.dwFlags = ReadEnumFlags<DDS_HEADER.DWFlags>();
        ddsHeader.dwHeight = ReadInt();
        ddsHeader.dwWidth = ReadInt();
        ddsHeader.dwPitchOrLinearSize = ReadInt();
        ddsHeader.dwDepth = ReadInt();
        ddsHeader.dwMipMapCount = ReadInt();
        ddsHeader.dwReserved1 = ReadInts(11);
        ddsHeader.ddspf = ReadPixelFormat();
        ddsHeader.dwCaps = ReadEnumFlags<DDS_HEADER.DWCaps>();
        ddsHeader.dwCaps2 = ReadEnumFlags<DDS_HEADER.DWCaps2>();
        ddsHeader.dwCaps3 = ReadInt();
        ddsHeader.dwCaps4 = ReadInt();
        ddsHeader.dwReserved2 = ReadInt();

        rShift = ((ddsHeader.ddspf.dwRBitMask >> 0) & 0xff & 0)
                 + ((ddsHeader.ddspf.dwRBitMask >> 8) & 0xff & 1)
                 + ((ddsHeader.ddspf.dwRBitMask >> 16) & 0xff & 2)
                 + ((ddsHeader.ddspf.dwRBitMask >> 24) & 0xff & 3);
        gShift = ((ddsHeader.ddspf.dwGBitMask >> 0) & 0xff & 0)
                 + ((ddsHeader.ddspf.dwGBitMask >> 8) & 0xff & 1)
                 + ((ddsHeader.ddspf.dwGBitMask >> 16) & 0xff & 2)
                 + ((ddsHeader.ddspf.dwGBitMask >> 24) & 0xff & 3);
        bShift = ((ddsHeader.ddspf.dwBBitMask >> 0) & 0xff & 0)
                 + ((ddsHeader.ddspf.dwBBitMask >> 8) & 0xff & 1)
                 + ((ddsHeader.ddspf.dwBBitMask >> 16) & 0xff & 2)
                 + ((ddsHeader.ddspf.dwBBitMask >> 24) & 0xff & 3);
        aShift = ((ddsHeader.ddspf.dwABitMask >> 0) & 0xff & 0)
                 + ((ddsHeader.ddspf.dwABitMask >> 8) & 0xff & 1)
                 + ((ddsHeader.ddspf.dwABitMask >> 16) & 0xff & 2)
                 + ((ddsHeader.ddspf.dwABitMask >> 24) & 0xff & 3);

        ddsData = new byte[ddsBytes.Length - DDS_HEADER_SIZE];

        Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, ddsData, 0, ddsData.Length);
    }

    T[] ReadEnumFlags<T>()
    {
        int enumint = ReadInt();
        List<T> dwFlags = new List<T>();
        int[] castedToInt = Enum.GetValues(typeof(T)).Cast<int>().ToArray();
        T[] castedToT = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        for (int i = 0; i < castedToInt.Length; i++)
        {
            if ((enumint & castedToInt[i]) > 0)
            {
                dwFlags.Add(castedToT[i]);
            }
        }

        return dwFlags.ToArray();
    }

    T ReadEnum<T>()
    {
        int enumint = ReadInt();
        return (T) Enum.ToObject(typeof(T), enumint);
    }

    DDS_PIXELFORMAT ReadPixelFormat()
    {
        DDS_PIXELFORMAT pf = new DDS_PIXELFORMAT();

        pf.dwSize = ReadInt();
        pf.dwFlags = ReadEnumFlags<DDS_PIXELFORMAT.DWFlags>();
        pf.dwFourCC = ReadString(4);
        pf.dwRGBBitCount = ReadInt();
        pf.dwRBitMask = ReadInt();
        pf.dwGBitMask = ReadInt();
        pf.dwBBitMask = ReadInt();
        pf.dwABitMask = ReadInt();

        return pf;
    }

    DDS_HEADER_DXT10 ReadHDXT10()
    {
        DDS_HEADER_DXT10 header = new DDS_HEADER_DXT10();

        header.dxgiFormat = ReadEnum<DDS_HEADER_DXT10.DXGI_FORMAT>();
        header.resourceDimension = ReadEnum<DDS_HEADER_DXT10.D3D10_RESOURCE_DIMENSION>();
        header.miscFlag = ReadUInt();
        header.arraySize = ReadUInt();
        header.miscFlags2 = ReadUInt();

        return header;
    }

    int ReadInt()
    {
        pointer += sizeof(int);
        return BitConverter.ToInt32(ddsBytes, pointer - sizeof(int));
    }

    long ReadLong()
    {
        pointer += sizeof(long);
        return BitConverter.ToInt64(ddsBytes, pointer - sizeof(long));
    }

    short ReadShort()
    {
        pointer += sizeof(short);
        return BitConverter.ToInt16(ddsBytes, pointer - sizeof(short));
    }

    uint ReadUInt()
    {
        pointer += sizeof(uint);
        return BitConverter.ToUInt32(ddsBytes, pointer - sizeof(uint));
    }

    byte ReadByte()
    {
        pointer += sizeof(byte);
        return ddsBytes[pointer - sizeof(byte)];
    }

    float ReadFloat()
    {
        pointer += sizeof(float);
        return BitConverter.ToSingle(ddsBytes, pointer - sizeof(float));
    }

    string ReadString(int length = -1, Encoding encoding = null)
    {
        if (length == -1)
        {
            List<byte> bytes = new List<byte>();
            int tmp = ReadByte();
            while (tmp != 0)
            {
                bytes.Add((byte) tmp);
                tmp = ReadByte();
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

    byte[] ReadBytes(int length)
    {
        byte[] byteBuffer = new byte[length];
        pointer += byteBuffer.Length;
        Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

        return byteBuffer;
    }

    int[] ReadInts(int length)
    {
        byte[] byteBuffer = new byte[length * sizeof(int)];
        pointer += byteBuffer.Length;
        Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);
        return Enumerable.Range(0, byteBuffer.Length / 4)
            .Select(i => BitConverter.ToInt32(byteBuffer, i * 4))
            .ToArray();
    }

    float[] ReadFloats(int length)
    {
        byte[] byteBuffer = new byte[length * sizeof(float)];
        pointer += byteBuffer.Length;
        Array.Copy(ddsBytes, pointer - byteBuffer.Length, byteBuffer, 0, byteBuffer.Length);

        return Enumerable.Range(0, byteBuffer.Length / 4)
            .Select(i => BitConverter.ToSingle(byteBuffer, i * 4))
            .ToArray();
    }

    short[] ReadShorts(int length)
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

    Vector2 ReadVector2()
    {
        return new Vector2(ReadFloat(), ReadFloat());
    }

    Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    Vector4 ReadVector4()
    {
        return new Vector4(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public static explicit operator Texture2D(DDSTexture ddsTexture)
    {
        return ddsTexture.ToTexture2D();
    }


    public Texture2D ToTexture2D()
    {
        if (texture2d != null)
        {
            return texture2d;
        }

        texture2d = new Texture2D(ddsHeader.dwWidth, ddsHeader.dwHeight);
        texture2d.filterMode = FilterMode.Bilinear;
        texture2d.anisoLevel = 16;

        Color32[] colors = DDSAssembler.Decompressor(ddsHeader, ddsData);
        texture2d.SetPixels32(colors);
        texture2d.Apply();
        return texture2d;
    }
}

[Serializable]
public struct DDS_HEADER
{
    public int dwSize;
    public DWFlags[] dwFlags;
    public int dwHeight;
    public int dwWidth;
    public int dwPitchOrLinearSize;
    public int dwDepth;
    public int dwMipMapCount;
    public int[] dwReserved1; //11
    public DDS_PIXELFORMAT ddspf;
    public DWCaps[] dwCaps;
    public DWCaps2[] dwCaps2;
    public int dwCaps3; //Unused
    public int dwCaps4; //Unused
    public int dwReserved2; //Unused

    [Serializable]
    public enum DWFlags
    {
        DDSD_CAPS = 0x1,
        DDSD_HEIGHT = 0x2,
        DDSD_WIDTH = 0x4,
        DDSD_PITCH = 0x8,
        DDSD_PIXELFORMAT = 0x1000,
        DDSD_MIPMAPCOUNT = 0x20000,
        DDSD_LINEARSIZE = 0x80000,
        DDSD_DEPTH = 0x800000
    }

    [Serializable]
    public enum DWCaps
    {
        DDSCAPS_COMPLEX = 0x8,
        DDSCAPS_MIPMAP = 0x400000,
        DDSCAPS_TEXTURE = 0x1000
    }

    [Serializable]
    public enum DWCaps2
    {
        DDSCAPS2_CUBEMAP = 0x200,
        DDSCAPS2_CUBEMAP_POSITIVEX = 0x400,
        DDSCAPS2_CUBEMAP_NEGATIVEX = 0x800,
        DDSCAPS2_CUBEMAP_POSITIVEY = 0x1000,
        DDSCAPS2_CUBEMAP_NEGATIVEY = 0x2000,
        DDSCAPS2_CUBEMAP_POSITIVEZ = 0x4000,
        DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x8000,
        DDSCAPS2_VOLUME = 0x200000
    }
}

[Serializable]
public struct DDS_PIXELFORMAT
{
    public int dwSize;
    public DWFlags[] dwFlags;
    public string dwFourCC;
    public int dwRGBBitCount;
    public int dwRBitMask;
    public int dwGBitMask;
    public int dwBBitMask;
    public int dwABitMask;

    [Serializable]
    public enum DWFlags
    {
        DDPF_ALPHAPIXELS = 0x1,
        DDPF_ALPHA = 0x2,
        DDPF_FOURCC = 0x4,
        DDPF_RGB = 0x40,
        DDPF_YUV = 0x200,
        DDPF_LUMINANCE = 0x20000
    }
}

[Serializable]
public struct DDS_HEADER_DXT10
{
    public DXGI_FORMAT dxgiFormat;
    public D3D10_RESOURCE_DIMENSION resourceDimension;
    public uint miscFlag;
    public uint arraySize;
    public uint miscFlags2;

    [Serializable]
    public enum DXGI_FORMAT
    {
        DXGI_FORMAT_UNKNOWN = 0,
        DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
        DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
        DXGI_FORMAT_R32G32B32A32_UINT = 3,
        DXGI_FORMAT_R32G32B32A32_SINT = 4,
        DXGI_FORMAT_R32G32B32_TYPELESS = 5,
        DXGI_FORMAT_R32G32B32_FLOAT = 6,
        DXGI_FORMAT_R32G32B32_UINT = 7,
        DXGI_FORMAT_R32G32B32_SINT = 8,
        DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
        DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
        DXGI_FORMAT_R16G16B16A16_UNORM = 11,
        DXGI_FORMAT_R16G16B16A16_UINT = 12,
        DXGI_FORMAT_R16G16B16A16_SNORM = 13,
        DXGI_FORMAT_R16G16B16A16_SINT = 14,
        DXGI_FORMAT_R32G32_TYPELESS = 15,
        DXGI_FORMAT_R32G32_FLOAT = 16,
        DXGI_FORMAT_R32G32_UINT = 17,
        DXGI_FORMAT_R32G32_SINT = 18,
        DXGI_FORMAT_R32G8X24_TYPELESS = 19,
        DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
        DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
        DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
        DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
        DXGI_FORMAT_R10G10B10A2_UNORM = 24,
        DXGI_FORMAT_R10G10B10A2_UINT = 25,
        DXGI_FORMAT_R11G11B10_FLOAT = 26,
        DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
        DXGI_FORMAT_R8G8B8A8_UNORM = 28,
        DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
        DXGI_FORMAT_R8G8B8A8_UINT = 30,
        DXGI_FORMAT_R8G8B8A8_SNORM = 31,
        DXGI_FORMAT_R8G8B8A8_SINT = 32,
        DXGI_FORMAT_R16G16_TYPELESS = 33,
        DXGI_FORMAT_R16G16_FLOAT = 34,
        DXGI_FORMAT_R16G16_UNORM = 35,
        DXGI_FORMAT_R16G16_UINT = 36,
        DXGI_FORMAT_R16G16_SNORM = 37,
        DXGI_FORMAT_R16G16_SINT = 38,
        DXGI_FORMAT_R32_TYPELESS = 39,
        DXGI_FORMAT_D32_FLOAT = 40,
        DXGI_FORMAT_R32_FLOAT = 41,
        DXGI_FORMAT_R32_UINT = 42,
        DXGI_FORMAT_R32_SINT = 43,
        DXGI_FORMAT_R24G8_TYPELESS = 44,
        DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
        DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
        DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
        DXGI_FORMAT_R8G8_TYPELESS = 48,
        DXGI_FORMAT_R8G8_UNORM = 49,
        DXGI_FORMAT_R8G8_UINT = 50,
        DXGI_FORMAT_R8G8_SNORM = 51,
        DXGI_FORMAT_R8G8_SINT = 52,
        DXGI_FORMAT_R16_TYPELESS = 53,
        DXGI_FORMAT_R16_FLOAT = 54,
        DXGI_FORMAT_D16_UNORM = 55,
        DXGI_FORMAT_R16_UNORM = 56,
        DXGI_FORMAT_R16_UINT = 57,
        DXGI_FORMAT_R16_SNORM = 58,
        DXGI_FORMAT_R16_SINT = 59,
        DXGI_FORMAT_R8_TYPELESS = 60,
        DXGI_FORMAT_R8_UNORM = 61,
        DXGI_FORMAT_R8_UINT = 62,
        DXGI_FORMAT_R8_SNORM = 63,
        DXGI_FORMAT_R8_SINT = 64,
        DXGI_FORMAT_A8_UNORM = 65,
        DXGI_FORMAT_R1_UNORM = 66,
        DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
        DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
        DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
        DXGI_FORMAT_BC1_TYPELESS = 70,
        DXGI_FORMAT_BC1_UNORM = 71,
        DXGI_FORMAT_BC1_UNORM_SRGB = 72,
        DXGI_FORMAT_BC2_TYPELESS = 73,
        DXGI_FORMAT_BC2_UNORM = 74,
        DXGI_FORMAT_BC2_UNORM_SRGB = 75,
        DXGI_FORMAT_BC3_TYPELESS = 76,
        DXGI_FORMAT_BC3_UNORM = 77,
        DXGI_FORMAT_BC3_UNORM_SRGB = 78,
        DXGI_FORMAT_BC4_TYPELESS = 79,
        DXGI_FORMAT_BC4_UNORM = 80,
        DXGI_FORMAT_BC4_SNORM = 81,
        DXGI_FORMAT_BC5_TYPELESS = 82,
        DXGI_FORMAT_BC5_UNORM = 83,
        DXGI_FORMAT_BC5_SNORM = 84,
        DXGI_FORMAT_B5G6R5_UNORM = 85,
        DXGI_FORMAT_B5G5R5A1_UNORM = 86,
        DXGI_FORMAT_B8G8R8A8_UNORM = 87,
        DXGI_FORMAT_B8G8R8X8_UNORM = 88,
        DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
        DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
        DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
        DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
        DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
        DXGI_FORMAT_BC6H_TYPELESS = 94,
        DXGI_FORMAT_BC6H_UF16 = 95,
        DXGI_FORMAT_BC6H_SF16 = 96,
        DXGI_FORMAT_BC7_TYPELESS = 97,
        DXGI_FORMAT_BC7_UNORM = 98,
        DXGI_FORMAT_BC7_UNORM_SRGB = 99,
        DXGI_FORMAT_AYUV = 100,
        DXGI_FORMAT_Y410 = 101,
        DXGI_FORMAT_Y416 = 102,
        DXGI_FORMAT_NV12 = 103,
        DXGI_FORMAT_P010 = 104,
        DXGI_FORMAT_P016 = 105,
        DXGI_FORMAT_420_OPAQUE = 106,
        DXGI_FORMAT_YUY2 = 107,
        DXGI_FORMAT_Y210 = 108,
        DXGI_FORMAT_Y216 = 109,
        DXGI_FORMAT_NV11 = 110,
        DXGI_FORMAT_AI44 = 111,
        DXGI_FORMAT_IA44 = 112,
        DXGI_FORMAT_P8 = 113,
        DXGI_FORMAT_A8P8 = 114,
        DXGI_FORMAT_B4G4R4A4_UNORM = 115,
        DXGI_FORMAT_P208 = 130,
        DXGI_FORMAT_V208 = 131,
        DXGI_FORMAT_V408 = 132,
        DXGI_FORMAT_FORCE_UINT = -1
    }

    [Serializable]
    public enum D3D10_RESOURCE_DIMENSION
    {
        D3D10_RESOURCE_DIMENSION_UNKNOWN = 0,
        D3D10_RESOURCE_DIMENSION_BUFFER = 1,
        D3D10_RESOURCE_DIMENSION_TEXTURE1D = 2,
        D3D10_RESOURCE_DIMENSION_TEXTURE2D = 3,
        D3D10_RESOURCE_DIMENSION_TEXTURE3D = 4
    }
}