using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public InputActionReference retry;
    public float maxShake;
    public float retryHoldTime;

    float retryHeld;
    Vector3 cameraStart;
    Transform cameraTransform;

    bool sceneLoaded;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraStart = cameraTransform.position;
    }

    private void Update()
    {
        retryHeld = retry.action.IsPressed() ? retryHeld + Time.deltaTime : 0;

        cameraTransform.position = cameraStart + (Vector3)(Random.insideUnitCircle * (sceneLoaded ? maxShake : Mathf.Clamp(retryHeld / retryHoldTime * maxShake, 0, maxShake)));

        if(retryHeld >= retryHoldTime && !sceneLoaded)
        {
            sceneLoaded = true;
            SceneTransition.TransitionScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
