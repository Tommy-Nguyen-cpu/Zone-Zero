using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    public List<Node> FindPath(Node start, Node goal)
    {
        PriorityQueue OpenQueue = new PriorityQueue();
        List<Node> ClosedQueue = new List<Node>();

        start.GoalScore = Heuristics(start, goal);
        start.StartScore = 0;
        start.EstimatedTotalScore = start.StartScore + start.GoalScore;

        OpenQueue.Enqueue(start);

        while (!OpenQueue.IsEmpty())
        {
            Node currentNode = OpenQueue.Dequeue();
            if (currentNode == goal)
                return ConstructPath(currentNode);

            ClosedQueue.Add(currentNode);

            foreach(var neighbor in currentNode.Neighbors)
            {
                if (ClosedQueue.Contains(neighbor))
                    continue;

                float tentativeStartScore = currentNode.StartScore + Heuristics(currentNode, neighbor);
                if (!OpenQueue.Contains(neighbor) || tentativeStartScore < neighbor.StartScore)
                {
                    neighbor.StartScore = tentativeStartScore;
                    neighbor.GoalScore = Heuristics(neighbor, goal);
                    neighbor.EstimatedTotalScore = neighbor.StartScore + neighbor.GoalScore;
                    neighbor.Parent = currentNode;

                    if (!OpenQueue.Contains(neighbor))
                        OpenQueue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Calculates the heuristic value (distance from 1 node to another).
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public static float Heuristics(Node start, Node goal)
    {
        float diffX = goal.X - start.X;
        float diffZ = goal.Z - start.Z;
        return Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffZ, 2));
    }

    /// <summary>
    /// Converts the recursive relationship of the path to the goal into a list of nodes.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private List<Node> ConstructPath(Node node)
    {
        List<Node> path = new List<Node>();
        Node current = node;

        while (current != null)
        {
            path.Insert(0, current);
            current = current.Parent;
        }

        return path;
    }

}

