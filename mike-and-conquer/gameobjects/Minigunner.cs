
using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;
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

        double movementVelocity = .010;
        double movementDistanceEpsilon;

        private static int globalId = 1;

        private GameWorld gameWorld;

        private MapTileInstance currentMapTileInstance;

        Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        protected Minigunner()
        {
        }


        public Minigunner(int xInWorldCoordinates, int yInWorldCoordinates, GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;

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

            UpdateVisibleMapTiles();
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

        private void UpdateVisibleMapTiles()
        {

            // TODO: Consider removing this if statement once map shroud is fully working
            if (GameOptions.DRAW_SHROUD == false)
            {
                return;
            }

            MapTileInstance possibleNewMapTileInstance =
                GameWorld.instance.FindMapTileInstance((int) positionInWorldCoordinates.X, (int) positionInWorldCoordinates.Y);

            if (possibleNewMapTileInstance == currentMapTileInstance)
            {
                return;
            }

            currentMapTileInstance = possibleNewMapTileInstance;

            // TODO:  Code south needs to handle literal edge cases where minigunner is near edge of 
            // map and there is NO east or west tile, etc
            UpdateNearbyMapTileVisibility(0, 0, MapTileInstance.MapTileVisibility.Visible);

            // east side
            if (IsSpecialCaseForSouthEastMapTileInstance())
            {
                UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 0, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

                UpdateNearbyMapTileVisibility(2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);

                UpdateNearbyMapTileVisibility(3, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(3, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(3, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            }
            else if (IsSpecialCaseForNorthEastMapTileInstance())
            {

                UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 0, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

                UpdateNearbyMapTileVisibility(2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 0, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(2, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);

                UpdateNearbyMapTileVisibility(3, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(3, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(3, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
            }
            else
            {
                UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 0, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

                UpdateNearbyMapTileVisibility(2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);

            }




            // west side
            UpdateNearbyMapTileVisibility(-1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);

            UpdateNearbyMapTileVisibility(-2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);



            // north side
            UpdateNearbyMapTileVisibility(-1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);

            UpdateNearbyMapTileVisibility(-2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(0, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(1, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);


            if (this.id == 9)
            {
                int x = 3;
            }
            // south side
            if (IsSpecialCaseForSouthWestMapTileInstance())
            {
                MikeAndConquerGame.instance.log.Information("IsSpecialCaseForSouthWestMapTileInstance() returned true.  minigunner.id=" + this.id);
                UpdateNearbyMapTileVisibility(-2, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(0, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

                UpdateNearbyMapTileVisibility(-2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(-1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(0, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);

            }
            else
            {
                UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(0, 1, MapTileInstance.MapTileVisibility.Visible);
                UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

                UpdateNearbyMapTileVisibility(-2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(-1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(0, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
                UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            }


            foreach(MapTileInstance mapTileInstance in GameWorld.instance.gameMap.MapTileInstanceList)
            {
                UpdateToVisibleIfSurroundedByVisibleTiles(mapTileInstance);
            }


        }

        private bool MapTileHasVisibility(MapTileInstance mapTileInstance, MapTileInstance.MapTileVisibility expectedVisbility)
        {
            return mapTileInstance != null && mapTileInstance.Visibility == expectedVisbility;

        }

        private void UpdateToVisibleIfSurroundedByVisibleTiles(MapTileInstance mapTileInstance)
        {
            
            MapTileInstance northMapTile = FindNearbyMapTileByOffset(mapTileInstance.PositionInWorldCoordinates,0, -1);
            MapTileInstance eastMapTile = FindNearbyMapTileByOffset(mapTileInstance.PositionInWorldCoordinates, 1, 0);
            MapTileInstance southMapTile = FindNearbyMapTileByOffset(mapTileInstance.PositionInWorldCoordinates, 0, 1);
            MapTileInstance westMapTile = FindNearbyMapTileByOffset(mapTileInstance.PositionInWorldCoordinates, -1, 0);

            int numAdjectTilesVisible = 0;

            if (MapTileHasVisibility(northMapTile, MapTileInstance.MapTileVisibility.Visible))
            {
                numAdjectTilesVisible++;
            }
            if (MapTileHasVisibility(eastMapTile, MapTileInstance.MapTileVisibility.Visible))
            {
                numAdjectTilesVisible++;
            }
            if (MapTileHasVisibility(southMapTile, MapTileInstance.MapTileVisibility.Visible))
            {
                numAdjectTilesVisible++;
            }
            if (MapTileHasVisibility(westMapTile, MapTileInstance.MapTileVisibility.Visible))
            {
                numAdjectTilesVisible++;
            }

            if (numAdjectTilesVisible > 2)
            {
                mapTileInstance.Visibility = MapTileInstance.MapTileVisibility.Visible;
            }


        }

        private bool IsSpecialCaseForSouthEastMapTileInstance()
        {
            bool isSpecialCase = false;
            MapTileInstance mapTile1 = FindNearbyMapTileByOffset(1, 0);
            MapTileInstance mapTile2 = FindNearbyMapTileByOffset(1, 1);
            MapTileInstance mapTile3 = FindNearbyMapTileByOffset(2, 1);

            if (MapTileHasVisibility(mapTile1, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile2, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile3, MapTileInstance.MapTileVisibility.PartiallyVisible) )
            {
                isSpecialCase = true;
            }

            return isSpecialCase;
        }

        private bool IsSpecialCaseForNorthEastMapTileInstance()
        {

            bool isSpecialCase = false;
            MapTileInstance mapTile1 = FindNearbyMapTileByOffset(1, 0);
            MapTileInstance mapTile2 = FindNearbyMapTileByOffset(1, -1);
            MapTileInstance mapTile3 = FindNearbyMapTileByOffset(2, 0);

            if (MapTileHasVisibility(mapTile1, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile2, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile3, MapTileInstance.MapTileVisibility.PartiallyVisible))
            {
                isSpecialCase = true;
            }


            return isSpecialCase;
        }



        private bool IsSpecialCaseForSouthWestMapTileInstance()
        {
            if (true)
                return false;

            bool isSpecialCase = false;
//            MapTileInstance mapTile1 = FindNearbyMapTileByOffset(-1, 2);
//            MapTileInstance mapTile2 = FindNearbyMapTileByOffset(-2, 2);
//            MapTileInstance mapTile3 = FindNearbyMapTileByOffset(-2, 3);

            MapTileInstance mapTile1 = FindNearbyMapTileByOffset(-1, 1);
            MapTileInstance mapTile2 = FindNearbyMapTileByOffset(-2, 1);
            MapTileInstance mapTile3 = FindNearbyMapTileByOffset(-2, 2);


            if (MapTileHasVisibility(mapTile1, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile2, MapTileInstance.MapTileVisibility.PartiallyVisible) &&
                MapTileHasVisibility(mapTile3, MapTileInstance.MapTileVisibility.PartiallyVisible))
            {
                isSpecialCase = true;
            }


            if (this.id == 10)
            {
                string message = "IsSpecialCaseForSouthWestMapTileInstance() info for minigunner.id=" + this.id + ",isSpecialCase=" + isSpecialCase;
                message += ", mapTile1.Visibility=" + mapTile1.Visibility + ",mapTile1.Position" +
                           mapTile1.PositionInWorldCoordinates +
                           ",mapTile2.Visibility=" + mapTile2.Visibility + ",mapTile2.Position" + mapTile2.PositionInWorldCoordinates +
                            ",mapTile3.Visibility=" + mapTile3.Visibility + ",mapTile3.Position" +
                           mapTile3.PositionInWorldCoordinates;
                MikeAndConquerGame.instance.log.Information(message);

            }
            return isSpecialCase;
        }


        MapTileInstance FindNearbyMapTileByOffset(Vector2 basePosition, int xOffset, int yOffset)
        {
            xOffset = xOffset * 24;
            yOffset = yOffset * 24;
            MapTileInstance mapTileInstance = GameWorld.instance.FindMapTileInstanceAllowNull((int)basePosition.X + xOffset, (int)basePosition.Y + yOffset);
            return mapTileInstance;

        }




        MapTileInstance FindNearbyMapTileByOffset(int xOffset, int yOffset)
        {
            return FindNearbyMapTileByOffset(this.positionInWorldCoordinates, xOffset, yOffset);

        }

        private void UpdateNearbyMapTileVisibility(int xOffset, int yOffset, MapTileInstance.MapTileVisibility mapTileVisibility)
        {
            MapTileInstance mapTileInstance = FindNearbyMapTileByOffset(xOffset, yOffset);

            if (mapTileInstance != null && mapTileInstance.PositionInWorldCoordinates.X == 612 &&
                mapTileInstance.PositionInWorldCoordinates.Y == 276 && mapTileVisibility == MapTileInstance.MapTileVisibility.Visible)
            {
                int x = 3;
            }
            if (mapTileInstance != null && mapTileInstance.Visibility != MapTileInstance.MapTileVisibility.Visible)
            {
                mapTileInstance.Visibility = mapTileVisibility;
            }


//            if (mapTileInstance != null && mapTileInstance.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
//            {
//                MapTileInstance northInstance = gameWorld.FindMapTileInstanceAllowNull(
//                    (int) mapTileInstance.PositionInWorldCoordinates.X, (int) mapTileInstance.PositionInWorldCoordinates.Y - GameWorld.MAP_TILE_HEIGHT);
//
//                MapTileInstance eastInstance = gameWorld.FindMapTileInstanceAllowNull(
//                    (int)mapTileInstance.PositionInWorldCoordinates.X + GameWorld.MAP_TILE_WIDTH, (int)mapTileInstance.PositionInWorldCoordinates.Y );
//
//                MapTileInstance southInstance = gameWorld.FindMapTileInstanceAllowNull(
//                    (int)mapTileInstance.PositionInWorldCoordinates.X + GameWorld.MAP_TILE_WIDTH, (int)mapTileInstance.PositionInWorldCoordinates.Y + GameWorld.MAP_TILE_HEIGHT);
//
//                if (MapTileHasVisibility(northInstance, MapTileInstance.MapTileVisibility.Visible) &&
//                    MapTileHasVisibility(eastInstance, MapTileInstance.MapTileVisibility.Visible) &&
//                    MapTileHasVisibility(southInstance, MapTileInstance.MapTileVisibility.Visible))
//                {
//                    mapTileInstance.Visibility = MapTileInstance.MapTileVisibility.Visible;
//                }
//
//            }

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

                MapTileInstance destinationMapTileInstance =
                    gameWorld.FindMapTileInstance(centerOfDestinationSquare.X, centerOfDestinationSquare.Y);

                Point currentDestinationPoint = destinationMapTileInstance.GetDestinationSlotForMinigunner(this);
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

            MapTileInstance currentMapTileInstanceLocation =
                gameWorld.FindMapTileInstance((int)this.positionInWorldCoordinates.X,
                    (int) this.positionInWorldCoordinates.Y);

            currentMapTileInstanceLocation.ClearSlotForMinigunner(this);
            int startColumn = (int)this.positionInWorldCoordinates.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.positionInWorldCoordinates.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destination.X / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = destination.Y / GameWorld.MAP_TILE_HEIGHT;

            Path foundPath = aStar.FindPath(gameWorld.navigationGraph, startPoint, destinationSquare);

            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;

            List<Point> listOfPoints = new List<Point>();
            List<Node> nodeList = foundPath.nodeList;
            foreach (Node node in nodeList)
            {
                Point point = gameWorld.ConvertMapSquareIndexToWorldCoordinate(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);
        }



        private void SetPath(List<Point> listOfPoints)
        {
            this.path = listOfPoints;
        }


        internal void OrderToMoveToAndAttackEnemyUnit(Minigunner enemyMinigunner)
        {

            int startColumn = (int)this.positionInWorldCoordinates.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.positionInWorldCoordinates.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = (int)enemyMinigunner.positionInWorldCoordinates.X / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = (int)enemyMinigunner.positionInWorldCoordinates.Y / GameWorld.MAP_TILE_HEIGHT;


            Path foundPath = null;
            try
            {
                foundPath = aStar.FindPath(this.gameWorld.navigationGraph, startPoint, destinationSquare);
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
                Point point = gameWorld.ConvertMapSquareIndexToWorldCoordinate(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);


        }



    }


}
