
using Point = Microsoft.Xna.Framework.Point;
using System;

namespace mike_and_conquer
{ 

    public class TerrainItem
    {

        private Point positionInWorldCoordinates;

        public Point PositionInWorldCoordinates
        {
            get { return positionInWorldCoordinates; }
        }

        private String terrainItemType;

        public String TerrainItemType
        {
            get { return terrainItemType; }
        }



        protected TerrainItem()
        {
        }


        public TerrainItem(int x, int y, String terrainItemType)
        {
            positionInWorldCoordinates = new Point(x, y);
            this.terrainItemType = terrainItemType;
        }


    }


}
