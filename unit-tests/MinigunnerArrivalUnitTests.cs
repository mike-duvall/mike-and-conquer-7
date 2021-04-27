using System;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using Minigunner = mike_and_conquer.gameobjects.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Point = Microsoft.Xna.Framework.Point;


namespace unit_tests
{
    [TestClass]
    public class MinigunnerArrivalUnitTests
    {


        // [TestMethod]
        // public void MinigunnerShouldNavigateSimpleObstacles()
        // {
        //     // given
        //     GameWorld gameWorld = new GameWorld();
        //     const int numColumns = 10;
        //     const int numRows = 10;
        //
        //     int[,] obstacleArray = new int[numRows, numColumns];
        //
        //     gameWorld.InitializeTestMap(obstacleArray);
        //
        //     List<Minigunner> minigunnerList = new List<Minigunner>();
        //     minigunnerList.Add(AddMinigunnerAtWorldtMapTileCoordinate(gameWorld, 0, 0));
        //     minigunnerList.Add(AddMinigunnerAtWorldtMapTileCoordinate(gameWorld, 1, 0));
        //     minigunnerList.Add(AddMinigunnerAtWorldtMapTileCoordinate(gameWorld, 1, 1));
        //     minigunnerList.Add(AddMinigunnerAtWorldtMapTileCoordinate(gameWorld, 2, 1));
        //     minigunnerList.Add(AddMinigunnerAtWorldtMapTileCoordinate(gameWorld, 2, 2));
        //
        //
        //     // when
        //     int destinationColumn = 8;
        //     int destinationRow = 7;
        //     Point destinationInWorldMapTileCoordinate = new Point(destinationColumn, destinationRow);
        //
        //     Point destinationInWorldCoordinates =
        //         gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(destinationInWorldMapTileCoordinate);
        //
        //     foreach (Minigunner minigunner in minigunnerList)
        //     {
        //         minigunner.OrderToMoveToDestination(destinationInWorldCoordinates);
        //     }
        //
        //     // then
        //     WaitForAllMinigunnersToArriveAtSlotDestinations(gameWorld, minigunnerList, destinationInWorldMapTileCoordinate);
        // }
        //
        // private void WaitForAllMinigunnersToArriveAtSlotDestinations(GameWorld gameWorld, List<Minigunner> minigunnerList, Point destinationInWorldMapTileCoordinate)
        // {
        //
        //     GameTime gameTime = new GameTime();
        //     TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 10);
        //     gameTime.ElapsedGameTime = timespan;
        //
        //     int numberOfMinigunner = minigunnerList.Count;
        //
        //     List<Minigunner> unArrivedMinigunnersList = minigunnerList;
        //
        //
        //     Point destinationInWorldCoordinates =
        //         gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(destinationInWorldMapTileCoordinate);
        //
        //
        //     for (int i = 0; i < numberOfMinigunner; i++)
        //     {
        //         Point landingSquareSlotDestinationInWorldCoordinates =
        //             UnitTestUtils.GetSlotLocationInWorldCoordinates(i, destinationInWorldCoordinates);
        //
        //         Minigunner arrivedMinigunner = WaitForAnyMinigunnerToArriveAtPosition(gameWorld, minigunnerList, gameTime,
        //             landingSquareSlotDestinationInWorldCoordinates);
        //
        //         unArrivedMinigunnersList.Remove(arrivedMinigunner);
        //
        //     }
        //
        //     Assert.IsTrue(unArrivedMinigunnersList.Count == 0);
        //
        // }
        //
        // private Minigunner AddMinigunnerAtWorldtMapTileCoordinate(GameWorld gameWorld,int x, int y)
        // {
        //     Point minigunnerLocationInMapSquareCoordinates = new Point(x, y);
        //     Point minigunnerLocationInWorldCoordinates =
        //         gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(minigunnerLocationInMapSquareCoordinates);
        //
        //     return gameWorld.AddGdiMinigunner(minigunnerLocationInWorldCoordinates);
        //
        // }
        //
        //
        // private Minigunner WaitForAnyMinigunnerToArriveAtPosition(GameWorld gameWorld, List<Minigunner> minigunnerList, GameTime gameTime, Point destination)
        // {
        //     bool done = false;
        //     int numAttempts = 0;
        //     int maxNumRepetitions = 3000;
        //
        //     bool isAtDestination = false;
        //
        //     while (!done)
        //     {
        //         if (numAttempts > maxNumRepetitions)
        //         {
        //             done = true;
        //         }
        //         else
        //         {
        //             gameWorld.Update(gameTime);
        //             foreach (Minigunner minigunner in minigunnerList)
        //             {
        //                 isAtDestination = UnitTestUtils.IsMinigunnerAtDestination(minigunner, destination);
        //                 if (isAtDestination)
        //                 {
        //                     return minigunner;
        //                 }
        //             }
        //             numAttempts++;
        //         }
        //     }
        //
        //     // then
        //     Assert.Fail("Did not find any minigunner that arrived at expected position:" + destination);
        //     return null;
        //
        // }


    }


}
