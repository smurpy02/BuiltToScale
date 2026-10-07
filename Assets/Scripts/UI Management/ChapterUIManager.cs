using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChapterUIManager : MonoBehaviour
{
    public LevelSelectUIManager levelUIManager;
    public Transform pivot;

    List<ChapterDetails> chapters = new List<ChapterDetails> ();
    int currentChapterIndex = 0;
    Vector3 rotation = Vector3.zero;

    const int rotateTheta = 45;

    void Start()
    {
        var levelHolder = LevelMemoryManager.GetChapterLevelHolder();

        chapters = levelHolder.chapters;

        foreach(var chapter in chapters)
        {
            Debug.Log("Chapter " + chapter.chapterName);

            int i = 0;

            foreach(var level in chapter.levels)
            {
                Debug.Log("Level " + i++);
                level.chapter = chapter;
            }
        }

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
        rotation.z = rotateTheta * currentChapterIndex;
        pivot.DORotate(rotation, .25f).SetEase(Ease.InOutQuad);

        levelUIManager.Clear();

        chapter.levels.ForEach(level => levelUIManager.SpawnButton(level));

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