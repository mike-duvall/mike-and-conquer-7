
using System.Collections.Generic;

namespace mike_and_conquer.gamesprite
{
    public class ShadowMapper
    {

        private Dictionary<int, int> unitsShadowMap;
        private Dictionary<int, int> sidebarBuildShadowMap;


        private Dictionary<int, int> mapTileShadowMap13;
        private Dictionary<int, int> mapTileShadowMap14;
        private Dictionary<int, int> mapTileShadowMap15;
        private Dictionary<int, int> mapTileShadowMap16;

        public ShadowMapper()
        {
            LoadUnitsShadowMap();
            LoadSidebarMap();
            LoadMapTileShadowMaps();
        }


        private void LoadMapTileShadowMaps()
        {
            System.IO.FileStream mrfFileStream = System.IO.File.Open("Content\\tshadow.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();


            mapTileShadowMap13 = new Dictionary<int, int>();
            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap13.Add(i - 256, byte1);
            }

            mapTileShadowMap14 = new Dictionary<int, int>();
            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap14.Add(i - 256, byte1);
            }
            mapTileShadowMap15 = new Dictionary<int, int>();
            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap15.Add(i - 256, byte1);
            }

            mapTileShadowMap16 = new Dictionary<int, int>();
            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap16.Add(i - 256, byte1);
            }



            mrfFileStream.Close();
        }

        private void LoadUnitsShadowMap()
        {
            unitsShadowMap = new Dictionary<int, int>();
            System.IO.FileStream mrfFileStream = System.IO.File.Open(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "tunits.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();

            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                unitsShadowMap.Add(i, byte1);
            }

            mrfFileStream.Close();
        }
         

        private void LoadSidebarMap()
        {
            sidebarBuildShadowMap = new Dictionary<int, int>();
            System.IO.FileStream mrfFileStream = System.IO.File.Open(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "tclock.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();

            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                sidebarBuildShadowMap.Add(i - 256, byte1);
            }

            mrfFileStream.Close();
        }


        internal int MapUnitsShadowPaletteIndex(int paletteIndex)
        {
            int mappedValue;
            unitsShadowMap.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }


        internal int MapSidebarBuildPaletteIndex(int paletteIndex)
        {
            int mappedValue;
            sidebarBuildShadowMap.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile13PaletteIndex(int paletteIndex)
        {
            // 13 is darkest shadow
            // mapTileShadowMap16 works, dark
            // mapTileShadowMap15 works, dark
            // mapTileShadowMap14 does NOT work
            // mapTileShadowMap13 does NOT work
            int mappedValue;
            mapTileShadowMap16.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile14PaletteIndex(int paletteIndex)
        {
            // 14 is the next darkest shadow
            // mapTileShadowMap16 does NOT work, dark
            // mapTileShadowMap15 does NOT work, dark
            // mapTileShadowMap14 might work
            // mapTileShadowMap13 might work

            int mappedValue;
            mapTileShadowMap15.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile15PaletteIndex(int paletteIndex)
        {
            // 15 is the next darkest shadow
            // mapTileShadowMap16 does NOT work, dark
            // mapTileShadowMap15 does NOT work, dark
            // mapTileShadowMap14 might work
            // mapTileShadowMap13 might work but lighter

            int mappedValue;
            mapTileShadowMap14.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile16PaletteIndex(int paletteIndex)
        {

            // 16 is the next darkest shadow
            // mapTileShadowMap16 
            // mapTileShadowMap15 
            // mapTileShadowMap14 
            // mapTileShadowMap13 


            int mappedValue;
            mapTileShadowMap13.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }


    }
}
