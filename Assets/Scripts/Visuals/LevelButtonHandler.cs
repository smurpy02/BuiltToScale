using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    LevelDetails level;

    public void Initiate(int levelNumber, int chapterNumber, LevelDetails level)
    {
        this.level = level;
        displayText.text = $"{chapterNumber}.{levelNumber}";
    }

    public void LoadLevel()
    {
        LevelLoader.currentChapter = level.chapter;
        LevelLoader.currentLevel = level;

        LevelLoader.LoadLevel(LevelMemoryManager.GetLevelData(level.json));
    }
}
