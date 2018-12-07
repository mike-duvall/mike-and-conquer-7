using GameTime = Microsoft.Xna.Framework.GameTime;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;

namespace mike_and_conquer
{
    class MissionFailedGameState : GameState
    {

        public MissionFailedGameState()
        {

            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.SetAnimate(false);
            }

            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.SetAnimate(false);
            }

        }

        public override string GetName()
        {
            return "Mission Failed";
        }

        public override GameState Update(GameTime gameTime)
        {
            GameState nextGameState = GameWorld.instance.ProcessGameEvents();
            if (nextGameState != null)
            {
                return nextGameState;
            }
            else
            {
                return this;
            }
        }



    }
}
