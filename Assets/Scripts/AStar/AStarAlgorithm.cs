using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Might be best to make this a standard script.
// Decision tree will call the A* algorithm script based on conditions.
public class AStarAlgorithm
{
    PriorityQueue OpenQueue = new PriorityQueue();
    List<Node> VisitedNodes = new List<Node>();
    public List<Node> PathToGoal = new List<Node>();


    List<Node> mazeFloor;
    GameObject? enemy = null;

    // Start is called before the first frame update
    public void GetNodeListAndEnemyObject(List<Node> floor, GameObject myEnemy)
    {
        // TODO: I'm going to assume that the collection of floor tiles Ian made DOES NOT contain any wall objects
        // and the collection does not contain floor tiles that are directly beneath a wall tile.
        // TODO: Once Ian is done with his procedural generation, retrieve a reference to the collection of floor tiles in his script.
        mazeFloor = floor;

        // TODO: Need to have a reference of the enemy generated at a random location.
        enemy = myEnemy;
    }

    // Update is called once per frame
    public void StartAStar()
    {
        Node startNode = FindStartNode();

        // Only time it is null is if we didn't generate the enemy at the location of a tile in the maze.
        if(startNode != null)
        {
            Node goalNode = FindEndNode();
            RunAStar(startNode, goalNode);
        }
    }

    /// <summary>
    /// Retrieve the node the enemy NPC is on right now.
    /// </summary>
    /// <returns> </returns>
    Node? FindStartNode()
    {
        foreach(var node in mazeFloor)
        {
            if (node.NodeCenter.Item1 == enemy.transform.position.x && node.NodeCenter.Item2 == enemy.transform.position.z)
                return node;
        }

        return null;
    }

    Node FindEndNode()
    {

        // Returns a random location for the enemy to move towards.
        return mazeFloor[Random.Range(0, mazeFloor.Count)];
    }


    /// <summary>
    /// Performs A* algorithm.
    /// </summary>
    void RunAStar(Node start, Node end)
    {
        OpenQueue = new PriorityQueue();
        VisitedNodes = new List<Node>();
        PathToGoal = new List<Node>() { start };

        // Calculates the estimated distance between start and end node.
        start.GoalScore = Heuristics(start, end);

        start.TotalEstimatedScore = start.StartScore + start.GoalScore;
        OpenQueue.Enqueue(start, start.TotalEstimatedScore);

        // Iterates until we have reached all nodes.
        while (!OpenQueue.IsEmpty())
        {
            Node newNode = OpenQueue.Dequeue();
            if (newNode == end)
                return;
            VisitedNodes.Add(newNode);

            foreach(var childNode in newNode.NeighborNodes)
            {
                if (VisitedNodes.Contains(childNode))
                    continue;

                float tentativeGScore = newNode.StartScore + Heuristics(newNode, childNode);
                if (!OpenQueue.Contains(childNode) || tentativeGScore < childNode.StartScore)
                {
                    childNode.StartScore = tentativeGScore;
                    childNode.GoalScore = Heuristics(childNode, end);
                    childNode.TotalEstimatedScore = childNode.StartScore + childNode.GoalScore;

                    if (!OpenQueue.Contains(childNode))
                        OpenQueue.Enqueue(childNode, newNode.TotalEstimatedScore);

                    AddNodeToPathList(childNode);
                }
            }
        }
    }

    /// <summary>
    /// Adds the new neighbor node to the list of nodes the enemy will traverse to reach the goal.
    /// </summary>
    /// <param name="newNode"></param>
    void AddNodeToPathList(Node newNode)
    {
        if (PathToGoal.Contains(newNode))
        {
            PathToGoal.Remove(newNode);
        }

        PathToGoal.Add(newNode);
    }

    /// <summary>
    /// Calculates the euclidean distance between 2 cartesian coordinates.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public static float Heuristics(Node start, Node goal)
    {
        float diffX = goal.NodeCenter.Item1 - start.NodeCenter.Item1;
        float diffZ = goal.NodeCenter.Item2 - start.NodeCenter.Item2;
        return Mathf.Sqrt(diffX * diffX + diffZ * diffZ);
    }
}
