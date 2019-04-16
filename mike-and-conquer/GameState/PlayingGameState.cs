

using Microsoft.Xna.Framework;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Boolean = System.Boolean;
using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController ;

using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;

using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer
{
    class PlayingGameState : GameState
    {

        private MouseState oldMouseState;

        //private Boolean isDragSelectHappening = false;

        private Rectangle selectionBoxRectangle;

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

            MouseState newMouseState = Mouse.GetState();

            UpdateMousePointer(newMouseState);

            Vector2 mouseScreenLocation = new Vector2(newMouseState.X, newMouseState.Y);
            Vector2 mouseWorldLocationVector2 = ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            Point mouseWorldLocationPoint = new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);



            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                HandleLeftClick(newMouseState.Position.X, newMouseState.Position.Y);
                selectionBoxRectangle = new Rectangle(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y, 0, 0);
            }
            else if (newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
            {
                HandleRightClick(newMouseState.Position.X, newMouseState.Position.Y);
            }




            //If the user is still holding the Left button down, then continue to re-size the 
            //selection square based on where the mouse has currently been moved to.
            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                //The starting location for the selection box remains the same, but increase (or decrease)
                //the size of the Width and Height but taking the current location of the mouse minus the
                //original starting location.
                selectionBoxRectangle = new Rectangle(
                    selectionBoxRectangle.X,
                    selectionBoxRectangle.Y,
                    mouseWorldLocationPoint.X - selectionBoxRectangle.X,
                    mouseWorldLocationPoint.Y - selectionBoxRectangle.Y);
            }

            //If the user has released the left mouse button, then reset the selection square
            if (newMouseState.LeftButton == ButtonState.Released)
            {
                foreach (Minigunner minigunner in MikeAndConquerGame.instance.gameWorld.gdiMinigunnerList)
                {
                    if (selectionBoxRectangle.Contains(minigunner.positionInWorldCoordinates))
                    {
                        minigunner.selected = true;
                    }
                }
                //Reset the selection square to no position with no height and width
                selectionBoxRectangle = new Rectangle(-1, -1, 0, 0);
            }

            MikeAndConquerGame.instance.log.Information("x:" + selectionBoxRectangle.X + 
                                                        ",y:" + selectionBoxRectangle.Y + 
                                                        ",width:" + selectionBoxRectangle.Width +
                                                        ",height:" + selectionBoxRectangle.Height);

            //            Pickup here:  now that selectionBoxRectangle is being update,
            //            make call to set the rectangle for the UnitSelectionBox
            //            and then update UnitSelectionBox to set it's size off of the rectangle
            //            may want to update this code to just directly modify UnitSelectionBox
            //            or even pass the mouse state to UnitSelectionBox and let it handle things
            MikeAndConquerGame.instance.unitSelectionBox.unitSelectionBoxRectangle = selectionBoxRectangle;

            oldMouseState = newMouseState;

            foreach (MinigunnerAIController nextMinigunnerAIController in GameWorld.instance.nodMinigunnerAIControllerList)
            {
                nextMinigunnerAIController.Update(gameTime);
            }

            foreach (Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {
                if(nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }

            foreach (Minigunner nextMinigunner in GameWorld.instance.nodMinigunnerList)
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
            else
            {
                return this;
            }

        }

        private void UpdateMousePointer(MouseState newState)
        {
            float scale = MikeAndConquerGame.instance.camera2D.Zoom;
            Vector2 scaledMousedPosition = new Vector2(newState.X / scale, newState.Y / scale);


            if (IsAMinigunnerSelected())
            {
                Point point = new Point();
                point.X = (int) scaledMousedPosition.X;
                point.Y = (int) scaledMousedPosition.Y;
                if (IsPointOverEnemy(point))
                {
                    MikeAndConquerGame.instance.gameCursor.SetToAttackEnemyLocationCursor();
                }
                else if (IsValidMoveDestination(point))
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
            return Vector2.Transform(screenLocation, Matrix.Invert(MikeAndConquerGame.instance.camera2D.TransformMatrix));
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
                        BasicMapSquare clickedBasicMapSquare =
                            MikeAndConquerGame.instance.FindMapSquare(mouseX, mouseY);
                        Point centerOfSquare = clickedBasicMapSquare.GetCenter();
                        nextMinigunner.OrderToMoveToDestination(centerOfSquare);
                    }
                }
            }
            return true;
        }

        private bool IsValidMoveDestination(Point pointInWorldCoordinates)
        {
            Boolean isValidMoveDestination = true;
            BasicMapSquare clickedBasicMapSquare =
                MikeAndConquerGame.instance.FindMapSquare(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);
            if (clickedBasicMapSquare.IsBlockingTerrain())
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
