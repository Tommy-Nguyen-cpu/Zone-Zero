using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Leaf
{
    /// <summary>
    /// Will be idle if all other states failed their conditional check.
    /// </summary>
    /// <param name="myGameObject"></param>
    /// <param name="myPlayer"></param>
    /// <returns></returns>
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        return true;
    }

    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        base.Action(player, myObject, runninSpeed, animator);
    }
}
