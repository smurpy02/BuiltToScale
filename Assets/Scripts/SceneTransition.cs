using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Transform lower;
    public Transform upper;

    public static SceneTransition instance;

    public int yValue = 15;

    private void Start()
    {
        instance = this;
        lower.DOMoveY(-yValue, 1f).SetEase(Ease.InCubic);
        upper.DOMoveY(yValue, 1f).SetEase(Ease.InCubic);
    }

    public static void TransitionScene(int scene, float transitionTime = 1)
    {
        instance.TransitionToNextScene(scene, transitionTime);
    }

    public void TransitionToNextScene(int scene, float transitionTime)
    {
        StartCoroutine(NextScene(scene, transitionTime));
    }

    IEnumerator NextScene(int scene, float transitionTime)
    {
        lower.DOMoveY(0, transitionTime).SetEase(Ease.InCubic);
        yield return upper.DOMoveY(0, transitionTime).SetEase(Ease.InCubic).WaitForCompletion();

        SceneManager.LoadSceneAsync(scene);
    }
}
