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
            bool containsPoint = mingunner.ContainsPoint(39, 14);


            // then
            Assert.IsTrue(containsPoint);


            // when
            containsPoint = mingunner.ContainsPoint(40, 14);


            // then
            Assert.IsFalse(containsPoint);


            Understand why this test passes, why 40,14 is not contained but 39,14 is


            //// when
            //mingunner.OrderToMoveToDestination(200, 200);

            //GameTime gameTime = new GameTime();
            //gameTime.
            //mingunner.Update(gameTime)
        }
    }
}
