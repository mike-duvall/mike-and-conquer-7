using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;

namespace mike_and_conquer.gameobjects
{
    public class BuildingPlacementIndicatorTile
    {


        private int relativeX;
        private int relativeY;

        private GameLocation gameLocation;
        private bool canPlaceBulding;

        private GameLocation baseGameLocation;


        public bool CanPlaceBuilding
        {
            get { return canPlaceBulding; }
            set { canPlaceBulding = value; }
        }

        public GameLocation GameLocation
        {
            get
            {
//                int x = baseGameLocation.X + (relativeX * GameWorld.MAP_TILE_WIDTH);
//                int y = baseGameLocation.Y + (relativeY * GameWorld.MAP_TILE_HEIGHT);
//                return GameLocation.CreateGameLocationInWorldCoordinates(x, y);
                return gameLocation;
            }
        }

        public BuildingPlacementIndicatorTile(GameLocation baseGameLocation, int x, int y)
        {
//            this.baseGameLocation = baseGameLocation;
            this.relativeX = x;
            this.relativeY = y;
            this.UpdateLocation(baseGameLocation);
        }


        public void UpdateLocation(GameLocation newGameLocation)
        {
            this.baseGameLocation = newGameLocation;
            int x = baseGameLocation.X + (relativeX * GameWorld.MAP_TILE_WIDTH);
            int y = baseGameLocation.Y + (relativeY * GameWorld.MAP_TILE_HEIGHT);
            this.gameLocation = GameLocation.CreateGameLocationInWorldCoordinates(x, y);
        }


    }
}
