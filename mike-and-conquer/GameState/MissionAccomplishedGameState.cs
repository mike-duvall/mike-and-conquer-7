using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GameTime = Microsoft.Xna.Framework.GameTime;
using MissionAccomplishedMessage = mike_and_conquer.gameobjects.MissionAccomplishedMessage;

namespace mike_and_conquer
{
    class MissionAccomplishedGameState : GameState
    {

        private MissionAccomplishedMessage message;

        public MissionAccomplishedGameState()
        {

            message = new MissionAccomplishedMessage();
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                nextMinigunner.SetAnimate(false);
            }

            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                nextMinigunner.SetAnimate(false);
            }

        }

        public override string GetName()
        {
            return "Game Over";
        }

        public override GameState Update(GameTime gameTime)
        {
            GameState nextGameState = MikeAndConqueryGame.instance.ProcessGameEvents();
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
            message.Draw(gameTime, spriteBatch);
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }

            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }

        }


    }
}
