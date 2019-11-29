
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using SharpDX.Direct3D9;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class MapTileInstanceView
    {

        public SingleTextureSprite singleTextureSprite;
        public MapTileInstance myMapTileInstance;

        private static Texture2D visibleMask = null;
        public static Texture2D partiallyVisibleMask = null;
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        private string textureKey;


        public MapTileInstanceView(MapTileInstance aMapTileInstance )
        {

            this.myMapTileInstance = aMapTileInstance;
            imageIndex = myMapTileInstance.ImageIndex;
            textureKey = myMapTileInstance.TextureKey;
            List<MapTileFrame>  mapTileFrameList = MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);
            if (MapTileInstanceView.visibleMask == null)
            {
                visibleMask = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapTileFrameList[imageIndex].Texture.Width, mapTileFrameList[imageIndex].Texture.Height);

                int numPixels = mapTileFrameList[imageIndex].Texture.Width *
                                mapTileFrameList[imageIndex].Texture.Height;

                Color[] textureData = new Color[numPixels];
                visibleMask.GetData(textureData);

                for (int i = 0; i < numPixels; i++)
                {
                    Color xnaColor = new Color(255, 254, 253, 0);
                    textureData[i] = xnaColor;
                }

                visibleMask.SetData(textureData);
                middleOfSpriteInSpriteCoordinates = new Vector2();

                middleOfSpriteInSpriteCoordinates.X = visibleMask.Width / 2;
                middleOfSpriteInSpriteCoordinates.Y = visibleMask.Height / 2;
            }

            if (MapTileInstanceView.partiallyVisibleMask == null)
            {
                List<UnitFrame> shadowFrameList =
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile("MapTileShadow");

                UnitFrame unitFrame = shadowFrameList[3];
                MapTileInstanceView.partiallyVisibleMask = unitFrame.Texture;

                int numPixels = MapTileInstanceView.partiallyVisibleMask.Width *
                                MapTileInstanceView.partiallyVisibleMask.Height;
                Color[] textureData = new Color[numPixels];
                partiallyVisibleMask.GetData(textureData);
                for (int i = 0; i < numPixels; i++)
                {
                    Color color = textureData[i];
                    if (color.R == 0 && color.G == 0 && color.B == 0)
                    {
                        color.R = 1;
                        color.G = 2;
                        color.B = 3;
                        color.A = 255;
                        textureData[i] = color;
                    }


                }

                MapTileInstanceView.partiallyVisibleMask.SetData(textureData);

                int x = 3;


            }
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

        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {

            float defaultScale = 1;

            if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.Visible)
            {
//                singleTextureSprite.Draw(
//                    gameTime,
//                    spriteBatch,
//                    this.myMapTileInstance.PositionInWorldCoordinates,
//                    SpriteSortLayers.MAP_SQUARE_DEPTH,
//                    GameOptions.DRAW_TERRAIN_BORDER,
//                    Color.White);

                spriteBatch.Draw(visibleMask, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
            else if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                spriteBatch.Draw(partiallyVisibleMask, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }



        }


    }
}
