
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
            int width = GameWorld.MAP_TILE_WIDTH;
            int height = GameWorld.MAP_TILE_HEIGHT;

            int leftX = (int)positionInWorldCoordinates.X - (width / 2);
            int topY = (int)positionInWorldCoordinates.Y - (height / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }


    }


}
