
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using MissionAccomplishedMessage = mike_and_conquer.gameobjects.MissionAccomplishedMessage;

namespace mike_and_conquer.gameview
{
    class MissionAccomplishedGameStateView : GameStateView
    {

        private MissionAccomplishedMessage message;



        public MissionAccomplishedGameStateView()
        {
            message = new MissionAccomplishedMessage();
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
