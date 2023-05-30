using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public GameObject Player;
    public Animator myAnimator;

    private Leaf root = new Leaf();
    // Start is called before the first frame update
    void Start()
    {
        // TODO: Manually set up tree.
        SetUpDecisionTree(root);
    }

    // Update is called once per frame
    void Update()
    {
        Leaf currentLeaf = root;

        // Continue to iterate through children until we reach a leaf with no children.
        while(currentLeaf.ChildLeafs.Count > 0)
        {
            foreach(var child in currentLeaf.ChildLeafs)
            {
                if(child.CheckCondition(gameObject, Player))
                {
                    currentLeaf = child;
                    break;
                }
            }
        }

        currentLeaf.Action(Player, gameObject, 5f, myAnimator);
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
