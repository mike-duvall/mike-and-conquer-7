﻿using System.Collections.Generic;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using ShpTDSprite = mike_and_conquer.openra.ShpTDSprite;
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


        private Minigunner myMinigunner;
        Texture2D texture;
        Texture2D boundingRectangle;
        private Texture2D healthBar;

        Boolean drawBoundingRectangle;

        private Vector2 middleOfSprite;

        float defaultScale = 1;


        public const string SHP_FILE_NAME = "select.shp";
        public const string SPRITE_KEY = "UnitSelectionCursor";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        private UnitSelectionCursor()
        {

        }

        public UnitSelectionCursor(Minigunner minigunner, int x, int y)
        {
            this.myMinigunner = minigunner;
            List<UnitFrame> unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY);
            UnitFrame theUnitFrame = unitFrameList[0];

            this.texture = theUnitFrame.Texture;

            position = new Vector2(x, y);
            boundingRectangle = InitializeBoundingRectangle();
            // healthBar = InitializeHealthBar();
            healthBar = null;

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

        internal void fillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color, int start, int end)
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



        internal void fillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }


        internal Texture2D InitializeBoundingRectangle()
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

        internal Texture2D InitializeHealthBar()
        {
            int healthBarHeight = 4;
            int healthBarWidth = 12;  // This is hard coded for minigunner

            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

            Color[] data = new Color[rectangle.Width * rectangle.Height];

            Color cncPalleteColorBlack = new Color(0, 255, 255, 255);
            Color cncPalleteColorGreen = new Color(4, 255, 255, 255);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
            // fillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorBlack);
            // fillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorBlack);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 3, cncPalleteColorBlack);

            fillVerticalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, 11, cncPalleteColorBlack);

            int maxHealth = 50;
            float ratio = 10f / maxHealth;

            int healthBarLength = (int) (myMinigunner.health * ratio);


            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorGreen, 1, healthBarLength);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorGreen, 1, healthBarLength);

            rectangle.SetData(data);
            return rectangle;

        }



        public void Update(GameTime gameTime)
        {
            healthBar = InitializeHealthBar();
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, layerDepth);
            if (drawBoundingRectangle)
            {
                
                spriteBatch.Draw(boundingRectangle, position, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, 0f);
            }

            Vector2 healthBarPosition = position;
            // healthBarPosition.X = position.X + (texture.Width - 30);
            healthBarPosition.X = position.X + 10;
            healthBarPosition.Y = position.Y - 1;
            spriteBatch.Draw(healthBar, healthBarPosition, null, Color.White, 0f, middleOfSprite, defaultScale, SpriteEffects.None, layerDepth);
        }




    }





}
