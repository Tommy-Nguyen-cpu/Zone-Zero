using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Manually set up tree.
        Leaf root = new Leaf();
        SetUpDecisionTree(root);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Manually sets up our decision tree.
    /// </summary>
    /// <param name="root"></param>
    void SetUpDecisionTree(Leaf root)
    {
        AttackChaseConditionalNode conditionalNode = new AttackChaseConditionalNode();
        AttackNode attackNode = new AttackNode();
        conditionalNode.ChildLeafs.Add(attackNode);
        StunnedNode stunnedNode = new StunnedNode();
        conditionalNode.ChildLeafs.Add(stunnedNode);
        ChaseNode chaseNode = new ChaseNode();
        conditionalNode.ChildLeafs.Add(chaseNode);

        root.ChildLeafs.Add(conditionalNode);

        IdleNode idleNode = new IdleNode();
        root.ChildLeafs.Add(idleNode);
    }
}
