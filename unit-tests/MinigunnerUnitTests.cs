using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;

using GameMap = mike_and_conquer.GameMap;
using TextureListMap = mike_and_conquer.TextureListMap;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;

using Point = Microsoft.Xna.Framework.Point;

namespace unit_tests
{
    [TestClass]
    public class MinigunnerUnitTests
    {

        [TestMethod]
        public void ShouldNavigatePath()
        {
            // given
            Minigunner minigunner = new Minigunner(10, 10);

            // when
            List<Point> listOfPoints = new List<Point>();
            Point pathPoint1 = new Point(15, 15);
            Point pathPoint2 = new Point(25, 35);
            listOfPoints.Add(pathPoint1);
            listOfPoints.Add(pathPoint2);

            minigunner.OrderToFollowPath(listOfPoints);


            // then
            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 15);
            gameTime.ElapsedGameTime = timespan;

            WaitForMinigunnerToArriveAtPosition(minigunner, pathPoint1, gameTime);
            WaitForMinigunnerToArriveAtPosition(minigunner, pathPoint2, gameTime);

        }

        private void WaitForMinigunnerToArriveAtPosition(Minigunner minigunner, Point destination, GameTime gameTime)
        {
            bool done = false;
            int numAttempts = 0;
            int maxNumRepetitions = 1200;

            bool isAtDestination = false;

            while (!done)
            {
                if (numAttempts > maxNumRepetitions)
                {
                    done = true;
                }
                else
                {
                    minigunner.Update(gameTime);
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

        [TestMethod]
        public void ContainsPoint_ShouldWorkAfterMinigunnerMoves()
        {
            // given
            Minigunner mingunner = new Minigunner(10, 10);


            // when
            bool containsPoint = mingunner.ContainsPoint(10, 10);


            // then
            Assert.IsTrue(containsPoint);


            // when
            containsPoint = mingunner.ContainsPoint(40, 14);


            // then
            Assert.IsFalse(containsPoint);

            // when
            mingunner.OrderToMoveToDestination(new Point(200, 200));

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 100);
            gameTime.ElapsedGameTime = timespan;


            // then
            bool done = false;
            int numAttempts = 0;
            int maxNumRepetitions = 200;

            while(!done)
            {
                if(numAttempts > maxNumRepetitions)
                {
                    done = true;
                }
                else
                {
                    mingunner.Update(gameTime);
                    containsPoint = mingunner.ContainsPoint(200, 200);
                    if (containsPoint)
                    {
                        done = true;
                    }
                    numAttempts++;
                }
            }

            // then
            Assert.IsTrue(containsPoint);


        }


    }

}
