
using Point = Microsoft.Xna.Framework.Point;
using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;

namespace mike_and_conquer
{ 

    public class TerrainItem
    {

        private Point positionInWorldCoordinates;

        public Point PositionInWorldCoordinates
        {
            get { return positionInWorldCoordinates; }
        }

//        private String terrainItemType;

        public String TerrainItemType
        {
            get { return terrainItemDescriptor.TerrainItemType; }
        }

        private TerrainItemDescriptor terrainItemDescriptor;



        protected TerrainItem()
        {
        }


        //        public TerrainItem(int x, int y, String terrainItemType)
        //        {
        //            positionInWorldCoordinates = new Point(x, y);
        //            this.terrainItemType = terrainItemType;
        //        }

        public TerrainItem(int x, int y, TerrainItemDescriptor terrainItemDescriptor)
        {
            positionInWorldCoordinates = new Point(x, y);
            this.terrainItemDescriptor = terrainItemDescriptor;
        }


        public List<MapTileInstance> GetBlockedMapTileInstances()
        {
            List<MapTileInstance> blockMapTileInstances = new List<MapTileInstance>();
            List<Point> blockMapTileRelativeCoordinates = terrainItemDescriptor.GetBlockMapTileRelativeCoordinates();


            foreach (Point point in blockMapTileRelativeCoordinates)
            {
                int mapTileX = positionInWorldCoordinates.X + (point.X * GameWorld.MAP_TILE_WIDTH) + 10;
                int mapTileY = positionInWorldCoordinates.Y + (point.Y * GameWorld.MAP_TILE_HEIGHT) + 10;
                MapTileInstance blockedMapTile = MikeAndConquerGame.instance.gameWorld.FindMapSquare(mapTileX, mapTileY);
                blockMapTileInstances.Add(blockedMapTile);
            }

            return blockMapTileInstances;

        }


        //        public List<MapTileInstance> GetBlockedMapTileInstances()
        //        {
        //
        //            //            LoadTerrainTexture("Content\\T01.tem");
        //            //            LoadTerrainTexture("Content\\T02.tem");
        //            //            LoadTerrainTexture("Content\\T05.tem");
        //            //            LoadTerrainTexture("Content\\T06.tem");
        //            //            LoadTerrainTexture("Content\\T07.tem");
        //            //            LoadTerrainTexture("Content\\T16.tem");
        //            //            LoadTerrainTexture("Content\\T17.tem");
        //            //            LoadTerrainTexture("Content\\TC01.tem");
        //            //            LoadTerrainTexture("Content\\TC02.tem");
        //            //            LoadTerrainTexture("Content\\TC04.tem");
        //            //            LoadTerrainTexture("Content\\TC05.tem");
        //
        //            List<MapTileInstance> blockMapTileInstances = new List<MapTileInstance>();
        //
        //            List<String> smallItems = new List<String>();
        //            smallItems.Add("Content\\T01.tem");
        //            smallItems.Add("Content\\T02.tem");
        //            smallItems.Add("Content\\T05.tem");
        //            smallItems.Add("Content\\T06.tem");
        //            smallItems.Add("Content\\T07.tem");
        //            smallItems.Add("Content\\T16.tem");
        //            smallItems.Add("Content\\T17.tem");
        //
        //            List<String> mediumItems = new List<string>();
        //            mediumItems.Add("Content\\TC01.tem");
        //            mediumItems.Add("Content\\TC02.tem");
        //
        //
        //
        //
        //
        //            if (smallItems.Contains(terrainItemType))
        //            {
        //
        //                MapTileInstance blockInstance =  MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                blockMapTileInstances.Add(blockInstance);
        //            }
        //
        //            if (mediumItems.Contains(terrainItemType))
        //            {
        //
        //                MapTileInstance blocked1 =  MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 34);
        //                MapTileInstance blocked2 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 30,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                blockMapTileInstances.Add(blocked1);
        //                blockMapTileInstances.Add(blocked2);
        //
        //            }
        //
        //            if (terrainItemType == "Content\\TC04.tem")
        //            {
        //                MapTileInstance blocked1 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 10);
        //
        //                MapTileInstance blocked2 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                MapTileInstance blocked3 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 58);
        //
        //                MapTileInstance blocked4 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 34,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                MapTileInstance blocked5 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 58,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                blockMapTileInstances.Add(blocked1);
        //                blockMapTileInstances.Add(blocked2);
        //                blockMapTileInstances.Add(blocked3);
        //                blockMapTileInstances.Add(blocked4);
        //                blockMapTileInstances.Add(blocked5);
        //
        //            }
        //
        //            if (terrainItemType == "Content\\TC05.tem")
        //            {
        //                MapTileInstance blocked1 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 10,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                MapTileInstance blocked2 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 34,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                MapTileInstance blocked3 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 58,
        //                    positionInWorldCoordinates.Y + 34);
        //
        //                MapTileInstance blocked4 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 34,
        //                    positionInWorldCoordinates.Y + 58);
        //
        //                MapTileInstance blocked5 = MikeAndConquerGame.instance.gameWorld.FindMapSquare(positionInWorldCoordinates.X + 58,
        //                    positionInWorldCoordinates.Y + 58);
        //
        //                blockMapTileInstances.Add(blocked1);
        //                blockMapTileInstances.Add(blocked2);
        //                blockMapTileInstances.Add(blocked3);
        //                blockMapTileInstances.Add(blocked4);
        //                blockMapTileInstances.Add(blocked5);
        //
        //            }
        //
        //
        //
        //            return blockMapTileInstances;
        //        }
    }


}
