using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public InputActionReference retry;

    bool sceneLoaded;

    private void Update()
    {
        if (retry.action.WasPressedThisFrame() && !sceneLoaded) ReloadScene();
    }

    void ReloadScene()
    {
        sceneLoaded = true;
        SceneTransition.TransitionScene(SceneManager.GetActiveScene().buildIndex, 0.8f);
    }
}
