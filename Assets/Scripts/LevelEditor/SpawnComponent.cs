using Unity.VisualScripting;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    public GameObject puzzleComponent, editableComponent;

    public void SpawnPuzzleComponent()
    {
        Vector3 spawnPosition = LevelEditorManager.instance.spawnComponentsPosition.position;

        Transform newComponent = Instantiate(editableComponent, spawnPosition, Quaternion.identity).transform;

        LevelEditorManager.instance.AddPuzzleComponent(newComponent, puzzleComponent);
    }
}
