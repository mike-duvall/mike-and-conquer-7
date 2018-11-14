﻿

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    class PlayingGameStateView : GameStateView
    {
        public override  void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (BasicMapSquare basicMapSquare in MikeAndConquerGame.instance.BasicMapSquareList)
            {
                basicMapSquare.Draw(gameTime, spriteBatch);
            }


            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }

            foreach (MinigunnerView nextMinigunnerView in MikeAndConquerGame.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }

            foreach (SandbagView nextSandbagView in MikeAndConquerGame.instance.SandbagViewList)
            {
                nextSandbagView.Draw(gameTime, spriteBatch);
            }


        }
    }
}
