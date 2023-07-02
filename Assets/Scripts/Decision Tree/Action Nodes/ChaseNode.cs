using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Leaf
{
    public override bool CheckCondition(GameObject myGameObject, GameObject myPlayer)
    {
        // TODO: Once I have time, I'll implement a raycasting system.
/*        float distance = Mathf.Abs(Vector3.Distance(myGameObject.transform.position, myPlayer.transform.position));
        
        if(distance <= 10f)
        {
            return true;
        }*/

        Vector3 fwd = myGameObject.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        IsWithinView(myGameObject, myPlayer, 100, .9f);

        if (Physics.Raycast(new Vector3(myGameObject.transform.position.x, 0f, myGameObject.transform.position.z), myGameObject.transform.forward, out hit, 100f))
        {
            if (hit.collider.name == "Player")
                return true;
        }

        

        return base.CheckCondition(myGameObject, myPlayer);
    }


    public override void Action(GameObject player, GameObject myObject, float runninSpeed, Animator animator)
    {
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
