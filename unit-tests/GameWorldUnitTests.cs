using System;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
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
            const int numColumns = 4;
            const int numRows = 3;
            

            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            };

            gameWorld.InitializeTestMap(obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(0, 0);
            Point minigunnerLocationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);
            
            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);

            // when
            int destinationColumn = 3;
            int destinationRow = 2;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then
            int[,] expectedPathArray = new int[numRows, numColumns]
            {
                { 1, 0, 0, 0 },
                { 2, 0, 0, 0 },
                { 0, 3, 4, 5 }
            };

            WaitForMinigunnerToFollowPath(gameWorld, minigunner, gameTime, expectedPathArray);
        }


        [TestMethod]
        public void MinigunnerShouldNavigateSimpleObstacles2()
        {
            // given
            GameWorld gameWorld = new GameWorld();
            const int numColumns = 6;
            const int numRows = 5;
//            gameWorld.InitializeDefaultMap();


            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0 }
            };

            gameWorld.InitializeTestMap(obstacleArray);
            //            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(1, 1);
            Point minigunnerLocationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);

            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);


            // when
            int destinationColumn = 3;
            int destinationRow = 1;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then

            int[,] expectedPathArray = new int[numRows, numColumns]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 7, 0, 0 },
                { 2, 0, 0, 6, 0, 0 },
                { 0, 3, 0, 5, 0, 0 },
                { 0, 0, 4, 0, 0, 0 }
            };

            WaitForMinigunnerToFollowPath(gameWorld, minigunner, gameTime, expectedPathArray);

        }


        [TestMethod]
        public void MinigunnerShouldNavigateSimpleObstacles3()
        {
            // given
            GameWorld gameWorld = new GameWorld();
            const int numColumns = 6;
            const int numRows = 5;
//            gameWorld.InitializeDefaultMap();


            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 }
            };

            //            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);
            gameWorld.InitializeTestMap(obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(0, 4);
            Point minigunnerLocationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);

            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);


            // when
            int destinationColumn = 3;
            int destinationRow = 4;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then

            int[,] expectedPathArray = new int[numRows, numColumns]
            {
                { 0, 0, 5, 0, 0, 0 },
                { 0, 4, 0, 6, 0, 0 },
                { 3, 0, 0, 7, 0, 0 },
                { 2, 0, 0, 8, 0, 0 },
                { 1, 0, 0, 9, 0, 0 }
            };



            WaitForMinigunnerToFollowPath(gameWorld, minigunner, gameTime, expectedPathArray);

        }


        [TestMethod]
        public void MinigunnerContainsPointShouldWorkBeforeAndAfterMovement()
        {
            // given
            GameWorld gameWorld = new GameWorld();
            const int numColumns = 4;
            const int numRows = 3;

            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            };

            gameWorld.InitializeTestMap(obstacleArray);

            Point minigunnerLocationInMapTileCoordinates = new Point(0, 0);
            Point minigunnerLocationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapTileCoordinates);

            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);

            // when
            int destinationColumn = 3;
            int destinationRow = 2;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then
            bool containsPoint = minigunner.ContainsPoint(minigunnerLocationInWorldCoordinates.X, minigunnerLocationInWorldCoordinates.Y);


            // then
            Assert.IsTrue(containsPoint);


            int[,] expectedPathArray = new int[numRows, numColumns]
            {
                { 1, 0, 0, 0 },
                { 2, 0, 0, 0 },
                { 0, 3, 4, 5 }
            };

            WaitForMinigunnerToFollowPath(gameWorld, minigunner, gameTime, expectedPathArray);

            // when

            destinationInWorldCoordinates.X =
                destinationInWorldCoordinates.X + 4;

            destinationInWorldCoordinates.Y =
                destinationInWorldCoordinates.Y - 3;

            containsPoint = minigunner.ContainsPoint(destinationInWorldCoordinates.X, destinationInWorldCoordinates.Y);


            // then
            Assert.IsTrue(containsPoint);


        }

        private void WaitForMinigunnerToFollowPath(
            GameWorld gameWorld,
            Minigunner minigunner,
            GameTime gameTime,
            int[,] expectedPathArray)
        {
            int currentPathIndex = 1;
            int maxPathIndex = FindMaxPathIndex(expectedPathArray);

            while (currentPathIndex <= maxPathIndex)
            {
                Point nextPathPoint = FindPointWithPathIndex(expectedPathArray, currentPathIndex);

                if (currentPathIndex != maxPathIndex)
                {
                    Point nextDestinationInWorldCoordinates =
                        gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(nextPathPoint);
                    WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, gameTime,
                        nextDestinationInWorldCoordinates);
                }
                else
                {
                    // Check for landing at Slot 0
                    Point landingSquareSlotDestinationInWorldCoordinates =
                        gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(nextPathPoint);

                    landingSquareSlotDestinationInWorldCoordinates =
                        UnitTestUtils.GetSlotLocationInWorldCoordinates(0,
                            landingSquareSlotDestinationInWorldCoordinates);

                    WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, gameTime,
                        landingSquareSlotDestinationInWorldCoordinates);

                }

                currentPathIndex++;
            }

        }


        Point FindPointWithPathIndex(int[,] pathArray, int pathIndex)
        {

            int rank1 = pathArray.GetLength(0);
            int rank2 = pathArray.GetLength(1);

            for (int x = 0; x < rank2; x++)
            {
                for (int y = 0; y < rank1; y++)
                {
                    int nextValue = pathArray[y, x];

                    if (nextValue == pathIndex)
                    {
                        return new Point(x,y);
                    }
                }
            }

            throw new SystemException("Did not find pathIndex");

        }

        int FindMaxPathIndex(int[,] pathArray)
        {
            int rank1 = pathArray.GetLength(0);
            int rank2 = pathArray.GetLength(1);

            int maxValue = 0;

            for (int x = 0; x < rank2; x++)
            {
                for (int y = 0; y < rank1; y++)
                {
                    int nextValue = pathArray[y, x];

                    if (nextValue > maxValue)
                    {
                        maxValue = nextValue;
                    }
                }
            }

            return maxValue;
        }

        private void WaitForMinigunnerToArriveAtPosition(GameWorld gameWorld, Minigunner minigunner,  GameTime gameTime, Point destination)
        {
            bool done = false;
            int numAttempts = 0;
            int maxNumRepetitions = 3000;

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
                    isAtDestination = UnitTestUtils.IsMinigunnerAtDestination(minigunner, destination);
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
