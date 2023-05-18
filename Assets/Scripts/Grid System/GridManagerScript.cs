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

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        for(float i = 0; i <= GridWidth; i += GridIncrement)
        {
            for(float j = 0; j <= GridHeight; j += GridIncrement)
            {
                Node newNode = new Node();
                newNode.NodeWidth = GridIncrement;
                newNode.NodeHeight = GridIncrement;
                newNode.NodeCenter = (i, j);

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

        algorithm.GetNodeListAndEnemyObject(FloorNodes, Enemy);

        algorithm.StartAStar();
    }

    private void OnDrawGizmos()
    {
        foreach(var node in FloorNodes)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(new Vector3(node.NodeCenter.Item1, 0f, node.NodeCenter.Item2), new Vector3(GridIncrement, 10f, GridIncrement));
        }

        Node previousNode = algorithm.PathToGoal[0];

        Debug.Log($"Node has {previousNode.NeighborNodes.Count} neighbors");

        Debug.Log($"Number of nodes in path: {algorithm.PathToGoal.Count}");
        for(int i = 1; i < algorithm.PathToGoal.Count; i++)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(previousNode.NodeCenter.Item1, 10f, previousNode.NodeCenter.Item2), new Vector3(algorithm.PathToGoal[i].NodeCenter.Item1, 10f, algorithm.PathToGoal[i].NodeCenter.Item2));
            previousNode = algorithm.PathToGoal[i];


            // Colors the goal block.
            if (i + 1 == algorithm.PathToGoal.Count)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(algorithm.PathToGoal[i].NodeCenter.Item1, 0f, algorithm.PathToGoal[i].NodeCenter.Item2), new Vector3(GridIncrement, 10f, GridIncrement));
            }

        }
    }

    
    void SetUpNodeNeighborhoods()
    {
        foreach(var nodeList in hashsetForNodes.Values)
        {
            foreach(var node in nodeList.Values)
            {
                List<Node> neighborNodes = new List<Node>();

                float leftColumn = node.NodeCenter.Item1 - GridIncrement;
                float rightColumn = node.NodeCenter.Item1 + GridIncrement;
                float bottomRow = node.NodeCenter.Item2 - GridIncrement;
                float topRow = node.NodeCenter.Item2 + GridIncrement;

                if (0 <= leftColumn)
                    neighborNodes.Add(hashsetForNodes[node.NodeCenter.Item2][leftColumn]);

                if (GridWidth >= rightColumn)
                    neighborNodes.Add(hashsetForNodes[node.NodeCenter.Item2][rightColumn]);

                if (0 <= bottomRow)
                    neighborNodes.Add(hashsetForNodes[bottomRow][node.NodeCenter.Item1]);

                if (GridHeight >= topRow)
                    neighborNodes.Add(hashsetForNodes[topRow][node.NodeCenter.Item1]);

                node.SetUpNeighbors(neighborNodes);
            }
        }
    }
}
