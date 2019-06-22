
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mike_and_conquer.pathfinding;
using Node = mike_and_conquer.pathfinding.Node;

namespace unit_tests
{
    [TestClass]
    public class GraphUnitTests
    {



        [TestMethod]
        public void ShouldHaveCorrectConnectedNodes1()
        {
            // given
//          0, 0, 0
//          0, 0, 0
//          0, 0, 0

            NavigationGraph navigationGraph = new NavigationGraph(3,3);

            // then
            Node node = navigationGraph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));

            node = navigationGraph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = navigationGraph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = navigationGraph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 8);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(7));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[8];
            Assert.IsTrue(node.id == 8);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(7));
        }


        [TestMethod]
        public void ShouldHaveCorrectConnectedNodes2()
        {
            // given

//          0, 0, 0
//          0, 1, 0
//          0, 0, 0

            NavigationGraph navigationGraph = new NavigationGraph(3, 3);

            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.RebuildAdajencyGraph();


            // then
            Node node = navigationGraph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));


            node = navigationGraph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = navigationGraph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = navigationGraph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(7));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[8];
            Assert.IsTrue(node.id == 8);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(7));
        }


        [TestMethod]
        public void ShouldHaveCorrectConnectedNodes3()
        {
            // given
//          0, 0, 1
//          0, 1, 1
//          0, 0, 0

            NavigationGraph navigationGraph = new NavigationGraph(3,3);

            navigationGraph.MakeNodeBlockingNode(2, 0);
            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.RebuildAdajencyGraph();

            // then
            Node node = navigationGraph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));


            node = navigationGraph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = navigationGraph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = navigationGraph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = navigationGraph.nodeList[8];
            Assert.IsTrue(node.id == 8);
            Assert.IsTrue(node.connectedNodes.Count == 1);
            Assert.IsTrue(node.connectedNodes.Contains(7));

        }


        [TestMethod]
        public void ShouldHaveCorrectConnectedNodesWithAsymmetricGraph1()
        {
            // given
            //  0, 0, 0, 0, 0
            //  0, 1, 1, 1, 0
            //  0, 0, 0, 0, 0

            // when
            NavigationGraph navigationGraph = new NavigationGraph(5,3);

            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.MakeNodeBlockingNode(3, 1);
            navigationGraph.RebuildAdajencyGraph();


            // then
            Node node = navigationGraph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = navigationGraph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(2));

            node = navigationGraph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = navigationGraph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(9));

            node = navigationGraph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(9));

            node = navigationGraph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(10));
            Assert.IsTrue(node.connectedNodes.Contains(11));

            node = navigationGraph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[8];
            Assert.IsTrue(node.id == 8);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = navigationGraph.nodeList[9];
            Assert.IsTrue(node.id == 9);
            Assert.IsTrue(node.connectedNodes.Count == 4);

            node = navigationGraph.nodeList[10];
            Assert.IsTrue(node.id == 10);
            Assert.IsTrue(node.connectedNodes.Count == 2);

            node = navigationGraph.nodeList[11];
            Assert.IsTrue(node.id == 11);
            Assert.IsTrue(node.connectedNodes.Count == 3);

            node = navigationGraph.nodeList[12];
            Assert.IsTrue(node.id == 12);
            Assert.IsTrue(node.connectedNodes.Count == 2);

            node = navigationGraph.nodeList[13];
            Assert.IsTrue(node.id == 13);
            Assert.IsTrue(node.connectedNodes.Count == 3);

            node = navigationGraph.nodeList[14];
            Assert.IsTrue(node.id == 14);
            Assert.IsTrue(node.connectedNodes.Count == 2);
        }


        [TestMethod]
        public void ShouldHaveCorrectConnectedNodesWithAsymmetricGraph2()
        {
            // given
            //  0, 0, 0, 0, 0, ... 26 columns
            //  0, 1, 1, 1, 0, ...
            //  0, 0, 0, 0, 0, ...
            //  .
            //  .
            // 24 rows

            NavigationGraph navigationGraph = new NavigationGraph(26, 24);


            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.MakeNodeBlockingNode(3, 1);
            navigationGraph.RebuildAdajencyGraph();

            // then
            Node node = navigationGraph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(26));

            node = navigationGraph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(26));

            node = navigationGraph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = navigationGraph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(30));

            node = navigationGraph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(30));
            Assert.IsTrue(node.connectedNodes.Contains(31));

            node = navigationGraph.nodeList[26];
            Assert.IsTrue(node.id == 26);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(52));
            Assert.IsTrue(node.connectedNodes.Contains(53));

            //node = navigationGraph.nodeList[6];
            //Assert.IsTrue(node.id == 6);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = navigationGraph.nodeList[7];
            //Assert.IsTrue(node.id == 7);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = navigationGraph.nodeList[8];
            //Assert.IsTrue(node.id == 8);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = navigationGraph.nodeList[9];
            //Assert.IsTrue(node.id == 9);
            //Assert.IsTrue(node.connectedNodes.Count == 4);

            //node = navigationGraph.nodeList[10];
            //Assert.IsTrue(node.id == 10);
            //Assert.IsTrue(node.connectedNodes.Count == 2);

            //node = navigationGraph.nodeList[11];
            //Assert.IsTrue(node.id == 11);
            //Assert.IsTrue(node.connectedNodes.Count == 3);

            //node = navigationGraph.nodeList[12];
            //Assert.IsTrue(node.id == 12);
            //Assert.IsTrue(node.connectedNodes.Count == 2);

            //node = navigationGraph.nodeList[13];
            //Assert.IsTrue(node.id == 13);
            //Assert.IsTrue(node.connectedNodes.Count == 3);

            //node = navigationGraph.nodeList[14];
            //Assert.IsTrue(node.id == 14);
            //Assert.IsTrue(node.connectedNodes.Count == 2);
        }




    }

}
