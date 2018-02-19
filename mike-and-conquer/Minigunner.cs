using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

using ShpTDSprite = OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite;

namespace mike_and_conquer
{

    public class Minigunner
    {
        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public Vector2 position { get; set; }

        Texture2D texture;
        Texture2D boundingRectangle;
        Boolean drawBoundingRectangle;
        float scale;

        private int worldWidth;
        private int worldHeight;

        private Vector2 middleOfSprite;


        //Make test pass:  def "should handle selecting deselecting gdi minigunner"()
        //    Add handling of left click that selects and deselects gdi minigunner if left clicked on or left cliced on nothing
        //    Set state to selected
        //    Verify unit selection cursor is drawn

        private Minigunner()
        {

        }

        private static int globalId = 1;

        //Create UnitSelectionCursor object, copy of Minigunner
        //Create base game object base class, with ShpFile, texture, and ability to draw bounding rectangle
        //Make Minigunner draw selection cursor when selected

        public Minigunner(int x, int y)
        {

            this.worldWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            this.worldHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;
            this.texture = loadTextureFromShpFile("Content\\e1.shp", 0);

            position = new Vector2(x, y);

            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;

            scale = 5f;
            boundingRectangle = initializeBoundingRectangle();

            middleOfSprite = new Vector2();
            middleOfSprite.X = texture.Width / 2;
            middleOfSprite.Y = texture.Height / 2;

            drawBoundingRectangle = false;
            selected = false;
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
            for (int i = beginIndex; i < (width * height); i+= width)
            {
                data[i] = color;
            }
        }


        internal Texture2D initializeBoundingRectangle()
        {
            Texture2D rectangle = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, texture.Width, texture.Height);
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
            //double velocity = .15;
            //double delta = gameTime.ElapsedGameTime.TotalMilliseconds * velocity;


            //position.X = position.X + (float)delta;
            ////position.X += 1;
            //if (position.X > worldWidth)
            //    position.X = 0;

            //position.Y = position.Y + (float)delta;
            //if (position.Y > worldHeight)
            //    position.Y = 0;


        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 minigunnerPlottedPosition = new Vector2();
            minigunnerPlottedPosition.X = (float)Math.Round(position.X);
            minigunnerPlottedPosition.Y = (float)Math.Round(position.Y);

            spriteBatch.Draw(texture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);
            //if(drawBoundingRectangle)
            if (selected)
            {
                spriteBatch.Draw(boundingRectangle, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);
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

            Texture2D texture2D = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
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
            return texture2D;

        }

        internal bool ContainsPoint(int mouseX, int mouseY)
        {
            int x = (int) Math.Round(position.X);
            int y = (int) Math.Round(position.Y);
            int width = (int) (boundingRectangle.Width * scale); 
            int height = (int) (boundingRectangle.Height * scale);



            x = x - (width / 2);
            y = y - (height / 2);


            //I believe this is not working becuase of scaling
            //    Can try scaling to 1 to see of it works
            //    but probably hit test rectangle will also need to be scaled up

            //    alternative might be to "scale" the mouse position

            Rectangle rect = new Rectangle(x, y, width, height);
            return rect.Contains(new Point(mouseX, mouseY));
        }
    }


}
