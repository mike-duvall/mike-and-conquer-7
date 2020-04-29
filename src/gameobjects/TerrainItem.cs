
using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameobjects
{ 

    public class TerrainItem
    {

        private Point positionInWorldCoordinates;

        public Point PositionInWorldCoordinates
        {
            get { return positionInWorldCoordinates; }
        }

        public String TerrainItemType
        {
            get { return terrainItemDescriptor.TerrainItemType; }
        }

        private TerrainItemDescriptor terrainItemDescriptor;

        private float layerDepthOffset;

        public float LayerDepthOffset
        {
            get { return layerDepthOffset; }
        }


        protected TerrainItem()
        {
        }


        public TerrainItem(int x, int y, TerrainItemDescriptor terrainItemDescriptor, float layerDepthOffset)
        {
            positionInWorldCoordinates = new Point(x, y);
            this.terrainItemDescriptor = terrainItemDescriptor;
            this.layerDepthOffset = layerDepthOffset;
        }


        public List<MapTileInstance> GetBlockedMapTileInstances()
        {
            List<MapTileInstance> blockMapTileInstances = new List<MapTileInstance>();
            List<Point> blockMapTileRelativeCoordinates = terrainItemDescriptor.GetBlockMapTileRelativeCoordinates();


            foreach (Point point in blockMapTileRelativeCoordinates)
            {
                int mapTileX = positionInWorldCoordinates.X + (point.X * GameWorld.MAP_TILE_WIDTH) + 10;
                int mapTileY = positionInWorldCoordinates.Y + (point.Y * GameWorld.MAP_TILE_HEIGHT) + 10;
                MapTileInstance blockedMapTile = MikeAndConquerGame.instance.gameWorld.FindMapTileInstance(mapTileX, mapTileY);
                blockMapTileInstances.Add(blockedMapTile);
            }

            return blockMapTileInstances;

        }

    }


}
