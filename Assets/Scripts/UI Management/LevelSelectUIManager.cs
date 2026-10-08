using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectUIManager : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject button;
    public ClockScrollUI clockScroll;

    List<GameObject> buttons = new();
    int levelNumber = 0, chapterNumber = 1;

    public void SpawnButton(LevelDetails level)
    {
        clockScroll.AddPoint();

        var newButton = Instantiate(button, spawnLocation.position, Quaternion.identity, clockScroll.pivot);

        buttons.Add(newButton);

        var buttonHandler = newButton.GetComponent<LevelButtonHandler>();

        if (buttonHandler != null) buttonHandler.Initiate(++levelNumber, chapterNumber, level);
    }

    public void SetChapterNumber(int chapterNumber) => this.chapterNumber = chapterNumber;

    public void Clear()
    {
        levelNumber = 0;

        buttons.ForEach(button =>
        {
            Destroy(button);
        });

        buttons.Clear();
        clockScroll.ResetRotation();
        clockScroll.ClearPoints();
    }
}
