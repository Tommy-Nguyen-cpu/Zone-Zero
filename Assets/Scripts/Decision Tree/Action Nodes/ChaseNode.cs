using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        // TODO: Once I have time, I'll implement a raycasting system.
        float distance = Mathf.Abs(Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position));
        
        if(distance <= 10f)
        {
            return true;
        }

        return base.CheckCondition(myGameObject, myPlayer);
    }


    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        myObject.transform.LookAt(player.transform);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, player.transform.position, .1f);
        Debug.Log("Chasing!");
        base.Action(player, myObject, runninSpeed, animator);
    }
}
