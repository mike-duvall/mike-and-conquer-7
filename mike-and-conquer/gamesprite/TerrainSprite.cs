using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameview;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace mike_and_conquer
{
    public class TerrainSprite
    {

        private List<UnitFrame> unitFrameList;
        Texture2D noShadowTexture;

        private Texture2D shadowOnlytexture2D;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 spritOrigin;

        private OpenRA.Graphics.ImmutablePalette palette;

        private int unitFrameImageIndex;

        private int width;
        public int Width
        {
            get { return width; }
        }

        private int height;
        public int Height
        {
            get { return height; }
        }


        public TerrainSprite(string spriteListKey, Point position )
        {
            unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(spriteListKey);
            unitFrameImageIndex = 0;

            spriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture();

            spritOrigin = new Vector2(0,0);

            UnitFrame firstUnitFrame = unitFrameList[unitFrameImageIndex];
            this.width = firstUnitFrame.Texture.Width;
            this.height = firstUnitFrame.Texture.Height;
            drawBoundingRectangle = false;
            int[] remap = { };
            palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            InitializeNoShadowTexture();
            InitializeShadowOnlyTexture(position);
        }

        private void InitializeNoShadowTexture()
        {
            Texture2D sourceTexture = unitFrameList[0].Texture;
            Color[] sourceTexturePixelData = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData(sourceTexturePixelData);

            noShadowTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, sourceTexture.Width,
                sourceTexture.Height);

            Color[] noShadowTexturePixelData = new Color[noShadowTexture.Width * noShadowTexture.Height];
            noShadowTexture.GetData(noShadowTexturePixelData);

            int index = 0;
            foreach (Color color in sourceTexturePixelData)
            {
                noShadowTexturePixelData[index] = color;
                index++;
            }

            noShadowTexture.SetData(noShadowTexturePixelData);

//            noShadowTexture = unitFrameList[0].Texture;
            UpdateShadowPixelsToTransparent();

        }


        private void InitializeShadowOnlyTexture(Point positionInWorldCoordinates)
        {
            shadowOnlytexture2D =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, noShadowTexture.Width, noShadowTexture.Height);
            
            Color[] texturePixelData = new Color[shadowOnlytexture2D.Width * shadowOnlytexture2D.Height];
            shadowOnlytexture2D.GetData(texturePixelData);

            List<int> shadowIndexList = unitFrameList[unitFrameImageIndex].ShadowIndexList;

            int topLeftXOfSpriteInWorldCoordinates = positionInWorldCoordinates.X;
            int topLeftYOfSpriteInWorldCoordinates = positionInWorldCoordinates.Y; 

            Color[]  texturePixelDatWithShadowsUpdated = ShadowHelper.UpdateShadowPixels(
                topLeftXOfSpriteInWorldCoordinates,
                topLeftYOfSpriteInWorldCoordinates,
                texturePixelData,
                shadowIndexList,
                shadowOnlytexture2D.Width,
                this.palette
            );

            shadowOnlytexture2D.SetData(texturePixelDatWithShadowsUpdated);

        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {
            int currentAnimationImageIndex = 0;
        
            float defaultScale = 1;
        
            updateShadowPixels2(positionInWorldCoordinates, currentAnimationImageIndex);
        
            spriteBatch.Draw(noShadowTexture, positionInWorldCoordinates, null, Color.White, 0f, spritOrigin, defaultScale, SpriteEffects.None, 0f);
        
            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, spritOrigin, defaultScale, SpriteEffects.None, 0f);
            }
        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {

            float defaultScale = 1;
            spriteBatch.Draw(shadowOnlytexture2D, positionInWorldCoordinates, null, Color.White, 0f, spritOrigin, defaultScale, SpriteEffects.None, 0f);

        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {
            float defaultScale = 1;

            spriteBatch.Draw(noShadowTexture, positionInWorldCoordinates, null, Color.White, 0f, spritOrigin, defaultScale, SpriteEffects.None, 0f);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, spritOrigin, defaultScale, SpriteEffects.None, 0f);
            }
        }


//        private void updateShadowPixels(Vector2 positionInWorldCoordinates, int imageIndex)
//        {
//            Color[] texturePixelData = new Color[noShadowTexture.Width * noShadowTexture.Height];
//            noShadowTexture.GetData(texturePixelData);
//
//
//            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;
//
//            foreach (int shadowIndex in shadowIndexList)
//            {
//                int shadowXSpriteCoordinate = shadowIndex % this.noShadowTexture.Width;
//                int shadowYSpriteCoordinate = shadowIndex / this.noShadowTexture.Width;
//
//
//                int topLeftXOfSpriteInWorldCoordinates =
//                    (int) positionInWorldCoordinates.X - (int) spritOrigin.X;
//                int topLeftYOfSpriteInWorldCoordinates =
//                    (int) positionInWorldCoordinates.Y - (int) spritOrigin.Y;
//
//
//                int shadowXWorldCoordinates = topLeftXOfSpriteInWorldCoordinates + shadowXSpriteCoordinate;
//                int shadowYWorldCoordinate = topLeftYOfSpriteInWorldCoordinates + shadowYSpriteCoordinate;
//
//
//
//                MapTileInstanceView underlyingMapTileInstanceView =
//                    GameWorldView.instance.FindMapSquareView(shadowXWorldCoordinates,
//                        shadowYWorldCoordinate);
//
//                int halfWidth = underlyingMapTileInstanceView.singleTextureSprite.Width / 2;
//                int topLeftXOfUnderlyingMapSquareWorldCoordinates = underlyingMapTileInstanceView.myMapTileInstance.GetCenter().X - halfWidth;
//                int topLeftYOfUnderlyingMapSquareWorldCoordinates = underlyingMapTileInstanceView.myMapTileInstance.GetCenter().Y - halfWidth;
//
//                int shadowXMapSquareCoordinate = shadowXWorldCoordinates - topLeftXOfUnderlyingMapSquareWorldCoordinates;
//                int shadowYMapSquareCoordinate = shadowYWorldCoordinate - topLeftYOfUnderlyingMapSquareWorldCoordinates;
//
//
//                int nonShadowPaletteIndexAtShadowLocation =
//                    underlyingMapTileInstanceView.GetPaletteIndexOfCoordinate(shadowXMapSquareCoordinate, shadowYMapSquareCoordinate);
//
//
//
//                int shadowPaletteIndex;
//
//                //                    shadowPaletteIndex =
//                //                        MikeAndConquerGame.instance.shadowMapper.MapUnitsShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);
//
//                shadowPaletteIndex =
//                    MikeAndConquerGame.instance.shadowMapper.MapUnitsShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);
//
//
//                if (shadowPaletteIndex != nonShadowPaletteIndexAtShadowLocation)
//                {
//                    // If we found a different color for the shadow pixel (which we should)
//                    // remap the color in the texture to be the shadow color
//                    uint mappedColor = palette[shadowPaletteIndex];
//                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int) mappedColor);
//                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
//                    texturePixelData[shadowIndex] = xnaColor;
//                }
//                else
//                {
//                    // If we didn't find a different shadow palette color, map it to bright green
//                    // so we can see it and debug it
//                    // TODO:  Or, consider throwing and exception and logging it
//                    texturePixelData[shadowIndex] = new Color(255, 252, 84);
//                }
//            }
//
//            noShadowTexture.SetData(texturePixelData);
//        }

        private void updateShadowPixels2(Vector2 positionInWorldCoordinates, int imageIndex)
        {
            Color[] texturePixelData = new Color[noShadowTexture.Width * noShadowTexture.Height];
            noShadowTexture.GetData(texturePixelData);


            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;
            int topLeftXOfSpriteInWorldCoordinates =
                (int)positionInWorldCoordinates.X - (int)spritOrigin.X;
            int topLeftYOfSpriteInWorldCoordinates =
                (int)positionInWorldCoordinates.Y - (int)spritOrigin.Y;


            Color[] texturePixelDatWithShadowsUpdated = ShadowHelper.UpdateShadowPixels(
                topLeftXOfSpriteInWorldCoordinates,
                topLeftYOfSpriteInWorldCoordinates,
                texturePixelData,
                shadowIndexList,
                shadowOnlytexture2D.Width,
                this.palette
            );

            noShadowTexture.SetData(texturePixelDatWithShadowsUpdated);

        }


        private void UpdateShadowPixelsToTransparent()
        {
            Color[] texturePixelData = new Color[noShadowTexture.Width * noShadowTexture.Height];
            noShadowTexture.GetData(texturePixelData);


            List<int> shadowIndexList = unitFrameList[0].ShadowIndexList;

            foreach (int shadowIndex in shadowIndexList)
            {
                texturePixelData[shadowIndex] = Color.Transparent;
            }

            noShadowTexture.SetData(texturePixelData);
        }



        internal Texture2D CreateSpriteBorderRectangleTexture()
        {
            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, unitFrameList[0].Texture.Width,
                    unitFrameList[0].Texture.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);

            int centerX = (rectangle.Width / 2) ;
            int centerY = (rectangle.Height / 2);

            // Check how this works for even sized sprites with true center

            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;

        }


        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }

    }





}
