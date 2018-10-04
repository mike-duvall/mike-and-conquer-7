
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;

using Path = mike_and_conquer.pathfinding.Path;
using AStar = mike_and_conquer.pathfinding.AStar;
using Node = mike_and_conquer.pathfinding.Node;


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

        enum Command { NONE, MOVE_TO_POINT, ATTACK_TARGET, FOLLOW_PATH };
        private Command currentCommand;


        private int destinationX;
        private int destinationY;

        public Vector2 DestinationPosition { get { return new Vector2(destinationX, destinationY); } }


        private List<Point> path;

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
            else if (this.currentCommand == Command.FOLLOW_PATH)
            {
                HandleCommandFollowPath(gameTime);
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
            MoveTowardsDestination(gameTime, destinationX, destinationY);
            if (IsAtDestination(destinationX, destinationY))
            {
                this.currentCommand = Command.NONE;
            }
        }

        

        private void HandleCommandFollowPath(GameTime gameTime)
        {
            if (path.Count > 0)
            {
                this.state = State.MOVING;
                Point currentDestinationPoint = path[0];
                SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);
                MoveTowardsDestination(gameTime,currentDestinationPoint.X, currentDestinationPoint.Y);
                if (IsAtDestination(currentDestinationPoint.X, currentDestinationPoint.Y))
                {
                    path.RemoveAt(0);
                }

            }
            else
            {
                this.currentCommand = Command.NONE;
            }

        }


        private bool IsFarEnoughRight(int destinationX)
        {
            return (position.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(int destinationX)
        {
            return (position.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(int destinationY)
        {
            return (position.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            return (position.Y < (destinationY + movementDistanceEpsilon));
        }


        private bool IsAtDestinationX(int destinationX)
        {
            return  (
                IsFarEnoughRight(destinationX) &&
                IsFarEnoughLeft(destinationX)
            );

        }

        private bool IsAtDestinationY(int destinationY)
        {
            return (
                IsFarEnoughDown(destinationY) &&
                IsFarEnoughUp(destinationY)
            );

        }


        private bool IsAtDestination(int destinationX, int destinationY)
        {
            return IsAtDestinationX(destinationX) && IsAtDestinationY(destinationY);
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
                MoveTowardsDestination(gameTime,destinationX, destinationY);
            }
        }

        void MoveTowardsDestination(GameTime gameTime, int destinationX, int destinationY)
        {

            float newX = position.X;
            float newY = position.Y;

            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * movementVelocity;
            //log.Information("delta:" + delta);


            if (!IsFarEnoughRight(destinationX))
            {
                newX += (float)delta;
            }
            else if (!IsFarEnoughLeft(destinationX))
            {
                newX -= (float)delta;
            }

            if (!IsFarEnoughDown(destinationY))
            {
                newY += (float)delta;
            }
            else if (!IsFarEnoughUp(destinationY))
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
            //            this.currentCommand = Command.MOVE_TO_POINT;
            //            this.state = State.MOVING;
            //            SetDestination(destination.X, destination.Y);

//            List<Point> listOfPoints = new List<Point>();
//            listOfPoints.Add(new Point(12 + 24, 12));
//            listOfPoints.Add(new Point(12, 12 + 24));
//            listOfPoints.Add(new Point(12 + 24, 12 + 48));
//            listOfPoints.Add(destination);
//            this.currentCommand = Command.FOLLOW_PATH;
//            this.state = State.MOVING;
//            this.SetPath(listOfPoints);
//            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);

//            Point startPoint = new Point(2, 0);
            int startColumn = (int)this.position.X / 24;
            int startRow = (int)this.position.Y / 24;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destination.X / 24;
            destinationSquare.Y = destination.Y / 24;
            
            Path foundPath = aStar.FindPath(MikeAndConquerGame.instance.navigationGraph, startPoint, destinationSquare);
            

            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;

            List<Point> listOfPoints = new List<Point>();
            List<Node> nodeList = foundPath.nodeList;
            foreach (Node node in nodeList)
            {
                Point point = ConvertMapSqaureIndexToMapPoint(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);

        }

        private Point ConvertMapSqaureIndexToMapPoint(int index)
        {
            Point point = new Point();
            int row = index / 26;
            int column = index - (row * 26);
            int widthOfMapSquare = 24;
            int heightOfMapSquare = 24;
            point.X = (column * widthOfMapSquare) + 12; ;
            point.Y = (row * heightOfMapSquare) + 12 ;
            return point;
        }

        public void OrderToFollowPath(List<Point> listOfPoints)
        {
            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;
            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);
        }

        private void SetPath(List<Point> listOfPoints)
        {
            this.path = listOfPoints;
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
