using System;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;


using Graph = mike_and_conquer.pathfinding.Graph;

using Point = Microsoft.Xna.Framework.Point;


namespace unit_tests
{
    [TestClass]
    public class GameWorldUnitTests
    {


        [TestMethod]
        public void MinigunnerShouldNavigateSimpleObstacles()
        {
            // given
            GameWorld gameWorld = new GameWorld();
            const int numRows = 3;
            const int  numColumns = 4;
            gameWorld.Initialize(numColumns, numRows);


            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            };

            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(0, 0);
            Point minigunnerLocationInWorldCoordinates =
                ConvertWorldMapSquareCoordiantesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);
            
            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);

            // when
            int destinationColumn = 3;
            int destinationRow = 2;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                ConvertWorldMapSquareCoordiantesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then
            WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, destinationInWorldCoordinates, gameTime);

        }


//        [TestMethod]
//        public void MinigunnerShouldNavigateSimpleObstacles2()
//        {
//            // given
//            GameWorld gameWorld = new GameWorld();
//            const int numRows = 5;
//            const int numColumns = 6;
//            gameWorld.Initialize(numRows, numColumns);
//
//
//            int[,] obstacleArray = new int[numRows, numColumns]
//            {
//                { 0, 0, 1, 0, 0, 0 },
//                { 0, 0, 1, 0, 0, 0 },
//                { 0, 0, 1, 0, 0, 0 },
//                { 0, 0, 1, 0, 0, 0 },
//                { 0, 0, 0, 0, 0, 0 }
//            };
//
//            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);
//
//            Minigunner minigunner = gameWorld.AddGdiMinigunner(12, 12);
//
//            // when
//            int destinationColumn = 3;
//            int destinationRow = 2;
//            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);
//
//            Point destinationInWorldCoordinates =
//                ConvertWorldMapSquareCoordiantesToWorldCoordinates(destinationInWorldMapSquareCoordinate);
//
//            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);
//
//            GameTime gameTime = new GameTime();
//            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
//            gameTime.ElapsedGameTime = timespan;
//
//            // then
//            WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, destinationInWorldCoordinates, gameTime);
//
//        }
//

        private void UpdateGraphFromFromObstacleArray(Graph graph, int[,] nodeArray)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (nodeArray[y, x] == 1)
                    {
                        graph.UpdateNode(x, y, 1);
                    }
                }
            }
        }


        private Point ConvertWorldMapSquareCoordiantesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int destinationRow = pointInWorldMapSquareCoordinates.Y;
            int destinationColumn = pointInWorldMapSquareCoordinates.X;

            int mapSquareSize = 24;
            int halfMapSquareSize = mapSquareSize / 2;

            int destinationX = (destinationColumn * mapSquareSize) + halfMapSquareSize;
            int destinationY = (destinationRow * mapSquareSize) + halfMapSquareSize;

            return new Point(destinationX, destinationY);
        }


        private bool IsMinigunnerAtDestination(Minigunner minigunner, Point destination)
        {
            int leeway = 1;
            bool isAtXDestination =
                (minigunner.position.X > destination.X - leeway) &&
                (minigunner.position.X < destination.X + leeway);

            bool isAtYDestination =
                (minigunner.position.Y > destination.Y - leeway) &&
                (minigunner.position.Y < destination.Y + leeway);


            return isAtXDestination && isAtYDestination;
        }



        private void WaitForMinigunnerToArriveAtPosition(GameWorld gameWorld, Minigunner minigunner, Point destination, GameTime gameTime)
        {
            bool done = false;
            int numAttempts = 0;
            int maxNumRepetitions = 2000;

            bool isAtDestination = false;

            while (!done)
            {
                if (numAttempts > maxNumRepetitions)
                {
                    done = true;
                }
                else
                {

                    gameWorld.Update(gameTime);
                    isAtDestination = IsMinigunnerAtDestination(minigunner, destination);
                    if (isAtDestination)
                    {
                        done = true;
                    }
                    numAttempts++;
                }
            }

            // then
            Assert.IsTrue(isAtDestination);

        }





    }


}
