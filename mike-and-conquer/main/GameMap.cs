

using System.Collections.Generic;

using File = System.IO.File;

using Stream= System.IO.Stream;


using BinaryReader = System.IO.BinaryReader;

namespace mike_and_conquer
{
    public class GameMap
    {

        private List<MapTile> mapTileList;


        public List<MapTile> MapTiles
        {
            get { return mapTileList; }
        }

        public GameMap()
        {
            int x = 3;
        }

        public GameMap(Stream inputStream, int startX, int startY, int endX, int endY)
         {
            //string fileName = "Content\\scg01ea.bin";
            //System.IO.FileStream tmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            //byte byte1 = (byte)tmpStream.ReadByte();
            //byte byte2 = (byte)tmpStream.ReadByte();
            ////shpBytes = stream.ReadBytes((int)(stream.Length - stream.Position));
            //byte[] bytes  = File.ReadAllBytes(fileName);
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

            for (int row = startY; row < endY; row++)
            {
                for (int column = startX; column <= endX; column++)
                {
                    MapTile mapTile = new MapTile();
                    int offset = calculateOffset(column, row);
                    //mapTile.byte1 = allBytes[offset];
                    //mapTile.byte2 = allBytes[offset + 1];
                    mapTile.textureKey = convertByteToTextureKey(allBytes[offset]);
                    mapTile.imageIndex = allBytes[offset + 1];

                    mapTileList.Add(mapTile);
                }
            }

        }


        //var map = new Dictionary<string, string>();

        //// ... Add some keys and values.
        //map.Add("cat", "orange");
        //map.Add("dog", "brown");


        private Dictionary<byte,string> mapFileCodeToTextureStringMap = new Dictionary<byte, string>();

        private void LoadCodeToTextureStringMap()
        {
            mapFileCodeToTextureStringMap.Add(0xff, TextureListMap.CLEAR1_SHP);
            mapFileCodeToTextureStringMap.Add(0x18, TextureListMap.S12_TEM);
        }


        private string convertByteToTextureKey(byte inputByte)
        {
            string textureKey;
            mapFileCodeToTextureStringMap.TryGetValue(inputByte, out textureKey);
            return textureKey;

            // TODO: Change to this once we get all tile types registered
            //return mapFileCodeToTextureStringMap[inputByte];
        }

        private int calculateOffset(int column, int row)
        {
            return (row * 64 * 2) + (column * 2);
        }


        //Write unit tests
        //    * Use stream instead of file, for easier testing
        //    * Given a stream of bytes, total map size(width and height in tiles), and sub map start tile and end tile
        //         * Parse that stream of bytes into the sub map (as MapTiles)
        //        * Hav the test spot check sampling of Map Tiles
        //        * Start with very small map, then progress to actual data in map 1 in the game



    }


}
