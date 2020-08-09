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

            int x = baseMapTileLocation.WorldMapTileCoordinatesAsPoint.X  + relativeX;
            int y = baseMapTileLocation.WorldMapTileCoordinatesAsPoint.Y + relativeY;

            this.mapTileLocation = MapTileLocation.CreateFromWorldMapTileCoordinates(x, y);


        }


    }
}
