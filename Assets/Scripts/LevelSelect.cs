using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public class World
{
    public string worldName;
    public int firstLevel;
    public int lastLevel;
}

public class LevelSelect : MonoBehaviour
{
    public static int level = 2;
    public TMP_Dropdown dropdown;

    public List<World> worlds;
    public GameObject worldButton;
    public Transform worldContainer;

    private int maxLevel;

    private void Start()
    {
        //UpdateLevel();

        //maxLevel = SceneManager.sceneCount - 2;

        //List<string> levelNames = new List<string>();

        //Debug.Log("scenes " + SceneManager.sceneCountInBuildSettings);

        //for (int scene = 2; scene < SceneManager.sceneCountInBuildSettings; scene++)
        //{
        //    Debug.Log(scene);
        //    if (scene != SceneManager.sceneCountInBuildSettings - 1)
        //    {
        //        levelNames.Add("Level " + (scene - 2));
        //    }
        //}

        //dropdown.ClearOptions();
        //dropdown.AddOptions(levelNames);

        foreach(World world in worlds)
        {
            Instantiate(worldButton, worldContainer).GetComponentInChildren<TextMeshProUGUI>().text = worlds.IndexOf(world).ToString("0");
        }
    }

    public void UpdateLevel()
    {
        level = dropdown.value + 2;
    }
}
