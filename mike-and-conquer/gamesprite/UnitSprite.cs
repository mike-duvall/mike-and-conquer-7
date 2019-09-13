using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public class UnitSprite
    {

        Dictionary<int, AnimationSequence> animationSequenceMap;
        int currentAnimationSequenceIndex;

        private List<UnitFrame> unitFrameList;
        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        //TODO Make this private
        public Vector2 middleOfSpriteInSpriteCoordinates;

        private bool animate;

        private OpenRA.Graphics.ImmutablePalette palette;

        public bool drawShadow;

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


        public UnitSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(spriteListKey);

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();

            UnitFrame firstUnitFrame = unitFrameList[0];
            middleOfSpriteInSpriteCoordinates.X = firstUnitFrame.Texture.Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = firstUnitFrame.Texture.Height / 2;
            this.width = firstUnitFrame.Texture.Width;
            this.height = firstUnitFrame.Texture.Height;
            drawBoundingRectangle = false;
            this.animate = true;
            int[] remap = { };
            palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);
            drawShadow = false;
        }

        public void SetCurrentAnimationSequenceIndex(int animationSequenceIndex)
        {
            if (currentAnimationSequenceIndex == animationSequenceIndex)
            {
                return;
            }

            currentAnimationSequenceIndex = animationSequenceIndex;

            AnimationSequence animationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            animationSequence.SetCurrentFrameIndex(0);

            int currentAnimationImageIndex = animationSequence.GetCurrentFrame();
            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;
        }


        public void AddAnimationSequence(int key, AnimationSequence  animationSequence)
        {
            animationSequenceMap[key] = animationSequence;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {
            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            if (animate)
            {
                currentAnimationSequence.Update();
            }

            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();
            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;

            float defaultScale = 1;

            if (drawShadow)
            {
                updateShadowPixels(positionInWorldCoordinates, currentAnimationImageIndex);
            }

            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
        }


        //        public void Update()
        //        {
        //            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
        //            if (animate)
        //            {
        //                currentAnimationSequence.Update();
        //            }
        //
        //            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();
        //            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;
        //
        //        }


        //        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        //        {
        //
        //            Texture2D shadowOnlytexture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, currentTexture.Width, currentTexture.Height);
        //
        //            Color[] texturePixelData = new Color[shadowOnlytexture2D.Width * shadowOnlytexture2D.Height];
        //            shadowOnlytexture2D.GetData(texturePixelData);
        //
        //
        //            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
        //            int imageIndex = currentAnimationSequence.GetCurrentFrame();
        //
        //            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;
        //
        //            foreach (int shadowIndex in shadowIndexList)
        //            {
        //                int shadowXSpriteCoordinate = shadowIndex % this.currentTexture.Width;
        //                int shadowYSpriteCoordinate = shadowIndex / this.currentTexture.Width;
        //
        //
        //                int topLeftXOfSpriteInWorldCoordinates =
        //                    (int)positionInWorldCoordinates.X - (int)middleOfSpriteInSpriteCoordinates.X;
        //                int topLeftYOfSpriteInWorldCoordinates =
        //                    (int)positionInWorldCoordinates.Y - (int)middleOfSpriteInSpriteCoordinates.Y;
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
        //                shadowPaletteIndex =
        //                    MikeAndConquerGame.instance.shadowMapper.MapShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);
        //
        //
        //                if (shadowPaletteIndex != nonShadowPaletteIndexAtShadowLocation)
        //                {
        //                    // If we found a different color for the shadow pixel (which we should)
        //                    // remap the color in the texture to be the shadow color
        //                    uint mappedColor = palette[shadowPaletteIndex];
        //                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
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
        //            shadowOnlytexture2D.SetData(texturePixelData);
        //
        //            float defaultScale = 1;
        //            spriteBatch.Draw(shadowOnlytexture2D, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
        //
        //        }

        //        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        //        {
        //            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
        //            //            if (animate)
        //            //            {
        //            //                currentAnimationSequence.Update();
        //            //            }
        //            //
        //            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();
        //            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;
        //
        //            float defaultScale = 1;
        //
        //            if (drawShadow)
        //            {
        //                updateShadowPixelsToBlank(positionInWorldCoordinates, currentAnimationImageIndex);
        //            }
        //
        //            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
        //
        //            if (drawBoundingRectangle)
        //            {
        //                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
        //            }
        //        }
        //


        // How to draw shadows:
        // X       Write method that returns current tile, given a point on the map
        // X       Write method that returns color index of given point on the map
        //        Write method that then maps the color index to the shadow index color
        //        Write method that creates new texture for minigunner with shadow colors fixed up 
        // Create new texture of same size
        // Copy pixels over one by one
        // If pixel is the shadow green:
        //    determine the screen x,y of that pixel 
        //    determine the palette value of the existing screen background at that position
        //    map that background to the proper shadow pixel
        //    set that pixel in the new texture to that shadow pixel
        // Draw the texture
        // release the texture? (or release above before creating the new one?


        private void updateShadowPixels(Vector2 positionInWorldCoordinates, int imageIndex)
        {
            Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData(texturePixelData);
            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;

            texturePixelData = ShadowHelper.UpdateShadowPixels(
                positionInWorldCoordinates,
                this.middleOfSpriteInSpriteCoordinates,
                texturePixelData,
                shadowIndexList,
                currentTexture.Width,
                this.palette
                );
            currentTexture.SetData(texturePixelData);

        }



        //        private void updateShadowPixels(Vector2 positionInWorldCoordinates, int imageIndex)
        //        {
        //            Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
        //            currentTexture.GetData(texturePixelData);
        //
        //
        //            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;
        //
        //            foreach (int shadowIndex in shadowIndexList)
        //            {
        //                int shadowXSpriteCoordinate = shadowIndex % this.currentTexture.Width;
        //                int shadowYSpriteCoordinate = shadowIndex / this.currentTexture.Width;
        //
        //
        //                int topLeftXOfSpriteInWorldCoordinates =
        //                    (int) positionInWorldCoordinates.X - (int) middleOfSpriteInSpriteCoordinates.X;
        //                int topLeftYOfSpriteInWorldCoordinates =
        //                    (int) positionInWorldCoordinates.Y - (int) middleOfSpriteInSpriteCoordinates.Y;
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
        //                    shadowPaletteIndex =
        //                        MikeAndConquerGame.instance.shadowMapper.MapShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);
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
        //            currentTexture.SetData(texturePixelData);
        //        }


        private void updateShadowPixelsToBlank(Vector2 positionInWorldCoordinates, int imageIndex)
        {
            Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData(texturePixelData);


            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;

            foreach (int shadowIndex in shadowIndexList)
            {

                texturePixelData[shadowIndex] = Color.Transparent;
            }

            currentTexture.SetData(texturePixelData);
        }



     


        internal Texture2D createSpriteBorderRectangleTexture()
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

        public void SetAnimate(bool animateFlag)
        {
            this.animate = animateFlag;
        }


    }





}
