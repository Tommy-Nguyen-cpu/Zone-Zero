using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChaseConditionalNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        float distance = Mathf.Abs(Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position));

        if (distance <= 10f)
        {
            return true;
        }

        return false;
    }
}
