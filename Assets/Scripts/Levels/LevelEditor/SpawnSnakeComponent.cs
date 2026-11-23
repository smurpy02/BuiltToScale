using UnityEngine;

public class SpawnSnakeComponent : SpawnComponent
{
    public override PuzzleComponentData GetPuzzleComponent() => new SnakeComponentData(prefabReference.id);
}