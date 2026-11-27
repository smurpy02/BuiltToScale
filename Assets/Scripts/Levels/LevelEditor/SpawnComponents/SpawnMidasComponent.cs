using System.Collections.Generic;
using UnityEngine;

public class SpawnMidasComponent : SpawnComponent
{
    public Pattern pattern;

    public override PuzzleComponentData GetPuzzleComponent()
    {
        List<Vector2Int> goldBlocks = new();

        foreach(Transform block in pattern.body)
        {
            var editorPattern = block.GetComponent<EditorPatternBlock>();

            if (editorPattern == null) continue;

            if (editorPattern.state == BlockState.Gold) goldBlocks.Add(Vector2Int.RoundToInt(block.localPosition));
        }

        return new MidasComponentData(prefabReference.id, goldBlocks);
    }
}
