using System;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.gameobjects
{ 

    public class GDIConstructionYard
    {

//        public Vector2 positionInWorldCoordinates { get; set; }

        private GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }


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
//            positionInWorldCoordinates = new Vector2(x, y);
            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(x, y);
        }



        public bool ContainsPoint(Point aPoint)
        {
            int width = 72;
            int height = 48;

            int leftX = (int)gameWorldLocation.WorldCoordinatesAsVector2.X - (width / 2);
            int topY = (int)gameWorldLocation.WorldCoordinatesAsVector2.Y - (height / 2);

            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }



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


        public void CreateBarracksAtPosition(MapTileLocation mapTileLocation)
        {

            MikeAndConquerGame.instance.AddGDIBarracksAtWorldCoordinates(mapTileLocation);
            isBarracksReadyToPlace = false;
            isBuildingBarracks = false;
        }


    }


}
