using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectUIManager : MonoBehaviour
{
    public Transform pivot, spawnLocation;
    public GameObject button;

    List<GameObject> buttons = new();
    Vector3 rotation = Vector3.zero;
    int levelNumber = 0;

    const float rotateTheta = 17;

    public void SpawnButton(string levelJson)
    {
        if (levelNumber != 0) rotation.z += rotateTheta;
        pivot.rotation = Quaternion.Euler(rotation);

        var newButton = Instantiate(button, spawnLocation.position, Quaternion.identity, pivot);

        buttons.Add(newButton);

        var buttonHandler = newButton.GetComponent<LevelButtonHandler>();

        if (buttonHandler != null) buttonHandler.Initiate(++levelNumber, levelJson);
    }

    public void ResetRotation()
    {
        rotation.z = 0;
        pivot.rotation = Quaternion.Euler(rotation);
    }

    public void Clear()
    {
        levelNumber = 0;

        buttons.ForEach(button =>
        {
            Destroy(button);
        });

        buttons.Clear();
        ResetRotation();
    }

    public void ShiftLeft()
    {
        rotation.z -= 17;
        RotatePivot();
    }

    public void ShiftRight()
    {
        rotation.z += 17;
        RotatePivot();
    }

    void RotatePivot()
    {
        pivot.DORotate(rotation, .35f).SetEase(Ease.InOutQuad);
    }
}
