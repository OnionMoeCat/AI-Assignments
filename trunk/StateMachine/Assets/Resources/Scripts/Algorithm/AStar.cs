using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine.Assertions;

namespace AISandbox
{
    public class AStar
    {
        public static bool GetShortestPath(PathfollowingController i_pathfollowingController, GridNode i_start, GridNode i_end, bool isDiagonal, out List<GridNode> o_path, out List<GridNode> o_open)
        {
            o_path = new List<GridNode>();
            o_open = new List<GridNode>();

            IDictionary<GridNode, GridNode> path = new Dictionary<GridNode, GridNode>();
            IDictionary<GridNode, float> open = new Dictionary<GridNode, float>();

            PriorityQueue<float, GridNode> priorityQueue = new PriorityQueue<float, GridNode>(30 * 30, Comparer<float>.Default);

            priorityQueue.Add(new KeyValuePair<float, GridNode>(GetDistance(i_start, i_end, isDiagonal), i_start));
            open.Add(new KeyValuePair<GridNode, float>(i_start, 0));

            bool succeed = false;
            while (!priorityQueue.IsEmpty)
            {
                KeyValuePair<float, GridNode> current = priorityQueue.Peek();
                GridNode currentNode = current.Value;
                float currentCost;
                if (!open.TryGetValue(currentNode, out currentCost))
                {
                    Debug.Assert(false);
                }

                if (currentNode == i_end)
                {
                    succeed = true;
                    break;
                }

                foreach (GridNode neighbour in currentNode.GetNeighbors(isDiagonal))
                {
                    if (EntityManager.GridPassable(neighbour, i_end, i_pathfollowingController))
                    {
                        float newCost = currentCost + GetCost(currentNode, neighbour, isDiagonal);
                        float oldCost;
                        if (!open.TryGetValue(neighbour, out oldCost))
                        {
                            path.Add(neighbour, currentNode);
                            open.Add(neighbour, newCost);
                            float dist = GetDistance(neighbour, i_end, isDiagonal);
                            priorityQueue.Add(new KeyValuePair<float, GridNode>(newCost + dist, neighbour));
                        }
                        else if (newCost < oldCost)
                        {
                            path[neighbour] = currentNode;
                            open[neighbour] = newCost;
                            float dist = GetDistance(neighbour, i_end, isDiagonal);
                            priorityQueue.Remove(new KeyValuePair<float, GridNode>(oldCost + dist, neighbour));
                            priorityQueue.Add(new KeyValuePair<float, GridNode>(newCost + dist, neighbour));
                        }
                    }
                }

                priorityQueue.Remove(current);
            }

            if (succeed)
            {

                GridNode currentNode = i_end;
                o_path.Add(currentNode);
                while (path.ContainsKey(currentNode))
                {
                    currentNode = path[currentNode];
                    o_path.Add(currentNode);
                }
                o_path.Reverse();

                
            }

            o_open = open.Keys.ToList();

            return succeed;

        }

        public static bool GetShortestPath(PathfollowingController i_pathfollowingController, GridNode i_start, GridNode i_end, bool isDiagonal, out List<GridNode> o_path)
        {
            List<GridNode> open;
            return GetShortestPath(i_pathfollowingController, i_start, i_end, isDiagonal, out o_path, out open);
        }

        private static float GetCost(GridNode i_from, GridNode i_to, bool isDiagonal)
        {
            float diffUnitX = Mathf.Abs(i_from.column - i_to.column);
            float diffUnitY = Mathf.Abs(i_from.row - i_to.row);
            if (isDiagonal)
            {
                Debug.Assert(Mathf.Max(diffUnitX, diffUnitY) <= 1);
            }
            else
            {
                Debug.Assert((diffUnitX + diffUnitY) <= 1f);
            }
            float length = new Vector2(diffUnitX, diffUnitY).magnitude;
            return length * (i_from.Cost + i_to.Cost) / 2;
        }

        private static float GetDistance(GridNode i_from, GridNode i_to, bool isDiagonal)
        {
            if (isDiagonal)
            {
                float diagnoalUnit = Mathf.Min(Mathf.Abs(i_from.column - i_to.column),
                    Mathf.Abs(i_from.row - i_to.row)) * Mathf.Sqrt(2);
                float straightUnit =
                    Mathf.Max(Mathf.Abs(i_from.column - i_to.column), Mathf.Abs(i_from.row - i_to.row)) - diagnoalUnit;
                return diagnoalUnit * Mathf.Sqrt(2) + straightUnit;
            }
            else
            {
                return Mathf.Abs(i_from.column - i_to.column) + Mathf.Abs(i_from.row - i_to.row);
            }
        }
    }
}