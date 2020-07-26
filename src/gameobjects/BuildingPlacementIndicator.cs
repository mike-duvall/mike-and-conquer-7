using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;

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
    }
}
