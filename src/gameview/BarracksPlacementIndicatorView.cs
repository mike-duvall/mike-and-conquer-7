﻿
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using mike_and_conquer.util;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BarracksPlacementIndicatorView
    {


        public SingleTextureSprite canPlaceBuildingSprite;
        private SingleTextureSprite canNotPlaceBuildingSprite;

        public Point position;
        public static string FILE_NAME = "trans.icn";
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        public BarracksPlacementIndicatorView()
        {
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
            Point placementSquarePosition = this.position;
            DrawPlacementSquareAtPosition(placementSquarePosition, gameTime, spriteBatch);
            placementSquarePosition.X = (placementSquarePosition.X + GameWorld.MAP_TILE_WIDTH);
            DrawPlacementSquareAtPosition(placementSquarePosition, gameTime, spriteBatch);
        }


        void DrawPlacementSquareAtPosition(Point position, GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameWorld.instance.IsValidBuildingPlacementLocation(position))
            {
                canPlaceBuildingSprite.Draw(
                    gameTime,
                    spriteBatch,
                    PointUtil.ConvertPointToVector2(position),
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    false,
                    Color.White);

            }
            else
            {
                canNotPlaceBuildingSprite.Draw(
                    gameTime,
                    spriteBatch,
                    PointUtil.ConvertPointToVector2(position),
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    false,
                    Color.White);

            }

        }




    }
}