
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

        Texture2D texture;

        Texture2D whiteSpriteBorderRectangleTexture;
        Texture2D redSpriteBorderRectangleTexture;
//        public Boolean drawWhiteBoundingRectangle;
        public Boolean drawRedBoundingRectangle;


        private Vector2 middleOfSpriteInSpriteCoordinates;


        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        public SingleTextureSprite(Texture2D texture)
        {
            this.texture = texture;
            whiteSpriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture(Color.White);
            redSpriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture(Color.Red);

            middleOfSpriteInSpriteCoordinates = new Vector2();

            middleOfSpriteInSpriteCoordinates.X = Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = Height / 2;

//            drawWhiteBoundingRectangle = false;
            drawRedBoundingRectangle = false;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, bool drawWhiteBoundingRectangle, float layerDepth)
        {

            float defaultScale = 1;

            spriteBatch.Draw(texture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            if (drawWhiteBoundingRectangle)
            {
                spriteBatch.Draw(whiteSpriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
            if (drawRedBoundingRectangle)
            {
                spriteBatch.Draw(redSpriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }

        }

        internal Texture2D CreateSpriteBorderRectangleTexture(Color color)
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, Width, Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, color);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, color);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, color);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, color);

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
