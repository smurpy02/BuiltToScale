using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static int level = 2;
    int maxLevel;

    public TMP_Dropdown dropdown;
    List<Scene> levels = new List<Scene>();

    private void Start()
    {
        UpdateLevel();

        maxLevel = SceneManager.sceneCount - 2;
        
        List<string> levelNames = new List<string>();

        Debug.Log("scenes " + SceneManager.sceneCountInBuildSettings);

        for(int scene = 2; scene < SceneManager.sceneCountInBuildSettings; scene++)
        {
            Debug.Log(scene);
            Scene level = SceneManager.GetSceneByBuildIndex(scene);
            levels.Add(level);
            levelNames.Add("Level " + (scene-2));
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(levelNames);
    }

    public void UpdateLevel()
    {
        level = dropdown.value + 2;
    }
}
