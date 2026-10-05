using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChapterUIManager : MonoBehaviour
{
    public LevelSelectUIManager levelUIManager;
    public Transform pivot;

    Dictionary<string, List<LevelDetails>> chapters = new();
    int currentChapterIndex = 0;
    Vector3 rotation = Vector3.zero;

    const int rotateTheta = 45;

    void Start()
    {
        var levelHolder = LevelMemoryManager.GetChapterLevelHolder();

        foreach(var level in levelHolder.levels)
        {
            if(string.IsNullOrEmpty(level.chapter)) continue;

            if (!chapters.ContainsKey(level.chapter))
            {
                chapters.Add(level.chapter, new List<LevelDetails>());
            }

            chapters[level.chapter].Add(level);
        }

        if (chapters.Count > 0) SetChapter(chapters.First().Key);
    }

    void SetChapter(int index)
    {
        var chapterKeys = chapters.Keys.ToList();

        if (chapterKeys.Count > index && index >= 0)
        {
            currentChapterIndex = index;
            SetChapter(chapterKeys[index]);
        }
    }

    void SetChapter(string chapter)
    {
        rotation.z = rotateTheta * currentChapterIndex;
        pivot.DORotate(rotation, .25f).SetEase(Ease.InOutQuad);

        levelUIManager.Clear();

        chapters[chapter].ForEach(level => levelUIManager.SpawnButton(level.json));

        levelUIManager.ResetRotation();
    }

    public void ShiftRight()
    {
        SetChapter(currentChapterIndex+1);
    }

    public void ShiftLeft()
    {
        SetChapter(currentChapterIndex-1);
    }
}