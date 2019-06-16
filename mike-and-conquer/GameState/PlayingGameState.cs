
using mike_and_conquer.gameview;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Boolean = System.Boolean;
using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace mike_and_conquer
{
    class PlayingGameState : GameState
    {

        private MouseState oldMouseState;

        public override string GetName()
        {
            return "Playing";
        }

        public override GameState Update(GameTime gameTime)
        {
            // TODO:  Consider pulling handling of GameEvents into base class
            GameState nextGameState = GameWorld.instance.ProcessGameEvents();
            if (nextGameState != null)
            {
                return nextGameState;
            }

            HandleInput();
//            UpdateAIControllers(gameTime);
//            UpdateGDIMinigunners(gameTime);
//            UpdateNodMinigunners(gameTime);
            GameWorld.instance.Update(gameTime);
            return DetermineNextGameState();
        }

        private void HandleInput()
        {
            MouseState newMouseState = Mouse.GetState();

            UpdateMousePointer(newMouseState);

            Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);
            UnitSelectionBox unitSelectionBox = MikeAndConquerGame.instance.unitSelectionBox;

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

        private GameState DetermineNextGameState()
        {
            if (NodMinigunnersExistAndAreAllDead())
            {
                return new MissionAccomplishedGameState();
            }

            if (GdiMinigunnersExistAndAreAllDead())
            {
                return new MissionFailedGameState();
            }
            else
            {
                return this;
            }
        }

//        private static void UpdateNodMinigunners(GameTime gameTime)
//        {
//            foreach (Minigunner nextMinigunner in GameWorld.instance.nodMinigunnerList)
//            {
//                if (nextMinigunner.health > 0)
//                {
//                    nextMinigunner.Update(gameTime);
//                }
//            }
//        }
//
//        private static void UpdateGDIMinigunners(GameTime gameTime)
//        {
//            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
//            {
//                if (nextMinigunner.health > 0)
//                {
//                    nextMinigunner.Update(gameTime);
//                }
//            }
//        }
//
//        private static void UpdateAIControllers(GameTime gameTime)
//        {
//            foreach (MinigunnerAIController nextMinigunnerAIController in GameWorld.instance.nodMinigunnerAIControllerList)
//            {
//                nextMinigunnerAIController.Update(gameTime);
//            }
//        }
//

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


        private Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }

        private void UpdateMousePointer(MouseState newMouseState)
        {
            Point mousePositionAsPointInWorldCoordinates = CalculateMousePositionInWorldCoordinates(newMouseState);

            if (IsAMinigunnerSelected())
            {
                if (IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
                {
                    MikeAndConquerGame.instance.gameCursor.SetToAttackEnemyLocationCursor();
                }
                else if (IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
                {
                    MikeAndConquerGame.instance.gameCursor.SetToMoveToLocationCursor();
                }
                else
                {
                    MikeAndConquerGame.instance.gameCursor.SetToMovementNotAllowedCursor();
                }
            }
            else
            {
                MikeAndConquerGame.instance.gameCursor.SetToMainCursor();
            }
        }

        private static Point CalculateMousePositionInWorldCoordinates(MouseState newMouseState)
        {
            float scale = MikeAndConquerGame.instance.mapViewportCamera.Zoom;
            float leftMostScrollX = MikeAndConquerGame.instance.CalculateLeftmostScrollX();
            float topMostScrollY = MikeAndConquerGame.instance.CalculateTopmostScrollY();

            float cameraOffsetX = MikeAndConquerGame.instance.mapViewportCamera.Location.X - leftMostScrollX;
            float mousePositionYInWorldCoordinates = (newMouseState.X / scale) + cameraOffsetX;

            float cameraOffsetY = MikeAndConquerGame.instance.mapViewportCamera.Location.Y - topMostScrollY;
            float mousePositionXInWorldCoordinates = (newMouseState.Y / scale) + cameraOffsetY;

            Vector2 mousePositionInWorldCoordinates =
                new Vector2(mousePositionYInWorldCoordinates, mousePositionXInWorldCoordinates);


            Point mousePositionAsPointInWorldCoordinates = new Point();
            mousePositionAsPointInWorldCoordinates.X = (int)mousePositionInWorldCoordinates.X;
            mousePositionAsPointInWorldCoordinates.Y = (int)mousePositionInWorldCoordinates.Y;

            return mousePositionAsPointInWorldCoordinates;
        }

        bool IsPointOverEnemy(Point pointInWorldCoordinates)
        {
            foreach (Minigunner nextNodMinigunner in GameWorld.instance.nodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsAMinigunnerSelected()
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.selected)
                {
                    return true;
                }
            }

            return false;

        }



        internal Boolean NodMinigunnersExistAndAreAllDead()
        {
            if(GameWorld.instance.nodMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in GameWorld.instance.nodMinigunnerList)
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
            if (GameWorld.instance.gdiMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    allDead = false;
                }
            }
            return allDead;
        }


        internal Vector2 ConvertScreenLocationToWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix));
        }

        internal void HandleLeftClick(int mouseX, int mouseY)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = mouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = ConvertScreenLocationToWorldLocation(mouseScreenLocation);

            int mouseWorldX = (int) mouseWorldLocation.X;
            int mouseWorldY = (int) mouseWorldLocation.Y;

            bool handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
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

        }




        private bool CheckForAndHandleLeftClickOnMap(int mouseX, int mouseY)
        {
            foreach(Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {
                if(nextMinigunner.selected == true)
                {
                    if (IsValidMoveDestination(new Point(mouseX, mouseY)))
                    {
                        MapTileInstance clickedMapTileInstance =
                            GameWorld.instance.FindMapSquare(mouseX, mouseY);
                        Point centerOfSquare = clickedMapTileInstance.GetCenter();
                        nextMinigunner.OrderToMoveToDestination(centerOfSquare);
                    }
                }
            }
            return true;
        }

        private bool IsValidMoveDestination(Point pointInWorldCoordinates)
        {
            Boolean isValidMoveDestination = true;
            MapTileInstance clickedMapTileInstance =
                GameWorld.instance.FindMapSquare(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);
            if (clickedMapTileInstance.IsBlockingTerrain())
            {
                isValidMoveDestination = false;
            }


            foreach (Sandbag nextSandbag in MikeAndConquerGame.instance.gameWorld.sandbagList)
            {

                if (nextSandbag.ContainsPoint(pointInWorldCoordinates))
                {
                    isValidMoveDestination = false;
                }
            }

            return isValidMoveDestination;


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
