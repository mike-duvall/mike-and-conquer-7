using System;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.gameobjects
{ 

    public class GDIConstructionYard
    {

        public Vector2 positionInWorldCoordinates { get; set; }


        private Boolean isBuildingBarracks;
        private int barracksBuildCountdown;
        private static int barracksBuildCountdownMax = 400;
        private Boolean isBarracksReadyToPlace;

        public Boolean IsBuildingBarracks
        {
            get { return isBuildingBarracks; }
        }

        public int PercentBarracksBuildComplete
        {
            get { return CalculatePercentBarracksBuildComplete(); }
        }

        public Boolean IsBarracksReadyToPlace
        {
            get { return isBarracksReadyToPlace; }
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



        public void StartBuildingBarracks()
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
                    isBarracksReadyToPlace = true;
                    isBuildingBarracks = false;
                }
            }
        }





        public void CreateBarracksFromConstructionYard(int x, int y)
        {

            MapTileInstance mapTileInstance = GameWorld.instance.FindMapTileInstance(x, y);

            int barracksX = (int) mapTileInstance.PositionInWorldCoordinates.X + 12;
            int barracksY = (int)mapTileInstance.PositionInWorldCoordinates.Y + 12;

            Point barracksPosition = new Point(barracksX, barracksY);
            MikeAndConquerGame.instance.AddGDIBarracksAtWorldCoordinates(barracksPosition);
            isBarracksReadyToPlace = false;
            isBuildingBarracks = false;
        }


    }


}
