using UnityEngine;

public class PlayAudioOnAwake : MonoBehaviour
{
    public AudioSource source;
    public bool randomisePitch;

    void Awake()
    {
        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) return;

        if (randomisePitch) source.pitch = Random.Range(0.8f, 1.3f);

        source.Play();
    }
}
