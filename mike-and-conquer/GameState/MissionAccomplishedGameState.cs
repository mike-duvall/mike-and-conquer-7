using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GameTime = Microsoft.Xna.Framework.GameTime;
using MissionAccomplishedMessage = mike_and_conquer.gameobjects.MissionAccomplishedMessage;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;

namespace mike_and_conquer
{
    class MissionAccomplishedGameState : GameState
    {

        private MissionAccomplishedMessage message;

        public MissionAccomplishedGameState()
        {

            message = new MissionAccomplishedMessage();
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
            return "Game Over";
        }

        public override GameState Update(GameTime gameTime)
        {
            GameState nextGameState = MikeAndConquerGame.instance.ProcessGameEvents();
            if(nextGameState != null)
            {
                return nextGameState;
            }
            else
            {
                return this;
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }

            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }
            message.Draw(gameTime, spriteBatch);

        }


    }
}
