
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
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

            Vector2 position = new Vector2(this.position.X, this.position.Y);

            if (GameWorld.instance.IsValidMoveDestination(this.position))
            {
                canPlaceBuildingSprite.Draw(
                    gameTime,
                    spriteBatch,
                    position,
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    false,
                    Color.White);

            }
            else
            {
                canNotPlaceBuildingSprite.Draw(
                    gameTime,
                    spriteBatch,
                    position,
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    false,
                    Color.White);

            }


        }







    }
}