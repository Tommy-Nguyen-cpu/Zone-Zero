using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf
{
    public enum LeafTypes
    {
        IDLE, CHASE, STUNNED, ATTACK, WIN
    }

    /// <summary>
    /// The type of leaf this current instance of the Leaf class is. If it is null, then that means
    /// this leaf is a decision leaf/node and not a terminal leaf.
    /// </summary>
    public LeafTypes? CurrentState = null;

    /// <summary>
    /// Condition the decision tree will check once it reaches this node/leaf. Null if this is a terminal leaf/node.
    /// </summary>
    public Func<bool> Condition = null;

    /// <summary>
    /// The child of the current leaf (only non-null if this is not a terminal leaf).
    /// </summary>
    public Leaf NextLeaf;

    /// <summary>
    /// If the condition is met, we retrieve the child leaf of the current leaf.
    /// </summary>
    /// <returns></returns>
    public Leaf CheckCondition()
    {
        if (Condition())
            return NextLeaf;
        return null;
    }

    /// <summary>
    /// Method responsible for deciding which action to take depending on this leafs' "CurrentState".
    /// </summary>
    public void Action()
    {
        switch (CurrentState)
        {
            case LeafTypes.CHASE:
                Chase();
                break;
            case LeafTypes.STUNNED:
                Stunned();
                break;
            case LeafTypes.ATTACK:
                Attack();
                break;
            case LeafTypes.WIN:
                Win();
                break;
            default:
                Idle();
                break;
        }
    }


    #region Action Type

    public void Idle()
    {
        Debug.Log("Enemy is idling!");
    }

    void Chase()
    {
        Debug.Log("Enemy is chasing!");
    }

    void Stunned()
    {
        Debug.Log("Enemy is stunned!");
    }

    void Attack()
    {
        Debug.Log("Enemy is attacking!");
    }

    void Win()
    {
        Debug.Log("Enemy wins!");
    }

    #endregion
}
