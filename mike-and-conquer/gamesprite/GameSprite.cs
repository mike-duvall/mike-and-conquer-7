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
        private Texture2D tempTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSprite;

        private bool animate;

        private OpenRA.Graphics.ImmutablePalette palette;

        public bool drawShadow;

        public GameSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            spriteTextureList = MikeAndConquerGame.instance.TextureListMap.GetTextureList(spriteListKey);

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSprite = new Vector2();

            middleOfSprite.X = spriteTextureList.textureWidth / 2;
            middleOfSprite.Y = spriteTextureList.textureHeight / 2;

            drawBoundingRectangle = false;
            this.animate = true;
            this.tempTexture = null;
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


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
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
                if (tempTexture == null)
                {
//                tempTexture.Dispose();
                    tempTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, currentTexture.Width,
                        currentTexture.Height);

                }
                //tempTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, currentTexture.Width,
                //        currentTexture.Height);


                Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
                Color[] newTexturePixelData = new Color[currentTexture.Width * currentTexture.Height];
                currentTexture.GetData(texturePixelData);

                int i = 0;

                foreach (Color color in texturePixelData)
                {

                    //                int x = i % 24;
                    //                int y = i / 24;
                    int x = i % 50;
                    int y = i / 50;


                    if (
                        (color.R == 84) && (color.G == 252) && (color.B == 84)
                    )
                    {
                        int topLeftXOfSprite = (int) position.X - (int) middleOfSprite.X;
                        int topLeftYOfSprite = (int) position.Y - (int) middleOfSprite.Y;
                        int screenPositionXOfThisPixel = topLeftXOfSprite + x;
                        int screenPositionYOfThisPixel = topLeftYOfSprite + y;

                        BasicMapSquare clickedBasicMapSquare2 =
                            MikeAndConquerGame.instance.FindMapSquare(screenPositionXOfThisPixel,
                                screenPositionYOfThisPixel);

                        int topLeftXOfClickedSquare = clickedBasicMapSquare2.GetCenter().X - 12;
                        int topLeftYOfClickedSquare = clickedBasicMapSquare2.GetCenter().Y - 12;

                        int squareMouseX = screenPositionXOfThisPixel - topLeftXOfClickedSquare;
                        int squareMouseY = screenPositionYOfThisPixel - topLeftYOfClickedSquare;
                        int paletteIndex =
                            clickedBasicMapSquare2.GetPaletteIndexOfCoordinate(squareMouseX, squareMouseY);


                        int shadowPaletteIndex = MapPaletteIndexToShadowPaletteIndex(paletteIndex);
                        if (shadowPaletteIndex != paletteIndex)
                        {
                            uint mappedColor = palette[shadowPaletteIndex];
                            System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int) mappedColor);
                            Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                            newTexturePixelData[i] = xnaColor;

                        }
                        else
                        {
                            newTexturePixelData[i] = color;
                        }

                        //                    newTexturePixelData[i] = new Color(255, 252, 84);

                    }
                    else
                    {
                        newTexturePixelData[i] = color;
                    }


                    i++;

                }

                tempTexture.SetData(newTexturePixelData);

//                spriteBatch.Draw(currentTexture, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(tempTexture, position, null, Color.White, 0f, middleOfSprite, defaultScale,
                    SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(currentTexture, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            }


            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            }
        }

        int MapPaletteIndexToShadowPaletteIndex(int index)
        {
            if (index == 55)
                return 249;
            if (index == 58)
                return 240;
            if (index == 60)
                return 240;
            if (index == 66)
                return 241;
            if (index == 67)
                return 241;
            if (index == 68)
                return 241;
            if (index == 70)
                return 242;
            if (index == 72)
                return 243;
            if (index == 77)
                return 242;


            if (index == 78)
                return 243;
            if (index == 79)
                return 243;

            if (index == 80)
                return 244;
            if (index == 81)
                return 244;
            if (index == 84)
                return 244;
            if (index == 189)
                return 243;

            //            New 72-> 243
            //            New 78-> 243
            //            New 79-> 243
//            New 80-> 244
//            New 81-> 244
//            New 84-> 244
//
//            New 189-> 243


            return index;

//            55-> 249
//            58-> 240
//            60-> 240
//            66-> 241
//            67-> 241
//            68-> 241
//            70-> 242
//            77-> 242

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
