using UnityEngine;

public class SpawnCloneComponent : SpawnComponent
{
    public override PuzzleComponentData GetPuzzleComponent()
    {
        return new CloneComponentData(puzzleComponent);
    }
}
