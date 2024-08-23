using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public InputActionReference retry;
    public float maxShake;
    public float retryHoldTime;

    public GameObject retryBar;
    public Transform retryBarProgress;

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

        float retryProgress = retryHeld / retryHoldTime;

        retryBar.SetActive(sceneLoaded || retryProgress > 0);
        Vector3 scale = retryBarProgress.localScale;
        scale.x = sceneLoaded ? 1 : Mathf.Clamp(retryProgress, 0 , 1);
        retryBarProgress.localScale = scale;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraStart + (Vector3)(Random.insideUnitCircle * (sceneLoaded ? maxShake : Mathf.Clamp(retryProgress * maxShake, 0, maxShake))), Time.deltaTime * 50);

        if(retryHeld >= retryHoldTime && !sceneLoaded)
        {
            sceneLoaded = true;
            SceneTransition.TransitionScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
