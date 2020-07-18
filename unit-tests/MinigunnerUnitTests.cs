using System;
using System.Collections.Generic;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mike_and_conquer.pathfinding;
using Minigunner = mike_and_conquer.gameobjects.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Point = Microsoft.Xna.Framework.Point;

namespace unit_tests
{
    [TestClass]
    public class MinigunnerUnitTests
    {



        //        [TestMethod]
        //        public void ContainsPoint_ShouldWorkAfterMinigunnerMoves()
        //        {
        //            // given
        //            NavigationGraph navigationGraph = new NavigationGraph(26, 24);
        //            Minigunner mingunner = new Minigunner(10, 10, navigationGraph);
        //
        //
        //            // when
        //            bool containsPoint = mingunner.ContainsPoint(10, 10);
        //
        //
        //            // then
        //            Assert.IsTrue(containsPoint);
        //
        //
        //            // when
        //            containsPoint = mingunner.ContainsPoint(40, 14);
        //
        //
        //            // then
        //            Assert.IsFalse(containsPoint);
        //
        //            // when
        //            mingunner.OrderToMoveToDestination(new Point(200, 200));
        //
        //            GameTime gameTime = new GameTime();
        //            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
        //            gameTime.ElapsedGameTime = timespan;
        //
        //
        //            // then
        //            bool done = false;
        //            int numAttempts = 0;
        //            int maxNumRepetitions = 2000;
        //
        //            while(!done)
        //            {
        //                if(numAttempts > maxNumRepetitions)
        //                {
        //                    done = true;
        //                }
        //                else
        //                {
        //                    mingunner.Update(gameTime);
        //                    containsPoint = mingunner.ContainsPoint(200, 200);
        //                    if (containsPoint)
        //                    {
        //                        done = true;
        //                    }
        //                    numAttempts++;
        //                }
        //            }
        //
        //            // then
        //            Assert.IsTrue(containsPoint);
        //
        //
        //        }
        //
        //
        //        private void WaitForMinigunnerToArriveAtPosition(Minigunner minigunner, Point destination, GameTime gameTime)
        //        {
        //            bool done = false;
        //            int numAttempts = 0;
        //            int maxNumRepetitions = 1200;
        //
        //            bool isAtDestination = false;
        //
        //            while (!done)
        //            {
        //                if (numAttempts > maxNumRepetitions)
        //                {
        //                    done = true;
        //                }
        //                else
        //                {
        //                    minigunner.Update(gameTime);
        //                    isAtDestination = IsMinigunnerAtDestination(minigunner, destination);
        //                    if (isAtDestination)
        //                    {
        //                        done = true;
        //                    }
        //                    numAttempts++;
        //                }
        //            }
        //
        //            // then
        //            Assert.IsTrue(isAtDestination);
        //
        //        }
        //
        //        private bool IsMinigunnerAtDestination(Minigunner minigunner, Point destination)
        //        {
        //            int leeway = 1;
        //            bool isAtXDestination =
        //                (minigunner.positionInWorldCoordinates.X > destination.X - leeway) &&
        //                (minigunner.positionInWorldCoordinates.X < destination.X + leeway);
        //
        //            bool isAtYDestination =
        //                (minigunner.positionInWorldCoordinates.Y > destination.Y - leeway) &&
        //                (minigunner.positionInWorldCoordinates.Y < destination.Y + leeway);
        //
        //
        //            return isAtXDestination && isAtYDestination;
        //        }
        //

//        [TestMethod]
//        public void MinigunnerContainsPointShouldWorkBeforeAndAfterMovement()
//        {
//            // given
//            GameWorld gameWorld = new GameWorld();
//            const int numColumns = 4;
//            const int numRows = 3;
//
//
//            int[,] obstacleArray = new int[numRows, numColumns]
//            {
//                { 0, 0, 1, 0 },
//                { 0, 1, 1, 0 },
//                { 0, 0, 0, 0 }
//            };
//
//            gameWorld.InitializeTestMap(obstacleArray);
//
//            Point minigunnerLocationInMapSquareCoordinates = new Point(0, 0);
//            Point minigunnerLocationInWorldCoordinates =
//                gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);
//
//            Minigunner minigunner = gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);
//
//            // when
//            int destinationColumn = 3;
//            int destinationRow = 2;
//            Point destinationInWorldMapSquareCoordinate = new Point(destinationColumn, destinationRow);
//
//            Point destinationInWorldCoordinates =
//                gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(destinationInWorldMapSquareCoordinate);
//
//            minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);
//
//            GameTime gameTime = new GameTime();
//            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
//            gameTime.ElapsedGameTime = timespan;
//
//            // then
//            int[,] expectedPathArray = new int[numRows, numColumns]
//            {
//                { 1, 0, 0, 0 },
//                { 2, 0, 0, 0 },
//                { 0, 3, 4, 5 }
//            };
//
//            WaitForMinigunnerToFollowPath(gameWorld, minigunner, gameTime, expectedPathArray);
//        }
//
//


    }


}
