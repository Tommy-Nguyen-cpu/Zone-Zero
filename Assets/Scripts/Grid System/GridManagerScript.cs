using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    float GridWidth = 50f;
    float GridHeight = 50f;

    (float, float) Origin = (-50f, -50f);

    public float GridIncrement = 10f;

    /// <summary>
    /// TODO: Used to allow us to more easily set up neighborhoods for nodes.
    /// </summary>
    Dictionary<float, Dictionary<float, Node>> hashsetForNodes = new Dictionary<float, Dictionary<float, Node>>();

    /// <summary>
    /// TODO: Used for A* search.
    /// </summary>
    public List<Node> FloorNodes = new List<Node>();

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: When we initialize a node, it is probably a better idea to remove any "obstacle" nodes before setting up neighborhoods.
        // This might increase the complexity of the neighborhood setup though.
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

    }

    private void OnDrawGizmos()
    {
        foreach(var node in FloorNodes)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(new Vector3(node.X, 0f, node.Z), new Vector3(GridIncrement, 10f, GridIncrement));
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

                if (Origin.Item1 <= leftColumn)
                    neighborNodes.Add(hashsetForNodes[node.Z][leftColumn]);

                if (GridWidth >= rightColumn)
                    neighborNodes.Add(hashsetForNodes[node.Z][rightColumn]);

                if (Origin.Item2 <= bottomRow)
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
