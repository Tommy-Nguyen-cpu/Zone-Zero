using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        return base.CheckCondition(myGameObject, myPlayer);
    }

    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        base.Action(player, myObject, runninSpeed, animator);
    }
}
