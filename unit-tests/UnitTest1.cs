using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GdiMinigunner = mike_and_conquer.GdiMinigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace unit_tests
{
    [TestClass]
    public class MinigunnerUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // given
            float scale = 5f;
            GdiMinigunner mingunner = new GdiMinigunner(10,10, scale);


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
            TimeSpan timespan = new TimeSpan(0,0, 0, 0, 100);
            gameTime.ElapsedGameTime = timespan;
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);
            mingunner.Update(gameTime);


            // when
            containsPoint = mingunner.ContainsPoint(200, 200);


            // then
            Assert.IsTrue(containsPoint);


        }
    }
}
