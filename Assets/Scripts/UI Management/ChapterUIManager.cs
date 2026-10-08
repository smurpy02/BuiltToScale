using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChapterUIManager : MonoBehaviour
{
    public LevelSelectUIManager levelUIManager;
    public ClockScrollUI clockScroll;

    List<ChapterDetails> chapters = new List<ChapterDetails>();
    int currentChapterIndex = 0;

    void Start()
    {
        clockScroll.shiftLeft += () => SetChapter(currentChapterIndex - 1);
        clockScroll.shiftRight += () => SetChapter(currentChapterIndex + 1);

        var levelHolder = LevelMemoryManager.GetChapterLevelHolder();

        chapters = levelHolder.chapters;

        foreach (var _ in chapters) clockScroll.AddPoint();

        clockScroll.ResetRotation();

        if (chapters.Count > 0) SetChapter(0);
    }

    void SetChapter(int index)
    {
        if (chapters.Count > index && index >= 0)
        {
            currentChapterIndex = index;
            SetChapter(chapters[index]);
        }
    }

    void SetChapter(ChapterDetails chapter)
    {
        levelUIManager.Clear();
        levelUIManager.SetChapterNumber(chapters.IndexOf(chapter) + 1);

        chapter.levels.ForEach(level => levelUIManager.SpawnButton(level));

        levelUIManager.clockScroll.ResetRotation();
    }
}