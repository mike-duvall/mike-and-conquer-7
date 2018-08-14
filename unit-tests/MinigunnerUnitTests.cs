using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;

using GameMap = mike_and_conquer.GameMap;
using TextureListMap = mike_and_conquer.TextureListMap;

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

            // when
            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            int numColumns = endX - startX + 1;
            int numRows = endY - startY + 1;

            GameMap gameMap = new GameMap(inputStream, startX, startY, endX, endY);

            
            int indexOffset = 0;
            // 1st row
            //Assert.IsTrue(gameMap.MapTiles[0].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[0].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 12);

            //Assert.IsTrue(gameMap.MapTiles[1].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[1].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 13);


            //Assert.IsTrue(gameMap.MapTiles[2].byte1 == 0x18);
            //Assert.IsTrue(gameMap.MapTiles[2].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.S12_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x02);

            //Assert.IsTrue(gameMap.MapTiles[3].byte1 == 0x18);
            //Assert.IsTrue(gameMap.MapTiles[3].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.S12_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x03);


            //Assert.IsTrue(gameMap.MapTiles[4].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[4].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 12);

            ////{ TextureListMap.CLEAR1_SHP,"13"}, // 5
            //Assert.IsTrue(gameMap.MapTiles[5].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[5].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 13);


            ////{ TextureListMap.D21_TEM,"0"}, // 6
            //Assert.IsTrue(gameMap.MapTiles[6].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[6].byte2 == 0x00);

            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x00);


            ////{ TextureListMap.D21_TEM,"1"}, // 7
            //Assert.IsTrue(gameMap.MapTiles[7].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[7].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x01);

            ////{ TextureListMap.D21_TEM,"2"}, // 8
            //Assert.IsTrue(gameMap.MapTiles[8].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[8].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x02);

            ////{ TextureListMap.D23_TEM,"6"}, // 9
            //Assert.IsTrue(gameMap.MapTiles[9].byte1 == 0x73);
            //Assert.IsTrue(gameMap.MapTiles[9].byte2 == 0x06);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D23_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x06);


            ////{ TextureListMap.D23_TEM,"7"}, // 10
            //Assert.IsTrue(gameMap.MapTiles[10].byte1 == 0x73);
            //Assert.IsTrue(gameMap.MapTiles[10].byte2 == 0x07);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D23_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x07);



            ////{ TextureListMap.CLEAR1_SHP,"15"}, // 11
            //Assert.IsTrue(gameMap.MapTiles[11].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[11].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 15);


            ////{ TextureListMap.CLEAR1_SHP,"12"}, // 12
            //Assert.IsTrue(gameMap.MapTiles[12].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[12].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 12);


            ////{ TextureListMap.CLEAR1_SHP,"13"}, // 13
            //Assert.IsTrue(gameMap.MapTiles[13].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[13].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 13);


            ////{ TextureListMap.P07_TEM,"00"}, // 14
            //Assert.IsTrue(gameMap.MapTiles[14].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[14].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x00);

            ////{ TextureListMap.P07_TEM,"01"}, // 15
            //Assert.IsTrue(gameMap.MapTiles[15].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[15].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x01);

            ////{ TextureListMap.P07_TEM,"02"}, // 16
            //Assert.IsTrue(gameMap.MapTiles[16].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[16].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x02);


            ////{ TextureListMap.P07_TEM,"03"}, // 17
            //Assert.IsTrue(gameMap.MapTiles[17].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[17].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x03);


            ////{ TextureListMap.CLEAR1_SHP,"14"}, // 18
            //Assert.IsTrue(gameMap.MapTiles[18].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[18].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 14);


            ////{ TextureListMap.CLEAR1_SHP,"15"}, // 19
            //Assert.IsTrue(gameMap.MapTiles[19].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[19].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 15);


            ////{ TextureListMap.CLEAR1_SHP,"12"}, // 20
            //Assert.IsTrue(gameMap.MapTiles[20].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[20].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 12);


            ////{ TextureListMap.CLEAR1_SHP,"13"}, // 21
            //Assert.IsTrue(gameMap.MapTiles[21].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[21].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 13);


            ////{ TextureListMap.CLEAR1_SHP,"14"}, // 22
            //Assert.IsTrue(gameMap.MapTiles[22].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[22].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 14);


            ////{ TextureListMap.CLEAR1_SHP,"15"}, // 23
            //Assert.IsTrue(gameMap.MapTiles[23].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[23].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 15);


            ////{ TextureListMap.CLEAR1_SHP,"12"}, // 24
            //Assert.IsTrue(gameMap.MapTiles[24].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[24].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 12);


            ////{ TextureListMap.CLEAR1_SHP,"13"}, // 25
            //Assert.IsTrue(gameMap.MapTiles[25].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[25].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 13);



            //// 2nd row

            ////{ TextureListMap.CLEAR1_SHP,"00"}, // 26
            //int indexOffset = numColumns;
            indexOffset = numColumns;
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x00);


            ////            { TextureListMap.CLEAR1_SHP,"01"}, // 27
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 1);


            ////{ TextureListMap.S09_TEM,"00"}, // 28
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x00);

            ////{ TextureListMap.S09_TEM,"01"}, // 29
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x01);


            ////{ TextureListMap.S09_TEM,"02"}, // 30
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x02);


            ////{ TextureListMap.CLEAR1_SHP,"01"}, // 31
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 1);


            ////{ TextureListMap.D21_TEM,"03"}, // 32
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x03);


            ////{ TextureListMap.D21_TEM,"04"}, // 33
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x04);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x04);


            ////{ TextureListMap.D21_TEM,"05"}, // 34
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x05);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x05);


            ////{ TextureListMap.CLEAR1_SHP,"01"}, // 35
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 1);


            ////{ TextureListMap.CLEAR1_SHP,"02"}, // 36
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 2);


            ////{ TextureListMap.CLEAR1_SHP,"03"}, // 37
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 3);


            ////{ TextureListMap.CLEAR1_SHP,"00"}, // 38
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0);


            ////{ TextureListMap.CLEAR1_SHP,"01"}, // 39
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 1);


            ////{ TextureListMap.P07_TEM,"04"}, // 40
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x04);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x04);


            ////{ TextureListMap.P07_TEM,"05"}, // 41
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x05);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x05);


            ////{ TextureListMap.P07_TEM,"06"}, // 42
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x06);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x06);


            ////{ TextureListMap.P07_TEM,"07"}, // 43
            //Assert.IsTrue(gameMap.MapTiles[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTiles[indexOffset++].byte2 == 0x07);
            Assert.IsTrue(gameMap.MapTiles[indexOffset].textureKey == TextureListMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTiles[indexOffset++].imageIndex == 0x07);





        }


        [TestMethod]
        public void ContainsPoint_ShouldWorkAfterMinigunnerMoves()
        {
            // given
            Minigunner mingunner = new Minigunner(10, 10, false);


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
