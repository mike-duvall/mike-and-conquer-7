using Microsoft.Xna.Framework;
using ShpTDSprite = OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using Boolean = System.Boolean;




namespace mike_and_conquer.gameview
{

    public class UnitSelectionBox
    {
        public Vector2 position { get; set; }

        Texture2D unitSelectionBoxTexture;
//        Boolean drawBoundingRectangle;

        public Boolean drawUnitSelectionBox;


        public Rectangle unitSelectionBoxRectangle = new Rectangle(0,0,10,10);
        float defaultScale = 1;


//        public UnitSelectionCursor(int x, int y)
//        {
//
//            this.worldWidth = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Width;
//            this.worldHeight = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Height;
//            this.texture = loadTextureFromShpFile("Content\\select.shp", 0);
//
//            position = new Vector2(x, y);
//            unitSelectionBoxTexture = initializeBoundingRectangle();
//
//            middleOfSprite = new Vector2();
//            middleOfSprite.X = 15;
//            middleOfSprite.Y = 14;
//
//            drawBoundingRectangle = false;
//        }




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


        internal Texture2D initializeBoundingRectangle()
        {
            int width = unitSelectionBoxRectangle.Right - unitSelectionBoxRectangle.Left;
            int height = unitSelectionBoxRectangle.Bottom - unitSelectionBoxRectangle.Top;
            position = new Vector2(unitSelectionBoxRectangle.X, unitSelectionBoxRectangle.Y);

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);
            int centerX = rectangle.Width / 2;
            int centerY = rectangle.Height / 2;
            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;


            rectangle.SetData(data);
            return rectangle;

        }

        public void Update(GameTime gameTime)
        {

        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (drawUnitSelectionBox)
            {
                unitSelectionBoxTexture = initializeBoundingRectangle();
                Vector2 origin = new Vector2(0, 0);
                spriteBatch.Draw(unitSelectionBoxTexture, position, null, Color.White, 0f, origin, defaultScale,
                    SpriteEffects.None, 0f);
            }

        }



    }





}
