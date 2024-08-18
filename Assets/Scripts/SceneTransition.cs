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

    private void Start()
    {
        instance = this;
        lower.DOMoveY(-15, 1f).SetEase(Ease.InCubic);
        upper.DOMoveY(15, 1f).SetEase(Ease.InCubic);
    }

    public static void TransitionScene(int scene)
    {
        instance.TransitionToNextScene(scene);
    }

    public void TransitionToNextScene(int scene)
    {
        StartCoroutine(NextScene(scene));
    }

    IEnumerator NextScene(int scene)
    {
        lower.DOMoveY(0, 1f).SetEase(Ease.InCubic);
        yield return upper.DOMoveY(0, 1f).SetEase(Ease.InCubic).WaitForCompletion();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
