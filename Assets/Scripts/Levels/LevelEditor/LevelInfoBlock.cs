using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfoBlock : MonoBehaviour
{
    public TextMeshProUGUI levelName;

    LevelData data;
    int levelNumber;

    public void Initiate(LevelData data, int levelNumber)
    {
        this.data = data;
        this.levelNumber = levelNumber;

        levelName.text = data.levelName;
    }

    public void OpenLevel()
    {
        LevelLoader.LoadLevel(data);
    }

    public void DeleteLevel()
    {
        LevelEditorManager.instance.DeleteLevel(levelNumber);
    }
}
