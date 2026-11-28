using System.Collections.Generic;
using UnityEngine;

public class SpawnMidasComponent : SpawnComponent
{
    public override PuzzleComponentData GetPuzzleComponent()
    {
        return new MidasComponentData(prefabReference.id);
    }
}
