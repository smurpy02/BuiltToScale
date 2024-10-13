using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelMatcher : MonoBehaviour
{
    public GameObject complete;

    void Update()
    {
        PatternMatcher matcher = GetComponent<PatternMatcher>();

        complete.SetActive(PlayerPrefs.GetInt(matcher.levelToLoad.ToString(), 0) == 1);
    }
}
