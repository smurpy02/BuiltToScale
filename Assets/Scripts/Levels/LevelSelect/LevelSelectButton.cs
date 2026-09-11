using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI uiNumberText;
    public GameObject complete;

    LevelSelectManager levelManager;
    LevelData levelData;

    public void Initiate(LevelSelectManager levelManager, LevelData levelData, int uiNumber)
    {
        this.levelManager = levelManager;
        this.levelData = levelData;

        uiNumberText.text = $"{uiNumber}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        levelManager.Hover(levelData.levelName);
    }

    public void Open()
    {
        levelManager.OpenLevel(levelData);
    }
}
