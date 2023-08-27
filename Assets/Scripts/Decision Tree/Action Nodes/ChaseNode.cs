using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChaseNode : Leaf
{
    #region Pathfinding Fields

    AStarAlgorithm Pathfinding = new AStarAlgorithm();
    public List<Node> PathToGoal = new List<Node>();

    Node StartNode;

    // Index of the start index
    int index = -1;
    Node GoalNode;
    int goalIndex = -1;
    int NextNode = 1;

    // TODO: Update this once Ian finishes his map generation script.
    Maze PCGMaze;

    /// <summary>
    /// "ReachedGoal" will keep track of whether or not the player has reached the goal in the A* algorithm.
    /// </summary>
    bool ReachedGoal = true;
    #endregion


    public bool PursuitPlayer = false;
    public float pursuitTime = 0f;
    const float MAX_PURSUIT_TIME = 5f;

    public ChaseNode(Maze maze)
    {
        PCGMaze = maze;
    }

    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        Collider[] checks = Physics.OverlapSphere(myGameObject.transform.position, 100f, ParentTree.PlayerLayerMask);

        if (checks.Length > 0)
        {
            Vector3 directionToPlayer = (myPlayer.transform.position - myGameObject.transform.position).normalized;
            if (Vector3.Angle(myGameObject.transform.forward, directionToPlayer) < ParentTree.Angle / 2)
            {
                float distanceToPlayer = Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position);
                if (!Physics.Raycast(myGameObject.transform.position, directionToPlayer, distanceToPlayer, ParentTree.ObstacleLayerMask))
                {
                    PursuitPlayer = true;
                    return true;
                }
            }
        }
        if (PursuitPlayer) // Makes sure enemy pursues player for a certain amount of time.
        {
            if(pursuitTime < MAX_PURSUIT_TIME)
            {
                pursuitTime += Time.deltaTime;
                Debug.Log("Time: " + pursuitTime);
                return true;
            }
            else
            {
                pursuitTime = 0f;
                PursuitPlayer = false;
                return false;
            }
        }



        return base.CheckCondition(myGameObject, myPlayer);
    }

    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        Vector3 direction = player.transform.position - myObject.transform.position;
        RaycastHit hit;
        if(!Physics.Raycast(myObject.transform.position, direction, out hit, Mathf.Infinity, ParentTree.ObstacleLayerMask))
        {
            ParentTree.PlayAnimation(DecisionTree.AnimationType.CHASING);
            myObject.transform.LookAt(player.transform);
            myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, new Vector3(player.transform.position.x, myObject.transform.position.y, player.transform.position.z), 1.5f * Time.deltaTime);
            base.Action(player, myObject, runninSpeed, animator);
        }
        else
        {
            PathToPlayer(myObject, player);
        }

    }

    private void PathToPlayer(GameObject myObject, GameObject player)
    {
        IsEnemyOnPath(myObject);
        // We only find the node the enemy is currently on at the start of the program.
        // In all other instances, "StartNode" will be the previous "GoalNode" the enemy reached.
        if (StartNode == null || Distance(myObject, StartNode) > .2)
        {
            (StartNode, index) = NearestNode(myObject);
            StartNode.Parent = null;
        }
        (_, goalIndex) = NearestNode(player);
        int2 coordinate = PCGMaze.IndexToCoordinates(goalIndex);
        GoalNode = Pathfinding.CreateNode(coordinate, PCGMaze);
        if (ReachedGoal)
        {
            NextNode = 0;
            ReachedGoal = false;

            PathToGoal = Pathfinding.FindPath(StartNode, GoalNode, PCGMaze);
        }

        float distance = Distance(myObject, PathToGoal[NextNode]);
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
            index = goalIndex;
            return;
        }

        Vector3 targetPosition = new Vector3(PathToGoal[NextNode].X, myObject.transform.position.y, PathToGoal[NextNode].Z);
        myObject.transform.LookAt(targetPosition);

        ParentTree.PlayAnimation(DecisionTree.AnimationType.CHASING);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, targetPosition, 1.5f * Time.deltaTime);
    }


    /// <summary>
    /// Iterates through all nodes to find the closest node to the enemys' current position.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private (Node, int) NearestNode(GameObject enemy)
    {
        Node closeNode = Pathfinding.CreateNode(PCGMaze.IndexToCoordinates(0), PCGMaze);
        float myDistance = Distance(enemy, closeNode);
        int index = 0;

        for (int i = 1; i < PCGMaze.Length; i++)
        {
            Node node = Pathfinding.CreateNode(PCGMaze.IndexToCoordinates(i), PCGMaze);
            float distance = Distance(enemy, node);

            if (distance < myDistance)
            {
                closeNode = node;
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

    public void IsEnemyOnPath(GameObject myObject)
    {
        if (PathToGoal == null)
            return;

        // Check to see if it is close to a node in the path.
        float lowestDistance = 1000;
        foreach (var node in PathToGoal)
        {
            float currentNodeDistance = Distance(myObject, node);

            if (currentNodeDistance < lowestDistance)
                lowestDistance = currentNodeDistance;
        }

        // If the enemy is far away from the node, we'll generate a new path with the current enemy location.
        if (lowestDistance > 1)
        {
            ReachedGoal = true;
            StartNode = null;
        }
    }
}
