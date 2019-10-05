using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gamesprite
{
    public class ShadowMapper
    {

        private Dictionary<int, int> unitsShadowMap;
        private Dictionary<int, int> sidebarBuildShadowMap;

        public ShadowMapper()
        {
            LoadUnitsShadowMap();
            LoadSidebarMap();
        }

        private void LoadUnitsShadowMap()
        {
            unitsShadowMap = new Dictionary<int, int>();
            System.IO.FileStream mrfFileStream = System.IO.File.Open("Content\\tunits.mrf", System.IO.FileMode.Open,
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
            System.IO.FileStream mrfFileStream = System.IO.File.Open("Content\\tclock.mrf", System.IO.FileMode.Open,
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

    }
}
