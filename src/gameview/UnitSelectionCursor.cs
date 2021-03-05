
using System;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
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
        // private Vector2 position;

        Texture2D boundingRectangle;

        private Minigunner myMinigunner;
        private MCV myMCV;
        private Texture2D selectionCursorTexture;
        private Vector2 selectionCursorPosition;


        private Texture2D healthBarTexture;
        private Texture2D healthBarShadowTexture;
        Vector2 healthBarPosition;

        Boolean drawBoundingRectangle;

        private Vector2 middleOfSprite;

        float defaultScale = 1;

        // public const string SHP_FILE_NAME = "select.shp";
        // public const string SPRITE_KEY = "UnitSelectionCursor";
        // public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();



        private UnitSelectionCursor()
        {

        }


        public UnitSelectionCursor(MCV mcv, int x, int y)
        {
            this.myMinigunner = null;
            this.myMCV = mcv;

            this.selectionCursorTexture = InitializeSelectionCursor();

            // position = new Vector2(x, y);
            boundingRectangle = InitializeBoundingRectangle();
            healthBarTexture = null;


            middleOfSprite = new Vector2();
            middleOfSprite.X = 15;
            middleOfSprite.Y = 14;

            drawBoundingRectangle = false;

            healthBarShadowTexture = InitializeHealthBarShadow();

        }

        public UnitSelectionCursor(Minigunner minigunner, int x, int y)
        {
            this.myMinigunner = minigunner;
            this.myMCV = null;

            this.selectionCursorTexture = InitializeSelectionCursor();

            // position = new Vector2(x, y);
            boundingRectangle = InitializeBoundingRectangle();
            healthBarTexture = null;


            middleOfSprite = new Vector2();
            middleOfSprite.X = 15;
            middleOfSprite.Y = 14;

            drawBoundingRectangle = false;

            healthBarShadowTexture = InitializeHealthBarShadow();
        }




        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color, int start, int end)
        {
            int beginIndex = width * lineIndex;
            int relativeStart = beginIndex + start;
            int relativeEnd = beginIndex + end;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                if (i >= relativeStart && i <= relativeEnd)
                {
                    data[i] = color;
                }

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


        void DrawHorizontalLine(Color[] data, Color color, int width, int height, int startX, int startY, int length)
        {
            if (startX + length > width)
            {
                throw new Exception("Attempt to create line outside bounds of texture width");
            }
            int beginIndex = startX + (width * startY);
        
            for (int i = beginIndex; i < beginIndex + length; i++)
            {
                data[i] = color;
            }
        
        }

        void DrawVerticalLine(Color[] data, Color color, int width, int height, int startX, int startY, int length)
        {

            int dataIndex = startX + (width * startY);


            for (int i = 0; i < length; i++)
            {
                data[dataIndex] = color;
                dataIndex += width;
            }

            // if (startX + length > width)
            // {
            //     throw new Exception("Attempt to create line outside bounds of texture width");
            // }
            // int beginIndex = startX + (width * startY);
            //
            // for (int i = beginIndex; i < beginIndex + length; i++)
            // {
            //     data[i] = color;
            // }

        }





        internal Texture2D InitializeBoundingRectangle()
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, selectionCursorTexture.Width, selectionCursorTexture.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);
            int centerX = rectangle.Width / 2;
            int centerY = rectangle.Height / 2;
            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;

        }


        private int GetUnitHealth()
        {
            if (this.myMCV != null)
            {
                return this.myMCV.health;
            }
            else if(this.myMinigunner !=null)
            {
                return this.myMinigunner.health;
            }
            else
            {
                throw new Exception("myMCV AND myMinigunner were null");
            }
        }


        internal Texture2D InitializeHealthBar()
        {

            // TODO free old textures if they exist?

            if (this.myMCV != null)
            {
                return InitializeHealthBarForMCV();
            }
            else if (this.myMinigunner != null)
            {
                return InitializeHealthBarForMinigunner();
            }
            else
            {
                throw new Exception("myMCV AND myMinigunner were null");
            }

        }


        private Texture2D InitializeHealthBarForMinigunner()
        {
            int healthBarHeight = 4;
            int healthBarWidth = 12;  // This is hard coded for minigunner

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

            Color[] data = new Color[rectangle.Width * rectangle.Height];

            Color cncPalleteColorBlack = new Color(0, 255, 255, 255);
            Color cncPalleteColorGreen = new Color(4, 255, 255, 255);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 3, cncPalleteColorBlack);

            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 11, cncPalleteColorBlack);

            int maxHealth = 50;
            float ratio = 10f / maxHealth;

            // int healthBarLength = (int) (myMinigunner.health * ratio);
            int healthBarLength = (int)(GetUnitHealth() * ratio);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorGreen, 1, healthBarLength);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorGreen, 1, healthBarLength);

            rectangle.SetData(data);

            return rectangle;
        }




        private Texture2D InitializeHealthBarForMCV()
        {
            int healthBarHeight = 4;
            int healthBarWidth = 36;  // This is hard coded for minigunner

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

            Color[] data = new Color[rectangle.Width * rectangle.Height];

            Color cncPalleteColorBlack = new Color(0, 255, 255, 255);
            Color cncPalleteColorGreen = new Color(4, 255, 255, 255);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 3, cncPalleteColorBlack);
            
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, healthBarWidth - 1, cncPalleteColorBlack);

            int nonBorderHealthBarWidth = healthBarWidth - 2;
            int maxHealth = 1000;
            // float ratio = 10f / maxHealth;
            float ratio = (float) nonBorderHealthBarWidth / (float) maxHealth;
            int unitHealth = GetUnitHealth();
            int healthBarLength = (int) (unitHealth * ratio);
            
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorGreen, 1, healthBarLength);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorGreen, 1, healthBarLength);

            rectangle.SetData(data);

            return rectangle;

        }


        internal Texture2D InitializeHealthBarShadow()
        {
            int healthBarHeight = 4;
            int healthBarWidth = 12;  // This is hard coded for minigunner

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

            Color[] data = new Color[rectangle.Width * rectangle.Height];

            Color cncPalleteColorShadow = new Color(255, 255, 255, 255);

            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorShadow);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorShadow);

            rectangle.SetData(data);
            return rectangle;
        }


        internal Texture2D InitializeSelectionCursorForMinigunner()
        {

            int width = 13;
            int height = 13;

            int horizontalLength = 3;
            int verticalLength = 4;

            return CreateUnitSelectionTexture(width, height, horizontalLength, verticalLength);
        }



        internal Texture2D InitializeSelectionCursorForMCV()
        {
            int width = 37;
            int height = 33;

            int horizontalLength = 8;
            int verticalLength = 8;

            return CreateUnitSelectionTexture(width, height, horizontalLength, verticalLength);
        }


        private Texture2D CreateUnitSelectionTexture(int width, int height, int horizontalLength, int verticalLength)
        {
            Color cncPalleteColorWhite = new Color(255, 255, 255, 255);

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);

            Color[] data = new Color[rectangle.Width * rectangle.Height];

            int startX = 0;
            int startY = 0;

            // top left
            DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);
            DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);

            // bottom left
            startY = height - verticalLength;
            DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);
            startY = height - 1;
            DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);

            // top right
            startX = width - horizontalLength;
            startY = 0;
            DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);
            startX = width - 1;
            DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);

            // bottom right
            startY = height - verticalLength;
            DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);
            startX = width - horizontalLength;

            startY = height - 1;
            DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);

            rectangle.SetData(data);

            return rectangle;
        }




        internal Texture2D InitializeSelectionCursor()
        {

            if (this.myMCV != null)
            {
                return InitializeSelectionCursorForMCV();
            }
            else if (this.myMinigunner != null)
            {
                return InitializeSelectionCursorForMinigunner();
            }
            else
            {
                throw new Exception("myMCV AND myMinigunner were null");
            }

        }


        public void Update(GameTime gameTime)
        {
            if (healthBarTexture != null)
            {
                healthBarTexture.Dispose();
            }
            healthBarTexture = InitializeHealthBar();
            if (this.myMinigunner != null)
            {
                int selectionCursorXOffset = 9;
                int selectionCursorYOffset = 4;

                selectionCursorPosition = new Vector2(
                    myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X + selectionCursorXOffset,
                    myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y + selectionCursorYOffset);

                healthBarPosition.X = myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X + 9;
                healthBarPosition.Y = myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y;

            }
            else if (this.myMCV != null)
            {
                int selectionCursorXOffset = 9;
                int selectionCursorYOffset = 0;

                selectionCursorPosition = new Vector2(
                    myMCV.GameWorldLocation.WorldCoordinatesAsVector2.X + selectionCursorXOffset - 12,
                    myMCV.GameWorldLocation.WorldCoordinatesAsVector2.Y + selectionCursorYOffset);

                healthBarPosition.X = myMCV.GameWorldLocation.WorldCoordinatesAsVector2.X - 3;
                healthBarPosition.Y = myMCV.GameWorldLocation.WorldCoordinatesAsVector2.Y - 4;
            }
            else
            {
                throw new Exception("myMCV AND myMinigunner were null");
            }




        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {

            spriteBatch.Draw(selectionCursorTexture, selectionCursorPosition, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, layerDepth);
            // if (drawBoundingRectangle)
            // {
            //     
            //     spriteBatch.Draw(boundingRectangle, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            // }

            spriteBatch.Draw(healthBarTexture, healthBarPosition, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, layerDepth);
        }



        internal void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(healthBarShadowTexture, healthBarPosition, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, layerDepth);
        }



    }





}
