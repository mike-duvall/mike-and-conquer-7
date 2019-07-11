
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



//            DrawLine(spriteBatch, new Vector2(32, 24), 180);
//            DrawLine(spriteBatch, new Vector2(32, 24), 225);
            DrawLine(spriteBatch, new Vector2(32, 24), 270);  // Straight up
//            DrawLine(spriteBatch, new Vector2(32, 24), 315);  // 45 degrees further, going clockwise
//            DrawLine(spriteBatch, new Vector2(32, 24), 360);  // 90 degrees further, going clockwise
//            DrawLine(spriteBatch, new Vector2(32, 24), 45); // 135 degrees
//            DrawLine(spriteBatch, new Vector2(32, 24), 90);
            DrawLine(spriteBatch, new Vector2(32, 24), 135);
            spriteBatch.End();

            


            FloodFill(spriteBatch, new Point(33, 0));

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);
        }


        private void FloodFill(SpriteBatch spriteBatch, Point startPixel)
        {

            int numPixels = lineDrawingTexture.Width * lineDrawingTexture.Height;
            Color[] texturePixelData = new Color[numPixels];
            lineDrawingTexture.GetData(texturePixelData);

            Queue<Point> frontier = new Queue<Point>();
            frontier.Enqueue(startPixel);

            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();

                texturePixelData[current.X + (current.Y * lineDrawingTexture.Width)] = Color.Red;
                List<Point> connectedNodesToFill = CalculateConnectedNodesToFill(current, texturePixelData);

                foreach (Point point in connectedNodesToFill)
                {
                    if (!frontier.Contains(point))
                    {
                        frontier.Enqueue(point);
                    }

                }

//                foreach (int next in current.connectedNodes)
//                {
//                    if (!came_from.ContainsKey(next))
//                    {
//                        frontier.Enqueue(navigationGraph.nodeList[next]);
//                        came_from[next] = current.id;
//                    }
//                }
            }

            lineDrawingTexture.SetData(texturePixelData);


        }

        private List<Point> CalculateConnectedNodesToFill(Point current, Color[] texturePixelData)
        {
            List<Point> pointList = new List<Point>();
            Point toRight = new Point(current.X + 1, current.Y);
            if (IsNodeToFill(toRight, texturePixelData))
            {
                pointList.Add(toRight);
            }

            Point toBottom = new Point(current.X, current.Y + 1 );
            if (IsNodeToFill(toBottom, texturePixelData))
            {
                pointList.Add(toBottom);
            }

            Point toLeft = new Point(current.X - 1, current.Y);
            if (IsNodeToFill(toLeft, texturePixelData))
            {
                pointList.Add(toLeft);
            }

            Point toTop = new Point(current.X, current.Y - 1);
            if (IsNodeToFill(toTop, texturePixelData))
            {
                pointList.Add(toTop);
            }
            return pointList;
        }

        private bool IsNodeToFill(Point toRight, Color[] texturePixelData)
        {

            bool isGoodToFill = false;

//            if (toRight.X <= lineDrawingTexture.Width - 2)
            if(IsValidLocation(toRight.X, toRight.Y, lineDrawingTexture.Width, lineDrawingTexture.Height))
            {
                Color toRightColor = texturePixelData[toRight.X + (toRight.Y * lineDrawingTexture.Width)];
                if (toRightColor == Color.Black)
                {
                    isGoodToFill = true;
                }

            }

            return isGoodToFill;
        }

        bool IsValidLocation(int x, int y, int width, int height)
        {
            bool isValid = (
                x >= 0 &&
                (x <= width - 1) &&
                y >= 0 &&
                (y <= height - 1));

            return isValid;
        }



        private void DrawLine(SpriteBatch spriteBatch, Vector2 startEndpoint, double angleInDegrees)
        {
            double angleDegreeInRadians = DegreeToRadian(angleInDegrees);
            int x2 = (int)(startEndpoint.X + (160 * Math.Cos(angleDegreeInRadians)));
            int y2 = (int)(startEndpoint.Y + (160 * Math.Sin(angleDegreeInRadians)));
            DrawLine(spriteBatch, startEndpoint, new Vector2(x2, y2));

        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
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
