using System.Collections.Generic;

public class PriorityQueue
{
    List<Node> nodeQueue = new List<Node>();

    /// <summary>
    /// Adds the node to the queue. It's location will be determined based on how close the node is to the goal.
    /// </summary>
    /// <param name="newNode"></param>
    /// <param name="priority"> Total cost from start node to goal node.</param>
    public void Enqueue(Node newNode, float priority)
    {
        // If the list is empty, we just add the new node to the list.
        if (nodeQueue.Count == 0)
            nodeQueue.Add(newNode);
        else
        {
            Node firstNode = nodeQueue[nodeQueue.Count-1];
            
            // If the new node has a smaller start-to-goal node distance, we add it to the beginning of the list.
            if(newNode.TotalEstimatedScore < firstNode.TotalEstimatedScore)
            {
                nodeQueue.Add(newNode);
            }
        }

    }

    /// <summary>
    /// Returns the first item in the priority queue.
    /// </summary>
    /// <returns></returns>
    public Node Dequeue()
    {
        Node firstNode = nodeQueue[nodeQueue.Count - 1];
        nodeQueue.RemoveAt(nodeQueue.Count - 1);
        return firstNode;
    }

    public bool IsEmpty()
    {
        return nodeQueue.Count == 0;
    }

    public bool Contains(Node childNode)
    {
        return nodeQueue.Contains(childNode);
    }


}
