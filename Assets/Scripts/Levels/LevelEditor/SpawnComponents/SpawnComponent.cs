using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    public PuzzleComponentPrefabReference prefabReference;

    public void SpawnPuzzleComponent()
    {
        Vector3 spawnPosition = LevelEditorManager.instance.spawnComponentsPosition.position;

        Transform newComponent = Instantiate(prefabReference.editablePrefab, spawnPosition, Quaternion.identity).transform;

        LevelEditorManager.instance.AddPuzzleComponent(new PuzzleComponentInstance(GetPuzzleComponent(), newComponent));
    }

    public virtual PuzzleComponentData GetPuzzleComponent() => new PuzzleComponentData(prefabReference.id);
}