using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace unit_tests
{
    [TestClass]
    public class MinigunnerUnitTests
    {

        [TestMethod]
        public void ContainsPoint_ShouldWorkAfterMinigunnerMoves()
        {
            // given
            float scale = 5f;
            Minigunner mingunner = new Minigunner(10, 10, false, scale);


            // when
            bool containsPoint = mingunner.ContainsPoint(10, 10);


            // then
            Assert.IsTrue(containsPoint);


            // when
            containsPoint = mingunner.ContainsPoint(40, 14);


            // then
            Assert.IsFalse(containsPoint);

            // when
            mingunner.OrderToMoveToDestination(200, 200);

            GameTime gameTime = new GameTime();
            TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 100);
            gameTime.ElapsedGameTime = timespan;


            // then
            bool done = false;
            int numAttempts = 0;
            int maxNumRepetitions = 50;

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
