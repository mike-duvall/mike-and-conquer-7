using Microsoft.Xna.Framework;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Boolean = System.Boolean;

namespace mike_and_conquer
{
    class PlayingGameState : GameState
    {

        private MouseState oldState;

        public override string GetName()
        {
            return "Playing";
        }

        public override GameState Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                HandleLeftClick(newState.Position.X, newState.Position.Y);
            }

            oldState = newState; // this reassigns

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();



            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                nextMinigunner.Update(gameTime);
            }


            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                nextMinigunner.Update(gameTime);
            }

            if (EnemyMinigunnersExistAndAreAllDead())
            {
                return new MissionAccomplishedGameState();
            }
            //else if (MinigunnersExistAndAreAllDead())
            //{
            //    return new MissionFailedGameState(game);
            //}
            else
            {
                return this;
            }

        }

        internal Boolean EnemyMinigunnersExistAndAreAllDead()
        {
            if(MikeAndConqueryGame.instance.nodMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                if( nextMinigunner.health > 0)
                {
                    allDead = false;
                }
            }

            return allDead;


        }



        internal void HandleLeftClick(int mouseX, int mouseY)
        {
            bool handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseX, mouseY);
            if (!handledEvent)
            {
                handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseX, mouseY);
            }
            //if (!handledEvent)
            //{
            //    CheckForAndHandleLeftClickOnMap(input);
            //}

        }

        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(int mouseX, int mouseY)
        {
            Boolean handled = false;
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    nextMinigunner.selected = true;
                }
            }

            return handled;

        }

        internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(int mouseX, int mouseY)
        {
            bool handled = false;
            foreach (NodMinigunner nextNodMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    foreach (Minigunner nextGdiMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
                    {
                        if (nextGdiMinigunner.selected)
                        {
                            nextGdiMinigunner.OrderToMoveToAndAttackEnemyUnit(nextNodMinigunner);
                        }
                    }
                }
            }

            return handled;

        }



    }
}
