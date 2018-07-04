

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
            mapTileList = new List<MapTile>();

            BinaryReader binaryReader = new BinaryReader(inputStream);
            long numBytes = binaryReader.BaseStream.Length;
            List<byte> allBytes = new List<byte>();
            for(int i = 0; i < numBytes; i++)
            {
                byte nextByte = binaryReader.ReadByte();
                allBytes.Add(nextByte);
            }

            int row = startY;
            int column = startX;

            MapTile mapTile = new MapTile();
            int offset = calculateOffset(column, row);
            mapTile.byte1 = allBytes[offset];
            mapTile.byte2 = allBytes[offset + 1];
            mapTileList.Add(mapTile);

            column++;
            mapTile = new MapTile();
            offset = calculateOffset(column, row);
            mapTile.byte1 = allBytes[offset];
            mapTile.byte2 = allBytes[offset + 1];
            mapTileList.Add(mapTile);

            column++;
            mapTile = new MapTile();
            offset = calculateOffset(column, row);
            mapTile.byte1 = allBytes[offset];
            mapTile.byte2 = allBytes[offset + 1];
            mapTileList.Add(mapTile);

            column++;
            mapTile = new MapTile();
            offset = calculateOffset(column, row);
            mapTile.byte1 = allBytes[offset];
            mapTile.byte2 = allBytes[offset + 1];
            mapTileList.Add(mapTile);

            column++;
            mapTile = new MapTile();
            offset = calculateOffset(column, row);
            mapTile.byte1 = allBytes[offset];
            mapTile.byte2 = allBytes[offset + 1];
            mapTileList.Add(mapTile);




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
