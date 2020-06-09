using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;


namespace mike_and_conquer.gameworld.humancontroller 
{
    public class UnitsSelectedMapState : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            if (!GameWorld.instance.IsAnyUnitSelected())
            {
                return new NeutralMapstate();
            }

            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {
                return new MousePointerOverSidebarState();
            }

            Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

            if (GameWorld.instance.IsAMinigunnerSelected())
            {
                UpdateMousePointerWhenMinigunnerSelected(mouseWorldLocationPoint);
            }
            else if (GameWorld.instance.IsAnMCVSelected())
            {
                UpdateMousePointerWhenMCVSelected(mouseWorldLocationPoint);
            }

            if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldLocationPoint);
                }

                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnMap(mouseWorldLocationPoint);
                }
            }

            if (MouseInputUtil.RightMouseButtonClicked(newMouseState, oldMouseState))
            {
                HandleRightClick(mouseWorldLocationPoint);
                return new NeutralMapstate();
            }

            return this;
        }


        internal void HandleRightClick(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                nextMinigunner.selected = false;
            }

            if (GameWorld.instance.MCV != null)
            {
                GameWorld.instance.MCV.selected = false;
            }

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


        private bool CheckForAndHandleLeftClickOnMap(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.selected == true)
                {
                    if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
                    {
                        MapTileInstance clickedMapTileInstance =
                            GameWorld.instance.FindMapTileInstance(mouseX, mouseY);
                        Point centerOfSquare = clickedMapTileInstance.GetCenter();
                        nextMinigunner.OrderToMoveToDestination(centerOfSquare);
                    }
                }
            }

            MCV mcv = GameWorld.instance.MCV;
            if (mcv != null)
            {
                if (mcv.selected == true)
                {
                    if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
                    {
                        MapTileInstance clickedMapTileInstance =
                            GameWorld.instance.FindMapTileInstance(mouseX, mouseY);
                        Point centerOfSquare = clickedMapTileInstance.GetCenter();
                        mcv.OrderToMoveToDestination(centerOfSquare);
                    }

                }
            }
            return true;

        }


        internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(Point mouseLocation)
        {
            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            bool handled = false;
            foreach (Minigunner nextNodMinigunner in GameWorld.instance.NodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    foreach (Minigunner nextGdiMinigunner in GameWorld.instance.GDIMinigunnerList)
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


        private static void UpdateMousePointerWhenMinigunnerSelected(Point mousePositionAsPointInWorldCoordinates)
        {
            if (GameWorld.instance.IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToAttackEnemyLocationCursor();
            }
            else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
        }

        private static void UpdateMousePointerWhenMCVSelected(Point mousePositionAsPointInWorldCoordinates)
        {
            if (GameWorld.instance.IsPointOverMCV(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToBuildConstructionYardCursor();
            }
            else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
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
