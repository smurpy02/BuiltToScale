using DG.Tweening;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectUIManager : MonoBehaviour
{
    public Transform pivot, spawnLocation;
    public GameObject button;

    Vector3 rotation = Vector3.zero;

    void Start()
    {
        SpawnButtons();
    }

    void SpawnButtons()
    {
        var levels = LevelMemoryManager.GetSavedPersistentLevels();

        levels.Reverse();

        int levelNumber = levels.Count;

        levels.ForEach(level =>
        {
            rotation.z -= 17;
            pivot.rotation = Quaternion.Euler(rotation);
            var newButton = Instantiate(button, spawnLocation.position, Quaternion.identity, pivot);

            var buttonHandler = newButton.GetComponent<LevelButtonHandler>();

            if (buttonHandler != null) buttonHandler.Initiate(levelNumber--);
        });
    }

    public void ShiftLeft()
    {
        rotation.z -= 17;
        pivot.DORotate(rotation, .35f).SetEase(Ease.InOutQuad);
    }

    public void ShiftRight()
    {
        rotation.z += 17;
        pivot.DORotate(rotation, .35f).SetEase(Ease.InOutQuad);
    }
}
