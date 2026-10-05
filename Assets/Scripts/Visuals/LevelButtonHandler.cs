using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;

    int levelNumber;

    public void Initiate(int levelNumber)
    {
        this.levelNumber = levelNumber;
        levelNumberText.text = $"{levelNumber}";
    }

    public void LoadLevel()
    {
        LevelLoader.LoadLevel(LevelMemoryManager.GetLevelData(levelNumber));
    }
}
