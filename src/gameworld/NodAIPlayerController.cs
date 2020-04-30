
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using mike_and_conquer.aicontroller;
using mike_and_conquer.gameobjects;


namespace mike_and_conquer.gameworld
{
    class NodAIPlayerController : PlayerController
    {


        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }

        public NodAIPlayerController()
        {
            nodMinigunnerAIControllerList = new List<MinigunnerAIController>();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateAIControllers(gameTime);
        }

        public override void Add(Minigunner minigunner, bool aiIsOn)
        {
            if (aiIsOn)
            {
                nodMinigunnerAIControllerList.Add(new MinigunnerAIController(minigunner));
            }

        }

        private void UpdateAIControllers(GameTime gameTime)
        {
            foreach (MinigunnerAIController nextMinigunnerAIController in nodMinigunnerAIControllerList)
            {
                nextMinigunnerAIController.Update(gameTime);
            }
        }






    }
}
