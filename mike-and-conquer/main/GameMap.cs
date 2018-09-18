

using System.Collections.Generic;

using Stream = System.IO.Stream;

using BinaryReader = System.IO.BinaryReader;

namespace mike_and_conquer
{
    public class GameMap
    {

        private List<MapTile> mapTileList;
        private Dictionary<byte, string> mapFileCodeToTextureStringMap = new Dictionary<byte, string>();


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


            int numColumns = endX - startX + 1;
            int numRows = endY - startY + 1;

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
