using Microsoft.Xna.Framework;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Boolean = System.Boolean;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace mike_and_conquer
{
    class MissionAccomplishedGameState : GameState
    {

        public MissionAccomplishedGameState()
        {
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

            return this;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
