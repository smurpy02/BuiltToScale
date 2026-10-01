using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulseLight : MonoBehaviour
{
    public Light2D light;

    [Range(.01f, 30)]
    public float frequency = 5;
    [Range(.01f, 30)]
    public float multiplier = 3;
    [Range(.01f, 30)]
    public float offset = 1;

    void Update()
    {
        light.intensity = (Mathf.Sin(Time.time / frequency)+1) * multiplier + offset;
    }
}
