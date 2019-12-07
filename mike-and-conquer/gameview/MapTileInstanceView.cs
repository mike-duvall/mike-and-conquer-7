﻿
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.util;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class MapTileInstanceView
    {

        public SingleTextureSprite singleTextureSprite;
        public MapTileInstance myMapTileInstance;

        // TODO Revisit handling of these visiblity masks and whether this field should be static
        private static Texture2D visibleMask = null;
        private static Vector2 middleOfSpriteInSpriteCoordinates;


        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;

        private int imageIndex;
        private string textureKey;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;



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

            partiallyVisibileMapTileMask = new PartiallyVisibileMapTileMask();
            mapTileBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                    Color.White,
                    this.singleTextureSprite.Width,
                    this.singleTextureSprite.Height);

            mapTileBlockingTerrainBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                new Color(127, 255, 255, 255),
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);

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
//            if (GameOptions.DRAW_BLOCKING_TERRAIN_BORDER && myMapTileInstance.IsBlockingTerrain)
//            {
//                singleTextureSprite.Draw(
//                    gameTime,
//                    spriteBatch,
//                    this.myMapTileInstance.PositionInWorldCoordinates,
//                    SpriteSortLayers.MAP_SQUARE_DEPTH,
//                    GameOptions.DRAW_BLOCKING_TERRAIN_BORDER,
//                    Color.Red);
//
//            }
//            else
//            {
//                singleTextureSprite.Draw(
//                    gameTime,
//                    spriteBatch,
//                    this.myMapTileInstance.PositionInWorldCoordinates,
//                    SpriteSortLayers.MAP_SQUARE_DEPTH,
//                    GameOptions.DRAW_TERRAIN_BORDER,
//                    Color.White);
//            }

            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                this.myMapTileInstance.PositionInWorldCoordinates,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                Color.White);

            if (GameOptions.DRAW_TERRAIN_BORDER)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBorder, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            }

            if (GameOptions.DRAW_BLOCKING_TERRAIN_BORDER && myMapTileInstance.IsBlockingTerrain)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBlockingTerrainBorder, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            }


        }

        private int DeterminePartiallyVisibleMaskTile()
        {

            int verticalOffset = GameWorld.MAP_TILE_HEIGHT / 2 + 2;
            int horizontalOffset = GameWorld.MAP_TILE_WIDTH / 2 + 2;
            MapTileInstance below = GameWorld.instance.FindMapSquare(
                (int)myMapTileInstance.PositionInWorldCoordinates.X, (int) (myMapTileInstance.PositionInWorldCoordinates.Y + verticalOffset));

            MapTileInstance above = GameWorld.instance.FindMapSquare(
                (int)myMapTileInstance.PositionInWorldCoordinates.X, (int)(myMapTileInstance.PositionInWorldCoordinates.Y - verticalOffset));


            MapTileInstance left = GameWorld.instance.FindMapSquare(
                (int)myMapTileInstance.PositionInWorldCoordinates.X - horizontalOffset, (int)(myMapTileInstance.PositionInWorldCoordinates.Y));


            MapTileInstance right = GameWorld.instance.FindMapSquare(
                (int)myMapTileInstance.PositionInWorldCoordinates.X + horizontalOffset, (int)(myMapTileInstance.PositionInWorldCoordinates.Y));


            if (right.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && below.Visibility == MapTileInstance.MapTileVisibility.Visible &&
                left.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && above.Visibility == MapTileInstance.MapTileVisibility.NotVisible)
            {
                return 3;
            }

            if( right.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && below.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible &&
                left.Visibility == MapTileInstance.MapTileVisibility.NotVisible && above.Visibility == MapTileInstance.MapTileVisibility.NotVisible)
            {
                return 9;
            }

            if (right.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && below.Visibility == MapTileInstance.MapTileVisibility.NotVisible &&
            left.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible &&  above.Visibility == MapTileInstance.MapTileVisibility.Visible)
            {
                return 0;
            }

            if (right.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && below.Visibility == MapTileInstance.MapTileVisibility.NotVisible &&
                left.Visibility == MapTileInstance.MapTileVisibility.NotVisible && above.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                return 8;
            }


            if (right.Visibility == MapTileInstance.MapTileVisibility.NotVisible && below.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible &&
                left.Visibility == MapTileInstance.MapTileVisibility.Visible && above.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                return 5;
            }

            if (right.Visibility == MapTileInstance.MapTileVisibility.NotVisible && below.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible &&
                left.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && above.Visibility == MapTileInstance.MapTileVisibility.NotVisible)
            {
                return 10;
            }

            if (right.Visibility == MapTileInstance.MapTileVisibility.NotVisible && below.Visibility == MapTileInstance.MapTileVisibility.NotVisible &&
                left.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible && above.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                return 11;
            }


            return 1;

        }

        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {

            float defaultScale = 1;

            if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.Visible)
            {

                spriteBatch.Draw(visibleMask, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
            else if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                spriteBatch.Draw(partiallyVisibileMapTileMask.GetMask(index), this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }

        }


    }
}
