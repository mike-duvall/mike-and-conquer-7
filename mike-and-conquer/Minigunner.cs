using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
//using System.Collections.Generic;

//using ShpTDSprite = OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;


namespace mike_and_conquer
{ 

    public class Minigunner
    {
        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public Vector2 position { get; set; }

//        AnimationSequence animationSequence;

        UnitSelectionCursor unitSelectionCursor;
        //Texture2D texture;
//        List<Texture2D> textureList;

        //Texture2D spriteBorderRectangleTexture;
        //Boolean drawBoundingRectangle;

        Rectangle clickDetectionRectangle;

//        float scale;

        //private int worldWidth;
        //private int worldHeight;

        //private Vector2 middleOfSprite;

        private String state;
        private Minigunner currentAttackTarget;

        private int destinationX;
        private int destinationY;

        public GameSprite gameSprite;




        protected Minigunner()
        {

        }

        private static int globalId = 1;


        public Minigunner(int x, int y, ShpFileColorMapper shpFileColorMapper)
        {

            gameSprite = new GameSprite(shpFileColorMapper);
            //this.worldWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            //this.worldHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;
            //this.textureList = new List<Texture2D>();
            //LoadAllTexturesFromShpFile("Content\\e1.shp");
//            this.texture = loadTextureFromShpFile("Content\\e1.shp", 0);

            position = new Vector2(x, y);

            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;

//            scale = 5f;
            //spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            //middleOfSprite = new Vector2();
            
            //middleOfSprite.X = textureList[0].Width / 2;
            //middleOfSprite.Y = textureList[0].Height / 2;


            clickDetectionRectangle = createClickDetectionRectangle();

//            drawBoundingRectangle = false;
            selected = false;
            unitSelectionCursor = new UnitSelectionCursor(x,y);
            AnimationSequence animationSequence = new AnimationSequence(10);
            animationSequence.AddFrame(16);
            animationSequence.AddFrame(17);
            animationSequence.AddFrame(18);
            animationSequence.AddFrame(19);
            animationSequence.AddFrame(20);
            animationSequence.AddFrame(21);
            animationSequence.SetAnimate(true);
            gameSprite.SetAnimationSequence(animationSequence);

        }

        internal Rectangle createClickDetectionRectangle()
        {

            int rectangleUnscaledWidth = 12;
            int rectangleUnscaledHeight = 12;
            int scaledWidth = (int)(rectangleUnscaledWidth * MikeAndConqueryGame.instance.scale);
            int scaledHeight = (int)(rectangleUnscaledHeight * MikeAndConqueryGame.instance.scale);

            int x = (int)(position.X - (scaledWidth / 2));
            int y = (int)(position.Y - scaledHeight) + (int)(1 * MikeAndConqueryGame.instance.scale);  

            Rectangle rectangle = new Rectangle(x,y,scaledWidth,scaledHeight);
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


            gameSprite.Update(gameTime);
//            animationSequence.Update();
            //int currentFrame = animationSequence.GetCurrentFrame();
            //Texture2D currentTexture = textureList[(int)currentFrame];
            //spriteBatch.Draw(currentTexture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);


            //if(drawBoundingRectangle)
            //{
            //    spriteBatch.Draw(spriteBorderRectangleTexture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);

            //}

            gameSprite.Draw(gameTime, spriteBatch, minigunnerPlottedPosition);

            if(selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch);
            }


        }




        internal bool ContainsPoint(int mouseX, int mouseY)
        {


            int x = (int) Math.Round(position.X);
            int y = (int) Math.Round(position.Y);
            //int width = (int) (spriteBorderRectangleTexture.Width * scale); 
            //int height = (int) (spriteBorderRectangleTexture.Height * scale);
            int width = (int)(gameSprite.unscaledWidth * MikeAndConqueryGame.instance.scale);
            int height = (int)(gameSprite.unscaledHeight * MikeAndConqueryGame.instance.scale);


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
