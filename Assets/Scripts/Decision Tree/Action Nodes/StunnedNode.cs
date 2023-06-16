using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        if (AttackNode.Attacked)
        {
            return true;
        }

        return base.CheckCondition(myGameObject, myPlayer);
    }

    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        Debug.Log("Stunned!");
        base.Action(player, myObject, runninSpeed, animator);

        // TODO: Freeze the enemy for a couple of seconds.

        // Resets flag.
        AttackNode.Attacked = false;
    }
}
