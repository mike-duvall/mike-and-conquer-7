
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using mike_and_conquer.util;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BarracksPlacementIndicatorView
    {

        private BuildingPlacementIndicator buildingPlacementIndicator;

        public SingleTextureSprite canPlaceBuildingSprite;
        private SingleTextureSprite canNotPlaceBuildingSprite;

        public static string FILE_NAME = "trans.icn";
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        public BarracksPlacementIndicatorView(BuildingPlacementIndicator buildingPlacementIndicator)
        {
//            this.position = buildingPlacementIndicator.GameLocation.ToPoint();

            this.buildingPlacementIndicator = buildingPlacementIndicator;

            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(FILE_NAME);

            // 0 == white
            // 1 == yellow
            // 2 == red
            this.canPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[0].Texture);
            this.canNotPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[2].Texture);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (BuildingPlacementIndicatorTile tile in  buildingPlacementIndicator.BuildingBuildingPlacementIndicatorTiles)
            {
                if (tile.CanPlaceBuilding)
                {
                    canPlaceBuildingSprite.Draw(
                        gameTime,
                        spriteBatch,
                        PointUtil.ConvertPointToVector2(tile.GameLocation.ToPoint()),
                        SpriteSortLayers.MAP_SQUARE_DEPTH,
                        false,
                        Color.White);

                }
                else
                {
                    canNotPlaceBuildingSprite.Draw(
                        gameTime,
                        spriteBatch,
                        PointUtil.ConvertPointToVector2(tile.GameLocation.ToPoint()),
                        SpriteSortLayers.MAP_SQUARE_DEPTH,
                        false,
                        Color.White);

                }

            }
        }


    }
}