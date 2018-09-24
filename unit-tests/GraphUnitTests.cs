
using System.Collections.Generic;
using mike_and_conquer.pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Graphics;
using AStar = mike_and_conquer.pathfinding.AStar;
using Path = mike_and_conquer.pathfinding.Path;
using Graph = mike_and_conquer.pathfinding.Graph;
using Node = mike_and_conquer.pathfinding.Node;

namespace unit_tests
{
    [TestClass]
    public class GraphUnitTests
    {



        [TestMethod]
        public void BasicPathfindingTest1()
        {
            // given

            int[,] nodeArray = new int[3, 3]
            {
                { 0, 0, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            Graph graph = new Graph(nodeArray);


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
        public void BasicPathfindingTest2()
        {
            // given

            int[,] nodeArray = new int[3, 3]
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 }
            };

            Graph graph = new Graph(nodeArray);


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



    }

}
