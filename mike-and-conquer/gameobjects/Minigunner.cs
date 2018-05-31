
using System.Collections.Generic;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer
{ 

    public class Minigunner
    {
        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public Vector2 position { get; set; }

        Rectangle clickDetectionRectangle;

        private Minigunner currentAttackTarget;

        public enum State { IDLE, MOVING, ATTACKING };
        public State state;

        enum Command { NONE, MOVE_TO_POINT, ATTACK_TARGET };
        private Command currentCommand;


        private int destinationX;
        private int destinationY;

        private int unscaledWidth;
        private int unscaledHeight;

        //private bool isEnemy;
        //private bool enemyStateIsSleeping;

        //private static readonly int ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE = 1000;
        //private int enemySleepCountdownTimer;
        private float scale;

        protected Minigunner()
        {

        }

        private static int globalId = 1;


        public Minigunner(int x, int y, bool isEnemy, float scale)
        {

            //this.isEnemy = isEnemy;
            //this.enemyStateIsSleeping = true;
            //this.enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
            this.state = State.IDLE;
            this.currentCommand = Command.NONE;


            // TODO move to base class and just have sublcass hard code
            this.unscaledWidth = 666;
            this.unscaledHeight = 666;

            this.scale = scale;

            position = new Vector2(x, y);

            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;


            clickDetectionRectangle = createClickDetectionRectangle();

            selected = false;

        }

        internal Rectangle createClickDetectionRectangle()
        {

            int rectangleUnscaledWidth = 12;
            int rectangleUnscaledHeight = 12;
            int scaledWidth = (int)(rectangleUnscaledWidth * this.scale);
            int scaledHeight = (int)(rectangleUnscaledHeight * this.scale);


            int x = (int)(position.X - (scaledWidth / 2));
            int y = (int)(position.Y - scaledHeight) + (int)(1 * this.scale);  

            Rectangle rectangle = new Rectangle(x,y,scaledWidth,scaledHeight);
            return rectangle;
        }




        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
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
            
            //unitSelectionCursor.position = new Vector2(this.position.X  , this.position.Y);

            //if (isEnemy)
            //{
            //    HandleEnemyUpdate(gameTime);
            //    return;
            //}

            if (this.currentCommand == Command.NONE)
            {
                HandleCommandNone(gameTime);
            }
            else if (this.currentCommand == Command.MOVE_TO_POINT)
            {
                HandleCommandMoveToPoint(gameTime);
            }
            else if (this.currentCommand == Command.ATTACK_TARGET)
            {
                HandleCommandAttackTarget(gameTime);
            }


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
                        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                        this.state = State.MOVING;
                        return;
                    }

                }

                if (IsInAttackRange())
                {
                    this.state = State.ATTACKING;
                    currentAttackTarget.ReduceHealth(10);
                }
                else
                {
                    this.state = State.MOVING;
                    SetDestination((int)currentAttackTarget.position.X, (int)currentAttackTarget.position.Y);
                    MoveTowardsDestination(gameTime);
                }

            }
            else
            {
                this.state = State.IDLE;
                enemySleepCountdownTimer--;
                if (enemySleepCountdownTimer <= 0)
                {
                    enemyStateIsSleeping = false;
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();
                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                    }

                }

            }

        }



        //private Minigunner FindFirstNonDeadGdiMinigunner()
        //{
        //    List<Minigunner> gdiMinigunners = (MikeAndConqueryGame.instance.gdiMinigunnerList);

        //    foreach (Minigunner nextMinigunner in gdiMinigunners)
        //    {
        //        if (nextMinigunner.health > 0)
        //        {
        //            return nextMinigunner;
        //        }
        //    }

        //    return null;

        //}


        private void HandleCommandNone(GameTime gameTime)
        {
            this.state = State.IDLE;
        }


        private void HandleCommandMoveToPoint(GameTime gameTime)
        {
            this.state = State.MOVING;
            MoveTowardsDestination(gameTime);
            if (IsAtDestination())
            {
                this.currentCommand = Command.NONE;
            }

        }

        bool IsAtDestination()
        {

            int buffer = 0;
            //return (
            //    position.X > (destinationX - buffer) &&
            //    position.Y < (destinationX + buffer) &&
            //    position.Y > (destinationY - buffer) &&
            //    position.Y < (destinationY + buffer)
            //    );

            return (
                position.X == destinationX &&
                position.Y == destinationY);


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


        private void HandleCommandAttackTarget(GameTime gameTime)
        {
            if(currentAttackTarget.health <= 0)
            {
                this.currentCommand = Command.NONE;
            }

            if (IsInAttackRange())
            {
                this.state = State.ATTACKING;
                currentAttackTarget.ReduceHealth(10);

            }
            else
            {
                this.state = State.MOVING;
                SetDestination( (int) currentAttackTarget.position.X, (int)currentAttackTarget.position.Y);
                MoveTowardsDestination(gameTime);
            }
        }

        void MoveTowardsDestination(GameTime gameTime)
        {
            int buffer = 0;

            int newX = (int)position.X;
            int newY = (int)position.Y;

//            double velocity = .15;
            double velocity = .07;
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


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            int x = (int) Math.Round(position.X);
            int y = (int) Math.Round(position.Y);
            int width = (int)(unscaledWidth * this.scale);
            int height = (int)(unscaledHeight * this.scale);

            x = x - (width / 2);
            y = y - (height / 2);

            clickDetectionRectangle = createClickDetectionRectangle();
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }


        public void OrderToMoveToDestination(int x, int y)
        {
            this.currentCommand = Command.MOVE_TO_POINT;
            this.state = State.MOVING;
            SetDestination(x, y);
        }


        internal void OrderToMoveToAndAttackEnemyUnit(Minigunner enemyMinigunner)
        {
            this.currentCommand = Command.ATTACK_TARGET;
            this.state = State.ATTACKING;
            currentAttackTarget = enemyMinigunner;
        }

    }


}
