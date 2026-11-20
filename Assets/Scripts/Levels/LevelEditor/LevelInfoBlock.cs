using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfoBlock : MonoBehaviour
{
    public TextMeshProUGUI levelName;

    LevelEditorManager manager;
    LevelData data;
    int chapter, level;

    public void Initiate(LevelData data, LevelEditorManager manager, int chapter, int level)
    {
        this.data = data;
        this.manager = manager;
        this.chapter = chapter;
        this.level = level;

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
        manager.DeleteLevel(chapter, level);
    }

    public void Overwrite()
    {
        manager.Overwrite(data, chapter, level);
    }
}
