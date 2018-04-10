using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;

namespace mike_and_conquer
{ 

    public class Minigunner
    {
        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public Vector2 position { get; set; }


        UnitSelectionCursor unitSelectionCursor;

        Rectangle clickDetectionRectangle;

        private String state;
        private Minigunner currentAttackTarget;

        private int destinationX;
        private int destinationY;

        public GameSprite gameSprite;

        private bool isEnemy;
        private bool enemyStateIsSleeping;
        private int enemySleepCountdownTimer;


        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        protected Minigunner()
        {

        }

        private static int globalId = 1;


        protected Minigunner(int x, int y, bool isEnemy, string spriteListKey)
        {

            this.isEnemy = isEnemy;
            this.enemyStateIsSleeping = true;
            this.enemySleepCountdownTimer = 400;


            gameSprite = new GameSprite(spriteListKey);
            position = new Vector2(x, y);

            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;


            clickDetectionRectangle = createClickDetectionRectangle();

            selected = false;
            unitSelectionCursor = new UnitSelectionCursor(x, y);

            SetupAnimations();

        }

        private void SetupAnimations()
        {
            AnimationSequence walkingUpAnimationSequence = new AnimationSequence(10);
            walkingUpAnimationSequence.AddFrame(16);
            walkingUpAnimationSequence.AddFrame(17);
            walkingUpAnimationSequence.AddFrame(18);
            walkingUpAnimationSequence.AddFrame(19);
            walkingUpAnimationSequence.AddFrame(20);
            walkingUpAnimationSequence.AddFrame(21);

            gameSprite.AddAnimationSequence((int)AnimationSequences.WALKING_UP, walkingUpAnimationSequence);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            gameSprite.AddAnimationSequence((int)AnimationSequences.STANDING_STILL, standingStillAnimationSequence);
            gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(10);
            shootinUpAnimationSequence.AddFrame(65);
            shootinUpAnimationSequence.AddFrame(66);
            shootinUpAnimationSequence.AddFrame(67);
            shootinUpAnimationSequence.AddFrame(68);
            shootinUpAnimationSequence.AddFrame(69);
            shootinUpAnimationSequence.AddFrame(70);
            shootinUpAnimationSequence.AddFrame(71);
            shootinUpAnimationSequence.AddFrame(72);
            gameSprite.AddAnimationSequence((int)AnimationSequences.SHOOTING_UP, shootinUpAnimationSequence);
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

            if (isEnemy)
            {
                HandleEnemyUpdate(gameTime);
                return;
            }

            if (state == "IDLE")
            {
                HandleIdleState(gameTime);
            }
            else if (state == "MOVING")
            {
                HandleMovingState(gameTime);
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


        private void HandleEnemyUpdate(GameTime gameTime)
        {
            if (!enemyStateIsSleeping)
            {

                if (currentAttackTarget != null && currentAttackTarget.health <= 0)
                {
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();

                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = 400;
                        gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
                        return;
                    }
                }



                if (IsInAttackRange())
                {
                    gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
                    currentAttackTarget.ReduceHealth(10);
                }
                else
                {
                    gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
                    SetDestination((int)currentAttackTarget.position.X, (int)currentAttackTarget.position.Y);
                    MoveTowardsDestination(gameTime);
                }

            }
            else
            {
                enemySleepCountdownTimer--;
                if (enemySleepCountdownTimer <= 0)
                {
                    enemyStateIsSleeping = false;
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();
                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = 400;
                    }

                }

            }

        }



        private Minigunner FindFirstNonDeadGdiMinigunner()
        {
            List<Minigunner> gdiMinigunners = (MikeAndConqueryGame.instance.gdiMinigunnerList);

            foreach (Minigunner nextMinigunner in gdiMinigunners)
            {
                if (nextMinigunner.health > 0)
                {
                    return nextMinigunner;
                }
            }

            return null;

        }


        private void HandleIdleState(GameTime gameTime)
        {
            gameSprite.SetCurrentAnimationSequenceIndex((int) AnimationSequences.STANDING_STILL);
        }


        private void HandleMovingState(GameTime gameTime)
        {

            gameSprite.SetCurrentAnimationSequenceIndex((int) AnimationSequences.WALKING_UP);

            MoveTowardsDestination(gameTime);
            if (IsAtDestination())
            {
                state = "IDLE";
            }

        }

        bool IsAtDestination()
        {

            int buffer = 2;
            return (
                position.X > (destinationX - buffer) &&
                position.Y < (destinationX + buffer) &&
                position.Y > (destinationY - buffer) &&
                position.Y < (destinationY + buffer)
                );

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
                gameSprite.SetCurrentAnimationSequenceIndex( (int)  AnimationSequences.SHOOTING_UP);
                currentAttackTarget.ReduceHealth(10);

            }
            else
            {
                //gameSprite->SetCurrentAnimationSequenceIndex(WALKING_UP);
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
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


//            gameSprite.Update(gameTime);
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

        public void SetAnimate(bool animateFlag)
        {
            gameSprite.SetAnimate(animateFlag);
        }
    }


}
