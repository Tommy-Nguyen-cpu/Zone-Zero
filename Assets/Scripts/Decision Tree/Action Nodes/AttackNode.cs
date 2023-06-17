using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Leaf
{
    /// <summary>
    /// Checks to see if the player is within 2 feet of the player.
    /// </summary>
    /// <param name="myGameObject"></param>
    /// <param name="myPlayer"></param>
    /// <returns></returns>
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        float distance = Mathf.Abs(Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position));

        if (distance < 2f)
            return true;

        return false;
    }

    /// <summary>
    /// Deals damage to the player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="myObject"></param>
    /// <param name="runninSpeed"></param>
    /// <param name="animator"></param>
    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        // TODO: Play the attack animation.
        // TODO: I'll reanimate everything later on.
        Debug.Log("Attacking!");


        // TODO: We might not want to decrease the players health every time.
        // I recalled we saying that we want there to be some sort of struggle mode, so this might be where
        // we implement that.

        // Decreases players health
        controller.SetHealth(-.01f);
    }
}