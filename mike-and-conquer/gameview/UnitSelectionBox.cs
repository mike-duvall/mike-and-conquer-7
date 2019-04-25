
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using Boolean = System.Boolean;
using Rectangle = Microsoft.Xna.Framework.Rectangle;




namespace mike_and_conquer.gameview
{

    public class UnitSelectionBox
    {
        public Vector2 position { get; set; }

        private Texture2D unitSelectionBoxTexture = null;
//        Boolean drawBoundingRectangle;

        public Boolean drawUnitSelectionBox;


        public Rectangle unitSelectionBoxRectangle = new Rectangle(0,0,10,10);
        float defaultScale = 1;


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


        internal void InitializeBoundingRectangle()
        {
            int width = unitSelectionBoxRectangle.Right - unitSelectionBoxRectangle.Left;
            int height = unitSelectionBoxRectangle.Bottom - unitSelectionBoxRectangle.Top;
            position = new Vector2(unitSelectionBoxRectangle.X, unitSelectionBoxRectangle.Y);

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (unitSelectionBoxTexture != null)
            {
                unitSelectionBoxTexture.Dispose();
            }
            unitSelectionBoxTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            FillHorizontalLine(data, width, height, 0, Color.White);
            FillHorizontalLine(data, width, height, height - 1, Color.White);
            FillVerticalLine(data, width, height, 0, Color.White);
            FillVerticalLine(data, width, height, width - 1, Color.White);
//            DrawRedDotInCenter(width, height, data);
            unitSelectionBoxTexture.SetData(data);
        }

        private void DrawRedDotInCenter(int width, int height, Color[] data)
        {
            int centerX = width / 2;
            int centerY = height / 2;
            int centerOffset = (centerY * width) + centerX;
            data[centerOffset] = Color.Red;
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (drawUnitSelectionBox)
            {
                InitializeBoundingRectangle();
                Vector2 origin = new Vector2(0, 0);
                spriteBatch.Draw(unitSelectionBoxTexture, position, null, Color.White, 0f, origin, defaultScale,
                    SpriteEffects.None, 0f);
            }

        }



    }





}
