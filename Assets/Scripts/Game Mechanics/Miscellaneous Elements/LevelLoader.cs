using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    [Header("Transition")]
    public int yValue = 15;
    public Transform lower, upper;

    private void Start()
    {
        instance = this;

        lower.gameObject.SetActive(true);
        upper.gameObject.SetActive(true);

        lower.DOMoveY(-yValue, 1f).SetEase(Ease.InCubic);
        upper.DOMoveY(yValue, 1f).SetEase(Ease.InCubic);
    }

    public static void LoadScene(int scene, float transitionTime = 1)
    {
        instance.TransitionScenes(scene, transitionTime);
    }

    public void TransitionScenes(int scene, float transitionTime)
    {
        StartCoroutine(ITransitionScenes(scene, transitionTime));
    }

    IEnumerator ITransitionScenes(int scene, float transitionTime)
    {
        lower.DOMoveY(0, transitionTime).SetEase(Ease.InCubic);
        yield return upper.DOMoveY(0, transitionTime).SetEase(Ease.InCubic).WaitForCompletion();

        // Generate Level
        SceneManager.LoadSceneAsync(scene);
    }
}
