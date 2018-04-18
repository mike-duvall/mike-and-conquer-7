using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GameTime = Microsoft.Xna.Framework.GameTime;
using MissionFailedMessage = mike_and_conquer.gameobjects.MissionFailedMessage;

namespace mike_and_conquer
{
    class MissionFailedGameState : GameState
    {



        private MissionFailedMessage message;

        public MissionFailedGameState()
        {

            message = new MissionFailedMessage();
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
            return "Mission Failed";
        }

        public override GameState Update(GameTime gameTime)
        {
            GameState nextGameState = MikeAndConqueryGame.instance.ProcessGameEvents();
            if (nextGameState != null)
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

            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Draw(gameTime, spriteBatch);
                }
            }

            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Draw(gameTime, spriteBatch);
                }

            }
            message.Draw(gameTime, spriteBatch);
        }


    }
}
