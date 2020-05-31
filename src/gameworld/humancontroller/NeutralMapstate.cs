using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;

namespace mike_and_conquer.gameworld.humancontroller
{
    class NeutralMapstate : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            Point mousePoint = newMouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);


            int mouseWorldX = (int)mouseWorldLocation.X;
            int mouseWorldY = (int)mouseWorldLocation.Y;

            GameWorldView.instance.gameCursor.SetToMainCursor();
            if (LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
                if (handledEvent)
                {
//                    mapState = UNITS_SELECTED_MAP_STATE;
                    return new UnitsSelectedMapState();
                }
            }
            if (LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return new DragSelectingMapState(newMouseState);
//                mapState = DRAG_SELECTING_MAP_STATE;
            }

            return this;
        }


        private bool LeftMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
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

        private bool LeftMouseButtonIsBeingHeldDown(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Released;
        }


        private Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }



    }
}
