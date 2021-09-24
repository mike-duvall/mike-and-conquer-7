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

    // public class MCV : GameObject
    public class MCV : GameObject2
    {
        public bool selected { get; set; }

        public Point destination {
            get { return new Point(destinationX, destinationY);}
        }

        Rectangle clickDetectionRectangle;

        public enum State { IDLE, MOVING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE,  FOLLOW_PATH };
        public Command currentCommand;
        public Command previousCommand;

        private int destinationX;
        private int destinationY;
        public Vector2 DestinationPosition { get { return new Vector2(destinationX, destinationY); } }

        private List<Point> path;

        double movementDistanceEpsilon;

        private static int baseCncSpeedInLeptons = (int)CncSpeed.MPH_MEDIUM_SLOW;   // MCV speed

        private static readonly double baseMovementSpeedInWorldCoordinates = baseCncSpeedInLeptons * GameWorld.WorldUnitsPerLepton;
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
            // this.currentCommand = Command.NONE;
            UpdateCommand(Command.NONE);
//            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(xInWorldCoordinates, yInWorldCoordinates);
            gameWorldLocationDouble = GameWorldLocationDouble.CreateFromWorldCoordinates(xInWorldCoordinates, yInWorldCoordinates);

            //            id = Minigunner.globalId;
            //            Minigunner.globalId++;

            this.maxHealth = 1000;
            this.health = this.maxHealth;
            this.unitSize = new UnitSize(36, 36);
            this.selectionCursorOffset = new Point(-18, -14);


            clickDetectionRectangle = CreateClickDetectionRectangle();
            float currentGameSpeedDivisor = GameOptions.instance.CurrentGameSpeedDivisor();


            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();
            //scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / divisor;
            movementDistanceEpsilon = scaledMovementSpeed + (double).04f;

            selected = false;
        }

        public void Update(GameTime gameTime)
        {
            // goal:  12601
            // float divisor = 40.0f; // 12193
            // float divisor = 40.5f; // 12189

//            float divisor = 40.8f;  // 12252

            // float divisor = 40.85f; // 12403, 12404, 12402

            // float divisor = 40.875f; // 12403, 12406

            // float divisor = 40.89f;  // 12403

                //float divisor = 40.895f;  // elapsed time: 12404, 12402, 12404, scaledMovementSpeed = 0.0275094751780697



                float divisor = 40.898f;  // elapsed time: 12612, 12605, 12617, scaledMovementSpeed = 0.0275074583778908


            // float divisor = 40.9f; // 12613, 12604, 12613, 12605
            // float divisor = 41.0f; // 12615




            // scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();
            //            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / divisor;



            // scaledMovementSpeed = 0.02750846678;  // 12405
            // scaledMovementSpeed = 0.02750796258;  // 12525
            // scaledMovementSpeed = 0.02750771048; // 12525
            // scaledMovementSpeed = 0.02750758443; // 12524



            // scaledMovementSpeed = 0.02750755292; // 12525
            // scaledMovementSpeed = 0.02750753716; // 12516, 12524
            // scaledMovementSpeed = 0.02750752928;  // 12526
            // scaledMovementSpeed = 0.02750752879;  // 12525
             scaledMovementSpeed = 0.02750752867;  // 12523, 12527
            //   scaledMovementSpeed = 0.02750752864;  // 12615
            //   scaledMovementSpeed = 0.02750752861;  // 12614
            // scaledMovementSpeed = 0.02750752855;  // 12615
            // scaledMovementSpeed = 0.0275075283;  // 12615
            // scaledMovementSpeed = 0.02750752731;  // 12615
            // scaledMovementSpeed = 0.02750752534;  // 12614
            // scaledMovementSpeed = 0.0275075214; // 12614
            // scaledMovementSpeed = 0.0275074583778908; // 12614


            MikeAndConquerGame.instance.log.Information("scaledMovementSpeed2:" + scaledMovementSpeed);


            UpdateVisibleMapTiles();
            if (this.currentCommand == Command.NONE)
            {
                // if (previousCommand != null && previousCommand == Command.FOLLOW_PATH)
                // {
                //     int mcvId = -1; // Don't have ids for MCVs yet
                //     GameWorld.instance.PublisheGameHistoryEvent("StopMoving", mcvId);
                // }

                HandleCommandNone(gameTime);
            }
            else if (this.currentCommand == Command.FOLLOW_PATH)
            {
                // if (previousCommand != null && previousCommand == Command.NONE)
                // {
                //     int mcvId = -1; // Don't have ids for MCVs yet
                //     GameWorld.instance.PublisheGameHistoryEvent("StartMoving", mcvId);
                // }

                HandleCommandFollowPath(gameTime);
            }

        }

        private void UpdateCommand(Command newCommand)
        {


            previousCommand = this.currentCommand;
            this.currentCommand = newCommand;

            if (previousCommand == Command.NONE && this.currentCommand == Command.FOLLOW_PATH)
            {
                int mcvId = -1; // Don't have ids for MCVs yet
                GameWorld.instance.PublisheGameHistoryEvent("StartMoving", mcvId);
            }

            if (previousCommand == Command.FOLLOW_PATH && this.currentCommand == Command.NONE)
            {
                int mcvId = -1; // Don't have ids for MCVs yet
                GameWorld.instance.PublisheGameHistoryEvent("StopMoving", mcvId);
            }


        }

        private void UpdateVisibleMapTiles()
        {

            // TODO: Consider removing this if statement once map shroud is fully working
            if (GameOptions.instance.DrawShroud == false)
            {
                return;
            }

            // MapTileInstance possibleNewMapTileInstance =
            //     GameWorld.instance.FindMapTileInstance(
            //         MapTileLocation.CreateFromWorldCoordinatesInVector2(gameWorldLocation.WorldCoordinatesAsVector2));


            // Vector2 worldCoordinatesAsVector2 = new Vector2((float) gameWorldLocationDouble.X, (float) gameWorldLocationDouble.Y);

            MapTileInstance possibleNewMapTileInstance =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinatesInVector2(gameWorldLocationDouble.AsVector2));



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
//            Vector2 worldCoordinatesAsVector2 = new Vector2((float)gameWorldLocationDouble.X, (float)gameWorldLocationDouble.Y);


            // MapTileLocation mapTileLocation = MapTileLocation
            //     .CreateFromWorldCoordinatesInVector2(gameWorldLocation.WorldCoordinatesAsVector2)
            //     .IncrementWorldMapTileX(xOffset)
            //     .IncrementWorldMapTileY(yOffset);

            MapTileLocation mapTileLocation = MapTileLocation
                .CreateFromWorldCoordinatesInVector2(gameWorldLocationDouble.AsVector2)
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


            // Vector2 worldCoordinatesAsVector2 = new Vector2((float)gameWorldLocationDouble.X, (float)gameWorldLocationDouble.Y);


            // int x = (int)(gameWorldLocation.WorldCoordinatesAsVector2.X - (unitWidth / 2));
            // int y = (int)(gameWorldLocation.WorldCoordinatesAsVector2.Y - (unitHeight / 2)) + (int)(1);
            int x = (int)(gameWorldLocationDouble.X - (unitWidth / 2));
            int y = (int)(gameWorldLocationDouble.Y - (unitHeight / 2)) + (int)(1);



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
                // this.currentCommand = Command.NONE;
                UpdateCommand(Command.NONE);
            }



        }




        private bool IsFarEnoughRight(int destinationX)
        {
            // return (gameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
            return (gameWorldLocationDouble.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(int destinationX)
        {
            // return (gameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
            return (gameWorldLocationDouble.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(int destinationY)
        {
            // return (gameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
            return (gameWorldLocationDouble.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            // return (gameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
            return (gameWorldLocationDouble.Y < (destinationY + movementDistanceEpsilon));
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


            // float oldX = gameWorldLocation.WorldCoordinatesAsVector2.X;
            // float oldY = gameWorldLocation.WorldCoordinatesAsVector2.Y;
            //
            //
            // float newX = gameWorldLocation.WorldCoordinatesAsVector2.X;
            // float newY = gameWorldLocation.WorldCoordinatesAsVector2.Y;


            double oldX = gameWorldLocationDouble.X;
            double oldY = gameWorldLocationDouble.Y;


            double newX = gameWorldLocationDouble.X;
            double newY = gameWorldLocationDouble.Y;



            // double deltaScaler = 0.9;  // 82483
            // double deltaScaler = 0.95;  // 78165
            // double deltaScaler = 0.97;  // 76522




            // double deltaScaler = 0.98;  // 75750, 75740

            // double deltaScaler = 0.982;  // 75600
            double deltaScaler = 30;

            //            double deltaScaler = 0.985;  // 75365, 75366

            // double deltaScaler = 0.985;  // 75365, 75366


            //double deltaScaler = 0.986;  // 75347, 75349


            // double deltaScaler = 0.99;  // 75014, new measurement:  74998, new: 75013


            // double deltaScaler = 1.0; // Presume 74231

            // double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledMovementSpeed * deltaScaler;
            // double delta =  scaledMovementSpeed * deltaScaler;


            // double delta = 0.8252258601;  // 12525
            // double delta = 0.8252258597;   // 12614
            // double delta = 0.8252258592;  // 12613


            double delta = 0.8252258592; // 12594
            float deltaFloat = 0.8252258592f;


            // double delta = 0.8252258599;
            // double eps   = 0.068124999105930326;




            // float remainingDistanceX = Math.Abs(destinationX - gameWorldLocation.WorldCoordinatesAsVector2.X);
            // float remainingDistanceY = Math.Abs(destinationY - gameWorldLocation.WorldCoordinatesAsVector2.Y);
            double remainingDistanceX = Math.Abs(destinationX - gameWorldLocationDouble.X);
            double remainingDistanceY = Math.Abs(destinationY - gameWorldLocationDouble.Y);

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
                // newX += (float)deltaX;
                newX += deltaX;

            }
            else if (!IsFarEnoughLeft(destinationX))
            {
                // newX -= (float)deltaX;
                newX -= deltaX;
            }

            if (!IsFarEnoughDown(destinationY))
            {
                // newY += (float)deltaY;
                newY += deltaY;
            }
            else if (!IsFarEnoughUp(destinationY))
            {
                // newY -= (float)deltaY;
                newY -= deltaY;
            }


            // TODO:  Leaving in this commented out code for debugging movement issues.
            // Should remove it later if end up not needing it
            // float xChange = Math.Abs(oldX - newX);
            // float yChange = Math.Abs(oldY - newY);
            double xChange = Math.Abs(oldX - newX);
            double yChange = Math.Abs(oldY - newY);

            float changeThreshold = 0.10f;

            // if (xChange < changeThreshold && yChange < changeThreshold)
            // {
                MikeAndConquerGame.instance.log.Information("delta:" + delta);
                bool isFarEnoughRight = IsFarEnoughRight(destinationX);
                bool isFarEnoughLeft = IsFarEnoughLeft(destinationX);
                bool isFarEnoughDown = IsFarEnoughDown(destinationY);
                bool isFarEnoughUp = IsFarEnoughUp(destinationY);

                MikeAndConquerGame.instance.log.Information("isFarEnoughRight:" + isFarEnoughRight);
                MikeAndConquerGame.instance.log.Information("isFarEnoughLeft:" + isFarEnoughLeft);
                MikeAndConquerGame.instance.log.Information("isFarEnoughDown:" + isFarEnoughDown);
                MikeAndConquerGame.instance.log.Information("isFarEnoughUp:" + isFarEnoughUp);
                MikeAndConquerGame.instance.log.Information("old:positionInWorldCoordinates=" + oldX + "," + oldY);
                // positionInWorldCoordinates = new Vector2(newX, newY);
                MikeAndConquerGame.instance.log.Information("new:positionInWorldCoordinates=" + newX + "," + newY);
                MikeAndConquerGame.instance.log.Information("change:=" + xChange + "," + yChange);

            // }
            // else
            // {
            //     positionInWorldCoordinates = new Vector2(newX, newY);
            // }


            // gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(newX, newY);
            gameWorldLocationDouble = GameWorldLocationDouble.CreateFromWorldCoordinates(newX, newY);

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

            // int startColumn = (int)this.gameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
            // int startRow = (int)this.gameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;
            int startColumn = (int)this.gameWorldLocationDouble.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.gameWorldLocationDouble.Y / GameWorld.MAP_TILE_HEIGHT;

            Point startPoint = new Point(startColumn, startRow);

            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destination.X / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = destination.Y / GameWorld.MAP_TILE_HEIGHT;

            Path foundPath = aStar.FindPath(GameWorld.instance.navigationGraph, startPoint, destinationSquare);

            // this.currentCommand = Command.FOLLOW_PATH;
            UpdateCommand(Command.FOLLOW_PATH);
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
