using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Node : IComparable
{
    #region MazeFlags Modification
    public MazeFlags flag;
    public int2 myIndex;
    #endregion

    #region Node Dimensions
    public float X;
    public float Z;
    public float NodeWidth;
    public float NodeLength;
    #endregion

    public List<Node> Neighbors;
    public float StartScore;
    public float GoalScore;
    public float EstimatedTotalScore;
    public Node Parent;

    public int CompareTo(object obj)
    {
        Node comparedNode = (Node)obj;
        if (this.EstimatedTotalScore < comparedNode.EstimatedTotalScore)
            return -1;
        else if (this.EstimatedTotalScore > comparedNode.EstimatedTotalScore)
            return 1;
        return 0;
    }
}
