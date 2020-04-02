
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer
{ 

    public class GDIConstructionYard
    {

        public Vector2 positionInWorldCoordinates { get; set; }



        protected GDIConstructionYard()
        {
        }


        public GDIConstructionYard(int x, int y)
        {
            positionInWorldCoordinates = new Vector2(x, y);
        }



//        public bool ContainsPoint(Point aPoint)
//        {
//            int width = GameWorld.MAP_TILE_WIDTH;
//            int height = GameWorld.MAP_TILE_HEIGHT;
//
//            int leftX = (int)positionInWorldCoordinates.X - (width / 2);
//            int topY = (int)positionInWorldCoordinates.Y - (height / 2);
//            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
//            return boundRectangle.Contains(aPoint);
//        }


    }


}
