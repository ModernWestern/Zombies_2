using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
    Manager manager;
    AudioSource audioSource;

    float lowPitch = .95f;
    float highPitch = 1.05f;

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

        if (sfx == SFX.CloseRoar) randomClip = manager.zombieAudioSettings.close[Random.Range(0, manager.zombieAudioSettings.close.Length - 1)];
        if (sfx == SFX.MidRoar) randomClip = manager.zombieAudioSettings.mid[Random.Range(0, manager.zombieAudioSettings.mid.Length - 1)];
        if (sfx == SFX.DistantRoar) randomClip = manager.zombieAudioSettings.distant[Random.Range(0, manager.zombieAudioSettings.distant.Length - 1)];

        return randomClip;
    }

    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
        audioSource = GetComponent<AudioSource>();
    }
}
