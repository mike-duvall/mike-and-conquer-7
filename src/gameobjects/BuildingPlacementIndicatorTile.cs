using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;

namespace mike_and_conquer.gameobjects
{
    public class BuildingPlacementIndicatorTile
    {


        private int relativeX;
        private int relativeY;

        private GameLocation baseGameLocation;

        public GameLocation GameLocation
        {
            get
            {
                int x = baseGameLocation.X + (relativeX * GameWorld.MAP_TILE_WIDTH);
                int y = baseGameLocation.Y + (relativeY * GameWorld.MAP_TILE_HEIGHT);
                return GameLocation.CreateGameLocationInWorldCoordinates(x, y);
            }
        }

        public BuildingPlacementIndicatorTile(GameLocation baseGameLocation, int x, int y)
        {
            this.baseGameLocation = baseGameLocation;
            this.relativeX = x;
            this.relativeY = y;
        }


    }
}
