using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Leaf
{
    /// <summary>
    /// Flag that is turned on after enemy has attacked. Used by "StunnedNode".
    /// </summary>
    public static bool Attacked = false;

    /// <summary>
    /// Checks to see if the player is within 2 feet of the player.
    /// </summary>
    /// <param name="myGameObject"></param>
    /// <param name="myPlayer"></param>
    /// <returns></returns>
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        /*        float distance = Mathf.Abs(Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position));

                if (distance < 2f)
                    return true;*/
        Vector3 fwd = myGameObject.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(myGameObject.transform.position.x, 0f, myGameObject.transform.position.z), fwd, out hit, 1f))
        {
            if (hit.collider.name == "Player")
                return true;
        }

        return base.CheckCondition(myGameObject, myPlayer);
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
        // controller.SetHealth(-.01f);

        ParentTree.PlayAnimation(DecisionTree.AnimationType.ATTACKING);
        myObject.GetComponent<StruggleSystem>().enabled = true;
        Attacked = true;
    }
}
