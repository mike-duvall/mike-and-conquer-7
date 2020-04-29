
using Microsoft.VisualStudio.TestTools.UnitTesting;


using GameMap = mike_and_conquer.gameworld.GameMap;


using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;


namespace unit_tests
{
    [TestClass]
    public class GameMapUnitTests
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
            //Assert.IsTrue(gameMap.MapTileInstanceList[0].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[0].byte2 == 0x00);
            //            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            //            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);



            //Assert.IsTrue(gameMap.MapTileInstanceList[1].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[1].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 13);


            //Assert.IsTrue(gameMap.MapTileInstanceList[2].byte1 == 0x18);
            //Assert.IsTrue(gameMap.MapTileInstanceList[2].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.S12_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x02);

            //Assert.IsTrue(gameMap.MapTileInstanceList[3].byte1 == 0x18);
            //Assert.IsTrue(gameMap.MapTileInstanceList[3].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.S12_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x03);


            //Assert.IsTrue(gameMap.MapTileInstanceList[4].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[4].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);

            ////{ GameMap.CLEAR1_SHP,"13"}, // 5
            //Assert.IsTrue(gameMap.MapTileInstanceList[5].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[5].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 13);


            ////{ GameMap.D21_TEM,"0"}, // 6
            //Assert.IsTrue(gameMap.MapTileInstanceList[6].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[6].byte2 == 0x00);

            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x00);


            ////{ GameMap.D21_TEM,"1"}, // 7
            //Assert.IsTrue(gameMap.MapTileInstanceList[7].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[7].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x01);

            ////{ GameMap.D21_TEM,"2"}, // 8
            //Assert.IsTrue(gameMap.MapTileInstanceList[8].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[8].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x02);

            ////{ GameMap.D23_TEM,"6"}, // 9
            //Assert.IsTrue(gameMap.MapTileInstanceList[9].byte1 == 0x73);
            //Assert.IsTrue(gameMap.MapTileInstanceList[9].byte2 == 0x06);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D23_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x06);


            ////{ GameMap.D23_TEM,"7"}, // 10
            //Assert.IsTrue(gameMap.MapTileInstanceList[10].byte1 == 0x73);
            //Assert.IsTrue(gameMap.MapTileInstanceList[10].byte2 == 0x07);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D23_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x07);



            ////{ GameMap.CLEAR1_SHP,"15"}, // 11
            //Assert.IsTrue(gameMap.MapTileInstanceList[11].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[11].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 15);


            ////{ GameMap.CLEAR1_SHP,"12"}, // 12
            //Assert.IsTrue(gameMap.MapTileInstanceList[12].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[12].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);


            ////{ GameMap.CLEAR1_SHP,"13"}, // 13
            //Assert.IsTrue(gameMap.MapTileInstanceList[13].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[13].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 13);


            ////{ GameMap.P07_TEM,"00"}, // 14
            //Assert.IsTrue(gameMap.MapTileInstanceList[14].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[14].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x00);

            ////{ GameMap.P07_TEM,"01"}, // 15
            //Assert.IsTrue(gameMap.MapTileInstanceList[15].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[15].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x01);

            ////{ GameMap.P07_TEM,"02"}, // 16
            //Assert.IsTrue(gameMap.MapTileInstanceList[16].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[16].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x02);


            ////{ GameMap.P07_TEM,"03"}, // 17
            //Assert.IsTrue(gameMap.MapTileInstanceList[17].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[17].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x03);


            ////{ GameMap.CLEAR1_SHP,"14"}, // 18
            //Assert.IsTrue(gameMap.MapTileInstanceList[18].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[18].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 14);


            ////{ GameMap.CLEAR1_SHP,"15"}, // 19
            //Assert.IsTrue(gameMap.MapTileInstanceList[19].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[19].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 15);


            ////{ GameMap.CLEAR1_SHP,"12"}, // 20
            //Assert.IsTrue(gameMap.MapTileInstanceList[20].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[20].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);


            ////{ GameMap.CLEAR1_SHP,"13"}, // 21
            //Assert.IsTrue(gameMap.MapTileInstanceList[21].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[21].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 13);


            ////{ GameMap.CLEAR1_SHP,"14"}, // 22
            //Assert.IsTrue(gameMap.MapTileInstanceList[22].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[22].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 14);


            ////{ GameMap.CLEAR1_SHP,"15"}, // 23
            //Assert.IsTrue(gameMap.MapTileInstanceList[23].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[23].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 15);


            ////{ GameMap.CLEAR1_SHP,"12"}, // 24
            //Assert.IsTrue(gameMap.MapTileInstanceList[24].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[24].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 12);


            ////{ GameMap.CLEAR1_SHP,"13"}, // 25
            //Assert.IsTrue(gameMap.MapTileInstanceList[25].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[25].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 13);



            //// 2nd row

            ////{ GameMap.CLEAR1_SHP,"00"}, // 26
            //int indexOffset = numColumns;
            indexOffset = numColumns;
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x00);


            ////            { GameMap.CLEAR1_SHP,"01"}, // 27
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 1);


            ////{ GameMap.S09_TEM,"00"}, // 28
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x00);

            ////{ GameMap.S09_TEM,"01"}, // 29
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x01);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x01);


            ////{ GameMap.S09_TEM,"02"}, // 30
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x15);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x02);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.S09_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x02);


            ////{ GameMap.CLEAR1_SHP,"01"}, // 31
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 1);


            ////{ GameMap.D21_TEM,"03"}, // 32
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x03);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x03);


            ////{ GameMap.D21_TEM,"04"}, // 33
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x04);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x04);


            ////{ GameMap.D21_TEM,"05"}, // 34
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x71);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x05);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.D21_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x05);


            ////{ GameMap.CLEAR1_SHP,"01"}, // 35
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 1);


            ////{ GameMap.CLEAR1_SHP,"02"}, // 36
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 2);


            ////{ GameMap.CLEAR1_SHP,"03"}, // 37
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 3);


            ////{ GameMap.CLEAR1_SHP,"00"}, // 38
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0);


            ////{ GameMap.CLEAR1_SHP,"01"}, // 39
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0xff);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x00);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.CLEAR1_SHP);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 1);


            ////{ GameMap.P07_TEM,"04"}, // 40
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x04);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x04);


            ////{ GameMap.P07_TEM,"05"}, // 41
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x05);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x05);


            ////{ GameMap.P07_TEM,"06"}, // 42
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x06);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x06);


            ////{ GameMap.P07_TEM,"07"}, // 43
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].byte1 == 0x49);
            //Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].byte2 == 0x07);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset].TextureKey == GameMap.P07_TEM);
            Assert.IsTrue(gameMap.MapTileInstanceList[indexOffset++].ImageIndex == 0x07);


        }


    }

}
