
using System.Collections.Generic;
using Microsoft.Xna.Framework;


using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
using Graph = mike_and_conquer.pathfinding.Graph;

using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
using CreateNodMinigunnerGameEvent = mike_and_conquer.gameevent.CreateNodMinigunnerGameEvent;
using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;
using ResetGameGameEvent = mike_and_conquer.gameevent.ResetGameGameEvent;
using GetCurrentGameStateGameEvent = mike_and_conquer.gameevent.GetCurrentGameStateGameEvent;
using CreateSandbagGameEvent = mike_and_conquer.gameevent.CreateSandbagGameEvent;

using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController;


namespace mike_and_conquer

{
    public class GameWorld
    {
        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }
        public List<Sandbag> sandbagList;

        public Graph navigationGraph;

        private List<AsyncGameEvent> gameEvents;

        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }


        private GameState currentGameState;

        public static GameWorld instance;

        public GameWorld()
        {
            gdiMinigunnerList = new List<Minigunner>();
            nodMinigunnerList = new List<Minigunner>();
            sandbagList = new List<Sandbag>();
            currentGameState = new PlayingGameState();

            gameEvents = new List<AsyncGameEvent>();

            nodMinigunnerAIControllerList = new List<MinigunnerAIController>();

            GameWorld.instance = this;
        }

        public GameState HandleReset()
        {
            gdiMinigunnerList.Clear();
            nodMinigunnerList.Clear();
            sandbagList.Clear();
            int oldNumColumns = this.navigationGraph.width;
            int oldNumRows = this.navigationGraph.height;
            navigationGraph = new Graph(oldNumColumns, oldNumRows);
            return new PlayingGameState();

        }

        internal GameState GetCurrentGameState()
        {
            return currentGameState;
        }


        public void Initialize(int numColumns, int numRows)
        {
            navigationGraph = new Graph(numColumns, numRows);
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
            currentGameState = currentGameState.Update(gameTime);
        }

        public Minigunner AddGdiMinigunner(Point worldCoordinates)
        {
            Minigunner newMinigunner = new Minigunner(worldCoordinates.X, worldCoordinates.Y, this.navigationGraph);
            gdiMinigunnerList.Add(newMinigunner);
            return newMinigunner;
        }

        public Minigunner AddNodMinigunner(int x, int y, bool aiIsOn)
        {
            Minigunner newMinigunner = new Minigunner(x, y, this.navigationGraph);
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


        public Minigunner CreateNodMinigunnerViaEvent(int x, int y, bool aiIsOn)
        {
            CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(x, y, aiIsOn);
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



    }
}
