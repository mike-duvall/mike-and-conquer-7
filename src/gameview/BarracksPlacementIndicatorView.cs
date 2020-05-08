
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BarracksPlacementIndicatorView
    {
        public SingleTextureSprite singleTextureSprite;


        public static string FILE_NAME = "trans.icn";
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        public BarracksPlacementIndicatorView()
        {
            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(FILE_NAME);

            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[0].Texture);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Vector2 position = new Vector2(100,100);
            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                position,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                Color.White);


        }







    }
}