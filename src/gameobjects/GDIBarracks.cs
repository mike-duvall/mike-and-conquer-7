﻿
using System;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameobjects
{ 

    public class GDIBarracks
    {


        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private Boolean isBuildingMinigunner;
        private int minigunnerBuildCountdown;
        private static int minigunnerBuildCountdownMax = 400;

        public Boolean IsBuildingMinigunner
        {
            get { return isBuildingMinigunner; }
        }
        
        public int PercentMinigunnerBuildComplete
        {
            get { return CalculatePercentMinigunnerBuildComplete(); }
        }


        private int CalculatePercentMinigunnerBuildComplete()
        {
            if (isBuildingMinigunner)
            {
                return 100 - ((minigunnerBuildCountdown * 100) / minigunnerBuildCountdownMax);
            }
            else
            {
                return 100;
            }
        }

        protected GDIBarracks()
        {
        }



        public GDIBarracks(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
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
            minigunnerBuildCountdown = minigunnerBuildCountdownMax;
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
            Point gdiMinigunnderPosition = mapTileLocation.WorldCoordinatesAsPoint;
            Minigunner builtMinigunner = MikeAndConquerGame.instance.AddGdiMinigunner(gdiMinigunnderPosition);

            Point destinationInWC = new Point(gdiMinigunnderPosition.X, gdiMinigunnderPosition.Y + 40);
            builtMinigunner.OrderToMoveToDestination(destinationInWC);

        }
    }


}
