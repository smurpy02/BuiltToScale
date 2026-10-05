using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chapter and level manager", menuName = "Chapter&Level Holder")]
public class ChapterLevelHolder : ScriptableObject
{
    public List<LevelDetails> levels = new List<LevelDetails>();

    public void AddLevel(string chapter, string json)
    {
        var levelDetails = new LevelDetails(chapter, json);

        levels.Add(levelDetails);
    }
}

[Serializable]
public class LevelDetails
{
    public string chapter, json;

    public LevelDetails(string chapter, string json)
    {
        this.chapter = chapter;
        this.json = json;
    }
}