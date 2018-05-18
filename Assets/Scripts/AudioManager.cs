using UnityEngine;

[System.Serializable]
public class ZombieClips
{
    public AudioClip[] distant;
    public AudioClip[] mid;
    public AudioClip[] close;
}

[System.Serializable]
public class SoundEffects
{
    public AudioClip[] steps;
    public AudioClip[] hits;
    public AudioClip[] gun;
    public AudioClip[] sfx;
}

public class AudioManager : MonoBehaviour
{
    public ZombieClips zombieClips;
    public SoundEffects soundEffects;
}
