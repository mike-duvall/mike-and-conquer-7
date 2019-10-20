
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
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
            imageIndex = myMapTileInstance.ImageIndex;
            textureKey = myMapTileInstance.TextureKey;
            List<MapTileFrame>  mapTileFrameList = MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);
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
            if (GameOptions.DRAW_BLOCKING_TERRAIN_BORDER && myMapTileInstance.IsBlockingTerrain)
            {
                singleTextureSprite.Draw(
                    gameTime,
                    spriteBatch,
                    this.myMapTileInstance.PositionInWorldCoordinates,
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    GameOptions.DRAW_BLOCKING_TERRAIN_BORDER,
                    Color.Red);

            }
            else
            {
                singleTextureSprite.Draw(
                    gameTime,
                    spriteBatch,
                    this.myMapTileInstance.PositionInWorldCoordinates,
                    SpriteSortLayers.MAP_SQUARE_DEPTH,
                    GameOptions.DRAW_TERRAIN_BORDER,
                    Color.White);
            }

        }


    }
}
