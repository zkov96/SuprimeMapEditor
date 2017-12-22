using UnityEngine;

namespace DDS
{
    [DDSAssembler(BLOCK_WIDTH, BLOCK_HEGHT, BLOCK_DATA_LENGHT, "")]
    public static class UncompressAssembler
    {
        public const int BLOCK_WIDTH = 1;
        public const int BLOCK_HEGHT = 1;
        public const int BLOCK_DATA_LENGHT = 4;

        [DDSCompressor]
        private static byte[] Compressor(Color32[] block)
        {
            return null;
        }

        [DDSDecompressor]
        public static Color32[][] Decompressor(byte[] blockData)
        {
            Color32[][] block = new Color32[][] {new Color32[] {new Color32(blockData[0], blockData[1], blockData[2], blockData[3])}};
            return block;
        }
    }
}