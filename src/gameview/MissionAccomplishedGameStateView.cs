
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using MissionAccomplishedMessage = mike_and_conquer.gameview.MissionAccomplishedMessage;

namespace mike_and_conquer.gameview
{
    class MissionAccomplishedGameStateView : GameStateView
    {

        private MissionAccomplishedMessage message;



        public MissionAccomplishedGameStateView()
        {
            message = new MissionAccomplishedMessage();
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
