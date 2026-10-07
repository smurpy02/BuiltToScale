using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chapter and level manager", menuName = "Chapter&Level Holder")]
public class ChapterLevelHolder : ScriptableObject
{
    public List<ChapterDetails> chapters = new List<ChapterDetails>();

    public void AddLevel(ChapterDetails chapter, string json)
    {
        if(!chapters.Contains(chapter)) chapters.Add(chapter);

        var levelDetails = new LevelDetails(chapter, json);

        chapter.levels.Add(levelDetails);
    }
}

[Serializable]
public class LevelDetails
{
    public ChapterDetails chapter;
    public string json;

    public LevelDetails(ChapterDetails chapter, string json)
    {
        this.chapter = chapter;
        this.json = json;
    }
}