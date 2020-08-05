using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;

namespace mike_and_conquer.gameobjects
{
    public class BuildingPlacementIndicatorTile
    {


        private int relativeX;
        private int relativeY;

        private MapTileLocation mapTileLocation;
        private bool canPlaceBulding;

        private MapTileLocation baseMapTileLocation;


        public bool CanPlaceBuilding
        {
            get { return canPlaceBulding; }
            set { canPlaceBulding = value; }
        }

        public MapTileLocation MapTileLocation
        {
            get
            {
                return mapTileLocation;
            }
        }

        public BuildingPlacementIndicatorTile(MapTileLocation baseMapTileLocation, int x, int y)
        {
            this.relativeX = x;
            this.relativeY = y;
            this.UpdateLocation(baseMapTileLocation);
        }


        public void UpdateLocation(MapTileLocation newMapTileLocation)
        {
            this.baseMapTileLocation = newMapTileLocation;
//            int x = baseMapTileLocation.X + (relativeX * GameWorld.MAP_TILE_WIDTH);
//            int y = baseMapTileLocation.Y + (relativeY * GameWorld.MAP_TILE_HEIGHT);
//            this.mapTileLocation = MapTileLocation.CreateFromWorldMapTileCoordinates(x, y);

            int x = baseMapTileLocation.XinWorldMapTileCoordinates + relativeX;
            int y = baseMapTileLocation.YinWorldMapTileCoordinates + relativeY;

            this.mapTileLocation = MapTileLocation.CreateFromWorldMapTileCoordinates(x, y);

            //            this.mapTileLocation.XinWorldMapTileCoordinates = x;
            //            this.mapTileLocation.YinWorldMapTileCoordinates = y;

        }


    }
}
