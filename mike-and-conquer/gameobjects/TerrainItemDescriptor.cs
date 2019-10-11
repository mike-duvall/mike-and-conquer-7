
using Point = Microsoft.Xna.Framework.Point;
using System;

namespace mike_and_conquer
{ 

    public class TerrainItemDescriptor
    {

        private String terrainItemType;
        private int width;
        private int height;


        public String TerrainItemType
        {
            get { return terrainItemType; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }


        protected TerrainItemDescriptor()
        {
        }


        public TerrainItemDescriptor(String terrainItemType, int width, int height)
        {
            this.terrainItemType = terrainItemType;
        }


    }


}
