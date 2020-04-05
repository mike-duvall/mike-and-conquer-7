
using System;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer
{ 

    public class GDIConstructionYard
    {

        public Vector2 positionInWorldCoordinates { get; set; }


        private Boolean isBuildingBarracks;
        private int barracksBuildCountdown;
        private static int barracksBuildCountdownMax = 400;

        public Boolean IsBuildingBarracks
        {
            get { return isBuildingBarracks; }
        }

        public int PercentBarracksBuildComplete
        {
            get { return CalculatePercentBarracksBuildComplete(); }
        }


        private int CalculatePercentBarracksBuildComplete()
        {
            if (isBuildingBarracks)
            {
                return 100 - ((barracksBuildCountdown * 100) / barracksBuildCountdownMax);
            }
            else
            {
                return 100;
            }
        }




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



        public void StartBuildingMinigunner()
        {

            isBuildingBarracks = true;
            barracksBuildCountdown = barracksBuildCountdownMax;
        }

        public void Update(GameTime gameTime)
        {
            if (isBuildingBarracks)
            {
                barracksBuildCountdown--;
                if (barracksBuildCountdown <= 0)
                {
                    CreateBarracksFromConstructionYard();
                    isBuildingBarracks = false;
                }
            }
        }

        // private void CreateMinigunnerFromBarracks()
        // {
        //     int minigunnerX = (int)positionInWorldCoordinates.X;
        //     int minigunnerY = (int)(positionInWorldCoordinates.Y);
        //
        //     Point gdiMinigunnderPosition = new Point(minigunnerX, minigunnerY);
        //     Minigunner builtMinigunner = MikeAndConquerGame.instance.AddGdiMinigunner(gdiMinigunnderPosition);
        //
        //     Point destinationInWC = new Point(gdiMinigunnderPosition.X, gdiMinigunnderPosition.Y + 40);
        //     builtMinigunner.OrderToMoveToDestination(destinationInWC);
        //
        // }

        private void CreateBarracksFromConstructionYard()
        {
            int barracksX = (int)positionInWorldCoordinates.X + 60;
            int barracksY = (int)(positionInWorldCoordinates.Y);

            Point barracksPosition = new Point(barracksX, barracksY);
            MikeAndConquerGame.instance.AddGDIBarracksAtWorldCoordinates(barracksPosition);

        }


    }


}
