using System;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.gameobjects
{ 

    public class GDIConstructionYard
    {


        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }


        private Boolean isBuildingBarracks;
        // private int barracksBuildCountdown;
        // private static int barracksBuildCountdownMax = 400;

        private float buildBarracksPercentComplete;
        private Boolean isBarracksReadyToPlace;

        private float scaledBuildSpeed;
        private float baseBuildSpeed = 0.65f;

        public Boolean IsBuildingBarracks
        {
            get { return isBuildingBarracks; }
        }

        // public int PercentBarracksBuildComplete
        // {
        //     get { return CalculatePercentBarracksBuildComplete(); }
        // }

        public int PercentBarracksBuildComplete
        {
            get { return (int) buildBarracksPercentComplete; }
        }


        public Boolean IsBarracksReadyToPlace
        {
            get { return isBarracksReadyToPlace; }
        }

        // private int CalculatePercentBarracksBuildComplete()
        // {
        //     if (isBuildingBarracks)
        //     {
        //         return 100 - ((barracksBuildCountdown * 100) / barracksBuildCountdownMax);
        //     }
        //     else
        //     {
        //         return 100;
        //     }
        // }

        protected GDIConstructionYard()
        {
        }

        public GDIConstructionYard(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
        }

        public bool ContainsPoint(Point aPoint)
        {
            int width = 72;
            int height = 48;

            int leftX = (int)mapTileLocation.WorldCoordinatesAsVector2.X - (width / 2);
            int topY = (int)mapTileLocation.WorldCoordinatesAsVector2.Y - (height / 2);

            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }



        public void StartBuildingBarracks()
        {

            isBuildingBarracks = true;
            buildBarracksPercentComplete = 0.0f;
            // barracksBuildCountdown = barracksBuildCountdownMax;
        }

        public void Update(GameTime gameTime)
        {
            scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;

            if (isBuildingBarracks)
            {
                // barracksBuildCountdown--;
                // if (barracksBuildCountdown <= 0)
                // {
                //     isBarracksReadyToPlace = true;
                //     isBuildingBarracks = false;
                // }
                double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;


                buildBarracksPercentComplete += (float) delta;
                if (buildBarracksPercentComplete >= 100.0f)
                {
                    isBarracksReadyToPlace = true;
                    isBuildingBarracks = false;
                }
            }
        }


        public void CreateBarracksAtPosition(MapTileLocation mapTileLocation)
        {

            MikeAndConquerGame.instance.AddGDIBarracks(mapTileLocation);
            isBarracksReadyToPlace = false;
            isBuildingBarracks = false;
        }


    }


}
