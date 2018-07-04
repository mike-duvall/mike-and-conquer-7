using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;

using GameMap = mike_and_conquer.GameMap;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;

namespace unit_tests
{
    [TestClass]
    public class MinigunnerUnitTests
    {


        [TestMethod]
        public void ShouldParseMapIntoSubMap()
        {
            // given

            System.IO.Stream inputStream = new FileStream("..\\..\\scg01ea.bin", FileMode.Open);
            //                        BinaryReader br = new BinaryReader(new FileStream("mydata", FileMode.Open));

            //System.IO.Stream inputStream = new System.IO.MemoryStream();
            //inputStream.WriteByte(0xff);
            //inputStream.WriteByte(0x00);
            //inputStream.Flush();
            //inputStream.Position = 0;

            // when
            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;
            GameMap gameMap = new GameMap(inputStream, startX, startY, endX, endY);


            // then
            // at assertions of size of gamemap and content of gamemap
            Assert.IsTrue(gameMap.MapTiles[0].byte1 == 0xff);
            Assert.IsTrue(gameMap.MapTiles[0].byte2 == 0x00);

            Assert.IsTrue(gameMap.MapTiles[1].byte1 == 0xff);
            Assert.IsTrue(gameMap.MapTiles[1].byte2 == 0x00);

            Assert.IsTrue(gameMap.MapTiles[2].byte1 == 0x18);
            Assert.IsTrue(gameMap.MapTiles[2].byte2 == 0x02);

            Assert.IsTrue(gameMap.MapTiles[3].byte1 == 0x18);
            Assert.IsTrue(gameMap.MapTiles[3].byte2 == 0x03);

            Assert.IsTrue(gameMap.MapTiles[4].byte1 == 0xff);
            Assert.IsTrue(gameMap.MapTiles[4].byte2 == 0x00);

        }


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
