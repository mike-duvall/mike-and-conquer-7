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


        UnitSelectionCursor unitSelectionCursor;
        Texture2D texture;

        Texture2D spriteBorderRectangleTexture;
        Boolean drawBoundingRectangle;

        Rectangle clickDetectionRectangle;

        float scale;

        private int worldWidth;
        private int worldHeight;

        private Vector2 middleOfSprite;

        private String state;
        private Minigunner currentAttackTarget;

        private int destinationX;
        private int destinationY;




        protected Minigunner()
        {

        }

        private static int globalId = 1;


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
            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSprite = new Vector2();
            middleOfSprite.X = texture.Width / 2;
            middleOfSprite.Y = texture.Height / 2;


            clickDetectionRectangle = createClickDetectionRectangle();

            drawBoundingRectangle = false;
            selected = false;
            unitSelectionCursor = new UnitSelectionCursor(x,y);
        }

        internal Rectangle createClickDetectionRectangle()
        {

            int rectangleUnscaledWidth = 12;
            int rectangleUnscaledHeight = 12;
            int scaledWidth = (int)(rectangleUnscaledWidth * scale);
            int scaledHeight = (int)(rectangleUnscaledHeight * scale);

            int x = (int)(position.X - (scaledWidth / 2));
            int y = (int)(position.Y - scaledHeight) + (int)(1 * scale);  

            Rectangle rectangle = new Rectangle(x,y,scaledWidth,scaledHeight);
            return rectangle;
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


        internal Texture2D createSpriteBorderRectangleTexture()
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
            
            unitSelectionCursor.position = new Vector2(this.position.X  , this.position.Y);

            //if (isEnemy)
            //{
            //    HandleEnemyUpdate(frameTime);
            //    return;
            //}

            if (state == "IDLE")
            {
                //HandleIdleState(frameTime);
            }
            else if (state == "MOVING")
            {
                //HandleMovingState(frameTime);
            }
            else if (state == "ATTACKING")
            {
                HandleAttackingState(gameTime);
            }

            //if (input->isRightMouseDown())
            //{
            //    SetSelected(false);
            //}


        }


        private double Distance(double dX0, double dY0, double dX1, double dY1)
        {
            return Math.Sqrt((dX1 - dX0) * (dX1 - dX0) + (dY1 - dY0) * (dY1 - dY0));
        }


        private int CalculateDistanceToTarget()
        {
            return (int)Distance(position.X, position.Y, currentAttackTarget.position.X, currentAttackTarget.position.Y);
        }


        private bool IsInAttackRange()
        {
            int distanceToTarget = CalculateDistanceToTarget();

            if (distanceToTarget < 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void HandleAttackingState(GameTime gameTime)
        {
            //if (IsInAttackRange())
            //{
            //    gameSprite->SetCurrentAnimationSequenceIndex(SHOOTING_UP);
            //    currentAttackTarget->ReduceHealth(10);
            //}
            //else
            //{
            //    gameSprite->SetCurrentAnimationSequenceIndex(WALKING_UP);
            //    SetDestination(currentAttackTarget->GetX(), currentAttackTarget->GetY());
            //    MoveTowardsDestination(frameTime);
            //}

            if (IsInAttackRange())
            {
                //gameSprite->SetCurrentAnimationSequenceIndex(SHOOTING_UP);
                currentAttackTarget.ReduceHealth(10);

            }
            else
            {
                //gameSprite->SetCurrentAnimationSequenceIndex(WALKING_UP);
                SetDestination( (int) currentAttackTarget.position.X, (int)currentAttackTarget.position.Y);
                MoveTowardsDestination(gameTime);
            }
        }

        void MoveTowardsDestination(GameTime gameTime)
        {
            int buffer = 0;

            int newX = (int)position.X;
            int newY = (int)position.Y;

            double velocity = .15;
            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * velocity;


            if (position.X < (destinationX - buffer))
            {
                newX += (int)delta;
            }
            else if (position.X > (destinationX + buffer))
            {
                newX -= (int)delta;
            }

            if (position.Y < (destinationY - buffer))
            {
                newY += (int)delta;
            }
            else if (position.Y > (destinationY + buffer))
            {
                newY -= (int)delta;
            }

            position = new Vector2(newX, newY);
        }


        private void SetDestination(int x, int y)
        {
            destinationX = x;
            destinationY = y;
        }


        private void ReduceHealth(int amount)
        {
            if (health > 0)
            {
                health -= amount;
            }

        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 minigunnerPlottedPosition = new Vector2();
            minigunnerPlottedPosition.X = (float)Math.Round(position.X);
            minigunnerPlottedPosition.Y = (float)Math.Round(position.Y);

            spriteBatch.Draw(texture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);
            if(drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);

            }

            if(selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch);
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
                int basePaletteIndex = frameData[i];
                int mappedPaletteIndex = MapColorIndex(basePaletteIndex);
                uint mappedColor = palette[mappedPaletteIndex];

                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;
            }

            texture2D.SetData(texturePixelData);
            shpStream.Close();
            return texture2D;

        }

        protected virtual int MapColorIndex(int index)
        {
            return index;
        }

        internal bool ContainsPoint(int mouseX, int mouseY)
        {
            int x = (int) Math.Round(position.X);
            int y = (int) Math.Round(position.Y);
            int width = (int) (spriteBorderRectangleTexture.Width * scale); 
            int height = (int) (spriteBorderRectangleTexture.Height * scale);

            x = x - (width / 2);
            y = y - (height / 2);

            Rectangle rect = new Rectangle(x, y, width, height);
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }

        internal void OrderToMoveToAndAttackEnemyUnit(NodMinigunner enemyMinigunner)
        {
            state = "ATTACKING";
            currentAttackTarget = enemyMinigunner;

        }
    }


}
