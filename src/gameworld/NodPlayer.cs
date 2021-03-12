using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.aicontroller;
using mike_and_conquer.gameobjects;

namespace mike_and_conquer.gameworld
{
    class NodPlayer
    {
        private PlayerController playerController;


        public List<Minigunner> nodMinigunnerList { get; }

        public List<Minigunner> NodMinigunnerList
        {
            get { return nodMinigunnerList; }
        }


        public NodPlayer(PlayerController playerController)
        {
            nodMinigunnerList = new List<Minigunner>();
            this.playerController = playerController;
        }


        public void HandleReset()
        {
            nodMinigunnerList.Clear();
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

        public void Update(GameTime gameTime)
        {
            playerController.Update(gameTime);
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.Health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }

        }


        public Minigunner AddNodMinigunner(Minigunner minigunner, bool aiIsOn)
        {
            this.nodMinigunnerList.Add(minigunner);
            playerController.Add(minigunner, aiIsOn);
            return minigunner;

        }

        public bool IsPointOverMinigunner(Point pointInWorldCoordinates)
        {
            foreach (Minigunner nextNodMinigunner in nodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
                {
                    return true;
                }
            }

            return false;


        }
    }
}
