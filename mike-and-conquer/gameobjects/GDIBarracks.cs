
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;
using System;

namespace mike_and_conquer
{ 

    public class GDIBarracks
    {

        public Vector2 positionInWorldCoordinates { get; set; }

        private Boolean isBuildingMinigunner;
        private int minigunnerBuildCountdown;

        protected GDIBarracks()
        {
        }


        public GDIBarracks(int x, int y)
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


        public void StartBuildingMinigunner()
        {

            isBuildingMinigunner = true;
            minigunnerBuildCountdown = 200;
        }

        public void Update(GameTime gameTime)
        {
            if (isBuildingMinigunner)
            {
                minigunnerBuildCountdown--;
                if (minigunnerBuildCountdown <= 0)
                {
                    CreateMinigunnerFromBarracks();
                    isBuildingMinigunner = false;
                }
            }
        }

        private void CreateMinigunnerFromBarracks()
        {
            int minigunnerX = (int) positionInWorldCoordinates.X;
            int minigunnerY = (int) (positionInWorldCoordinates.Y);

            Point gdiMinigunnderPosition = new Point(minigunnerX, minigunnerY);
            Minigunner builtMinigunner = MikeAndConquerGame.instance.AddGdiMinigunner(gdiMinigunnderPosition);

            Point destinationInWC = new Point(gdiMinigunnderPosition.X, gdiMinigunnderPosition.Y + 40);
            builtMinigunner.OrderToMoveToDestination(destinationInWC);

        }
    }


}
