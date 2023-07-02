using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        Collider[] checks = Physics.OverlapSphere(myGameObject.transform.position, 100f, ParentTree.PlayerLayerMask);

        if (checks.Length > 0)
        {
            Vector3 directionToPlayer = (myPlayer.transform.position - myGameObject.transform.position).normalized;
            if (Vector3.Angle(myGameObject.transform.forward, directionToPlayer) < ParentTree.Angle / 2)
            {
                float distanceToPlayer = Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position);
                if (!Physics.Raycast(myGameObject.transform.position, directionToPlayer, distanceToPlayer, ParentTree.ObstacleLayerMask))
                {
                    return true;
                }
            }
        }



        return base.CheckCondition(myGameObject, myPlayer);
    }


    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
        ParentTree.PlayAnimation(DecisionTree.AnimationType.CHASING);
        myObject.transform.LookAt(player.transform);
        myObject.transform.position = Vector3.MoveTowards(myObject.transform.position, new Vector3(player.transform.position.x, myObject.transform.position.y, player.transform.position.z), 1.5f * Time.deltaTime);
        Debug.Log("Chasing!");
        base.Action(player, myObject, runninSpeed, animator);
    }


    private bool IsWithinView(GameObject myObject, GameObject target, float height, float coneAngle)
    {
        // Normalized vector of the enemys' facing direction.
        Vector3 directionVector = myObject.transform.forward;

        Vector3 leftBound = Quaternion.Euler(0f, -1*coneAngle, 0f) * directionVector;
        Vector3 rightBound = Quaternion.Euler(0f, coneAngle, 0f) * directionVector;

        RaycastHit hit;
        bool hitPlayer = Physics.Raycast(myObject.transform.position, target.transform.position.normalized, out hit, height);

        if (hitPlayer)
        {
            if(hit.collider.name == "Player")
            {
                Debug.Log("Test Vision: Left(X: " + leftBound.x + ", Y: " + leftBound.y + ", Z: " + leftBound.z);
                Debug.Log("Test Vision: Right(X: " + rightBound.x + ", Y: " + rightBound.y + ", Z: " + rightBound.z);
                Debug.Log("Test Vision: Raycast(X: " + hit.normal.x + ", Y: " + hit.normal.y + ", Z: " + hit.normal.z);
            }
        }

        return false;
    }

}
