
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

        public Vector2 positionInWordlCoordinates { get; set; }


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
            positionInWordlCoordinates = new Vector2(x, y);
            this.sandbagType = sandbagType;
        }

        internal int GetMapSquareX()
        {
            int mapSquareX = (int)((this.positionInWordlCoordinates.X - 12) / 24);
            return mapSquareX;
        }

        internal int GetMapSquareY()
        {
            int mapSquareY = (int)((this.positionInWordlCoordinates.Y - 12) / 24);
            return mapSquareY;

        }


        //        public Vector2 GetScreenPosition()
        //        {
        //            return Vector2.Transform(positionInWordlCoordinates, MikeAndConquerGame.instance.camera2D.TransformMatrix);
        //        }


    }


}
