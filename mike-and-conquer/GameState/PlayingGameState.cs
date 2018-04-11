﻿using Microsoft.Xna.Framework;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Boolean = System.Boolean;
using System;

using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

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
                if(nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }

            }


            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.nodMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }

            if (NodMinigunnersExistAndAreAllDead())
            {
                return new MissionAccomplishedGameState();
            }

            if (GdiMinigunnersExistAndAreAllDead())
            {
                return new MissionFailedGameState();
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

        internal Boolean NodMinigunnersExistAndAreAllDead()
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


        internal Boolean GdiMinigunnersExistAndAreAllDead()
        {
            if (MikeAndConqueryGame.instance.gdiMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.health > 0)
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
            if (!handledEvent)
            {
                CheckForAndHandleLeftClickOnMap(mouseX, mouseY);
            }

        }


        private bool CheckForAndHandleLeftClickOnMap(int mouseX, int mouseY)
        {
            foreach(Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                if(nextMinigunner.selected == true)
                {
                    nextMinigunner.OrderToMoveToDestination(mouseX, mouseY);
                }
            }
            return true;
        }


        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(int mouseX, int mouseY)
        {
            Boolean handled = false;
            foreach (Minigunner nextMinigunner in MikeAndConqueryGame.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    MikeAndConqueryGame.instance.SelectSingleGDIUnit(nextMinigunner);
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

        }
    }
}
