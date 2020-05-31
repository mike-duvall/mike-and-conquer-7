using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;

namespace mike_and_conquer.gameworld.humancontroller 
{
    public class UnitsSelectedMapState : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {
            Point mousePoint = newMouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);


            int mouseWorldX = (int)mouseWorldLocation.X;
            int mouseWorldY = (int)mouseWorldLocation.Y;


            if (LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
//                if (handledEvent)
//                {
//                    mapState = UNITS_SELECTED_MAP_STATE;
//                }
//                if (!handledEvent)
//                {
//                    handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldX, mouseWorldY);
//                }
//
//                if (!handledEvent)
//                {
//                    CheckForAndHandleLeftClickOnMap(mouseWorldX, mouseWorldY);
//                }
            }


            if (RightMouseButtonClicked(newMouseState, oldMouseState))
            {
                HandleRightClick(mouseWorldX, mouseWorldY);
                return new NeutralMapstate();
            }

            return this;

        }


        private bool RightMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        private bool LeftMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }


        internal void HandleRightClick(int mouseX, int mouseY)
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                nextMinigunner.selected = false;
            }

            if (GameWorld.instance.MCV != null)
            {
                GameWorld.instance.MCV.selected = false;
            }

        }


        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(int mouseX, int mouseY)
        {
            Boolean handled = false;
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    GameWorld.instance.SelectSingleGDIUnit(nextMinigunner);
                }
            }

//            if (!handled)
//            {
//                handled = CheckForAndHandleLeftClickOnMCV(mouseX, mouseY);
//            }

            return handled;
        }





    }
}
