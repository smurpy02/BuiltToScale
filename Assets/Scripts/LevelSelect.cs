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
    public List<World> worlds;
    public List<PatternMatcher> patterns;
    public GameObject worldButton;
    public Transform worldContainer;

    bool buttonSelected;

    private void Start()
    {
        foreach (World world in worlds)
        {
            Debug.Log("world");
            GameObject button = Instantiate(worldButton, worldContainer);
            int worldIndex = worlds.IndexOf(world);
            button.GetComponentInChildren<TextMeshProUGUI>().text = worldIndex.ToString("0");

            foreach (PatternMatcher pattern in patterns)
            {
                int patternIndex = patterns.IndexOf(pattern);
                int level = world.firstLevel + patternIndex;
                button.GetComponentInChildren<Button>().onClick.AddListener(() => SetPatternLevel(patternIndex, level));

                if (!buttonSelected)
                {
                    SetPatternLevel(patternIndex, level);
                }
            }

            buttonSelected = true;
        }
    }

    void SetPatternLevel(int pattern, int level)
    {
        patterns[pattern].levelToLoad = level;
        patterns[pattern].levelText.text = (level-3).ToString("0");
    }
}
