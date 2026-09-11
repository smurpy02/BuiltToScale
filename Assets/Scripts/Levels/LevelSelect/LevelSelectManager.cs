using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelButton;
    public Transform levelButtonsContainer;
    public TextMeshProUGUI levelNameText;

    int uiNumber = 0;

    void Start()
    {
        SpawnLevelButtons();
    }

    void SpawnLevelButtons()
    {
        var saveData = LevelMemoryManager.GetLocationData();

        if (saveData == null) return;

        saveData.SaveLocations.ForEach(levelNumber => SpawnLevelButton(levelNumber));
    }

    void SpawnLevelButton(int levelNumber)
    {
        var levelData = LevelMemoryManager.GetLevelData(levelNumber);

        if (levelData == null) return;

        var buttonObject = Instantiate(levelButton, levelButtonsContainer);
        var button = buttonObject.GetComponent<LevelSelectButton>();

        if(button == null)
        {
            Debug.LogWarning("Could not find LevelSelectButton component");
            return;
        }

        button.Initiate(this, levelData, ++uiNumber);
    }

    public void Hover(string levelName)
    {
        levelNameText.text = levelName;
    }

    public void OpenLevel(LevelData data)
    {
        LevelLoader.LoadLevel(data);
    }
}
