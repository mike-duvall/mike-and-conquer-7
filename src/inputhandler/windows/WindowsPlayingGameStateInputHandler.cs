
using System;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;
using mike_and_conquer.util;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;



namespace mike_and_conquer.src.inputhandler.windows
{
    class WindowsPlayingGameStateInputHandler
    {

        public static WindowsPlayingGameStateInputHandler instance;

        private MouseState oldMouseState;


        public MouseState MouseState
        {
            get { return oldMouseState; }
        }

        public WindowsPlayingGameStateInputHandler()
        {
            instance = this;
        }

        public void HandleInput()
        {
            MouseState newMouseState = Mouse.GetState();

            UpdateMousePointer(newMouseState);

            Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);

            UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;

            if (LeftMouseButtonUnclicked(newMouseState))
            {
                if (unitSelectionBox.isDragSelectHappening)
                {
                    unitSelectionBox.HandleEndDragSelect();
                }
                else
                {
                    HandleLeftClick(newMouseState.Position.X, newMouseState.Position.Y);
                }
            }
            else if (LeftMouseButtonClicked(newMouseState))
            {
                unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
            }
            else if (RightMouseButtonClicked(newMouseState))
            {
                unitSelectionBox.isDragSelectHappening = false;
                HandleRightClick(newMouseState.Position.X, newMouseState.Position.Y);
            }


            if (LeftMouseButtonIsBeingHeldDown(newMouseState))
            {
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
            }

            oldMouseState = newMouseState;

        }

        private void UpdateMousePointer(MouseState mouseState)
        {
            Vector2 mousePointerLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mousePositionAsPointInWorldCoordinatesAsVector2 =
                GameWorldView.instance.ConvertScreenLocationToWorldLocation(mousePointerLocation);

            Point mousePositionAsPointInWorldCoordinates =
                PointUtil.ConvertVector2ToPoint(mousePositionAsPointInWorldCoordinatesAsVector2);

            if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && GameWorld.instance.IsAMinigunnerSelected())
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
            else if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && GameWorld.instance.IsAnMCVSelected())
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
        
            else
            {
                GameWorldView.instance.gameCursor.SetToMainCursor();
            }
        }

        private Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }

        private bool LeftMouseButtonIsBeingHeldDown(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Released;
        }

        private bool RightMouseButtonClicked(MouseState newMouseState)
        {
            return newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        private bool LeftMouseButtonClicked(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }

        private bool LeftMouseButtonUnclicked(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        }

        internal void HandleLeftClick(int mouseX, int mouseY)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = mouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);


            int mouseWorldX = (int)mouseWorldLocation.X;
            int mouseWorldY = (int)mouseWorldLocation.Y;

            bool handledEvent = CheckForAndHandleLeftClickOnSidebar(mouseScreenLocation);
            if (!handledEvent)
            {
                handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
            }
            if (!handledEvent)
            {
                handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldX, mouseWorldY);
            }
            if (!handledEvent)
            {
                CheckForAndHandleLeftClickOnMap(mouseWorldX, mouseWorldY);
            }

        }

        internal void HandleRightClick(int mouseX, int mouseY)
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {
                nextMinigunner.selected = false;
            }

            if (GameWorld.instance.MCV != null)
            {
                GameWorld.instance.MCV.selected = false;
            }

        }

        private bool CheckForAndHandleLeftClickOnMap(int mouseX, int mouseY)
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
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


        // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
        // them and ask if they contain point where sidebar was clicked
        internal bool CheckForAndHandleLeftClickOnSidebar(Vector2 mouseScreenLocation)
        {
            bool handled = false;

            Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

            if (sidebarLocation.X > 0 && sidebarLocation.X < 64 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
            {
                HandleClickOnBuildBarracks();
                handled = true;
            }
            else if (sidebarLocation.X > 80 && sidebarLocation.X < 144 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
            {
                HandleClickOnBuildMinigunner();
                handled = true;
            }

            return handled;

        }


        private void HandleClickOnBuildBarracks()
        {
            GameWorld.instance.GDIConstructionYard.StartBuildingMinigunner();
        }


        private void HandleClickOnBuildMinigunner()
        {
            GameWorld.instance.GDIBarracks.StartBuildingMinigunner();
        }


        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(int mouseX, int mouseY)
        {
            Boolean handled = false;
            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
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


        internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(int mouseX, int mouseY)
        {
            bool handled = false;
            foreach (Minigunner nextNodMinigunner in GameWorld.instance.nodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    foreach (Minigunner nextGdiMinigunner in GameWorld.instance.gdiMinigunnerList)
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
