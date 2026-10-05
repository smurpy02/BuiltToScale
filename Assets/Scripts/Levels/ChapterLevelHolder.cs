using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chapter and level manager", menuName = "Chapter&Level Holder")]
public class ChapterLevelHolder : ScriptableObject
{
    public List<LevelDetails> levels = new List<LevelDetails>();

    public void AddLevel(int chapter, string json) => levels.Add(new LevelDetails(chapter, json));
}

[Serializable]
public class LevelDetails
{
    public int chapter;
    public string json;

    public LevelDetails(int chapter, string json)
    {
        this.chapter = chapter;
        this.json = json;
    }
}