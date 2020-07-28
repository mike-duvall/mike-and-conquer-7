using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;
using SharpDX.X3DAudio;

namespace mike_and_conquer.gameobjects
{
    public class BuildingPlacementIndicator
    {

        private GameLocation gameLocation;

        public GameLocation GameLocation
        {
            get {  return gameLocation; }
        }

        private List<BuildingPlacementIndicatorTile> buildingBuildingPlacementIndicatorTiles;

        public List<BuildingPlacementIndicatorTile> BuildingBuildingPlacementIndicatorTiles
        {
            get { return buildingBuildingPlacementIndicatorTiles; }
        }

        public BuildingPlacementIndicator(GameLocation gameLocation)
        {
            this.gameLocation = gameLocation;
            this.buildingBuildingPlacementIndicatorTiles = new List<BuildingPlacementIndicatorTile>();
        }

        public void AddTileAtRelativeLocation(int x, int y)
        {
            BuildingPlacementIndicatorTile newTile = new BuildingPlacementIndicatorTile(this.GameLocation, x, y);
            buildingBuildingPlacementIndicatorTiles.Add(newTile);
        }

        public void UpdateLocation(int x, int y)
        {
            gameLocation.X = x;
            gameLocation.Y = y;
            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                tile.UpdateLocation(gameLocation);
                tile.CanPlaceBuilding = false;
            }

            bool isAnyTileTouchingExistingBase = false;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                if (GameWorld.instance.IsPointAdjacentToConstructionYard(tile.GameLocation.ToPoint()))
                {
                    isAnyTileTouchingExistingBase = true;
                }
            }

            if (isAnyTileTouchingExistingBase)
            {
                foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
                {

                    if (GameWorld.instance.IsValidMoveDestination(tile.GameLocation.ToPoint()))
                    {
                        tile.CanPlaceBuilding = true;
                    }
                }
            }
        }

        public bool ValidBuildingLocation()
        {
            bool isValidBuildingLocation = true;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                if (!tile.CanPlaceBuilding)
                {
                    isValidBuildingLocation = false;
                }
            }

            return isValidBuildingLocation;
        }
    }
}
