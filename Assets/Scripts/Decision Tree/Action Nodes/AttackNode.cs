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

        // Decreases players health
        controller.SetHealth(-2);
    }
}
