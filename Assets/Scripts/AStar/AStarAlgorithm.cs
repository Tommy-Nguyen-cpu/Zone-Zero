using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Might be best to make this a standard script.
// Decision tree will call the A* algorithm script based on conditions.
public class AStarAlgorithm
{
    PriorityQueue OpenQueue = new PriorityQueue();
    List<Node> ClosedQueue = new List<Node>();


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

    /// <summary>
    /// Retrieve the node the enemy NPC is on right now.
    /// </summary>
    /// <returns> </returns>
    public Node? FindStartNode()
    {
        foreach(var node in mazeFloor)
        {
            if (node.NodeCenter.Item1 == enemy.transform.position.x && node.NodeCenter.Item2 == enemy.transform.position.z)
                return node;
        }

        return null;
    }

    /// <summary>
    /// TODO: Currently it only generates a random point in the grid to be the exit. We will want to change this so that it moves towards the player when the player gets closer to the enemy.
    /// </summary>
    /// <returns></returns>
    public Node FindEndNode()
    {

        // Returns a random location for the enemy to move towards.
        return mazeFloor[Random.Range(0, mazeFloor.Count)];
    }

    // TODO: For some reason, the A* algorithm does not provide the shortest path.
    /// <summary>
    /// Performs A* algorithm.
    /// </summary>
    public List<Node> RunAStar(Node start, Node end)
    {
        OpenQueue = new PriorityQueue();
        ClosedQueue = new List<Node>();

        start.GoalScore = Heuristics(start, end);
        start.StartScore = 0;
        start.TotalEstimatedScore = start.StartScore + start.GoalScore;

        OpenQueue.Enqueue(start);

        while (!OpenQueue.IsEmpty())
        {
            Node currentNode = OpenQueue.Dequeue();

            if (currentNode == end)
                return ConstructPath(currentNode);

            ClosedQueue.Add(currentNode);

            foreach(var neighbor in currentNode.NeighborNodes)
            {
                if (ClosedQueue.Contains(neighbor))
                    continue;

                float tentativeStartScore = currentNode.StartScore + Heuristics(currentNode, neighbor);
                if(!OpenQueue.Contains(neighbor) || tentativeStartScore < neighbor.StartScore)
                {
                    neighbor.StartScore = tentativeStartScore;
                    neighbor.GoalScore = Heuristics(neighbor, end);
                    neighbor.TotalEstimatedScore = neighbor.StartScore + neighbor.GoalScore;
                    neighbor.cameFrom = currentNode;

                    if (!OpenQueue.Contains(neighbor))
                        OpenQueue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Adds the new neighbor node to the list of nodes the enemy will traverse to reach the goal.
    /// </summary>
    /// <param name="newNode"></param>
    List<Node> ConstructPath(Node goalNode)
    {
        List<Node> PathToGoal = new List<Node>();
        Node current = goalNode;

        while (current != null)
        {
            PathToGoal.Insert(0, current);
            current = current.cameFrom;
        }

        return PathToGoal;
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
        return Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffZ, 2));
    }
}
