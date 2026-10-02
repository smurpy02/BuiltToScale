using UnityEngine;

public class SpawnTextComponent : SpawnComponent
{
    public override PuzzleComponentData GetPuzzleComponent() => new TextComponentData(prefabReference.id, "text");
}
