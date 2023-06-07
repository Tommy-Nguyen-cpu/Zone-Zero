using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public GameObject Player;
    public Animator myAnimator;


    // TODO: Update this to be the script Ian makes for map generation.
    public GridManagerScript GridManager;


    private Leaf root = new Leaf();
    // Start is called before the first frame update
    void Start()
    {
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

        IdleNode idleNode = new IdleNode(GridManager);
        root.ChildLeafs.Add(idleNode);
    }


    private void OnDrawGizmos()
    {
        List<Node> path = ((IdleNode)root.ChildLeafs[root.ChildLeafs.Count - 1]).PathToGoal;

        if(path.Count > 0)
        {
            Node previousNode = path[0];

            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawLine(new Vector3(previousNode.X, 10f, previousNode.Z), new Vector3(path[i].X, 10f, path[i].Z));
                previousNode = path[i];


                // Colors the goal block.
                if (i + 1 == path.Count)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(new Vector3(path[i].X, 0f, path[i].Z), new Vector3(GridManager.GridIncrement, 10f, GridManager.GridIncrement));
                }

            }
        }
    }
}
