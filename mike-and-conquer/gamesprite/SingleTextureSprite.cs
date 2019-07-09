
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameview;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

using OpenRA.Graphics;

namespace mike_and_conquer
{
    public class SingleTextureSprite
    {

        Texture2D texture;
        private Texture2D cloudTexture;
//        private Texture2D lineDrawingTexture;
        private RenderTarget2D lineDrawingTexture;
        public bool drawCloudTexture;

        private byte[] frameData;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSpriteInSpriteCoordinates;


        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }


        public SingleTextureSprite(Texture2D texture) : this(texture, null)
        {
            
        }


        public SingleTextureSprite(Texture2D texture, byte[] frameData)
        {
            this.texture = texture;
            this.frameData = frameData;
            this.drawCloudTexture = false;
            spriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();


            middleOfSpriteInSpriteCoordinates.X = Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = Height / 2;

            drawBoundingRectangle = false;

        }

        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {

            float defaultScale = 1;


            spriteBatch.Draw(texture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);

            if (drawCloudTexture)
            {
                Vector2 cloudPosition = new Vector2(positionInWorldCoordinates.X + 100, positionInWorldCoordinates.Y);
                spriteBatch.Draw(cloudTexture, cloudPosition, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates,
                    defaultScale, SpriteEffects.None, 0f);

                Vector2 lineDrawingTexturePosition = new Vector2(positionInWorldCoordinates.X, positionInWorldCoordinates.Y + 100);
                spriteBatch.Draw(lineDrawingTexture, lineDrawingTexturePosition, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates,
                    defaultScale, SpriteEffects.None, 0f);

            }

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
        }



        public  void RemapAllPixels()
        {

            cloudTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, texture.Width, texture.Height);
            int numPixels = cloudTexture.Width * cloudTexture.Height;
            Color[] texturePixelData = new Color[numPixels];

            GdiShpFileColorMapper shpFileColorMapper = new GdiShpFileColorMapper();
            int[] remap = { };
            ImmutablePalette palette = new ImmutablePalette("Content\\temperat.pal", remap);

            for (int i = 0; i < numPixels; i++)
            {
                int basePaletteIndex = frameData[i];
                int mappedPaletteIndex = shpFileColorMapper.MapColorIndex(basePaletteIndex);
                int shadowPaletteIndex =
                    MikeAndConquerGame.instance.shadowMapper.MapSidebarBuildPaletteIndex(mappedPaletteIndex);

                uint mappedColor = palette[shadowPaletteIndex];

                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;

            }

            cloudTexture.SetData(texturePixelData);


            SetupLineDrawingTexture();



        }

        private void SetupLineDrawingTexture()
        {

            //            lineDrawingTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, texture.Width, texture.Height);
            lineDrawingTexture = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice, texture.Width, texture.Height);


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(lineDrawingTexture);
            SpriteBatch spriteBatch = new SpriteBatch(MikeAndConquerGame.instance.GraphicsDevice);

            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp);
            //            spriteBatch.Draw(lineDrawingTexture, new Rectangle(0, 0, lineDrawingTexture.Width, lineDrawingTexture.Height), Color.Blue);
            DrawLine(spriteBatch, new Vector2(32, 24), new Vector2(32, 0));
            DrawLine(spriteBatch, new Vector2(32, 24), new Vector2(64, 0));
            spriteBatch.End();


            int numPixels = lineDrawingTexture.Width * lineDrawingTexture.Height;
            Color[] texturePixelData = new Color[numPixels];

            lineDrawingTexture.GetData(texturePixelData);

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

//            int numPixels = lineDrawingTexture.Width * lineDrawingTexture.Height;
//            Color[] texturePixelData = new Color[numPixels];
//
//
//            FillHorizontalLine(texturePixelData, lineDrawingTexture.Width, lineDrawingTexture.Height, 0, Color.White);


//            lineDrawingTexture.SetData(texturePixelData);

        }


        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {

            Texture2D t = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });// fill the te

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

            t.Dispose();

        }

        internal Texture2D CreateSpriteBorderRectangleTexture()
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, Width, Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);

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
