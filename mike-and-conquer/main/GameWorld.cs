
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameview;
using mike_and_conquer.pathfinding;
using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
using CreateNodMinigunnerGameEvent = mike_and_conquer.gameevent.CreateNodMinigunnerGameEvent;
using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;
using ResetGameGameEvent = mike_and_conquer.gameevent.ResetGameGameEvent;
using GetCurrentGameStateGameEvent = mike_and_conquer.gameevent.GetCurrentGameStateGameEvent;
using CreateSandbagGameEvent = mike_and_conquer.gameevent.CreateSandbagGameEvent;

using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController;
using Exception = System.Exception;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;


namespace mike_and_conquer

{
    public class GameWorld
    {
        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }
        public List<Sandbag> sandbagList;

        public NavigationGraph navigationGraph;

        private List<AsyncGameEvent> gameEvents;

        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }


        public GameMap gameMap;

        public static GameWorld instance;

        public GameWorld()
        {
            gdiMinigunnerList = new List<Minigunner>();
            nodMinigunnerList = new List<Minigunner>();
            sandbagList = new List<Sandbag>();

            gameEvents = new List<AsyncGameEvent>();

            nodMinigunnerAIControllerList = new List<MinigunnerAIController>();

            GameWorld.instance = this;
        }

        public GameState HandleReset()
        {
            gdiMinigunnerList.Clear();
            nodMinigunnerList.Clear();
            sandbagList.Clear();
            return new PlayingGameState();
        }




        public void InitializeDefaultMap()
        {
            LoadMap();
            navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
        }

        public void InitializeTestMap(int[,] obstacleArray)
        {
            gameMap = new GameMap(obstacleArray);
            navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
            InitializeNavigationGraph();
        }


        private void LoadMap()
        {

            System.IO.Stream inputStream = new FileStream("Content\\scg01ea.bin", FileMode.Open);

            //  (Starting at 0x13CC in the file)

            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            gameMap = new GameMap(inputStream, startX, startY, endX, endY);
        }



        internal Minigunner GetGdiMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }
            }

            return foundMinigunner;
        }


        internal Minigunner GetNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }

            }
            return foundMinigunner;
        }


        internal Minigunner GetGdiOrNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foundMinigunner = GetGdiMinigunner(id);
            if (foundMinigunner == null)
            {
                foundMinigunner = GetNodMinigunner(id);
            }
            return foundMinigunner;
        }

        internal void SelectSingleGDIUnit(Minigunner minigunner)
        {
            minigunner.selected = true;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.id != minigunner.id)
                {
                    nextMinigunner.selected = false;
                }
            }

        }

        public void Update(GameTime gameTime)
        {
            UpdateAIControllers(gameTime);
            UpdateGDIMinigunners(gameTime);
            UpdateNodMinigunners(gameTime);
        }


        private void UpdateNodMinigunners(GameTime gameTime)
        {
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }
        }

        private void UpdateGDIMinigunners(GameTime gameTime)
        {
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }
        }

        private void UpdateAIControllers(GameTime gameTime)
        {
            foreach (MinigunnerAIController nextMinigunnerAIController in nodMinigunnerAIControllerList)
            {
                nextMinigunnerAIController.Update(gameTime);
            }
        }




        public class BadMinigunnerLocationException : Exception
        {
            public BadMinigunnerLocationException(Point badLocation)
                : base("Bad minigunner location.  x:" + badLocation.X + ", y:" + badLocation.Y)
            {
            }
        }


        private void AssertIsValidMinigunnerPosition(Point positionInWorldCoordinates)
        {
            foreach (MapTileInstance nexBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nexBasicMapSquare.ContainsPoint(positionInWorldCoordinates) &&
                    nexBasicMapSquare.IsBlockingTerrain)
                {
                    throw new BadMinigunnerLocationException(positionInWorldCoordinates);
                }
            }
        }


        public Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {
            
            AssertIsValidMinigunnerPosition(positionInWorldCoordinates);

            Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this.navigationGraph);
            gdiMinigunnerList.Add(newMinigunner);
            return newMinigunner;
        }


        public Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {

            AssertIsValidMinigunnerPosition(positionInWorldCoordinates);


            Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this.navigationGraph);
            this.nodMinigunnerList.Add(newMinigunner);

            // TODO:  In future, don't couple Nod having to be AI controlled enemy
            if (aiIsOn)
            {
                MinigunnerAIController minigunnerAIController = new MinigunnerAIController(newMinigunner);
                nodMinigunnerAIControllerList.Add(minigunnerAIController);
            }

            return newMinigunner;
        }


        public GameState ProcessGameEvents()
        {
            GameState newGameState = null;

            lock (gameEvents)
            {
                foreach (AsyncGameEvent nextGameEvent in gameEvents)
                {
                    GameState returnedGameState = nextGameEvent.Process();
                    if (returnedGameState != null && newGameState == null)
                    {
                        newGameState = returnedGameState;
                    }
                }
                gameEvents.Clear();
            }

            return newGameState;

        }


        public Minigunner CreateGDIMinigunnerViaEvent(Point minigunnerPosition)
        {
            CreateGDIMinigunnerGameEvent gameEvent = new CreateGDIMinigunnerGameEvent(minigunnerPosition);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;

        }

        public Sandbag CreateSandbagViaEvent(int x, int y, int index)
        {
            CreateSandbagGameEvent gameEvent = new CreateSandbagGameEvent(x, y, index);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Sandbag sandbag = gameEvent.GetSandbag();
            return sandbag;

        }

        public Minigunner GetGDIMinigunnerByIdViaEvent(int id)
        {
            GetGDIMinigunnerByIdGameEvent gameEvent = new GetGDIMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public Minigunner CreateNodMinigunnerViaEvent(Point positionInWorldCoordinates, bool aiIsOn)
        {
            CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(positionInWorldCoordinates, aiIsOn);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner minigunner = gameEvent.GetMinigunner();
            return minigunner;

        }


        public Minigunner GetNodMinigunnerByIdViaEvent(int id)
        {
            GetNodMinigunnerByIdGameEvent gameEvent = new GetNodMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public void ResetGameViaEvent()
        {
            ResetGameGameEvent gameEvent = new ResetGameGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

        }

        public GameState GetCurrentGameStateViaEvent()
        {
            GetCurrentGameStateGameEvent gameEvent = new GetCurrentGameStateGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            return gameEvent.GetGameState();
        }

        public MapTileInstance FindMapSquare(int xWorldCoordinate, int yWorldCoordinate)
        {
        
            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquare;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);
        
        }






        public void InitializeNavigationGraph()
        {
            // TODO:  Fix this.  This code should be in GameWorld, not MikeAndConquerGame
            // but has to be here for now, since MapTileInstanceList is in MikeAndConquerGame
            // Need to separate out view of MapTileInstance into a MapTileInstanceView
            // And let GameWorld hold MapTileInstanceList(with no view data, just
            // the terrain type and whether it's blocking or not, etc

            navigationGraph.Reset();

            foreach (Sandbag nextSandbag in sandbagList)
            {
                navigationGraph.MakeNodeBlockingNode(nextSandbag.GetMapSquareX(), nextSandbag.GetMapSquareY());
            }


            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.IsBlockingTerrain)
                {
                    //                    nextBasicMapSquare.gameSprite.drawBoundingRectangle = true;
                    navigationGraph.MakeNodeBlockingNode(nextBasicMapSquare.GetMapSquareX(), nextBasicMapSquare.GetMapSquareY());
                }
            }

            navigationGraph.RebuildAdajencyGraph();

        }


    }
}
