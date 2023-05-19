using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{
    /// <summary>
    /// How far the current node is from the goal node.
    /// </summary>
    public float GoalScore = 0f;

    /// <summary>
    /// How far the start node is from the current node.
    /// </summary>
    public float StartScore = 0f;

    /// <summary>
    /// Estimated distance from start node to goal node (StartScore + GoalScore).
    /// </summary>
    public float TotalEstimatedScore = 0f;

    public float NodeWidth = 0f;
    public float NodeHeight = 0f;

    /// <summary>
    /// Contains the X and Z axis coordinate for the node, respectively.
    /// </summary>
    public (float, float) NodeCenter = (0f, 0f);

    public Node? cameFrom = null;

    /// <summary>
    /// List of all adjacent nodes.
    /// </summary>
    public List<Node> NeighborNodes { get; private set; } = new List<Node>();

    /// <summary>
    /// Adds all neighbor nodes associated with the current node.
    /// </summary>
    /// <param name="nodes"></param>
    public void SetUpNeighbors(List<Node> nodes)
    {
        foreach(var node in nodes)
        {
            NeighborNodes.Add(node);
        }
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.TotalEstimatedScore < node.TotalEstimatedScore)
            return -1;
        if (this.TotalEstimatedScore > node.TotalEstimatedScore)
            return 1;

        return 0;
    }
}
