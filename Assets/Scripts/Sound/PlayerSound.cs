using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource footsteps;

    [SerializeField]
    private CapsuleCollider player_colldier;

    public void Update()
    {
        if (Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
        {
            footsteps.enabled = true;
        } else
        {
            footsteps.enabled = false;
        }
    }
}
