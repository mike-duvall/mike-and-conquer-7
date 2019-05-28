
using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;
using mike_and_conquer.pathfinding;
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
        public Vector2 positionInWorldCoordinates { get; set; }
        public Point destination {
            get { return new Point(destinationX, destinationY);}
        }

        Rectangle clickDetectionRectangle;

        private Minigunner currentAttackTarget;

        public enum State { IDLE, MOVING, ATTACKING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE,  ATTACK_TARGET, FOLLOW_PATH };
        public Command currentCommand;

        private int destinationX;
        private int destinationY;

        public Vector2 DestinationPosition { get { return new Vector2(destinationX, destinationY); } }

        private List<Point> path;

        double movementVelocity = .015;
        double movementDistanceEpsilon;

        private static int globalId = 1;

        private Graph navigationGraph;

        Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        protected Minigunner()
        {
        }


        public Minigunner(int xInWorldCoordinates, int yInWorldCoordinates, Graph navigationGraph)
        {
            this.navigationGraph = navigationGraph;
            this.state = State.IDLE;
            this.currentCommand = Command.NONE;
            positionInWorldCoordinates = new Vector2(xInWorldCoordinates, yInWorldCoordinates);

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

            int x = (int)(positionInWorldCoordinates.X - (unitWidth / 2));
            int y = (int)(positionInWorldCoordinates.Y - unitHeight) + (int)(1);  

            Rectangle rectangle = new Rectangle(x,y,unitWidth,unitHeight);
            return rectangle;
        }



        private void HandleCommandNone(GameTime gameTime)
        {
            this.state = State.IDLE;
        }


        private void MoveTowardsCurrentDestinationInPath(GameTime gameTime)
        {
            this.state = State.MOVING;
            Point currentDestinationPoint = path[0];
            SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);
            MoveTowardsDestination(gameTime, currentDestinationPoint.X, currentDestinationPoint.Y);

            if (IsAtDestination(currentDestinationPoint.X, currentDestinationPoint.Y))
            {
                path.RemoveAt(0);
            }

        }

        private void LandOnFinalDestinationMapSquare(GameTime gameTime)
        {

            if (this.state == State.MOVING)
            {
                Point centerOfDestinationSquare = path[0];

                BasicMapSquare destinationBasicMapSquare =
                    GameWorld.instance.FindMapSquare(centerOfDestinationSquare.X, centerOfDestinationSquare.Y);

                Point currentDestinationPoint = destinationBasicMapSquare.GetDestinationSlotForMinigunner(this);
                SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);

            }

            this.state = State.LANDING_AT_MAP_SQUARE;

            MoveTowardsDestination(gameTime, destinationX, destinationY);

            if (IsAtDestination(destinationX, destinationY))
            {
                path.RemoveAt(0);
            }

        }



        private void HandleCommandFollowPath(GameTime gameTime)
        {
            if (path.Count > 1)
            {
                MoveTowardsCurrentDestinationInPath(gameTime);

            }
            else if (path.Count == 1)
            {

                // TODO:  Currently waiting until units almost arrive to assign
                // them slots on the destination square, but when
                // handling more than 5 units, will probably need to assign slots
                // when the move is initiated, rather than up on arrival
                LandOnFinalDestinationMapSquare(gameTime);
            }
            else
            {
                this.currentCommand = Command.NONE;
            }

        }

        private void HandleCommandAttackTarget(GameTime gameTime)
        {
            if (currentAttackTarget.health <= 0)
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
                if (path.Count > 0)
                {
                    MoveTowardsCurrentDestinationInPath(gameTime);

                }

            }
        }



        private bool IsFarEnoughRight(int destinationX)
        {
            return (positionInWorldCoordinates.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(int destinationX)
        {
            return (positionInWorldCoordinates.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(int destinationY)
        {
            return (positionInWorldCoordinates.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            return (positionInWorldCoordinates.Y < (destinationY + movementDistanceEpsilon));
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
            return (int)Distance(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, currentAttackTarget.positionInWorldCoordinates.X, currentAttackTarget.positionInWorldCoordinates.Y);
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



        void MoveTowardsDestination(GameTime gameTime, int destinationX, int destinationY)
        {

            float newX = positionInWorldCoordinates.X;
            float newY = positionInWorldCoordinates.Y;

            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * movementVelocity;

            float remainingDistanceX = Math.Abs(destinationX - positionInWorldCoordinates.X);
            float remainingDistanceY = Math.Abs(destinationY - positionInWorldCoordinates.Y);
            double deltaX = delta;
            double deltaY = delta;

            if (remainingDistanceX < deltaX)
            {
                deltaX = remainingDistanceX;
            }

            if (remainingDistanceY < deltaY)
            {
                deltaY = remainingDistanceY;
            }

            if (!IsFarEnoughRight(destinationX))
            {
                newX += (float)deltaX;
            }
            else if (!IsFarEnoughLeft(destinationX))
            {
                newX -= (float)deltaX;
            }

            if (!IsFarEnoughDown(destinationY))
            {
                newY += (float)deltaY;
            }
            else if (!IsFarEnoughUp(destinationY))
            {
                newY -= (float)deltaY;
            }


            // TODO:  Leaving in this commented out code for debugging movement issues.
            // Should remove it later if end up not needing it
//            float xChange = Math.Abs(positionInWorldCoordinates.X - newX);
//            float yChange = Math.Abs(positionInWorldCoordinates.Y - newY);
//            float changeThreshold = 0.10f;
//
//            if (xChange < changeThreshold && yChange < changeThreshold)
//            {
//                MikeAndConquerGame.instance.log.Information("delta:" + delta);
//                Boolean isFarEnoughRight = IsFarEnoughRight(destinationX);
//                Boolean isFarEnoughLeft = IsFarEnoughLeft(destinationX);
//                Boolean isFarEnoughDown = IsFarEnoughDown(destinationY);
//                Boolean isFarEnoughUp = IsFarEnoughUp(destinationY);
//
//                MikeAndConquerGame.instance.log.Information("isFarEnoughRight:" + isFarEnoughRight);
//                MikeAndConquerGame.instance.log.Information("isFarEnoughLeft:" + isFarEnoughLeft);
//                MikeAndConquerGame.instance.log.Information("isFarEnoughDown:" + isFarEnoughDown);
//                MikeAndConquerGame.instance.log.Information("isFarEnoughUp:" + isFarEnoughUp);
//                MikeAndConquerGame.instance.log.Information("old:positionInWorldCoordinates=" + positionInWorldCoordinates);
//                positionInWorldCoordinates = new Vector2(newX, newY);
//                MikeAndConquerGame.instance.log.Information("new:positionInWorldCoordinates=" + positionInWorldCoordinates);
//            }
//            else
//            {
//                positionInWorldCoordinates = new Vector2(newX, newY);
//            }

            positionInWorldCoordinates = new Vector2(newX, newY);

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

            BasicMapSquare currentMapSquareLocation =
                GameWorld.instance.FindMapSquare((int)this.positionInWorldCoordinates.X,
                    (int) this.positionInWorldCoordinates.Y);

            currentMapSquareLocation.ClearSlotForMinigunner(this);
            int startColumn = (int)this.positionInWorldCoordinates.X / 24;
            int startRow = (int)this.positionInWorldCoordinates.Y / 24;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destination.X / 24;
            destinationSquare.Y = destination.Y / 24;

            Path foundPath = aStar.FindPath(navigationGraph, startPoint, destinationSquare);

            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;

            List<Point> listOfPoints = new List<Point>();
            List<Node> nodeList = foundPath.nodeList;
            foreach (Node node in nodeList)
            {
                Point point = ConvertMapSquareIndexToWorldCoordinate(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);

        }

        private Point ConvertMapSquareIndexToWorldCoordinate(int index)
        {
            int numColumns = this.navigationGraph.width;
            Point point = new Point();
            int row = index / numColumns;
            int column = index - (row * numColumns);
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

            int startColumn = (int)this.positionInWorldCoordinates.X / 24;
            int startRow = (int)this.positionInWorldCoordinates.Y / 24;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = (int)enemyMinigunner.positionInWorldCoordinates.X / 24;
            destinationSquare.Y = (int)enemyMinigunner.positionInWorldCoordinates.Y / 24;


            Path foundPath = null;
            try
            {
                foundPath = aStar.FindPath(navigationGraph, startPoint, destinationSquare);
            }
            catch (Exception e)
            {
                MikeAndConquerGame.instance.log.Information("Exception thrown trying to find path");
                MikeAndConquerGame.instance.log.Information("startPoint.X:" + startPoint.X + " startPoint.Y:" + startPoint.Y);
                MikeAndConquerGame.instance.log.Information("destinationSquare.X:" + destinationSquare.X + " destinationSquare.Y:" + destinationSquare.Y);
                throw e;
            }

            this.currentCommand = Command.ATTACK_TARGET;
            this.state = State.ATTACKING;
            currentAttackTarget = enemyMinigunner;

            List<Point> listOfPoints = new List<Point>();
            List<Node> nodeList = foundPath.nodeList;
            foreach (Node node in nodeList)
            {
                Point point = ConvertMapSquareIndexToWorldCoordinate(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);


        }


        public Vector2 GetScreenPosition()
        {
            return Vector2.Transform(positionInWorldCoordinates, MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix);
        }


    }


}
