
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;

using Serilog;

namespace mike_and_conquer
{ 

    public class Minigunner
    {
        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public Vector2 position { get; set; }
        public Point destination {
            get { return new Point(destinationX, destinationY);}
        }

        Rectangle clickDetectionRectangle;

        private Minigunner currentAttackTarget;

        public enum State { IDLE, MOVING, ATTACKING };
        public State state;

        enum Command { NONE, MOVE_TO_POINT, ATTACK_TARGET };
        private Command currentCommand;


        private int destinationX;
        private int destinationY;

        public Vector2 DestinationPosition { get { return new Vector2(destinationX, destinationY); } }

        double movementVelocity = .015;
        double movementDistanceEpsilon;

        private static int globalId = 1;

        Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        protected Minigunner()
        {
        }


        public Minigunner(int x, int y)
        {
            this.state = State.IDLE;
            this.currentCommand = Command.NONE;
            position = new Vector2(x, y);

            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;

            clickDetectionRectangle = CreateClickDetectionRectangle();
            movementDistanceEpsilon = movementVelocity + (double).04f;
            selected = false;
        }

        public void Update(GameTime gameTime)
        {

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


        internal Rectangle CreateClickDetectionRectangle()
        {

            int unitWidth = 12;
            int unitHeight = 12;

            int x = (int)(position.X - (unitWidth / 2));
            int y = (int)(position.Y - unitHeight) + (int)(1);  

            Rectangle rectangle = new Rectangle(x,y,unitWidth,unitHeight);
            return rectangle;
        }



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

        private bool IsFarEnoughRight()
        {
            return (position.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft()
        {
            return (position.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown()
        {
            return (position.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp()
        {
            return (position.Y < (destinationY + movementDistanceEpsilon));
        }


        private bool IsAtDestinationX()
        {
            return  (
                IsFarEnoughRight() &&
                IsFarEnoughLeft()
            );

        }

        private bool IsAtDestinationY()
        {
            return (
                IsFarEnoughDown() &&
                IsFarEnoughUp()
            );

        }


        private bool IsAtDestination()
        {
            return IsAtDestinationX() && IsAtDestinationY();
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

            if (distanceToTarget < 35)
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

            float newX = position.X;
            float newY = position.Y;

            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * movementVelocity;
            //log.Information("delta:" + delta);


            if (!IsFarEnoughRight())
            {
                newX += (float)delta;
            }
            else if (!IsFarEnoughLeft())
            {
                newX -= (float)delta;
            }

            if (!IsFarEnoughDown())
            {
                newY += (float)delta;
            }
            else if (!IsFarEnoughUp())
            {
                newY -= (float)delta;
            }

            position = new Vector2(newX, newY);
//            MikeAndConquerGame.log.Debug("position=" + position);
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
            clickDetectionRectangle = CreateClickDetectionRectangle();
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }


        public void OrderToMoveToDestination(Point destination)
        {
            this.currentCommand = Command.MOVE_TO_POINT;
            this.state = State.MOVING;
            SetDestination(destination.X, destination.Y);
        }

        public void OrderToFollowPath(List<Point> listOfPoints)
        {
            return;
        }



        internal void OrderToMoveToAndAttackEnemyUnit(Minigunner enemyMinigunner)
        {
            this.currentCommand = Command.ATTACK_TARGET;
            this.state = State.ATTACKING;
            currentAttackTarget = enemyMinigunner;
        }


        public Vector2 GetScreenPosition()
        {
            return Vector2.Transform(position, MikeAndConquerGame.instance.camera2D.TransformMatrix);
        }


    }


}
