

using System;
using System.Collections.Generic;

using Stream = System.IO.Stream;

using BinaryReader = System.IO.BinaryReader;
using System.Linq;
using mike_and_conquer.gameview;

namespace mike_and_conquer
{
    public class GameMap
    {

        public const string CLEAR1_SHP = "Content\\clear1.tem";

        public const string D04_TEM = "Content\\d04.tem";
        public const string D09_TEM = "Content\\d09.tem";
        public const string D13_TEM = "Content\\d13.tem";
        public const string D15_TEM = "Content\\d15.tem";
        public const string D20_TEM = "Content\\d20.tem";
        public const string D21_TEM = "Content\\d21.tem";
        public const string D23_TEM = "Content\\d23.tem";

        public const string P07_TEM = "Content\\p07.tem";
        public const string P08_TEM = "Content\\p08.tem";

        public const string S09_TEM = "Content\\s09.tem";
        public const string S10_TEM = "Content\\s10.tem";
        public const string S11_TEM = "Content\\s11.tem";
        public const string S12_TEM = "Content\\s12.tem";
        public const string S14_TEM = "Content\\s14.tem";
        public const string S22_TEM = "Content\\s22.tem";
        public const string S29_TEM = "Content\\s29.tem";
        public const string S32_TEM = "Content\\s32.tem";
        public const string S34_TEM = "Content\\s34.tem";
        public const string S35_TEM = "Content\\s35.tem";

        public const string SH1_TEM = "Content\\sh1.tem";
        public const string SH2_TEM = "Content\\sh2.tem";
        public const string SH3_TEM = "Content\\sh3.tem";
        public const string SH4_TEM = "Content\\sh4.tem";
        public const string SH5_TEM = "Content\\sh5.tem";
        public const string SH6_TEM = "Content\\sh6.tem";
        public const string SH9_TEM = "Content\\sh9.tem";
        public const string SH10_TEM = "Content\\sh10.tem";
        public const string SH17_TEM = "Content\\sh17.tem";
        public const string SH18_TEM = "Content\\sh18.tem";

        public const string W1_TEM = "Content\\w1.tem";
        public const string W2_TEM = "Content\\w2.tem";


        private List<MapTileInstance> mapTileInstanceList;

        public List<MapTileInstance> MapTileInstanceList
        {
            get { return mapTileInstanceList; }
        }


        private Dictionary<byte, string> mapFileCodeToTextureStringMap = new Dictionary<byte, string>();

        public int numColumns;
        public int numRows;

        private Dictionary<string, int[]> blockingTerrainMap = new Dictionary<string, int[]>();

        private GameMap()
        {
        }

        public GameMap(Stream inputStream, int startX, int startY, int endX, int endY)
        {
            LoadCodeToTextureStringMap();

            List<byte> allBytes = ReadAllBytesFromStream(inputStream);

            numColumns = endX - startX + 1;
            numRows = endY - startY + 1;

            InitializeBlockTerrainMap();

            mapTileInstanceList = new List<MapTileInstance>();

            int x = 12;
            int y = 12;

            int i = 0;
            for (int row = startY; row <= endY; row++)
            {
                for (int column = startX; column <= endX; column++)
                {
                    int offset = CalculateOffset(column, row);
                    string textureKey = ConvertByteToTextureKey(allBytes[offset]);
                    byte imageIndex = CalculateImageIndexForTextureKey(textureKey,allBytes, column, row, offset);
                    bool isBlockingTerrain = IsBlockingTerrain(textureKey, imageIndex);

                    MapTileInstance mapTileInstance =
                        new MapTileInstance(x, y, textureKey, imageIndex, isBlockingTerrain);
                    this.MapTileInstanceList.Add(mapTileInstance);

                    x = x + GameWorld.MAP_TILE_HEIGHT_AND_WIDTH;

                    bool incrementRow = ((i + 1) % 26) == 0;
                    if (incrementRow)
                    {
                        x = 12;
                        y = y + GameWorld.MAP_TILE_HEIGHT_AND_WIDTH;
                    }

                    i++;

                }
            }
        }

        public GameMap(int[,] nodeArray)
        {
            mapTileInstanceList = new List<MapTileInstance>();


            numRows = nodeArray.GetLength(0);
            numColumns = nodeArray.GetLength(1);


            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numColumns; x++)
                {

                    int mapTileInstanceX = (x * GameWorld.MAP_TILE_HEIGHT_AND_WIDTH) + 12;
                    int mapTileInstanceY = (y * GameWorld.MAP_TILE_HEIGHT_AND_WIDTH) + 12;
                    string dummyTexture = "";
                    byte dummyImageIndex = 0;

                    if (nodeArray[y, x] == 1)
                    {
                        MapTileInstance mapTileInstance =
                            new MapTileInstance(mapTileInstanceX, mapTileInstanceY, dummyTexture, dummyImageIndex, true);
                        this.MapTileInstanceList.Add(mapTileInstance);

                    }
                    else
                    {
                        MapTileInstance mapTileInstance =
                            new MapTileInstance(mapTileInstanceX, mapTileInstanceY, dummyTexture, dummyImageIndex, false);
                        this.MapTileInstanceList.Add(mapTileInstance);

                    }
                }
            }



            //            int x = 12;
            //            int y = 12;
            //
            //            int i = 0;
            //            for (int row = startY; row <= endY; row++)
            //            {
            //                for (int column = startX; column <= endX; column++)
            //                {
            //                    int offset = CalculateOffset(column, row);
            //                    string textureKey = ConvertByteToTextureKey(allBytes[offset]);
            //                    byte imageIndex = CalculateImageIndexForTextureKey(textureKey, allBytes, column, row, offset);
            //                    bool isBlockingTerrain = IsBlockingTerrain(textureKey, imageIndex);
            //
            //                    MapTileInstance mapTileInstance =
            //                        new MapTileInstance(x, y, textureKey, imageIndex, isBlockingTerrain);
            //                    this.MapTileInstanceList.Add(mapTileInstance);
            //
            //                    x = x + 24;
            //
            //                    bool incrementRow = ((i + 1) % 26) == 0;
            //                    if (incrementRow)
            //                    {
            //                        x = 12;
            //                        y = y + 24;
            //                    }
            //
            //                    i++;
            //
            //                }
            //            }

        }


        private byte CalculateImageIndexForTextureKey(string textureKey, List<byte> allBytes, int column, int row, int offset)
        {

            if (textureKey == CLEAR1_SHP)
            {
                return CalculateImageIndexForClear1(column, row);
            }
            else
            {
                return allBytes[offset + 1];
            }

        }

        private List<byte> ReadAllBytesFromStream(Stream inputStream)
        {
            BinaryReader binaryReader = new BinaryReader(inputStream);
            long numBytes = binaryReader.BaseStream.Length;
            List<byte> allBytes = new List<byte>();
            for (int i = 0; i < numBytes; i++)
            {
                byte nextByte = binaryReader.ReadByte();
                allBytes.Add(nextByte);
            }

            return allBytes;
        }

        private bool IsBlockingTerrain(string textureKey, byte imageIndex)
        {
            
            if (blockingTerrainMap.ContainsKey(textureKey))
            {
                int[] blockedImageIndexes = blockingTerrainMap[textureKey];
                if (blockedImageIndexes == null)
                {
                    return true;
                }
                else
                {
                    return blockedImageIndexes.Contains(imageIndex);
                }
            }

            return false;

        }

        private void InitializeBlockTerrainMap()
        {
            blockingTerrainMap.Add(S09_TEM, null);
            blockingTerrainMap.Add(S10_TEM, null);
            blockingTerrainMap.Add(S11_TEM, null);
            blockingTerrainMap.Add(S12_TEM, null);
            blockingTerrainMap.Add(S14_TEM, null);
            blockingTerrainMap.Add(S22_TEM, null);
            blockingTerrainMap.Add(S29_TEM, null);
            blockingTerrainMap.Add(S32_TEM, null);
            blockingTerrainMap.Add(S34_TEM, null);
            blockingTerrainMap.Add(S35_TEM, null);
            blockingTerrainMap.Add(SH1_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(SH2_TEM, new int[] { 3, 4, 5, 6, 7, 8 });
            blockingTerrainMap.Add(SH3_TEM, null);
            blockingTerrainMap.Add(SH4_TEM, null);
            blockingTerrainMap.Add(SH5_TEM, new int[] {  6, 7, 8 });
            blockingTerrainMap.Add(SH6_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(SH9_TEM, new int[] { 6 });
            blockingTerrainMap.Add(SH10_TEM, new int[] {0,2,3 });
            blockingTerrainMap.Add(SH17_TEM, null);
            blockingTerrainMap.Add(SH18_TEM, null);
            blockingTerrainMap.Add(W1_TEM, null);
            blockingTerrainMap.Add(W2_TEM, null);
        }


        private byte CalculateImageIndexForClear1(int column, int row)
        {
            return (byte)((column % 4) + ((row % 4) * 4));
        }

        private void LoadCodeToTextureStringMap()
        {
            mapFileCodeToTextureStringMap.Add(0xff, CLEAR1_SHP);

            mapFileCodeToTextureStringMap.Add(0x60, D04_TEM);
            mapFileCodeToTextureStringMap.Add(0x69, D13_TEM);
            mapFileCodeToTextureStringMap.Add(0x70, D20_TEM);
            mapFileCodeToTextureStringMap.Add(0x71, D21_TEM);
            mapFileCodeToTextureStringMap.Add(0x73, D23_TEM);

            mapFileCodeToTextureStringMap.Add(0x49, P07_TEM);
            mapFileCodeToTextureStringMap.Add(0x4A, P08_TEM);

            mapFileCodeToTextureStringMap.Add(0x15, S09_TEM);
            mapFileCodeToTextureStringMap.Add(0x16, S10_TEM);
            mapFileCodeToTextureStringMap.Add(0x17, S11_TEM);
            mapFileCodeToTextureStringMap.Add(0x18, S12_TEM);
            mapFileCodeToTextureStringMap.Add(0x1a, S14_TEM);
            mapFileCodeToTextureStringMap.Add(0x22, S22_TEM);
            mapFileCodeToTextureStringMap.Add(0x29, S29_TEM);


            mapFileCodeToTextureStringMap.Add(0x2c, S32_TEM);
            mapFileCodeToTextureStringMap.Add(0x2e, S34_TEM);
            mapFileCodeToTextureStringMap.Add(0x2f, S35_TEM);


            mapFileCodeToTextureStringMap.Add(0x03, SH1_TEM);
            mapFileCodeToTextureStringMap.Add(0x04, SH2_TEM);
            mapFileCodeToTextureStringMap.Add(0x05, SH3_TEM);

            mapFileCodeToTextureStringMap.Add(0x06, SH4_TEM);

            mapFileCodeToTextureStringMap.Add(0x07, SH5_TEM);
            mapFileCodeToTextureStringMap.Add(0x58, SH6_TEM);
            mapFileCodeToTextureStringMap.Add(0x5b, SH9_TEM);
            mapFileCodeToTextureStringMap.Add(0x5c, SH10_TEM);
            mapFileCodeToTextureStringMap.Add(0x4c, SH17_TEM);
            mapFileCodeToTextureStringMap.Add(0x4d, SH18_TEM);

            mapFileCodeToTextureStringMap.Add(0x01, W1_TEM);
            mapFileCodeToTextureStringMap.Add(0x02, W2_TEM);
        }

        private string ConvertByteToTextureKey(byte inputByte)
        {

            if(mapFileCodeToTextureStringMap.ContainsKey(inputByte))
            {
                string textureKey;
                mapFileCodeToTextureStringMap.TryGetValue(inputByte, out textureKey);
                return textureKey;
            }
            else
            {
                string textureKey;
                mapFileCodeToTextureStringMap.TryGetValue(inputByte, out textureKey);
                return textureKey;
            }

            // TODO: Change to this once we get all tile types registered
            //return mapFileCodeToTextureStringMap[inputByte];
        }

        private int CalculateOffset(int column, int row)
        {
            return (row * 64 * 2) + (column * 2);
        }




    }


}
