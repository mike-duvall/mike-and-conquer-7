
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;
using System;

namespace mike_and_conquer
{ 

    public class TerrainItem
    {

        private Vector2 positionInWorldCoordinates;

        public Vector2 PositionInWorldCoordinates
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
            positionInWorldCoordinates = new Vector2(x, y);
            this.terrainItemType = terrainItemType;
        }


    }


}
