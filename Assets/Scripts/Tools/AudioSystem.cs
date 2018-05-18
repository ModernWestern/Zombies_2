﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSystem : MonoBehaviour
{
    AudioManager manager;
    AudioSource audioSource;
    readonly float lowPitch = .95f;
    readonly float highPitch = 1.05f;

    public void SetClip(SFX sfx)
    {
        switch(sfx)
        {
            case (SFX.DistantRoar):
                if (!audioSource.isPlaying)
                {
                    audioSource.pitch = Random.Range(lowPitch, highPitch);
                    audioSource.PlayOneShot(RandomClip(SFX.DistantRoar));
                    //print("DISTANT");
                }
                break;
            case (SFX.MidRoar):
                if (!audioSource.isPlaying)
                {
                    audioSource.pitch = Random.Range(lowPitch, highPitch);
                    audioSource.PlayOneShot(RandomClip(SFX.MidRoar));
                    //print("MID");
                }
                break;
            case (SFX.CloseRoar):
                if (!audioSource.isPlaying)
                {
                    audioSource.pitch = Random.Range(lowPitch, highPitch);
                    audioSource.PlayOneShot(RandomClip(SFX.CloseRoar));
                    //print("CLOSE");
                }
                break;
            default:
                print("ERROR");
                break;
        }
    }

    AudioClip RandomClip(SFX sfx)
    {
        AudioClip randomClip = null;

        if (sfx == SFX.CloseRoar) randomClip = manager.zombieClips.close[Random.Range(0, manager.zombieClips.close.Length - 1)];
        if (sfx == SFX.MidRoar) randomClip = manager.zombieClips.mid[Random.Range(0, manager.zombieClips.mid.Length - 1)];
        if (sfx == SFX.DistantRoar) randomClip = manager.zombieClips.distant[Random.Range(0, manager.zombieClips.distant.Length - 1)];

        return randomClip;
    }

    private void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
        audioSource = GetComponent<AudioSource>();
    }
}