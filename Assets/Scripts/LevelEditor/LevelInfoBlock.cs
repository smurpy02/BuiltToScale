using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfoBlock : MonoBehaviour
{
    public TextMeshProUGUI levelName;

    LevelData data;
    string levelLocation;
    int levelNumber;

    public void Initiate(LevelData data, string levelLocation, int levelNumber)
    {
        this.data = data;
        this.levelLocation = levelLocation;
        this.levelNumber = levelNumber;

        levelName.text = data.levelName;
    }

    public void OpenLevel()
    {
        GenerateLevelFromSave.levelToLoadLocation = levelLocation;
        LevelEditorManager.instance.OpenGameLevelScene();
    }

    public void DeleteLevel()
    {
        PlayerPrefs.DeleteKey(levelLocation);
        LevelEditorManager.instance.DeleteLevel(levelNumber);
    }
}
