using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using Serilog;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Math = System.Math;
using Point = Microsoft.Xna.Framework.Point;

using Path = mike_and_conquer.pathfinding.Path;
using AStar = mike_and_conquer.pathfinding.AStar;
using Node = mike_and_conquer.pathfinding.Node;


namespace mike_and_conquer.gameobjects
{




    public class Minigunner : GameObject
    {
        public int id { get; set; }

        public bool selected { get; set; }

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

//        private double reloadTimeInSeconds = 4.5f;
        private double reloadTimeInSeconds;
        private double reloadCountdownTimer;

        // CnC source code shows Infantry speed to be CncSpeed.MPH_SLOW, which is 8, but my empirical tests
        // of actual Cnc game show it to be about speed 11, when compared to MCV and Jeep speeds
        // I don't fully understand why, but think it has to do with differences in Cnc in how
        // Infantry is moved vs how vehicles are moved.  It appears to be completely different code
        // In addition, in Cnc, minigunners moving left to right are slower than minigunners
        // moving right to left.  So the code seems suspect....
        // Manually tweaking the speed I use to match empirical measurements
        //private static int baseCncSpeedInLeptons = (int) CncSpeed.MPH_SLOW;  // MPH_SLOW == 8
        private static int baseCncSpeedInLeptons = 11;  // Calibrated based on empirical tests
        private static readonly double baseMovementSpeedInWorldCoordinates = baseCncSpeedInLeptons * GameWorld.WorldUnitsPerLepton;
        private double scaledMovementSpeed;
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
            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(xInWorldCoordinates, yInWorldCoordinates);

            this.maxHealth = 50;
            this.health = this.maxHealth;
            this.unitSize = new UnitSize(12, 16);
            this.selectionCursorOffset = new Point(-6, -10);



            id = Minigunner.globalId;
            Minigunner.globalId++;

            clickDetectionRectangle = CreateClickDetectionRectangle();
            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();
            movementDistanceEpsilon = scaledMovementSpeed + (double).04f;
            selected = false;
            reloadCountdownTimer = 0;
            int minigunnerROF = 20;
            double divisor = 1111.111111111111111111111111;

            // double dmrof = 20;
            // double dgameSpeed = GameOptions.instance.GameSpeedDelayDivisor;
            // double dr = dmrof * dgameSpeed / divisor;


            reloadTimeInSeconds = minigunnerROF * GameOptions.instance.CurrentGameSpeedDivisor() / divisor;
            MikeAndConquerGame.instance.log.Information("GameOptions.instance.CurrentGameSpeedDivisor():" + GameOptions.instance.CurrentGameSpeedDivisor());
            MikeAndConquerGame.instance.log.Information("reloadTimeInSeconds:" + reloadTimeInSeconds);
            int x = 3;

        }



        // private double previousElapsedTimeTotalMilliseconds;

        public void Update(GameTime gameTime)
        {

            // if (id == 1)
            // {
            //     double elapsedTimeTotalMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            //     MikeAndConquerGame.instance.log.Information("elapsedTimeTotalMilliseconds.:" + elapsedTimeTotalMilliseconds);
            // }
            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();

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
            if (GameOptions.instance.DrawShroud == false)
            {
                return;
            }

            MapTileInstance possibleNewMapTileInstance =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinatesInVector2(GameWorldLocation.WorldCoordinatesAsVector2));

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


//            if (this.id == 9)
//            {
//                int x = 3;
//            }
            // south side
            UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

            UpdateNearbyMapTileVisibility(-2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(0, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(1, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);



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
            
            MapTileInstance northMapTile = FindNearbyMapTileByOffset(mapTileInstance.MapTileLocation.WorldCoordinatesAsVector2,0, -1);
            MapTileInstance eastMapTile = FindNearbyMapTileByOffset(mapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, 1, 0);
            MapTileInstance southMapTile = FindNearbyMapTileByOffset(mapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, 0, 1);
            MapTileInstance westMapTile = FindNearbyMapTileByOffset(mapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, -1, 0);

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


        MapTileInstance FindNearbyMapTileByOffset(Vector2 basePosition, int xOffset, int yOffset)
        {
            MapTileLocation offsetMapTileLocation = 
                MapTileLocation.CreateFromWorldCoordinatesInVector2(basePosition)
                    .IncrementWorldMapTileX(xOffset)
                    .IncrementWorldMapTileY(yOffset);

            MapTileInstance mapTileInstance = GameWorld.instance.FindMapTileInstanceAllowNull(offsetMapTileLocation);
            return mapTileInstance;
        }


        MapTileInstance FindNearbyMapTileByOffset(int xOffset, int yOffset)
        {
            return FindNearbyMapTileByOffset(this.GameWorldLocation.WorldCoordinatesAsVector2, xOffset, yOffset);

        }

        private void UpdateNearbyMapTileVisibility(int xOffset, int yOffset, MapTileInstance.MapTileVisibility mapTileVisibility)
        {
            MapTileInstance mapTileInstance = FindNearbyMapTileByOffset(xOffset, yOffset);

            // if (mapTileInstance != null && mapTileInstance.PositionInWorldCoordinates.X == 612 &&
            //     mapTileInstance.PositionInWorldCoordinates.Y == 276 && mapTileVisibility == MapTileInstance.MapTileVisibility.Visible)
            // {
            //     int x = 3;
            // }
            if (mapTileInstance != null && mapTileInstance.Visibility != MapTileInstance.MapTileVisibility.Visible)
            {
                mapTileInstance.Visibility = mapTileVisibility;
            }

        }


        internal Rectangle CreateClickDetectionRectangle()
        {

            int unitWidth = 12;
            int unitHeight = 12;

            int x = (int)(GameWorldLocation.WorldCoordinatesAsVector2.X - (unitWidth / 2));
            int y = (int)(GameWorldLocation.WorldCoordinatesAsVector2.Y - unitHeight) + (int)(1);  

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
                    gameWorld.FindMapTileInstance(
                        MapTileLocation.CreateFromWorldCoordinates(centerOfDestinationSquare.X,
                            centerOfDestinationSquare.Y));

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
                if (reloadCountdownTimer <= 0.0f)
                {
                    currentAttackTarget.ReduceHealth(10);
                    GameWorld.instance.PublisheGameHistoryEvent("FirePrimaryWeapon", this.id);
                    reloadCountdownTimer = reloadTimeInSeconds;
                }
                else
                {
                    double totalElapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
                    double totalElapsedSeconds = totalElapsedMilliseconds / 1000.0f;
                    reloadCountdownTimer = reloadCountdownTimer - totalElapsedSeconds;
                }
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
            return (GameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(int destinationX)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(int destinationY)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
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
            return (int)Distance(GameWorldLocation.WorldCoordinatesAsVector2.X, GameWorldLocation.WorldCoordinatesAsVector2.Y, currentAttackTarget.GameWorldLocation.WorldCoordinatesAsVector2.X, currentAttackTarget.GameWorldLocation.WorldCoordinatesAsVector2.Y);
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

            float newX = GameWorldLocation.WorldCoordinatesAsVector2.X;
            float newY = GameWorldLocation.WorldCoordinatesAsVector2.Y;

            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledMovementSpeed;

            float remainingDistanceX = Math.Abs(destinationX - GameWorldLocation.WorldCoordinatesAsVector2.X);
            float remainingDistanceY = Math.Abs(destinationY - GameWorldLocation.WorldCoordinatesAsVector2.Y);
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

            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(newX, newY);

        }


        private void SetDestination(int x, int y)
        {
            destinationX = x;
            destinationY = y;
        }


        public void ReduceHealth(int amount)
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
                gameWorld.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinatesInVector2(
                        this.GameWorldLocation.WorldCoordinatesAsVector2));

            currentMapTileInstanceLocation.ClearSlotForMinigunner(this);
            int startColumn = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;
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

            int startColumn = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = (int)enemyMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = (int)enemyMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;


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
