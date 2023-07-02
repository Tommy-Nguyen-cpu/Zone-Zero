using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AStarAlgorithm
{
    public List<Node> FindPath(Node start, Node goal, Maze maze)
    {
        PriorityQueue Open = new PriorityQueue();
        List<(float, float)> Closed = new List<(float, float)>();

        start.StartScore = 0;
        start.GoalScore = Heuristics(start, goal);
        start.EstimatedTotalScore = start.StartScore + start.GoalScore;

        Open.Enqueue(start);

        while (!Open.IsEmpty())
        {
            Node current = Open.Dequeue();
            Closed.Add((current.X, current.Z));

            if (current.X == goal.X && current.Z == goal.Z)
                return ConstructPath(current);

            List<Node> neighbors = Expand(current, maze);
            foreach(var neighbor in neighbors)
            {
                bool Visited = false;
                foreach(var closedEntry in Closed)
                {
                    if(neighbor.X == closedEntry.Item1 && neighbor.Z == closedEntry.Item2)
                    {
                        Visited = true;
                        break;
                    }
                }
                if (Visited)
                    continue;

                neighbor.StartScore = current.StartScore + Heuristics(current, neighbor);
                neighbor.GoalScore = Heuristics(neighbor, goal);
                neighbor.EstimatedTotalScore = neighbor.StartScore + neighbor.GoalScore;
                neighbor.Parent = current;
                Open.Enqueue(neighbor);
            }
        }

        return null;
    }

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

    public List<Node> Expand(Node nodeToExpand, Maze maze)
    {
        MazeFlags myFlags = nodeToExpand.flag;

        List<Node> neighbors = new List<Node>();
        switch (myFlags)
        {
            case MazeFlags.PassageN:
                // Debug.Log("Has north.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                break;
            case MazeFlags.PassageS:
                // Debug.Log("Has south.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageE:
                // Debug.Log("Has east.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                break;
            case MazeFlags.PassageW:
                // Debug.Log("Has west");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                break;
            case MazeFlags.PassageN | MazeFlags.PassageS:
                // Debug.Log("Has north and south");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageE | MazeFlags.PassageW:
                // Debug.Log("Has east and west");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                break;
            case MazeFlags.PassageN | MazeFlags.PassageE:
                // Debug.Log("has north and east.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                break;
            case MazeFlags.PassageE | MazeFlags.PassageS:
                // Debug.Log("Has east and south.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageS | MazeFlags.PassageW:
                // Debug.Log("Has south and west.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                break;
            case MazeFlags.PassageW | MazeFlags.PassageN:
                // Debug.Log("Has west and north");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                break;

            case MazeFlags.PassageAll & ~MazeFlags.PassageW:
                // Debug.Log("Has all except for west.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageAll & ~MazeFlags.PassageN:
                // Debug.Log("Has all except for north.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageAll & ~MazeFlags.PassageE:
                // Debug.Log("Has all except for east.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;
            case MazeFlags.PassageAll & ~MazeFlags.PassageS:
                // Debug.Log("Has all except for south.");
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                break;
            case MazeFlags.PassageAll:
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x + 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x - 1, nodeToExpand.myIndex.y), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y + 1), maze));
                neighbors.Add(CreateNode(CreateNewCoordinate(nodeToExpand.myIndex.x, nodeToExpand.myIndex.y - 1), maze));
                break;

        }

        return neighbors;
    }

    public int2 CreateNewCoordinate(int newX, int newY)
    {
        return new int2(newX, newY);
    }

    public Node CreateNode(int2 index, Maze maze)
    {
        Node newNode = new Node();
        newNode.flag = maze[maze.CoordinatesToIndex(index)];
        newNode.myIndex = index;
        Vector3 pos = maze.CoordinatesToWorldPosition(index);
        newNode.X = pos.x;
        newNode.Z = pos.z;

        return newNode;
    }

}

