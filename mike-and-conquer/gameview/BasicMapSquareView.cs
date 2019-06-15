
using System.Collections.Generic;
using mike_and_conquer.gamesprite;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquareView
    {

        public SingleTextureSprite singleTextureSprite;
        public BasicMapSquare myBasicMapSquare;

        private int imageIndex;
        private string textureKey;

        public BasicMapSquareView(BasicMapSquare aBasicMapSquare )
        {

            this.myBasicMapSquare = aBasicMapSquare;
            imageIndex = myBasicMapSquare.ImageIndex;
            textureKey = myBasicMapSquare.TextureKey;
            List<MapTileFrame>  mapTileFrameList = MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);
            this.singleTextureSprite.drawBoundingRectangle = false;
        }


        internal int GetPaletteIndexOfCoordinate(int x, int y)
        {

            List<MapTileFrame> mapTileFrameList = MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            MapTileFrame mapTileFrame = mapTileFrameList[imageIndex];
            byte[] frameData = mapTileFrame.FrameData;

            int frameDataIndex = y * singleTextureSprite.Width + x;
            return frameData[frameDataIndex];
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            singleTextureSprite.Draw(gameTime, spriteBatch, this.myBasicMapSquare.PositionInWorldCoordinates);
        }


    }
}
