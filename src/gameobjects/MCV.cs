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


    public class MCV
    {
//        public int id { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }

        private GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation;  }
        }

        public Point destination {
            get { return new Point(destinationX, destinationY);}
        }

        Rectangle clickDetectionRectangle;


        public enum State { IDLE, MOVING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE,  FOLLOW_PATH };
        public Command currentCommand;

        private int destinationX;
        private int destinationY;

        public Vector2 DestinationPosition { get { return new Vector2(destinationX, destinationY); } }

        private List<Point> path;

        // double movementVelocity = .010;
        double movementDistanceEpsilon;


        private static int baseCncSpeed = (int)CncSpeed.MPH_MEDIUM_SLOW;
        // private static double baseMovementSpeed = 0.75f;
        // private static readonly double baseMovementSpeed = baseCncSpeed * 24.0 / 256.0;
        private static readonly double baseMovementSpeed = baseCncSpeed * GameWorld.WorldUnitsPerLepton;
        // double scaledMovementSpeed = .010;
        private double scaledMovementSpeed;


        //        private static int globalId = 1;


        Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        private MapTileInstance currentMapTileInstance;

        protected MCV()
        {
        }


        public MCV(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            this.state = State.IDLE;
            this.currentCommand = Command.NONE;
            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(xInWorldCoordinates, yInWorldCoordinates);

            health = 1000;
//            id = Minigunner.globalId;
//            Minigunner.globalId++;

            clickDetectionRectangle = CreateClickDetectionRectangle();
            // movementDistanceEpsilon = movementVelocity + (double).04f;
            scaledMovementSpeed = baseMovementSpeed / GameOptions.GAME_SPEED_DELAY_DIVISOR;
            movementDistanceEpsilon = scaledMovementSpeed + (double).04f;

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

        }

        private void UpdateVisibleMapTiles()
        {

            // TODO: Consider removing this if statement once map shroud is fully working
            if (GameOptions.DRAW_SHROUD == false)
            {
                return;
            }

            MapTileInstance possibleNewMapTileInstance =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinatesInVector2(gameWorldLocation.WorldCoordinatesAsVector2));

            if (possibleNewMapTileInstance == currentMapTileInstance)
            {
                return;
            }

            currentMapTileInstance = possibleNewMapTileInstance;



            // TODO:  Code south needs to handle literal edge cases where minigunner is near edge of 
            // map and there is NO east or west tile, etc
            UpdateNearbyMapTileVisibility(0, 0, MapTileInstance.MapTileVisibility.Visible);


            // top side
            UpdateNearbyMapTileVisibility(-2, -3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, -3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(0, -3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(1, -3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(2, -3, MapTileInstance.MapTileVisibility.PartiallyVisible);


            UpdateNearbyMapTileVisibility(-3, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, -2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, -2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, -2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(2, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(3, -2, MapTileInstance.MapTileVisibility.PartiallyVisible);


            UpdateNearbyMapTileVisibility(-3, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(2, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(3, -1, MapTileInstance.MapTileVisibility.PartiallyVisible);


            // same row
            UpdateNearbyMapTileVisibility(-3, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(2, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(3, 0, MapTileInstance.MapTileVisibility.PartiallyVisible);


            // bottom
            UpdateNearbyMapTileVisibility(-3, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(2, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(3, 1, MapTileInstance.MapTileVisibility.PartiallyVisible);

            UpdateNearbyMapTileVisibility(-3, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, 2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, 2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 2, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(2, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(3, 2, MapTileInstance.MapTileVisibility.PartiallyVisible);


            UpdateNearbyMapTileVisibility(-2, 3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(-1, 3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(0, 3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(1, 3, MapTileInstance.MapTileVisibility.PartiallyVisible);
            UpdateNearbyMapTileVisibility(2, 3, MapTileInstance.MapTileVisibility.PartiallyVisible);

        }

        private void UpdateNearbyMapTileVisibility(int xOffset, int yOffset, MapTileInstance.MapTileVisibility mapTileVisibility)
        {

            MapTileLocation mapTileLocation = MapTileLocation
                .CreateFromWorldCoordinatesInVector2(gameWorldLocation.WorldCoordinatesAsVector2)
                .IncrementWorldMapTileX(xOffset)
                .IncrementWorldMapTileY(yOffset);

            MapTileInstance mapTileInstance = GameWorld.instance.FindMapTileInstanceAllowNull(mapTileLocation);

            if (mapTileInstance != null && mapTileInstance.Visibility != MapTileInstance.MapTileVisibility.Visible)
            {
                mapTileInstance.Visibility = mapTileVisibility;
            }

        }

        internal Rectangle CreateClickDetectionRectangle()
        {

            int unitWidth = 24;
            int unitHeight = 24;


            int x = (int)(gameWorldLocation.WorldCoordinatesAsVector2.X - (unitWidth / 2));
            int y = (int)(gameWorldLocation.WorldCoordinatesAsVector2.Y - (unitHeight / 2)) + (int)(1);



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

                Point currentDestinationPoint = centerOfDestinationSquare;

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




        private bool IsFarEnoughRight(int destinationX)
        {
            return (gameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(int destinationX)
        {
            return (gameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(int destinationY)
        {
            return (gameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            return (gameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
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



        void MoveTowardsDestination(GameTime gameTime, int destinationX, int destinationY)
        {

            float newX = gameWorldLocation.WorldCoordinatesAsVector2.X;
            float newY = gameWorldLocation.WorldCoordinatesAsVector2.Y;

            // double delta = gameTime.ElapsedGameTime.TotalMilliseconds * movementVelocity;
            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledMovementSpeed;


            float remainingDistanceX = Math.Abs(destinationX - gameWorldLocation.WorldCoordinatesAsVector2.X);
            float remainingDistanceY = Math.Abs(destinationY - gameWorldLocation.WorldCoordinatesAsVector2.Y);
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

            int startColumn = (int)this.gameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.gameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destination.X / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = destination.Y / GameWorld.MAP_TILE_HEIGHT;

            Path foundPath = aStar.FindPath(GameWorld.instance.navigationGraph, startPoint, destinationSquare);

            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;

            List<Point> listOfPoints = new List<Point>();
            List<Node> nodeList = foundPath.nodeList;
            foreach (Node node in nodeList)
            {
                Point point = GameWorld.instance.ConvertMapSquareIndexToWorldCoordinate(node.id);
                listOfPoints.Add(point);
            }

            this.SetPath(listOfPoints);
            SetDestination(listOfPoints[0].X, listOfPoints[0].Y);
        }

        


        private void SetPath(List<Point> listOfPoints)
        {
            this.path = listOfPoints;
        }



    }


}
