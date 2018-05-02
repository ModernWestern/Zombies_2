using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioReverbZone))]

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public static bool oncePlay;

    float lowPitch = .95f;
    float highPitch = 1.05f;

    public void SetClip(SFX sfx)
    {
        switch(sfx)
        {
            case (SFX.DistantRoar):
                if (oncePlay == true)
                {
                    audioSource.pitch = Random.Range(lowPitch, highPitch);
                    audioSource.PlayOneShot(RandomClip(SFX.DistantRoar));
                    //print("DISTANT");
                }
                break;
            case (SFX.MidRoar):
                if (oncePlay == true)
                {
                    audioSource.pitch = Random.Range(lowPitch, highPitch);
                    audioSource.PlayOneShot(RandomClip(SFX.MidRoar));
                    //print("MID");
                }
                break;
            case (SFX.CloseRoar):
                if (oncePlay == true)
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

        if (sfx == SFX.CloseRoar) randomClip = Manager.close[Random.Range(0, Manager.close.Length - 1)];
        if (sfx == SFX.MidRoar) randomClip = Manager.mid[Random.Range(0, Manager.mid.Length - 1)];
        if (sfx == SFX.DistantRoar) randomClip = Manager.distant[Random.Range(0, Manager.distant.Length - 1)];

        return randomClip;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //print("BOX CLOSE: " + Manager.close.Length);
        //print("BOX MID: " + Manager.mid.Length);
        //print("BOX DISTANT: " + Manager.distant.Length);
    }
}
