using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "new chapter", menuName = "Chapter Details")]
public class ChapterDetails : ScriptableObject
{
    public string chapterName;

    public List<LevelDetails> levels = new List<LevelDetails>();
}
