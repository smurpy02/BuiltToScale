using UnityEngine;

public class LevelAudioManager : MonoBehaviour
{
    public static LevelAudioManager instance;

    public AudioSource matchAudio;

    void Start()
    {
        instance = this;
    }

    public static void Match()
    {
        instance.matchAudio.Play();
    }
}
