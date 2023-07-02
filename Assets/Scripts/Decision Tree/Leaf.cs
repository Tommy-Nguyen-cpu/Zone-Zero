using System;
using System.Collections.Generic;
using UnityEngine;

public class Leaf
{
    public List<Leaf> ChildLeafs = new List<Leaf>();

    public DecisionTree ParentTree;

    /// <summary>
    /// If the condition is met, we retrieve the child leaf of the current leaf.
    /// </summary>
    /// <param name="myGameObject"> Enemys' GameObject.</param>
    /// <param name="myPlayer"> GameObject of the player.</param>
    /// <returns></returns>
    public virtual bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        return false;
    }

    /// <summary>
    /// Method responsible for deciding which action to take depending on this leafs' "CurrentState".
    /// </summary>
    public virtual void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {

    }


    #region Helper Methods

    /// <summary>
    /// Resets the animations.
    /// </summary>
    /// <param name="animator"></param>
    private void ResetAllBools(Animator animator)
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsIdle", false);
    }

    #endregion
}
