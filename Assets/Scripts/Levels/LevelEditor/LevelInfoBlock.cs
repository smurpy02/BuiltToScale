using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfoBlock : MonoBehaviour
{
    public TextMeshProUGUI levelName;

    LevelEditorManager manager;
    LevelData data;
    int levelNumber;

    public void Initiate(LevelData data, int levelNumber, LevelEditorManager manager)
    {
        this.data = data;
        this.levelNumber = levelNumber;
        this.manager = manager;

        levelName.text = data.levelName;
    }

    public void OpenLevel()
    {
        LevelLoader.LoadLevel(data);
    }

    public void EditLevel()
    {
        manager.generateEditable.GenerateLevelEditable(data);
    }

    public void DeleteLevel()
    {
        LevelEditorManager.instance.DeleteLevel(levelNumber);
    }

    public void Overwrite()
    {
        LevelEditorManager.instance.Overwrite(levelNumber);
    }
}
