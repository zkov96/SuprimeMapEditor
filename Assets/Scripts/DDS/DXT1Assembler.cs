using UnityEngine;

namespace DDS
{
    [DDSAssembler(BLOCK_WIDTH, BLOCK_HEGHT, BLOCK_DATA_LENGHT, "DXT1")]
    public static class DXT1Assembler
    {
        public const int BLOCK_WIDTH = 4;
        public const int BLOCK_HEGHT = 4;
        public const int BLOCK_DATA_LENGHT = 8;

        [DDSCompressor]
        private static byte[] Compressor(Color32[] block)
        {
            return null;
        }

        [DDSDecompressor]
        public static Color32[][] Decompressor(byte[] blockData)
        {
            Color32[][] block = new Color32[BLOCK_WIDTH][];

            int color0 = (int) (blockData[0]) + (int) (blockData[1] << 8);
            int color1 = (int) (blockData[2]) + (int) (blockData[3] << 8);
            int codes = (int) blockData[4] + (int) (blockData[5] << 8) + (int) (blockData[6] << 16) + (int) (blockData[7] << 24);

            int color2 = Utils.MixColors(color0, color1, 2, 1, 3);
            int color3 = Utils.MixColors(color0, color1, 1, 1, 2);
            int color4 = Utils.MixColors(color0, color1, 1, 2, 3);


            for (int i = 0; i < BLOCK_WIDTH; i++)
            {
                block[i] = new Color32[BLOCK_HEGHT];
                
                for (int j = 0; j < BLOCK_HEGHT; j++)
                {
                    byte code = (byte) ((codes >> (2 * (j + i * 4))) & 3);

                    int rgb = 0;

                    switch (code)
                    {
                        case 0:
                            rgb = color0;
                            break;
                        case 1:
                            rgb = color1;
                            break;
                        case 2:
                            rgb = color0 > color1 ? color2 : color3;
                            break;
                        case 3:
                            rgb = color0 > color1 ? color4 : 0;
                            break;
                    }

                    block[i][j] = Utils.ColorRGBtoColor32(Utils.Color565to888(rgb));
                }
            }

            return block;
        }
    }
}