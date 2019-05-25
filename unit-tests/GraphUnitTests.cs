
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graph = mike_and_conquer.pathfinding.Graph;
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

            Graph graph = new Graph(3,3);

            // then
            Node node = graph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));

            node = graph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = graph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = graph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[4];
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

            node = graph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(7));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = graph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 5);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = graph.nodeList[8];
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

            Graph graph = new Graph(3, 3);

            graph.MakeNodeBlockingNode(1, 1);
            graph.RebuildAdajencyGraph();


            // then
            Node node = graph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));


            node = graph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = graph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = graph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(7));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = graph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = graph.nodeList[8];
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

            Graph graph = new Graph(3,3);

            graph.MakeNodeBlockingNode(2, 0);
            graph.MakeNodeBlockingNode(1, 1);
            graph.MakeNodeBlockingNode(2, 1);
            graph.RebuildAdajencyGraph();

            // then
            Node node = graph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));


            node = graph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = graph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(7));

            node = graph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(6));
            Assert.IsTrue(node.connectedNodes.Contains(8));

            node = graph.nodeList[8];
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
            Graph graph = new Graph(5,3);

            graph.MakeNodeBlockingNode(1, 1);
            graph.MakeNodeBlockingNode(2, 1);
            graph.MakeNodeBlockingNode(3, 1);
            graph.RebuildAdajencyGraph();


            // then
            Node node = graph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(5));

            node = graph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(2));

            node = graph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = graph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(9));

            node = graph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(9));

            node = graph.nodeList[5];
            Assert.IsTrue(node.id == 5);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(10));
            Assert.IsTrue(node.connectedNodes.Contains(11));

            node = graph.nodeList[6];
            Assert.IsTrue(node.id == 6);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[7];
            Assert.IsTrue(node.id == 7);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[8];
            Assert.IsTrue(node.id == 8);
            Assert.IsTrue(node.connectedNodes.Count == 0);

            node = graph.nodeList[9];
            Assert.IsTrue(node.id == 9);
            Assert.IsTrue(node.connectedNodes.Count == 4);

            node = graph.nodeList[10];
            Assert.IsTrue(node.id == 10);
            Assert.IsTrue(node.connectedNodes.Count == 2);

            node = graph.nodeList[11];
            Assert.IsTrue(node.id == 11);
            Assert.IsTrue(node.connectedNodes.Count == 3);

            node = graph.nodeList[12];
            Assert.IsTrue(node.id == 12);
            Assert.IsTrue(node.connectedNodes.Count == 2);

            node = graph.nodeList[13];
            Assert.IsTrue(node.id == 13);
            Assert.IsTrue(node.connectedNodes.Count == 3);

            node = graph.nodeList[14];
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

            Graph graph = new Graph(26, 24);


            graph.MakeNodeBlockingNode(1, 1);
            graph.MakeNodeBlockingNode(2, 1);
            graph.MakeNodeBlockingNode(3, 1);
            graph.RebuildAdajencyGraph();

            // then
            Node node = graph.nodeList[0];
            Assert.IsTrue(node.id == 0);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(26));

            node = graph.nodeList[1];
            Assert.IsTrue(node.id == 1);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(26));

            node = graph.nodeList[2];
            Assert.IsTrue(node.id == 2);
            Assert.IsTrue(node.connectedNodes.Count == 2);
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(3));

            node = graph.nodeList[3];
            Assert.IsTrue(node.id == 3);
            Assert.IsTrue(node.connectedNodes.Count == 3);
            Assert.IsTrue(node.connectedNodes.Contains(2));
            Assert.IsTrue(node.connectedNodes.Contains(4));
            Assert.IsTrue(node.connectedNodes.Contains(30));

            node = graph.nodeList[4];
            Assert.IsTrue(node.id == 4);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(3));
            Assert.IsTrue(node.connectedNodes.Contains(5));
            Assert.IsTrue(node.connectedNodes.Contains(30));
            Assert.IsTrue(node.connectedNodes.Contains(31));

            node = graph.nodeList[26];
            Assert.IsTrue(node.id == 26);
            Assert.IsTrue(node.connectedNodes.Count == 4);
            Assert.IsTrue(node.connectedNodes.Contains(0));
            Assert.IsTrue(node.connectedNodes.Contains(1));
            Assert.IsTrue(node.connectedNodes.Contains(52));
            Assert.IsTrue(node.connectedNodes.Contains(53));

            //node = graph.nodeList[6];
            //Assert.IsTrue(node.id == 6);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = graph.nodeList[7];
            //Assert.IsTrue(node.id == 7);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = graph.nodeList[8];
            //Assert.IsTrue(node.id == 8);
            //Assert.IsTrue(node.connectedNodes.Count == 0);

            //node = graph.nodeList[9];
            //Assert.IsTrue(node.id == 9);
            //Assert.IsTrue(node.connectedNodes.Count == 4);

            //node = graph.nodeList[10];
            //Assert.IsTrue(node.id == 10);
            //Assert.IsTrue(node.connectedNodes.Count == 2);

            //node = graph.nodeList[11];
            //Assert.IsTrue(node.id == 11);
            //Assert.IsTrue(node.connectedNodes.Count == 3);

            //node = graph.nodeList[12];
            //Assert.IsTrue(node.id == 12);
            //Assert.IsTrue(node.connectedNodes.Count == 2);

            //node = graph.nodeList[13];
            //Assert.IsTrue(node.id == 13);
            //Assert.IsTrue(node.connectedNodes.Count == 3);

            //node = graph.nodeList[14];
            //Assert.IsTrue(node.id == 14);
            //Assert.IsTrue(node.connectedNodes.Count == 2);
        }




    }

}
