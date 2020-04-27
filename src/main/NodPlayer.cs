
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.aicontroller;
using mike_and_conquer.gameobjects;

namespace mike_and_conquer.main
{
    class NodPlayer
    {
        private PlayerController playerController;


        public List<Minigunner> nodMinigunnerList { get; }
        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }


        public List<Minigunner> NodMinigunnerList
        {
            get { return nodMinigunnerList; }
        }


        //

        public NodPlayer()
        {
            nodMinigunnerList = new List<Minigunner>();
            nodMinigunnerAIControllerList = new List<MinigunnerAIController>();
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
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }

            UpdateAIControllers(gameTime);

        }


        private void UpdateAIControllers(GameTime gameTime)
        {
            foreach (MinigunnerAIController nextMinigunnerAIController in nodMinigunnerAIControllerList)
            {
                nextMinigunnerAIController.Update(gameTime);
            }
        }



        public Minigunner AddNodMinigunner(Minigunner minigunner, bool aiIsOn)
        {

            this.nodMinigunnerList.Add(minigunner);

            // TODO:  In future, don't couple Nod having to be AI controlled enemy
            if (aiIsOn)
            {
                MinigunnerAIController minigunnerAIController = new MinigunnerAIController(minigunner);
                nodMinigunnerAIControllerList.Add(minigunnerAIController);
            }

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
