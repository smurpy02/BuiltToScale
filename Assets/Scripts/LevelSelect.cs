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
    public int numLevels;
}

public class LevelSelect : MonoBehaviour
{
    public List<World> worlds;
    public List<PatternMatcher> patterns;
    public GameObject worldButton;
    public Transform worldContainer;
    public TextMeshProUGUI worldText;

    bool buttonSelected;

    private void Start()
    {
        foreach (World world in worlds)
        {
            GameObject button = Instantiate(worldButton, worldContainer);
            int worldIndex = worlds.IndexOf(world);
            button.GetComponentInChildren<TextMeshProUGUI>().text = worldIndex.ToString("0");
            button.GetComponentInChildren<Button>().onClick.AddListener(() => ChangeWorldText(world.worldName));

            foreach (PatternMatcher pattern in patterns)
            {
                int patternIndex = patterns.IndexOf(pattern);
                int level = patternIndex >= world.numLevels ? -1 : world.firstLevel + patternIndex + 3;

                button.GetComponentInChildren<Button>().onClick.AddListener(() => SetPatternLevel(patternIndex, level));

                if (!buttonSelected)
                {
                    SetPatternLevel(patternIndex, -1);
                }
            }

            buttonSelected = true;
        }
    }

    void SetPatternLevel(int pattern, int level)
    {
        patterns[pattern].gameObject.SetActive(level >= 0);

        patterns[pattern].levelToLoad = level;
        patterns[pattern].levelText.text = (level-3).ToString("0");
    }

    void ChangeWorldText(string world)
    {
        worldText.text = world;
    }
}
