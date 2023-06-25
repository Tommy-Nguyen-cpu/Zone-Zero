using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    List<Node> nodeQueue = new List<Node>();

    /// <summary>
    /// Adds the node to the queue. It's location will be determined based on how close the node is to the goal.
    /// </summary>
    /// <param name="newNode"></param>
    /// <param name="priority"> Total cost from start node to goal node.</param>
    public void Enqueue(Node newNode)
    {
        nodeQueue.Add(newNode);
        nodeQueue.Sort();
    }

    /// <summary>
    /// Returns the first item in the priority queue.
    /// </summary>
    /// <returns></returns>
    public Node Dequeue()
    {
        Node firstNode = nodeQueue[0];
        nodeQueue.Remove(firstNode);
        return firstNode;
    }

    public bool IsEmpty()
    {
        return nodeQueue.Count == 0;
    }

    public bool Contains(Node childNode)
    {
        foreach(var node in nodeQueue)
        {
            if (node.X == childNode.X && node.Z == childNode.Z)
                return true;
        }

        return false;
    }


}
