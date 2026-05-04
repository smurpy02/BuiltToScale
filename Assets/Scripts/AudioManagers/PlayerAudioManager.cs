using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance;

    public AudioSource jumpAudio, landingAudio, popAudio;

    void Start()
    {
        instance = this;
    }

    public static void Jump()
    {
        instance.jumpAudio.Play();
    }

    public static void Land()
    {
        instance.landingAudio.Play();
    }

    public static void Pop()
    {
        instance.popAudio.Play();
    }
}
