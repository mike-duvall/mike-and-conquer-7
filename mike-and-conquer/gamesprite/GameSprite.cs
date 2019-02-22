using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;

namespace mike_and_conquer
{
    public class GameSprite
    {

        Dictionary<int, AnimationSequence> animationSequenceMap;
        int currentAnimationSequenceIndex;

        SpriteTextureList spriteTextureList;
        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSpriteInSpriteCoordinates;

        private bool animate;

        private OpenRA.Graphics.ImmutablePalette palette;

        public bool drawShadow;

        public GameSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            spriteTextureList = MikeAndConquerGame.instance.TextureListMap.GetTextureList(spriteListKey);

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();

            middleOfSpriteInSpriteCoordinates.X = spriteTextureList.textureWidth / 2;
            middleOfSpriteInSpriteCoordinates.Y = spriteTextureList.textureHeight / 2;

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

            int currentTextureIndex = animationSequence.GetCurrentFrame();
            currentTexture = spriteTextureList.textureList[currentTextureIndex];
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

            int currentTextureIndex = currentAnimationSequence.GetCurrentFrame();
            currentTexture = spriteTextureList.textureList[currentTextureIndex];

            float defaultScale = 1;

            if (drawShadow)
            {
                updateShadowPixels(positionInWorldCoordinates, currentTextureIndex);
            }

            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
        }

        private void updateShadowPixels(Vector2 positionInWorldCoordinates, int currentTextureIndex)
        {
            Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData(texturePixelData);

            List<int> currentShadowIndexList = spriteTextureList.shadowIndexLists[currentTextureIndex];

            foreach (int shadowIndex in currentShadowIndexList)
            {
                int shadowXSpriteCoordinate = shadowIndex % spriteTextureList.textureWidth;
                int shadowYSpriteCoordinate = shadowIndex / spriteTextureList.textureWidth;

                int topLeftXOfSpriteInWorldCoordinates =
                    (int) positionInWorldCoordinates.X - (int) middleOfSpriteInSpriteCoordinates.X;
                int topLeftYOfSpriteInWorldCoordinates =
                    (int) positionInWorldCoordinates.Y - (int) middleOfSpriteInSpriteCoordinates.Y;


                int shadowXWorldCoordinates = topLeftXOfSpriteInWorldCoordinates + shadowXSpriteCoordinate;
                int shadowYWorldCoordinate = topLeftYOfSpriteInWorldCoordinates + shadowYSpriteCoordinate;

                BasicMapSquare underlyingMapSquare =
                    MikeAndConquerGame.instance.FindMapSquare(shadowXWorldCoordinates,
                        shadowYWorldCoordinate);


                // TODO:  Un-hard code 12
                int topLeftXOfUnderlyingMapSquareWorldCoordinates = underlyingMapSquare.GetCenter().X - 12;
                int topLeftYOfUnderlyingMapSquareWorldCoordinates = underlyingMapSquare.GetCenter().Y - 12;

                int shadowXMapSquareCoordinate = shadowXWorldCoordinates - topLeftXOfUnderlyingMapSquareWorldCoordinates;
                int shadowYMapSquareCoordinate = shadowYWorldCoordinate - topLeftYOfUnderlyingMapSquareWorldCoordinates;

                int nonShadowPaletteIndexAtShadowLocation =
                    underlyingMapSquare.GetPaletteIndexOfCoordinate(shadowXMapSquareCoordinate, shadowYMapSquareCoordinate);

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
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);
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
