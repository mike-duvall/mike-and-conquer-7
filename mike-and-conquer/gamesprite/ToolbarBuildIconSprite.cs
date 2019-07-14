﻿
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class ToolbarBuildIconSprite
    {

        Texture2D staticTexture;
        private Texture2D buildInProcessTexture = null;
        private RenderTarget2D buildInProcessSkeletonTexture = null;
        public bool isBuilding;

        private byte[] frameData;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

        private Vector2 middleOfSpriteInSpriteCoordinates;

        public int Width
        {
            get { return staticTexture.Width; }
        }

        public int Height
        {
            get { return staticTexture.Height; }
        }



        public ToolbarBuildIconSprite(Texture2D staticTexture, byte[] frameData)
        {
            this.staticTexture = staticTexture;
            this.frameData = frameData;
            this.isBuilding = false;
            spriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();


            middleOfSpriteInSpriteCoordinates.X = Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = Height / 2;

            drawBoundingRectangle = false;

        }

        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {

            float defaultScale = 1;

            if (isBuilding)
            {
                //                Vector2 cloudPosition = new Vector2(positionInWorldCoordinates.X + 100, positionInWorldCoordinates.Y);
                //                spriteBatch.Draw(buildInProcessTexture, cloudPosition, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates,
                //                    defaultScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(buildInProcessTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);

                //                Vector2 lineDrawingTexturePosition = new Vector2(positionInWorldCoordinates.X, positionInWorldCoordinates.Y + 100);
                //                spriteBatch.Draw(buildInProcessSkeletonTexture, lineDrawingTexturePosition, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates,
                //                    defaultScale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(staticTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }
        }


        public void SetPercentBuildComplete(int percentComplete)
        {
            int angle = 360 * percentComplete / 100;
            angle += 270;
            if (angle > 360)
            {
                angle -= 360;
            }

            this.UpdateBuildInProcessTexture(angle);
        }

        private void UpdateBuildInProcessTexture(double angleInDegrees)
        {
            UpdateBuildInProcessSkeletonTexture(angleInDegrees);

            if (buildInProcessTexture != null)
            {
                buildInProcessTexture.Dispose();
                buildInProcessTexture = null;
            }
            buildInProcessTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, staticTexture.Width, staticTexture.Height);
            int numPixels = buildInProcessTexture.Width * buildInProcessTexture.Height;
            Color[] texturePixelData = new Color[numPixels];

            Color[] lineDrawingTexturePixelData = new Color[numPixels];
            buildInProcessSkeletonTexture.GetData(lineDrawingTexturePixelData);


            GdiShpFileColorMapper shpFileColorMapper = new GdiShpFileColorMapper();
            int[] remap = { };
            ImmutablePalette palette = new ImmutablePalette("Content\\temperat.pal", remap);

            for (int i = 0; i < numPixels; i++)
            {
                int basePaletteIndex = frameData[i];
                int mappedPaletteIndex = shpFileColorMapper.MapColorIndex(basePaletteIndex);

                if (lineDrawingTexturePixelData[i] == Color.Black)
                {
                    mappedPaletteIndex =
                        MikeAndConquerGame.instance.shadowMapper.MapSidebarBuildPaletteIndex(mappedPaletteIndex);
                }

                uint mappedColor = palette[mappedPaletteIndex];

                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;

            }

            buildInProcessTexture.SetData(texturePixelData);

        }


        private void UpdateBuildInProcessSkeletonTexture(double angleInDegrees)
        {
            if (buildInProcessSkeletonTexture != null)
            {
                buildInProcessSkeletonTexture.Dispose();
                buildInProcessSkeletonTexture = null;
            }
            buildInProcessSkeletonTexture = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice, staticTexture.Width, staticTexture.Height);

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(buildInProcessSkeletonTexture);
            SpriteBatch spriteBatch = new SpriteBatch(MikeAndConquerGame.instance.GraphicsDevice);

            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp);

            DrawLine(spriteBatch, new Vector2(32, 24), 270);  // Straight up
            DrawLine(spriteBatch, new Vector2(33, 24), angleInDegrees);

            spriteBatch.End();

            FloodFill( new Point(33, 0));

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);
        }


        private void FloodFill(Point startPixel)
        {

            int numPixels = buildInProcessSkeletonTexture.Width * buildInProcessSkeletonTexture.Height;
            Color[] texturePixelData = new Color[numPixels];
            buildInProcessSkeletonTexture.GetData(texturePixelData);

            Queue<Point> frontier = new Queue<Point>();
            frontier.Enqueue(startPixel);

            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();

                texturePixelData[current.X + (current.Y * buildInProcessSkeletonTexture.Width)] = Color.Red;
                List<Point> connectedNodesToFill = CalculateConnectedNodesToFill(current, texturePixelData);

                foreach (Point point in connectedNodesToFill)
                {
                    if (!frontier.Contains(point))
                    {
                        frontier.Enqueue(point);
                    }

                }
            }

            buildInProcessSkeletonTexture.SetData(texturePixelData);

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

            if(IsValidLocation(toRight.X, toRight.Y, buildInProcessSkeletonTexture.Width, buildInProcessSkeletonTexture.Height))
            {
                Color toRightColor = texturePixelData[toRight.X + (toRight.Y * buildInProcessSkeletonTexture.Width)];
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
                    (int)edge.Length(), //sb will strech the staticTexture to fill this rectangle
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
