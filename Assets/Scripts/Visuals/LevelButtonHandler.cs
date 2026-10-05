using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    string levelJson;

    public void Initiate(int displayNumber, string levelJson)
    {
        this.levelJson = levelJson;
        displayText.text = $"{displayNumber}";
    }

    public void LoadLevel()
    {
        LevelLoader.LoadLevel(LevelMemoryManager.GetLevelData(levelJson));
    }
}
