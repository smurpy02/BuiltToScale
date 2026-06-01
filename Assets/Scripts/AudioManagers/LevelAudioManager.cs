using UnityEngine;

public class LevelAudioManager : MonoBehaviour
{
    public static LevelAudioManager instance;

    public AudioSource matchAudio;
    public AudioSource spinAudio;

    void Start()
    {
        instance = this;
    }

    public static void Match()
    {
        instance.matchAudio.Play();
    }

    public static void Spin()
    {
        instance.spinAudio.Play();
    }
}
