﻿

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

        private MouseState oldState;

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

            MouseState newState = Mouse.GetState();

            float scale = MikeAndConquerGame.instance.camera2D.Zoom;
            Vector2 mousePosition = new Vector2(newState.X / scale, newState.Y / scale);

            Boolean isAMinigunnerSelected = false;

            foreach(Minigunner nextMinigunner in GameWorld.instance.gdiMinigunnerList)
            {

                if (nextMinigunner.selected == true)
                {
                    isAMinigunnerSelected = true;
                }

            }

            if (isAMinigunnerSelected)
            {
                Point point = new Point();
                point.X = (int)mousePosition.X;
                point.Y = (int)mousePosition.Y;
                if (IsPointOverBlockingTerrain(point))
                {
                    MikeAndConquerGame.instance.gameCursor.SetToMovementNotAllowedCursor();
                }
                else
                {
                    MikeAndConquerGame.instance.gameCursor.SetToMoveToLocationCursor();
                }

            }
            else
            {
                MikeAndConquerGame.instance.gameCursor.SetToMainCursor();
            }




            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                HandleLeftClick(newState.Position.X, newState.Position.Y);
            }
            else if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released)
            {
                HandleRightClick(newState.Position.X, newState.Position.Y);
            }

            oldState = newState;

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


        bool IsPointOverBlockingTerrain(Point pointInWorldCoordinates)
        {
            foreach (BasicMapSquare nexBasicMapSquare in MikeAndConquerGame.instance.BasicMapSquareList)
            {
                if (nexBasicMapSquare.ContainsPoint(pointInWorldCoordinates) &&
                    nexBasicMapSquare.IsBlockingTerrain())
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
                    BasicMapSquare clickedBasicMapSquare = MikeAndConquerGame.instance.FindMapSquare(mouseX, mouseY);
                    Point centerOfSquare = clickedBasicMapSquare.GetCenter();
                    nextMinigunner.OrderToMoveToDestination(centerOfSquare);
                }
            }
            return true;
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
