﻿

using System.Collections.Generic;

using Stream = System.IO.Stream;

using BinaryReader = System.IO.BinaryReader;

namespace mike_and_conquer
{
    public class GameMap
    {

// X       Draw class diagram and understand current structure
// X       Write method that returns current tile, given a point on the map
// X       Write method that returns color index of given point on the map
//        Write method that then maps the color index to the shadow index color
//        Write method that creates new texture for minigunner with shadow colors fixed up 
              // Create new texture of same size
              // Copy pixels over one by one
              // If pixel is the shadow green:
              //    determine the screen x,y of that pixel 
              //    determine the palette value of the existing screen background at that position
              //    map that background to the proper shadow pixel
              //    set that pixel in the new texture to that shadow pixel
              // Draw the texture
              // release the texture? (or release above before creating the new one?
        private List<MapTile> mapTileList;
        private Dictionary<byte, string> mapFileCodeToTextureStringMap = new Dictionary<byte, string>();

        public int numColumns;
        public int numRows;

        public Dictionary<string, int[]> blockingTerrainMap = new Dictionary<string, int[]>();


        public List<MapTile> MapTiles
        {
            get { return mapTileList; }
        }

        private GameMap()
        {
        }

        public GameMap(Stream inputStream, int startX, int startY, int endX, int endY)
         {
            LoadCodeToTextureStringMap();
            mapTileList = new List<MapTile>();

            BinaryReader binaryReader = new BinaryReader(inputStream);
            long numBytes = binaryReader.BaseStream.Length;
            List<byte> allBytes = new List<byte>();
            for(int i = 0; i < numBytes; i++)
            {
                byte nextByte = binaryReader.ReadByte();
                allBytes.Add(nextByte);
            }


            numColumns = endX - startX + 1;
            numRows = endY - startY + 1;

            for (int row = startY; row <= endY; row++)
            {
                for (int column = startX; column <= endX; column++)
                {
                    MapTile mapTile = new MapTile();
                    int offset = calculateOffset(column, row);
                    mapTile.textureKey = convertByteToTextureKey(allBytes[offset]);
                    if(mapTile.textureKey == TextureListMap.CLEAR1_SHP)
                    {
                        mapTile.imageIndex = CalculateImageIndexForClear1(column, row);
                    }
                    else
                    {
                        mapTile.imageIndex = allBytes[offset + 1];
                    }

                    mapTileList.Add(mapTile);
                }
            }

            InitializeBlockTerrainMap();

         }


        private void InitializeBlockTerrainMap()
        {
            blockingTerrainMap.Add(TextureListMap.S09_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S10_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S11_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S12_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S14_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S22_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S29_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S32_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S34_TEM, null);
            blockingTerrainMap.Add(TextureListMap.S35_TEM, null);
            blockingTerrainMap.Add(TextureListMap.SH1_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(TextureListMap.SH2_TEM, new int[] { 3, 4, 5, 6, 7, 8 });
            blockingTerrainMap.Add(TextureListMap.SH3_TEM, null);
            blockingTerrainMap.Add(TextureListMap.SH4_TEM, null);
            blockingTerrainMap.Add(TextureListMap.SH5_TEM, new int[] {  6, 7, 8 });
            blockingTerrainMap.Add(TextureListMap.SH6_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(TextureListMap.SH9_TEM, new int[] { 6 });
            blockingTerrainMap.Add(TextureListMap.SH10_TEM, new int[] {0,2,3 });
            blockingTerrainMap.Add(TextureListMap.SH17_TEM, null);
            blockingTerrainMap.Add(TextureListMap.SH18_TEM, null);
            blockingTerrainMap.Add(TextureListMap.W1_TEM, null);
            blockingTerrainMap.Add(TextureListMap.W2_TEM, null);
        }


        private byte CalculateImageIndexForClear1(int column, int row)
        {
            return (byte)((column % 4) + ((row % 4) * 4));
        }

        private void LoadCodeToTextureStringMap()
        {
            mapFileCodeToTextureStringMap.Add(0xff, TextureListMap.CLEAR1_SHP);

            mapFileCodeToTextureStringMap.Add(0x60, TextureListMap.D04_TEM);
            mapFileCodeToTextureStringMap.Add(0x69, TextureListMap.D13_TEM);
            mapFileCodeToTextureStringMap.Add(0x70, TextureListMap.D20_TEM);
            mapFileCodeToTextureStringMap.Add(0x71, TextureListMap.D21_TEM);
            mapFileCodeToTextureStringMap.Add(0x73, TextureListMap.D23_TEM);

            mapFileCodeToTextureStringMap.Add(0x49, TextureListMap.P07_TEM);
            mapFileCodeToTextureStringMap.Add(0x4A, TextureListMap.P08_TEM);

            mapFileCodeToTextureStringMap.Add(0x15, TextureListMap.S09_TEM);
            mapFileCodeToTextureStringMap.Add(0x16, TextureListMap.S10_TEM);
            mapFileCodeToTextureStringMap.Add(0x17, TextureListMap.S11_TEM);
            mapFileCodeToTextureStringMap.Add(0x18, TextureListMap.S12_TEM);
            mapFileCodeToTextureStringMap.Add(0x1a, TextureListMap.S14_TEM);
            mapFileCodeToTextureStringMap.Add(0x22, TextureListMap.S22_TEM);
            mapFileCodeToTextureStringMap.Add(0x29, TextureListMap.S29_TEM);


            mapFileCodeToTextureStringMap.Add(0x2c, TextureListMap.S32_TEM);
            mapFileCodeToTextureStringMap.Add(0x2e, TextureListMap.S34_TEM);
            mapFileCodeToTextureStringMap.Add(0x2f, TextureListMap.S35_TEM);


            mapFileCodeToTextureStringMap.Add(0x03, TextureListMap.SH1_TEM);
            mapFileCodeToTextureStringMap.Add(0x04, TextureListMap.SH2_TEM);
            mapFileCodeToTextureStringMap.Add(0x05, TextureListMap.SH3_TEM);

            mapFileCodeToTextureStringMap.Add(0x06, TextureListMap.SH4_TEM);

            mapFileCodeToTextureStringMap.Add(0x07, TextureListMap.SH5_TEM);
            mapFileCodeToTextureStringMap.Add(0x58, TextureListMap.SH6_TEM);
            mapFileCodeToTextureStringMap.Add(0x5b, TextureListMap.SH9_TEM);
            mapFileCodeToTextureStringMap.Add(0x5c, TextureListMap.SH10_TEM);
            mapFileCodeToTextureStringMap.Add(0x4c, TextureListMap.SH17_TEM);
            mapFileCodeToTextureStringMap.Add(0x4d, TextureListMap.SH18_TEM);

            mapFileCodeToTextureStringMap.Add(0x01, TextureListMap.W1_TEM);
            mapFileCodeToTextureStringMap.Add(0x02, TextureListMap.W2_TEM);
        }

        private string convertByteToTextureKey(byte inputByte)
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

        private int calculateOffset(int column, int row)
        {
            return (row * 64 * 2) + (column * 2);
        }




    }


}
