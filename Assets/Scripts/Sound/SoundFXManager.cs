using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public AudioSource FX_player;
    public AudioClip GoodExit;
    public AudioClip BadExit;
    // Start is called before the first frame update

    private void PlayGoodExit()
    {
        FX_player.PlayOneShot(GoodExit, 1f);
    }

    private void PlayBadExit()
    {
        FX_player.PlayOneShot(BadExit, 1f);
    }

    public void PlayExitSound(bool flag)
    {
        if (flag)
            PlayGoodExit();
        else
            PlayBadExit();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
