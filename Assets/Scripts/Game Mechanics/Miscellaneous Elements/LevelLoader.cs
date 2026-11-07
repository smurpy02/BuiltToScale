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

    private void Start()
    {
        instance = this;
    }

    public static void LoadScene(int sceneIndex)
    {
        LoadScene(SceneManager.GetSceneAt(sceneIndex).name);
    }

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.ExitTransition(sceneName));
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
        LoadScene("GameLevel");
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
        yield return new WaitForSeconds(2.4f);

        LoadLevel(json);
    }

    IEnumerator ExitTransition(string scene)
    {
        yield return new WaitForSeconds(.5f);

        SceneManager.LoadSceneAsync(scene);
    }
}
