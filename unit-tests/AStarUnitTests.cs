
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
    public class AStarUnitTests
    {



        [TestMethod]
        public void BasicPathfindingTest1()
        {
            // given
            AStar aStar = new AStar();

            // and
            int[,] nodeArray = new int[3, 3]
            {
                { 0, 0, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            Graph graph = new Graph(nodeArray);

            // when
            Path foundPath = aStar.FindPath(graph,0,8);
            
            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 3);

            Assert.IsTrue(foundPath.nodeList[0].id == 0);
            Assert.IsTrue(foundPath.nodeList[1].id == 4);
            Assert.IsTrue(foundPath.nodeList[2].id == 8);


        }


        [TestMethod]
        public void BasicPathfindingTest2()
        {
            // given
            AStar aStar = new AStar();

            // and
            int[,] nodeArray = new int[3, 3]
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 }
            };

            Graph graph = new Graph(nodeArray);

            // when
            Path foundPath = aStar.FindPath(graph, 0, 8);

            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 4);

            Assert.IsTrue(foundPath.nodeList[0].id == 0);
            Assert.IsTrue(foundPath.nodeList[1].id == 1);
            Assert.IsTrue(foundPath.nodeList[2].id == 5);
            Assert.IsTrue(foundPath.nodeList[3].id == 8);


        }


    }

}
