using System;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
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
            const int numColumns = 4;
            const int numRows = 3;
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
                ConvertMapSquareCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);
            
            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);

            // when
            int destinationColumn = 3;
            int destinationRow = 2;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
            gameTime.ElapsedGameTime = timespan;

            // then
            //            WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, gameTime,  destinationInWorldCoordinates);
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
            gameWorld.Initialize(numColumns, numRows);


            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0 }
            };

            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(1, 1);
            Point minigunnerLocationInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);

            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);


            // when
            int destinationColumn = 3;
            int destinationRow = 1;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

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
            gameWorld.Initialize(numColumns, numRows);


            int[,] obstacleArray = new int[numRows, numColumns]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 1, 0 }
            };

            UpdateGraphFromFromObstacleArray(gameWorld.navigationGraph, obstacleArray);

            Point minigunnerLocationInMapSquareCoordinates = new Point(0, 4);
            Point minigunnerLocationInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);

            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);


            // when
            int destinationColumn = 3;
            int destinationRow = 4;
            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);

            Point destinationInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);

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



        private void UpdateGraphFromFromObstacleArray(Graph graph, int[,] nodeArray)
        {

            int rank1 = nodeArray.GetLength(0);
            int rank2 = nodeArray.GetLength(1);

            for (int x = 0; x < rank2; x++)
            {
                for (int y = 0; y < rank1; y++)
                {
                    if (nodeArray[y, x] == 1)
                    {
                        graph.MakeNodeBlockingNode(x, y);
                    }
                }
            }

            graph.RebuildAdajencyGraph();
        }

        private Point ConvertMapSquareCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int destinationRow = pointInWorldMapSquareCoordinates.Y;
            int destinationColumn = pointInWorldMapSquareCoordinates.X;

            int mapSquareSize = 24;
            int halfMapSquareSize = mapSquareSize / 2;

            int destinationX = (destinationColumn * mapSquareSize) + halfMapSquareSize;
            int destinationY = (destinationRow * mapSquareSize) + halfMapSquareSize;

            return new Point(destinationX, destinationY);
        }

        private Point ConvertWorldCoordinatesToMapSquareCoordinates(Point pointInWorldCoordinates)
        {

            int destinationRow = pointInWorldCoordinates.Y;
            int destinationColumn = pointInWorldCoordinates.X;

            int mapSquareSize = 24;

            int destinationX = destinationColumn / mapSquareSize;
            int destinationY = destinationRow / mapSquareSize;

            return new Point(destinationX, destinationY);
        }


        private List<Point> ConvertWorldCoordinatesListToMapSquareCoordinatesList(List<Point> worldCoordinatesList)
        {
            List<Point> mapSquareCoordinatesList = new List<Point>();

            foreach(Point point in worldCoordinatesList)
            {
                mapSquareCoordinatesList.Add(ConvertWorldCoordinatesToMapSquareCoordinates(point));
            }

            return mapSquareCoordinatesList;
        }


        private bool IsMinigunnerAtDestination(Minigunner minigunner, Point destination)
        {
            int leeway = 1;
            bool isAtXDestination =
                (minigunner.positionInWorldCoordinates.X > destination.X - leeway) &&
                (minigunner.positionInWorldCoordinates.X < destination.X + leeway);

            bool isAtYDestination =
                (minigunner.positionInWorldCoordinates.Y > destination.Y - leeway) &&
                (minigunner.positionInWorldCoordinates.Y < destination.Y + leeway);


            return isAtXDestination && isAtYDestination;
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
                Point nextDestinationInWorldCoordinates =
                    ConvertMapSquareCoordinatesToWorldCoordinates(nextPathPoint);
                WaitForMinigunnerToArriveAtPosition(gameWorld, minigunner, gameTime,
                    nextDestinationInWorldCoordinates);
                currentPathIndex++;
            }

        }


        Point FindPointWithPathIndex(int[,] pathArray, int pathIndex)
        {

            int rank1 = pathArray.GetLength(0);
            int rank2 = pathArray.GetLength(1);

            int maxValue = 0;

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
