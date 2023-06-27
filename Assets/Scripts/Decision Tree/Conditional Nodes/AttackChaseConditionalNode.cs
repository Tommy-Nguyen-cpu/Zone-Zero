using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChaseConditionalNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        Vector3 fwd = myGameObject.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Debug.DrawRay(new Vector3(myGameObject.transform.position.x, 0f, myGameObject.transform.position.z), fwd, Color.red, 20f);

        if (Physics.Raycast(new Vector3(myGameObject.transform.position.x, 0f, myGameObject.transform.position.z), myGameObject.transform.forward, out hit, 100f))
        {
            if (hit.collider.name == "Player")
                return true;
        }

        return base.CheckCondition(myGameObject, myPlayer);
    }
}
