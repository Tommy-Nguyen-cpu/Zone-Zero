using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Leaf
{

    AStarAlgorithm Pathfinding = new AStarAlgorithm();
    public List<Node> PathToGoal = new List<Node>();
    Node StartNode;
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
        // If we reached the goal node, we will generate a new path
        if (ReachedGoal)
        {
            NextNode = 1;
            ReachedGoal = false;
            StartNode = GridManager.FloorNodes[0];
            GoalNode = GridManager.FloorNodes[Random.Range(1, GridManager.FloorNodes.Count)];
            Debug.Log("Goal Position: " + GoalNode.X + ", " + GoalNode.Z);
            PathToGoal = Pathfinding.FindPath(StartNode, GoalNode);
        }

        Debug.Log("Number of Nodes In Path: " + PathToGoal.Count);

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
            return;
        }

        Vector3 targetPosition = new Vector3(PathToGoal[NextNode].X, myObject.transform.position.y, PathToGoal[NextNode].Z);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, targetPosition, .1f);

        Debug.Log("Idling!");
    }
}
