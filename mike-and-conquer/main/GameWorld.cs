using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Graph = mike_and_conquer.pathfinding.Graph;

namespace mike_and_conquer

{
    public class GameWorld
    {
        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }
        public List<Sandbag> sandbagList;

        public Graph navigationGraph;

        private GameState currentGameState;

        public static GameWorld instance;

        public GameWorld()
        {
            gdiMinigunnerList = new List<Minigunner>();
            nodMinigunnerList = new List<Minigunner>();
            sandbagList = new List<Sandbag>();
            currentGameState = new PlayingGameState();

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


        internal void Initialize(int numColumns, int numRows)
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

        internal void Update(GameTime gameTime)
        {
            currentGameState = currentGameState.Update(gameTime);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentGameState.Draw(gameTime, spriteBatch);
        }
    }
}
