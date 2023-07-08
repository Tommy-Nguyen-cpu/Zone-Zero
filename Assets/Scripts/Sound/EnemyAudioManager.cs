using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioSource idle_sound;
    //public RandomAudioClip idlesounds;
    //public RandomAudioClip chasesounds;
    //public AudioClip footsteps;

    public AudioClip[] idle_clips;
    public AudioClip[] chase_clips;

    public DecisionTree dt;

    //this bool just tracks that the current animation_state hasn't changed. Is used to repeat looping audio clips.
    private bool unchaged_state;

    // Start is called before the first frame update
    void Start()
    {
        //getcurrentstate
    }

    private void playIdleSequentially()
    {

    }

    IEnumerator playIdleAudioSequentially()
    {
        yield return null;

        while (unchaged_state)
        {
            for (int i = 0; i < idle_clips.Length; i++)
            {
                idle_sound.clip = idle_clips[i];

                idle_sound.Play();

                while (idle_sound.isPlaying)
                {
                    yield return null;
                }
            }
        }
    }

    IEnumerator playChaseAudioSequentially()
    {
        yield return null;
        for (int i=0; i < chase_clips.Length; i++)
        {
            idle_sound.clip = chase_clips[i];

            idle_sound.Play();

            while (idle_sound.isPlaying)
            {
                yield return null;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //If enemy has changed states
        if (dt.cur_anim_type!=dt.prev_anim_type)
        {
            unchaged_state = false;
            if (dt.cur_anim_type == DecisionTree.AnimationType.WALKING)
            {
                unchaged_state = true;
                print("WALKING: Played IDLE sound.");
                StartCoroutine(playIdleAudioSequentially());
            }
            else if (dt.cur_anim_type == DecisionTree.AnimationType.IDLING)
            {
                unchaged_state = true;
                print("IDLING: Played IDLE sound.");
                StartCoroutine(playIdleAudioSequentially());
            }
            else if (dt.cur_anim_type == DecisionTree.AnimationType.CHASING)
            {
                unchaged_state = true;
                print("CHASING: Played CHASING sound.");
                StartCoroutine(playChaseAudioSequentially());
            }
            else if (dt.cur_anim_type == DecisionTree.AnimationType.ATTACKING)
            {
                unchaged_state = true;
                print("ATTACKING: Played CHASING sound.");
                StartCoroutine(playChaseAudioSequentially());
            }
        }
        
    }
}
