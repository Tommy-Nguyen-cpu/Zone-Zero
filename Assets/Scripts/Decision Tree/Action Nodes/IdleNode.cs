using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Leaf
{

    AStarAlgorithm Pathfinding = new AStarAlgorithm();
    public List<Node> PathToGoal = new List<Node>();

    Node StartNode;

    // Index of the start index
    int index = -1;
    Node GoalNode;
    int NextNode = 1;
    
    // TODO: Update this once Ian finishes his map generation script.
    GridManagerScript GridManager;


    /// <summary>
    /// "ReachedGoal" will keep track of whether or not the player has reached the goal in the A* algorithm.
    /// </summary>
    bool ReachedGoal = true;

    // TODO: Update this once Ian finishes his map generation script.
    public IdleNode(GridManagerScript gridManager)
    {
        GridManager = gridManager;
    }

    /// <summary>
    /// Will be idle if all other states failed their conditional check.
    /// </summary>
    /// <param name="myGameObject"></param>
    /// <param name="myPlayer"></param>
    /// <returns></returns>
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        return true;
    }

    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {

        IsEnemyOnPath(myObject);

        // If we reached the goal node, we will generate a new path
        if (ReachedGoal)
        {
            NextNode = 0;
            ReachedGoal = false;

            // We only find the node the enemy is currently on at the start of the program.
            // In all other instances, "StartNode" will be the previous "GoalNode" the enemy reached.
            if(StartNode == null)
            {
                (StartNode, index) = NearestNode(myObject, GridManager.FloorNodes);
                StartNode.Parent = null;
            }

            GoalNode = GridManager.FloorNodes[RandomIndex(index)];
            PathToGoal = Pathfinding.FindPath(StartNode, GoalNode);

            Debug.Log("Path Count: " + PathToGoal.Count);
        }

        // Debug.Log("Number of Nodes In Path: " + PathToGoal.Count);

        float diffX = myObject.transform.position.x - PathToGoal[NextNode].X;
        float diffZ = myObject.transform.position.z - PathToGoal[NextNode].Z;
        float distance = Mathf.Sqrt(diffX * diffX + diffZ * diffZ);
        if (distance == 0)
        {
            NextNode++;
        }

        // If we reached the end of the path to goal list.
        if (NextNode >= PathToGoal.Count)
        {
            ReachedGoal = true;
            StartNode = GoalNode;
            StartNode.Parent = null;
            index = GridManager.FloorNodes.IndexOf(GoalNode);
            return;
        }

        Vector3 targetPosition = new Vector3(PathToGoal[NextNode].X, myObject.transform.position.y, PathToGoal[NextNode].Z);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, targetPosition, .03f);

        //Debug.Log("Idling!");
    }



    /// <summary>
    /// Iterates through all nodes to find the closest node to the enemys' current position.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private (Node, int) NearestNode(GameObject enemy, List<Node> path)
    {
        Node closeNode = path[0];
        float myDistance = Distance(enemy, closeNode);
        int index = 0;
        
        for(int i = 1; i < path.Count; i++)
        {
            float distance = Distance(enemy, path[i]);

            if(distance < myDistance)
            {
                closeNode = path[i];
                myDistance = distance;
                index = i;
            }
        }

        return (closeNode, index);
    }

    // TODO: Same thing as the "Heuristic" method in AStarAlgorithm class. We might want to consolidate all math methods in 1 class.
    private float Distance(GameObject enemy, Node node)
    {
        float diffX = enemy.transform.position.x - node.X;
        float diffZ = enemy.transform.position.z - node.Z;

        return Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffZ, 2));
    }


    public int RandomIndex(int index)
    {
        int randIndex = -1;
        do
        {
            randIndex = Random.Range(0, GridManager.FloorNodes.Count);
        } while (randIndex == index);

        return randIndex;
    }

    /// <summary>
    /// We check to see if the enemy is on the path. The only time it is not, is if it chased the player but the player managed to escape.
    /// </summary>
    /// <param name="myObject"></param>
    public void IsEnemyOnPath(GameObject myObject)
    {
        // Check to see if it is close to a node in the path.
        float lowestDistance = 1000;
        foreach (var node in PathToGoal)
        {
            float currentNodeDistance = Distance(myObject, node);

            if (currentNodeDistance < lowestDistance)
                lowestDistance = currentNodeDistance;
        }

        // If the enemy is far away from the node, we'll generate a new path with the current enemy location.
        if (lowestDistance > 10)
        {
            ReachedGoal = true;
            StartNode = null;
        }
    }
}
