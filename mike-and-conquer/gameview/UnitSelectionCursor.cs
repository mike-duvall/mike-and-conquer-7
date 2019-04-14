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

    public class UnitSelectionCursor
    {
        public Vector2 position { get; set; }

        Texture2D texture;
        Texture2D boundingRectangle;
        Boolean drawBoundingRectangle;

        private int worldWidth;
        private int worldHeight;

        private Vector2 middleOfSprite;

        float defaultScale = 1;

        private UnitSelectionCursor()
        {

        }

        public UnitSelectionCursor(int x, int y)
        {

            this.worldWidth = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Width;
            this.worldHeight = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Height;
            this.texture = loadTextureFromShpFile("Content\\select.shp", 0);

            position = new Vector2(x, y);
            boundingRectangle = initializeBoundingRectangle();

            middleOfSprite = new Vector2();
            middleOfSprite.X = 15;
            middleOfSprite.Y = 14;

            drawBoundingRectangle = false;
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


        internal Texture2D initializeBoundingRectangle()
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, texture.Width, texture.Height);
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
            spriteBatch.Draw(texture, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(boundingRectangle, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            }
        }

        internal Texture2D loadTextureFromShpFile(string shpFileName, int indexOfFrameToLoad)
        {
            //if (loader.IsShpTD(stream))
            //{
            //    frames = null;
            //    return false;
            //}
            //loader.TryParseSprite(stream, out frames);

            int[] remap = { };

            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            System.IO.FileStream shpStream = System.IO.File.Open(shpFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader loader = new OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader();
            ShpTDSprite shpTDSprite = new OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite(shpStream);


            OpenRA.Graphics.ISpriteFrame frame = shpTDSprite.Frames[indexOfFrameToLoad];
            byte[] frameData = frame.Data;

            Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
            int numPixels = texture2D.Width * texture2D.Height;
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                uint paletteX = palette[frameData[i]];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)palette[frameData[i]]);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;
            }
            texture2D.SetData(texturePixelData);
            shpStream.Close();
            return texture2D;

        }


    }





}
