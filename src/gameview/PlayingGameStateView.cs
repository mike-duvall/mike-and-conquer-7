

using mike_and_conquer.gameobjects;
using mike_and_conquer.main;
using mike_and_conquer.src.inputhandler.windows;
using GameTime = Microsoft.Xna.Framework.GameTime;
// using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;

using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.gameview
{
    class PlayingGameStateView : GameStateView
    {


        public override void Update(GameTime gameTime)
        {

            UpdateMousePointer(WindowsPlayingGameStateInputHandler.instance.MouseState);

        }

        private void UpdateMousePointer(MouseState newMouseState)
        {
            Point mousePositionAsPointInWorldCoordinates = CalculateMousePositionInWorldCoordinates(newMouseState);

            if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && IsAMinigunnerSelected())
            {
                if (IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
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
            else if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && IsAnMCVSelected())
            {
                if (IsPointOverMCV(mousePositionAsPointInWorldCoordinates))
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

        bool IsPointOverMCV(Point pointInWorldCoordinates)
        {

            if (GameWorld.instance.MCV != null)
            {
                if (GameWorld.instance.MCV.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
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

        bool IsAnMCVSelected()
        {
            MCV mcv = GameWorld.instance.MCV;
            if (mcv != null)
            {
                return mcv.selected;
            }

            return false;
        }


        private static Point CalculateMousePositionInWorldCoordinates(MouseState newMouseState)
        {
            float scale = GameWorldView.instance.mapViewportCamera.Zoom;
            float leftMostScrollX = GameWorldView.instance.CalculateLeftmostScrollX();
            float topMostScrollY = GameWorldView.instance.CalculateTopmostScrollY();

            float cameraOffsetX = GameWorldView.instance.mapViewportCamera.Location.X - leftMostScrollX;
            float mousePositionYInWorldCoordinates = (newMouseState.X / scale) + cameraOffsetX;

            float cameraOffsetY = GameWorldView.instance.mapViewportCamera.Location.Y - topMostScrollY;
            float mousePositionXInWorldCoordinates = (newMouseState.Y / scale) + cameraOffsetY;

            Vector2 mousePositionInWorldCoordinates =
                new Vector2(mousePositionYInWorldCoordinates, mousePositionXInWorldCoordinates);


            Point mousePositionAsPointInWorldCoordinates = new Point();
            mousePositionAsPointInWorldCoordinates.X = (int)mousePositionInWorldCoordinates.X;
            mousePositionAsPointInWorldCoordinates.Y = (int)mousePositionInWorldCoordinates.Y;

            return mousePositionAsPointInWorldCoordinates;
        }



        public override  void Draw(GameTime gameTime)
        {
            GameWorldView.instance.Draw(gameTime);
            // foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
            // {
            //     basicMapSquareView.Draw(gameTime, spriteBatch);
            // }
            //
            // TODO:  Consider pulling this code back into this class, from MikeAndConquerGame
//            GameWorldView.instance.GDIBarracksView.Draw(gameTime, spriteBatch);
            // GameWorldView.instance.GdiConstructionYardView.Draw(gameTime,spriteBatch);
//
//            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
//            {
//                nextMinigunnerView.Draw(gameTime, spriteBatch);
//            }
//
//            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
//            {
//                nextMinigunnerView.Draw(gameTime, spriteBatch);
//            }
//
//            foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
//            {
//                nextSandbagView.Draw(gameTime, spriteBatch);
//            }
//
            // foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            // {
            //     nextTerrainView.DrawFull(gameTime, spriteBatch);
            // }
//
//            MikeAndConquerGame.instance.unitSelectionBox.Draw(gameTime, spriteBatch);


        }
    }
}
