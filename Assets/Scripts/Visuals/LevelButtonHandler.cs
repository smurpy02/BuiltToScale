using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    LevelDetails level;

    public void Initiate(int levelNumber, LevelDetails level)
    {
        this.level = level;
        displayText.text = $"{level.chapter.chapterName}-{levelNumber}";
    }

    public void LoadLevel()
    {
        LevelLoader.currentChapter = level.chapter;
        LevelLoader.currentLevel = level;

        LevelLoader.LoadLevel(LevelMemoryManager.GetLevelData(level.json));
    }
}
