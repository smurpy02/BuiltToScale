using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public static ChapterDetails currentChapter;
    public static LevelDetails currentLevel;

    [Header("Transition")]
    public int yValue = 15;
    public Transform lower, upper;

    private void Start()
    {
        instance = this;

        return;
        lower.gameObject.SetActive(true);
        upper.gameObject.SetActive(true);

        lower.DOMoveY(-yValue, 1f).SetEase(Ease.InCubic);
        upper.DOMoveY(yValue, 1f).SetEase(Ease.InCubic);
    }

    public static void LoadScene(int scene, float transitionTime = 1)
    {
        instance.TransitionScenes(scene, transitionTime);
    }

    public static void LoadLevel(string json)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        if (data != null) LoadLevel(data);
        else
        {
            Debug.LogError("Level Json has Invalid Format");
        }
    }

    public static void LoadLevel(LevelData data)
    {
        GenerateLevelFromSave.levelData = data;
        SceneManager.LoadSceneAsync("GameLevel");
    }

    public static void LoadNextLevel()
    {
        if (currentChapter == null || currentLevel == null) LoadScene(0);

        if(currentChapter.levels.Last() == currentLevel)
        {
            var chapterLevelHolder = LevelMemoryManager.GetChapterLevelHolder();

            if(chapterLevelHolder.chapters.Last() == currentChapter)
            {
                Debug.LogError("CONGRATULATIONS! That's the last level");
                throw new NotImplementedException();
            }

            currentChapter = chapterLevelHolder.chapters[chapterLevelHolder.chapters.IndexOf(currentChapter) + 1];

            if(currentChapter.levels.Count == 0)
            {
                Debug.LogError("No Levels in this Chapter");
                return;
            }

            currentLevel = currentChapter.levels[0];
        }

        currentLevel = currentChapter.levels[currentChapter.levels.IndexOf(currentLevel) + 1];

        instance.StartCoroutine(ILoadLevel(currentLevel.json));
    }

    static IEnumerator ILoadLevel(string json)
    {
        yield return new WaitForSeconds(.8f);

        LoadLevel(json);
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
