using UnityEngine;

namespace DDS
{
    [DDSAssembler(BLOCK_WIDTH, BLOCK_HEGHT, BLOCK_DATA_LENGHT, "DXT5")]
    public static class DXT5Assembler
    {
        public const int BLOCK_WIDTH = 4;
        public const int BLOCK_HEGHT = 4;
        public const int BLOCK_DATA_LENGHT = 16;

        [DDSCompressor]
        private static byte[] Compressor(Color32[] block)
        {
            return null;
        }

        [DDSDecompressor]
        public static Color32[][] Decompressor(byte[] blockData)
        {
            Color32[][] block = new Color32[BLOCK_WIDTH][];

            int alpha0 = (int) (blockData[0]) + (int) (blockData[1] << 8);
            int alpha1 = (int) (blockData[2]) + (int) (blockData[3] << 8);
            int codes = (int) blockData[4] + (int) (blockData[5] << 8) + (int) (blockData[6] << 16) + (int) (blockData[7] << 24);

            int alpha2_1 = Utils.MixColors(alpha0, alpha1, 6, 1, 7);
            int alpha2_2 = Utils.MixColors(alpha0, alpha1, 4, 1, 5);
            
            int alpha3_1 = Utils.MixColors(alpha0, alpha1, 5, 2, 7);
            int alpha3_2 = Utils.MixColors(alpha0, alpha1, 3, 2, 5);
            
            int alpha4_1 = Utils.MixColors(alpha0, alpha1, 4, 3, 7);
            int alpha4_2 = Utils.MixColors(alpha0, alpha1, 2, 3, 5);
            
            int alpha5_1 = Utils.MixColors(alpha0, alpha1, 3, 4, 7);
            int alpha5_2 = Utils.MixColors(alpha0, alpha1, 1, 4, 5);
            
            int alpha6_1 = Utils.MixColors(alpha0, alpha1, 2, 5, 7);
            int alpha6_2 = 0;
            
            int alpha7_1 = Utils.MixColors(alpha0, alpha1, 1, 6, 7);
            int alpha7_2 = 1;

            bool a0ma1 = alpha0 > alpha1;

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
                            rgb = alpha0;
                            break;
                        case 1:
                            rgb = alpha1;
                            break;
                        case 2:
                            rgb = a0ma1 ? alpha2_1 : alpha2_2;
                            break;
                        case 3:
                            rgb = a0ma1 ? alpha3_1 : alpha3_2;
                            break;
                        case 4:
                            rgb = a0ma1 ? alpha4_1 : alpha4_2;
                            break;
                        case 5:
                            rgb = a0ma1 ? alpha5_1 : alpha5_2;
                            break;
                        case 6:
                            rgb = a0ma1 ? alpha6_1 : alpha6_2;
                            break;
                        case 7:
                            rgb = a0ma1 ? alpha7_1 : alpha7_2;
                            break;
                        
                    }

                    block[i][j] = Utils.ColorRGBtoColor32(Utils.Color565to888(rgb));
                }
            }

            return block;
        }
    }
}