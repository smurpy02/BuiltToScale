using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance;

    [Header("Audio")]
    public AudioSource jumpAudio;
    public AudioSource landingAudio;
    public AudioSource popAudio;

    void Start()
    {
        instance = this;
    }

    public void Jump()
    {
        jumpAudio.Play();
    }

    public void Land()
    {
        landingAudio.Play();
    }

    public void Pop()
    {
        popAudio.Play();
    }
}
