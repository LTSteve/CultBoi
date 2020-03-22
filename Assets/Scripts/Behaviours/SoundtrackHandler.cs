using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackHandler : MonoBehaviour
{
    public AudioSource Intro;
    public AudioSource Loop;

    private float volume;

    private bool firstTime = true;

    private void Awake()
    {
        volume = Intro.volume;
    }

    private void FixedUpdate()
    {
        if (TextScroller.Open) { return; }
        if(!TextScroller.Open && firstTime)
        {
            firstTime = false;
            Intro.Play();
            return;
        }

        if (PauseMenu.IsOpen)
        {
            Loop.volume = volume * 0.5f;
            Intro.volume = volume * 0.5f;
        }
        else
        {
            Loop.volume = volume;
            Intro.volume = volume;
        }

        if(Intro.isPlaying && (Intro.time + Time.fixedDeltaTime) >= Intro.clip.length)
        {
            Loop.PlayDelayed(Intro.clip.length - Intro.time);
            Intro.gameObject.SetActive(false);
        }
        if(!Intro.isPlaying && !Loop.isPlaying)
        {
            Loop.Play();
            Intro.gameObject.SetActive(false);
        }
    }
}
