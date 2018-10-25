﻿

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Boolean = System.Boolean;

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
        public int width;
        public int height;
        private int currentRow = 0;
        private int currentNodeId = 0;

        private int[,] nodeArray;

        public Graph(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.nodeArray = new int[width, height];
            this.nodeList = new List<Node>();
            for (int i = 0; i < nodeArray.Length; i++)
            {
                List<int> adjacentNodes = CalculateAdjacentNodes(currentNodeId);
                Node node = new Node(currentNodeId, adjacentNodes);
                nodeList.Add(node);
                currentNodeId++;
            }


        }

        public void UpdateNode(int x, int y, int newValue)
        {
            this.nodeArray[x, y] = newValue;
            this.nodeList = new List<Node>();
            this.currentNodeId = 0;
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
            int currentNodey = nodeId / width;

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

    }

    public class AStar
    {
        public Path FindPath(Graph graph, Point startPoint, Point endPoint)
        {
            int startPointIndex = (startPoint.Y * graph.width) + startPoint.X;
            int endPointIndex = (endPoint.Y * graph.width) + endPoint.X;
            return this.FindPath(graph, startPointIndex, endPointIndex);
        }

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


