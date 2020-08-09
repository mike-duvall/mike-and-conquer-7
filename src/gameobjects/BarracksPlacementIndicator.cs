using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;
using SharpDX.X3DAudio;

namespace mike_and_conquer.gameobjects
{
    public class BarracksPlacementIndicator
    {

        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get {  return mapTileLocation; }
        }

        private List<BuildingPlacementIndicatorTile> buildingBuildingPlacementIndicatorTiles;

        public List<BuildingPlacementIndicatorTile> BuildingBuildingPlacementIndicatorTiles
        {
            get { return buildingBuildingPlacementIndicatorTiles; }
        }

        public BarracksPlacementIndicator(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
            this.buildingBuildingPlacementIndicatorTiles = new List<BuildingPlacementIndicatorTile>();
            AddTileAtRelativeLocation(0, 0);
            AddTileAtRelativeLocation(1, 0);

            AddTileAtRelativeLocation(0, 1);
            AddTileAtRelativeLocation(1, 1);

            AddTileAtRelativeLocation(0, 2);
            AddTileAtRelativeLocation(1, 2);

        }

        private void AddTileAtRelativeLocation(int x, int y)
        {
            BuildingPlacementIndicatorTile newTile = new BuildingPlacementIndicatorTile(this.MapTileLocation, x, y);
            buildingBuildingPlacementIndicatorTiles.Add(newTile);
        }



        public void UpdateLocationInWorldCoordinates(Point mouseLocationWordCoordinates)
        {
            mapTileLocation.UpdateLocationInWorldCoordinates(mouseLocationWordCoordinates);
            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                tile.UpdateLocation(mapTileLocation);
                tile.CanPlaceBuilding = false;
            }

            bool isAnyTileTouchingExistingBase = false;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                if (GameWorld.instance.IsPointAdjacentToConstructionYardAndClearForBuilding(tile.MapTileLocation.WorldCoordinatesAsPoint))
                {
                    isAnyTileTouchingExistingBase = true;
                }
            }

            if (isAnyTileTouchingExistingBase)
            {
                foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
                {

                    if (GameWorld.instance.IsValidMoveDestination(tile.MapTileLocation.WorldCoordinatesAsPoint))
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
