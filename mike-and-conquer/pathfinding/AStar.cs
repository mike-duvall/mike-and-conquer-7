using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.pathfinding
{

    public class Path
    {
        public List<Node> nodeList;

        public Path()
        {
            nodeList = new List<Node>();
        }

    }

    public class Graph
    {

        public List<Node> nodeList;
        private int width;
        private int height;
        private int currentRow = 0;
        private int currentNodeId = 0;

        private int[,] nodeArray;

//        public Graph(int width, int height)
//        {
//            nodeList = new List<Node>();
//            this.width = width;
//            this.height = height;
//        }

        //        public void AddNode(Node aNode)
        //        {
        //            nodeList.Add(aNode);
        //        }

        public Graph(int[,] nodeArray)
        {
            this.nodeArray = nodeArray;
            nodeList = new List<Node>();
            width = nodeArray.GetLength(0);
            height = nodeArray.GetLength(1);

            for (int i = 0; i < nodeArray.Length; i++)
            {
                List<int> adjacentNodes = CalculateAdjacentNodes(currentNodeId);
                Node node = new Node(currentNodeId, adjacentNodes);
                nodeList.Add(node);
                currentNodeId++;
            }

        }

        List<int> CalculateAdjacentNodes(int nodeId)
        {

            List<int> adjacentNodes = new List<int>();
            int currentNodex = nodeId % width;
            int currentNodey = nodeId / height;

            if (IsLocationOpen(currentNodex, currentNodey))
            {
                for (int y = currentNodey - 1; y <= currentNodey + 1; y++)
                for (int x = currentNodex - 1; x <= currentNodex + 1; x++)
                {
                    if (IsValidLocation(x, y) && IsLocationOpen(x, y) && !(currentNodex == x && currentNodey == y))
                    {
                        int adjacentNodeIndex = y * width + x;
                        adjacentNodes.Add(adjacentNodeIndex);
                    }
                }
            }

            return adjacentNodes;
        }


        bool IsValidLocation(int x, int y)
        {
            bool isValid = (
                x >= 0 &&
                (x <= width - 1) &&
                y >= 0 &&
                (y <= height - 1));

            return isValid;
        }

        bool IsLocationOpen(int x, int y)
        {
            bool isOpen = nodeArray[x, y] == 0;
            return isOpen;
        }

        public void AddRow(string row)
        {
            string[] mapSquares = row.Split(' ');
            foreach (string mapSquare in mapSquares)
            {
                if (mapSquare == "0")
                {
                    Node newNode = new Node(currentNodeId++);

                }
            }
        }
    }

    public class Node
    {
        public int id;
        public List<int> connectedNodes;

        public Node(int id, List<int> connectedNodes)
        {
            this.id = id;
            this.connectedNodes = connectedNodes;
        }

        public Node(int id)
        {
            this.id = id;
//            this.connectedNodes = connectedNodes;
        }


    }

    public class AStar
    {



        public Path FindPath(Graph graph, int startLocation, int goalLocation)

        {
//            frontier = Queue()
//            frontier.put(start)
//            came_from = { }
//            came_from[start] = None
//
//            while not frontier.empty()
//            {
//                current = frontier.get()
//                for next in graph.neighbors(current)
//                {
//                    if next not in came_from
//                    {
//                        frontier.put(next)
//                        came_from[next] = current
//                    }
//                }
//            }

            Queue<Node> frontier  = new Queue<Node>();
            frontier.Enqueue(graph.nodeList[startLocation]);
            Dictionary<int, int> came_from = new Dictionary<int, int>();
            came_from[startLocation] = -1;

            while (frontier.Count > 0)
            {
                Node current = frontier.Dequeue();
                foreach (int next in current.connectedNodes)
                {
                    if (!came_from.ContainsKey(next))
                    {
                        frontier.Enqueue(graph.nodeList[next]);
                        came_from[next] = current.id;
                    }
                }
            }

            Path thePath = new Path();

            Node nextNode = graph.nodeList[goalLocation];
            List<Node> nodesInReverseOrder = new List<Node>();
            Boolean moreNodes = true;
            while (moreNodes)
            {

                nodesInReverseOrder.Add(nextNode);
                if (came_from[nextNode.id] != -1)
                {
                    nextNode = graph.nodeList[came_from[nextNode.id]];
                }
                else
                {
                    moreNodes = false;
                }
            }

            nodesInReverseOrder.Reverse(0, nodesInReverseOrder.Count);
            thePath.nodeList = nodesInReverseOrder;
            return thePath;
        }

    }
}



//        public static Path FindPath(Graph graph, int startLocation, int goalLocation)
//        {
////            frontier = PriorityQueue()
////            frontier.put(start, 0)
////            came_from = { }
////            cost_so_far = { }
////            came_from[start] = None
////            cost_so_far[start] = 0
////
////            while not frontier.empty()
////            {
////                current = frontier.get()
////
////                if current == goal {
////                    break
////                }
////
////                for next in graph.neighbors(current) {
////                    new_cost = cost_so_far[current] + graph.cost(current, next)
////                    if next not in cost_so_far or new_cost < cost_so_far[next] {
////                        cost_so_far[next] = new_cost
////                        priority = new_cost + heuristic(goal, next)
////                        frontier.put(next, priority)
////                        came_from[next] = current
////                    }
////                }
////            }
//
//            SortedList<int, int> frontier = new SortedList<int, int>();
//            frontier.Add(0, startLocation);
//
//            IList<int> currentFrontierKeys = frontier.Keys;
//            while (currentFrontierKeys.Count != 0)
//            {
//                revisit how to get first sorted item off list
//                    revisit is it sorted ascending or descending
//
//
//                int current = frontier.Values
//            }
//
//
//            return null;
//
//        }
