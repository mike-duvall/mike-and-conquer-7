using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    class NeutralMapstate : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            if (GameWorld.instance.IsAnyUnitSelected())
            {
                return new UnitsSelectedMapState();
            }

            Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

            GameWorldView.instance.gameCursor.SetToMainCursor();
            if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                if (handledEvent)
                {
                    return new UnitsSelectedMapState();
                }
            }
            if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return new DragSelectingMapState(newMouseState);
            }

            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {
                return new MousePointerOverSidebarState();
            }

            return this;
        }


        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            Boolean handled = false;
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    GameWorld.instance.SelectSingleGDIUnit(nextMinigunner);
                }
            }

            if (!handled)
            {
                handled = CheckForAndHandleLeftClickOnMCV(mouseX, mouseY);
            }

            return handled;
        }



        private static bool CheckForAndHandleLeftClickOnMCV(int mouseX, int mouseY)
        {
            Boolean handled = false;
            MCV mcv = GameWorld.instance.MCV;
            if (mcv != null)
            {
                if (mcv.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    if (mcv.selected == false)
                    {
                        GameWorld.instance.SelectMCV(GameWorld.instance.MCV);
                    }
                    else
                    {
                        Point mcvPositionInWorldCoordinates = new Point((int)mcv.positionInWorldCoordinates.X,
                            (int)mcv.positionInWorldCoordinates.Y);
                        MikeAndConquerGame.instance.RemoveMCV();
                        MikeAndConquerGame.instance.AddGDIConstructionYardAtWorldCoordinates(mcvPositionInWorldCoordinates);
                    }
                }
            }

            return handled;
        }





    }
}
