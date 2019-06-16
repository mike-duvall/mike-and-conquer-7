
using System.Collections.Generic;
using mike_and_conquer.gamesprite;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class MapTileInstanceView
    {

        public SingleTextureSprite singleTextureSprite;
        public MapTileInstance myMapTileInstance;

        private int imageIndex;
        private string textureKey;

        public MapTileInstanceView(MapTileInstance aMapTileInstance )
        {

            this.myMapTileInstance = aMapTileInstance;
            imageIndex = myMapTileInstance.MapTileType.ImageIndex;
            textureKey = myMapTileInstance.MapTileType.TextureKey;
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
            singleTextureSprite.Draw(gameTime, spriteBatch, this.myMapTileInstance.PositionInWorldCoordinates);
        }


    }
}
