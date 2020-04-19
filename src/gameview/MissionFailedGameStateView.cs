
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using MissionFailedMessage = mike_and_conquer.gameview.MissionFailedMessage;

namespace mike_and_conquer.gameview
{
    class MissionFailedGameStateView : GameStateView
    {

        private MissionFailedMessage message;



        public MissionFailedGameStateView()
        {
            message = new MissionFailedMessage();
        }


        // public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
        //     {
        //         nextMinigunnerView.Draw(gameTime, spriteBatch);
        //     }
        //
        //     foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
        //     {
        //         nextMinigunnerView.Draw(gameTime, spriteBatch);
        //     }
        //     message.Draw(gameTime, spriteBatch);
        //
        // }

        public override void Draw(GameTime gameTime)
        {
            GameWorldView.instance.Draw(gameTime);

        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
