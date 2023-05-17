using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Might be best to make this a standard script.
// Decision tree will call the A* algorithm script based on conditions.
public class AStarAlgorithm : MonoBehaviour
{
    PriorityQueue OpenQueue = new PriorityQueue();
    List<Node> VisitedNodes = new List<Node>();


    List<GameObject> mazeFloor;
    GameObject? enemy = null;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: I'm going to assume that the collection of floor tiles Ian made DOES NOT contain any wall objects
        // and the collection does not contain floor tiles that are directly beneath a wall tile.
        // TODO: Once Ian is done with his procedural generation, retrieve a reference to the collection of floor tiles in his script.
        mazeFloor = new List<GameObject>();

        // TODO: Need to have a reference of the enemy generated at a random location.
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject startNode = FindStartNode();

        // Only time it is null is if we didn't generate the enemy at the location of a tile in the maze.
        if(startNode != null)
        {

        }
    }

    /// <summary>
    /// Retrieve the node the enemy NPC is on right now.
    /// </summary>
    /// <returns> </returns>
    GameObject? FindStartNode()
    {
        foreach(var node in mazeFloor)
        {
            if (node.transform.position.x == enemy.transform.position.x && node.transform.position.z == enemy.transform.position.z)
                return node;
        }

        return null;
    }

    GameObject FindEndNode()
    {

        // Returns a random location for the enemy to move towards.
        return mazeFloor[Random.Range(0, mazeFloor.Count)];
    }


    /// <summary>
    /// Performs A* algorithm.
    /// </summary>
    void RunAStar(Node start, Node end)
    {

    }
}
