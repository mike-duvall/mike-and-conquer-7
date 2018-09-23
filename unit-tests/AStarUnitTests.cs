
using System.Collections.Generic;
using mike_and_conquer.pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void ContainsPoint_ShouldWorkAfterMinigunnerMoves()
        {
            // given
            AStar aStar = new AStar();

            // and
            Graph graph = new Graph();

            // 1 2 3
            // 4 5 6
            // 7 8 9

            List<int> nodeList = new List<int>();

            nodeList.Add(2);
            nodeList.Add(4);
            nodeList.Add(5);
            graph.AddNode(new Node(1, nodeList));

            nodeList = new List<int>();
            nodeList.Add(1);
            nodeList.Add(4);
            nodeList.Add(5);
            nodeList.Add(6);
            nodeList.Add(3);
            graph.AddNode(new Node(2, nodeList));

            nodeList = new List<int>();
            nodeList.Add(2);
            nodeList.Add(5);
            nodeList.Add(6);
            graph.AddNode(new Node(3, nodeList));

            nodeList = new List<int>();
            nodeList.Add(1);
            nodeList.Add(2);
            nodeList.Add(5);
            nodeList.Add(7);
            nodeList.Add(8);
            graph.AddNode(new Node(4, nodeList));

            nodeList = new List<int>();
            nodeList.Add(1);
            nodeList.Add(2);
            nodeList.Add(3);
            nodeList.Add(4);
            nodeList.Add(6);
            nodeList.Add(7);
            nodeList.Add(8);
            nodeList.Add(9);
            graph.AddNode(new Node(5, nodeList));

            nodeList = new List<int>();
            nodeList.Add(2);
            nodeList.Add(3);
            nodeList.Add(5);
            nodeList.Add(8);
            nodeList.Add(9);
            graph.AddNode(new Node(6, nodeList));

            nodeList = new List<int>();
            nodeList.Add(4);
            nodeList.Add(5);
            nodeList.Add(8);
            graph.AddNode(new Node(7, nodeList));

            nodeList = new List<int>();
            nodeList.Add(4);
            nodeList.Add(5);
            nodeList.Add(6);
            nodeList.Add(7);
            nodeList.Add(9);
            graph.AddNode(new Node(8, nodeList));

            nodeList = new List<int>();
            nodeList.Add(5);
            nodeList.Add(6);
            nodeList.Add(8);
            graph.AddNode(new Node(9, nodeList));


            // when
            Path foundPath = aStar.FindPath(graph,1,9);
            
            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 3);

        }


    }

}
