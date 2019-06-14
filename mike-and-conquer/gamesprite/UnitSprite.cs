using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using mike_and_conquer.gamesprite;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;
using BasicMapSquareView = mike_and_conquer.gameview.BasicMapSquareView;

namespace mike_and_conquer
{
    public class UnitSprite
    {

        Dictionary<int, AnimationSequence> animationSequenceMap;
        int currentAnimationSequenceIndex;

//        SpriteTextureList spriteTextureList;
        private List<UnitFrame> unitFrameList;
        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSpriteInSpriteCoordinates;

        private bool animate;

        private OpenRA.Graphics.ImmutablePalette palette;

        public bool drawShadow;

        public UnitSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(spriteListKey);
//            spriteTextureList = MikeAndConquerGame.instance.TextureListMap.GetTextureList(spriteListKey);

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();

//            middleOfSpriteInSpriteCoordinates.X = spriteTextureList.textureWidth / 2;
//            middleOfSpriteInSpriteCoordinates.Y = spriteTextureList.textureHeight / 2;
            UnitFrame firstUnitFrame = unitFrameList[0];
            middleOfSpriteInSpriteCoordinates.X = firstUnitFrame.Texture.Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = firstUnitFrame.Texture.Height / 2;

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
//            currentTexture = spriteTextureList.shpFileImageList[currentAnimationImageIndex].texture;
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
//            currentTexture = spriteTextureList.shpFileImageList[currentAnimationImageIndex].texture;
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

//            List<int> shadowIndexList = spriteTextureList.shpFileImageList[imageIndex].shadowIndexList;
            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;

            foreach (int shadowIndex in shadowIndexList)
            {
                //                int shadowXSpriteCoordinate = shadowIndex % spriteTextureList.textureWidth;
                //                int shadowYSpriteCoordinate = shadowIndex / spriteTextureList.textureWidth;
                int shadowXSpriteCoordinate = shadowIndex % this.currentTexture.Width;
                int shadowYSpriteCoordinate = shadowIndex / this.currentTexture.Height;


                int topLeftXOfSpriteInWorldCoordinates =
                    (int) positionInWorldCoordinates.X - (int) middleOfSpriteInSpriteCoordinates.X;
                int topLeftYOfSpriteInWorldCoordinates =
                    (int) positionInWorldCoordinates.Y - (int) middleOfSpriteInSpriteCoordinates.Y;


                int shadowXWorldCoordinates = topLeftXOfSpriteInWorldCoordinates + shadowXSpriteCoordinate;
                int shadowYWorldCoordinate = topLeftYOfSpriteInWorldCoordinates + shadowYSpriteCoordinate;

                BasicMapSquareView underlyingMapSquareView =
                    MikeAndConquerGame.instance.FindMapSquareView(shadowXWorldCoordinates,
                        shadowYWorldCoordinate);

                // TODO:  Un-hard code 12
                int topLeftXOfUnderlyingMapSquareWorldCoordinates = underlyingMapSquareView.myBasicMapSquare.GetCenter().X - 12;
                int topLeftYOfUnderlyingMapSquareWorldCoordinates = underlyingMapSquareView.myBasicMapSquare.GetCenter().Y - 12;

                int shadowXMapSquareCoordinate = shadowXWorldCoordinates - topLeftXOfUnderlyingMapSquareWorldCoordinates;
                int shadowYMapSquareCoordinate = shadowYWorldCoordinate - topLeftYOfUnderlyingMapSquareWorldCoordinates;

                int nonShadowPaletteIndexAtShadowLocation =
                    underlyingMapSquareView.GetPaletteIndexOfCoordinate(shadowXMapSquareCoordinate, shadowYMapSquareCoordinate);

                int shadowPaletteIndex =
                    MikeAndConquerGame.instance.shadowMapper.MapShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);

                if (shadowPaletteIndex != nonShadowPaletteIndexAtShadowLocation)
                {
                    // If we found a different color for the shadow pixel (which we should)
                    // remap the color in the texture to be the shadow color
                    uint mappedColor = palette[shadowPaletteIndex];
                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int) mappedColor);
                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                    texturePixelData[shadowIndex] = xnaColor;
                }
                else
                {
                    // If we didn't find a different shadow palette color, map it to bright green
                    // so we can see it and debug it
                    // TODO:  Or, consider throwing and exception and logging it
                    texturePixelData[shadowIndex] = new Color(255, 252, 84);
                }
            }

            currentTexture.SetData(texturePixelData);
        }


        internal Texture2D createSpriteBorderRectangleTexture()
        {
//            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, unitFrameList[0].Texture.Width,
                    unitFrameList[0].Texture.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);

            //            int centerX = (rectangle.Width / 2) - 1;
            //            int centerY = (rectangle.Height / 2) - 1;
            int centerX = (rectangle.Width / 2) ;
            int centerY = (rectangle.Height / 2);

            // Check how this works for even sized sprites with true center

            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;

        }


        internal void fillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void fillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
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
