using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace mike_and_conquer
{
    public class SingleTextureSprite
    {

        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSpriteInSpriteCoordinates;

        private bool animate;

        private OpenRA.Graphics.ImmutablePalette palette;

        public bool drawShadow;

        public SingleTextureSprite(string spriteKey)
        {

            currentTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(spriteKey);
            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();


            middleOfSpriteInSpriteCoordinates.X = currentTexture.Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = currentTexture.Height / 2;

            drawBoundingRectangle = false;
            this.animate = true;
            int[] remap = { };
            palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);
            drawShadow = false;
        }



        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {

            float defaultScale = 1;


            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
        }



        internal Texture2D createSpriteBorderRectangleTexture()
        {
//            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, currentTexture.Width, currentTexture.Height);
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



    }





}
