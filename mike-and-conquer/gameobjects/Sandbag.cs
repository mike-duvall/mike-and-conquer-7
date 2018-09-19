
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer
{ 

    public class Sandbag
    {

        public Vector2 position { get; set; }


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
            position = new Vector2(x, y);
            this.sandbagType = sandbagType;
        }


        public Vector2 GetScreenPosition()
        {
            return Vector2.Transform(position, MikeAndConquerGame.instance.camera2D.TransformMatrix);
        }


    }


}
