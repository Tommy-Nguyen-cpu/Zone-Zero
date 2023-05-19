using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    float GridWidth = 100f;
    float GridHeight = 100f;

    (float, float) Origin = (0f, 0f);

    float GridIncrement = 10f;

    /// <summary>
    /// TODO: Used to allow us to more easily set up neighborhoods for nodes.
    /// </summary>
    Dictionary<float, Dictionary<float, Node>> hashsetForNodes = new Dictionary<float, Dictionary<float, Node>>();

    /// <summary>
    /// TODO: Used for A* search.
    /// </summary>
    List<Node> FloorNodes = new List<Node>();

    AStarAlgorithm algorithm = new AStarAlgorithm();
    List<Node> PathToGoal = new List<Node>();

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        for(float i = Origin.Item1; i <= GridWidth; i += GridIncrement)
        {
            for(float j = Origin.Item2; j <= GridHeight; j += GridIncrement)
            {
                Node newNode = new Node();
                newNode.Z = i;
                newNode.X = j;

                newNode.NodeWidth = GridIncrement;
                newNode.NodeLength = GridIncrement;

                FloorNodes.Add(newNode);

                if (!hashsetForNodes.ContainsKey(i))
                {
                    hashsetForNodes.Add(i, new Dictionary<float, Node> { { j, newNode } });
                }
                else
                {
                    if (!hashsetForNodes[i].ContainsKey(j))
                    {
                        hashsetForNodes[i].Add(j, newNode);
                    }
                }
            }
        }

        SetUpNodeNeighborhoods();

        PathToGoal = algorithm.FindPath(FloorNodes[0], FloorNodes[Random.Range(0, FloorNodes.Count)]);
        Debug.Log("Nodes: " + PathToGoal.Count);
    }

    private void OnDrawGizmos()
    {
        foreach(var node in FloorNodes)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(new Vector3(node.X, 0f, node.Z), new Vector3(GridIncrement, 10f, GridIncrement));
        }


        Node previousNode = PathToGoal[0];

        Debug.Log($"Node has {previousNode.Neighbors.Count} neighbors");


        Debug.Log($"Number of nodes in path: {PathToGoal.Count}");

        for(int i = 1; i < PathToGoal.Count; i++)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(previousNode.X, 10f, previousNode.Z), new Vector3(PathToGoal[i].X, 10f, PathToGoal[i].Z));
            previousNode = PathToGoal[i];


            // Colors the goal block.
            if (i + 1 == PathToGoal.Count)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(PathToGoal[i].X, 0f, PathToGoal[i].Z), new Vector3(GridIncrement, 10f, GridIncrement));
            }

        }
    }

    
    void SetUpNodeNeighborhoods()
    {
        foreach(var nodeList in hashsetForNodes.Values)
        {
            foreach(var node in nodeList.Values)
            {
                node.Neighbors = new List<Node>();
                List<Node> neighborNodes = new List<Node>();

                float leftColumn = node.X - GridIncrement;
                float rightColumn = node.X + GridIncrement;
                float bottomRow = node.Z - GridIncrement;
                float topRow = node.Z + GridIncrement;

                if (0 <= leftColumn)
                    neighborNodes.Add(hashsetForNodes[node.Z][leftColumn]);

                if (GridWidth >= rightColumn)
                    neighborNodes.Add(hashsetForNodes[node.Z][rightColumn]);

                if (0 <= bottomRow)
                    neighborNodes.Add(hashsetForNodes[bottomRow][node.X]);

                if (GridHeight >= topRow)
                    neighborNodes.Add(hashsetForNodes[topRow][node.X]);

                foreach(var neighbor in neighborNodes)
                {
                    node.Neighbors.Add(neighbor);
                }
            }
        }
    }
}
