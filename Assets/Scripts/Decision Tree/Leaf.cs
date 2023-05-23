using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public List<Leaf> ChildLeafs;

    /// <summary>
    /// If the condition is met, we retrieve the child leaf of the current leaf.
    /// </summary>
    /// <returns></returns>
    public Leaf CheckCondition()
    {
        foreach(var childLeaf in ChildLeafs)
        {
            if (childLeaf.Condition())
                return childLeaf;
        }
        return null;
    }

    /// <summary>
    /// Method responsible for deciding which action to take depending on this leafs' "CurrentState".
    /// </summary>
    public void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        switch (CurrentState)
        {
            case LeafTypes.CHASE:
                Chase(player, myObject, runninSpeed, animator);
                break;
            case LeafTypes.STUNNED:
                Stunned(animator);
                break;
            case LeafTypes.ATTACK:
                Attack(player, animator);
                break;
            case LeafTypes.WIN:
                Win();
                break;
            default:
                Idle(animator);
                break;
        }
    }


    #region Action Type

    public void Idle(Animator animator)
    {
        ResetAllBools(animator);
        float randomValue = UnityEngine.Random.value;
        if(randomValue > .5)
        {
            // TODO: Play the idle animation and nothing else.
            animator.SetBool("IsIdle", true);
        }
        else
        {
            animator.SetBool("IsRunning", true);
            // TODO: Find a path to some random location and move towards it.
        }

        Debug.Log("Enemy is idling!");
    }

    void Chase(GameObject player, GameObject myObject, float runningSpeed, Animator animator)
    {
        ResetAllBools(animator);

        myObject.transform.LookAt(player.transform);

        animator.SetBool("IsRunning", true);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, player.transform.position, Time.deltaTime * runningSpeed);

        Debug.Log("Enemy is chasing!");
    }

    void Stunned(Animator animator)
    {
        ResetAllBools(animator);
        animator.SetBool("IsStunned", true);
        Debug.Log("Enemy is stunned!");
    }

    void Attack(GameObject player, Animator animator)
    {
        ResetAllBools(animator);
        animator.SetBool("IsAttack", true);
        Debug.Log("Enemy is attacking!");
    }

    void Win()
    {
        // TODO: Create the GAME OVER screen.
        SceneManager.LoadScene(0);
        Debug.Log("Enemy wins!");
    }

    #endregion


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
