
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using Boolean = System.Boolean;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;



namespace mike_and_conquer.gameview
{

    public class UnitSelectionBox
    {
        public Vector2 position { get; set; }

        private Texture2D unitSelectionBoxTexture = null;
        public Boolean isDragSelectHappening;
        public Point selectionBoxDragStartPoint;

        public Rectangle selectionBoxRectangle = new Rectangle(0,0,10,10);
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
            int width = selectionBoxRectangle.Right - selectionBoxRectangle.Left;
            int height = selectionBoxRectangle.Bottom - selectionBoxRectangle.Top;
            position = new Vector2(selectionBoxRectangle.X, selectionBoxRectangle.Y);

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
            if (isDragSelectHappening)
            {
                InitializeBoundingRectangle();
                Vector2 origin = new Vector2(0, 0);
                spriteBatch.Draw(unitSelectionBoxTexture, position, null, Color.White, 0f, origin, defaultScale,
                    SpriteEffects.None, 0f);
            }

        }


        public void HandleDragFromLeftToRight(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.Y > selectionBoxDragStartPoint.Y)
            {
                HandleDragFromTopLeftToBottomRight(mouseWorldLocationPoint);
            }
            else
            {
                HandleDrapFromBottomLeftToTopRight(mouseWorldLocationPoint);
            }
        }

        public void HandleDragFromRightToLeft(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.Y > selectionBoxDragStartPoint.Y)
            {
                HandleDragFromTopRightToBottomLeft(mouseWorldLocationPoint);
            }
            else
            {
                HandleDragFromBottomRightToTopLeft(mouseWorldLocationPoint);
            }
        }


        private void HandleDragFromBottomRightToTopLeft(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                mouseWorldLocationPoint.X,
                mouseWorldLocationPoint.Y,
                selectionBoxDragStartPoint.X - mouseWorldLocationPoint.X,
                selectionBoxDragStartPoint.Y - mouseWorldLocationPoint.Y);
        }

        private void HandleDragFromTopRightToBottomLeft(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                mouseWorldLocationPoint.X,
                selectionBoxDragStartPoint.Y,
                selectionBoxDragStartPoint.X - mouseWorldLocationPoint.X,
                mouseWorldLocationPoint.Y - selectionBoxDragStartPoint.Y);
        }

        private void HandleDrapFromBottomLeftToTopRight(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                selectionBoxDragStartPoint.X,
                mouseWorldLocationPoint.Y,
                mouseWorldLocationPoint.X - selectionBoxDragStartPoint.X,
                selectionBoxDragStartPoint.Y - mouseWorldLocationPoint.Y);
        }

        private void HandleDragFromTopLeftToBottomRight(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                selectionBoxDragStartPoint.X,
                selectionBoxDragStartPoint.Y,
                mouseWorldLocationPoint.X - selectionBoxDragStartPoint.X,
                mouseWorldLocationPoint.Y - selectionBoxDragStartPoint.Y);
        }

        public bool MouseLocationHasMovedSinceLeftClick(Point mouseWorldLocationPoint)
        {
            return mouseWorldLocationPoint.X != selectionBoxDragStartPoint.X || mouseWorldLocationPoint.Y != selectionBoxDragStartPoint.Y;
        }

        public void HandleEndDragSelect()
        {
            foreach (Minigunner minigunner in MikeAndConquerGame.instance.gameWorld.gdiMinigunnerList)
            {
                if (selectionBoxRectangle.Contains(minigunner.positionInWorldCoordinates))
                {
                    minigunner.selected = true;
                }
                else
                {
                    minigunner.selected = false;
                }
            }
        }



    }





}
