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
        // public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }


        public List<Minigunner> NodMinigunnerList
        {
            get { return nodMinigunnerList; }
        }


        public NodPlayer(PlayerController playerController)
        {
            nodMinigunnerList = new List<Minigunner>();
            this.playerController = playerController;
            // nodMinigunnerAIControllerList = new List<MinigunnerAIController>();
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
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }

            // UpdateAIControllers(gameTime);

        }


        // private void UpdateAIControllers(GameTime gameTime)
        // {
        //     foreach (MinigunnerAIController nextMinigunnerAIController in nodMinigunnerAIControllerList)
        //     {
        //         nextMinigunnerAIController.Update(gameTime);
        //     }
        // }



        public Minigunner AddNodMinigunner(Minigunner minigunner, bool aiIsOn)
        {

            this.nodMinigunnerList.Add(minigunner);
            playerController.Add(minigunner, aiIsOn);


            // // TODO:  In future, don't couple Nod having to be AI controlled enemy
            // if (aiIsOn)
            // {
            //     MinigunnerAIController minigunnerAIController = new MinigunnerAIController(minigunner);
            //     playerController.
            //     nodMinigunnerAIControllerList.Add(minigunnerAIController);
            // }

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
