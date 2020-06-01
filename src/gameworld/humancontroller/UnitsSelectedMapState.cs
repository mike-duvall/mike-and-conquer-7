using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.util;

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


            Vector2 mousePointerLocation = new Vector2(newMouseState.X, newMouseState.Y);
            Vector2 mousePositionAsPointInWorldCoordinatesAsVector2 =
                GameWorldView.instance.ConvertScreenLocationToWorldLocation(mousePointerLocation);

            Point mousePositionAsPointInWorldCoordinates =
                PointUtil.ConvertVector2ToPoint(mousePositionAsPointInWorldCoordinatesAsVector2);



            if (GameWorld.instance.IsAMinigunnerSelected())
            {
                UpdateMousePointerWhenMinigunnerSelected(mousePositionAsPointInWorldCoordinates);
            }
            else if (GameWorld.instance.IsAnMCVSelected())
            {
                UpdateMousePointerWhenMCVSelected(mousePositionAsPointInWorldCoordinates);
            }



            if (LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldX, mouseWorldY);
                }

                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnMap(mouseWorldX, mouseWorldY);
                }
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


        private bool CheckForAndHandleLeftClickOnMap(int mouseX, int mouseY)
        {
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


        internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(int mouseX, int mouseY)
        {
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




    }
}
