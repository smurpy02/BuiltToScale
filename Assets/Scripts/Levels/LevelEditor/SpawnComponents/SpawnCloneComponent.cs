using UnityEngine;

public class SpawnCloneComponent : SpawnComponent
{
    public override PuzzleComponentData GetPuzzleComponent() => new CloneComponentData(prefabReference.id);
}