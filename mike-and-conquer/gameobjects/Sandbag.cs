
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;
using System;

namespace mike_and_conquer
{ 

    public class Sandbag
    {

        public Vector2 positionInWorldCoordinates { get; set; }


        private int sandbagType;

        public int SandbagType
        {
            get { return sandbagType; }
        }



        protected Sandbag()
        {
        }


        public Sandbag(int x, int y, int sandbagType)
        {
            positionInWorldCoordinates = new Vector2(x, y);
            this.sandbagType = sandbagType;
        }


        public bool ContainsPoint(Point aPoint)
        {
            int leftX = (int)positionInWorldCoordinates.X - (GameWorld.MAP_TILE_WIDTH / 2);
            int topY = (int)positionInWorldCoordinates.Y - (GameWorld.MAP_TILE_HEIGHT / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, GameWorld.MAP_TILE_WIDTH, GameWorld.MAP_TILE_HEIGHT);
            return boundRectangle.Contains(aPoint);
        }


    }


}
