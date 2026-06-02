using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorExpansionEngine : ExpansionEngine
{
    List<Vector2Int> plusBlocks = new List<Vector2Int>();

    public GameObject plusBlock;

    void Awake()
    {
        SpawnPlus(Vector2Int.zero, Vector2Int.up);
        SpawnPlus(Vector2Int.zero, Vector2Int.down);
        SpawnPlus(Vector2Int.zero, Vector2Int.left);
        SpawnPlus(Vector2Int.zero, Vector2Int.right);
    }

    public void SpawnNewBlock(Vector2Int plusBlockPosition)
    {
        if(plusBlocks.Contains(plusBlockPosition)) plusBlocks.Remove(plusBlockPosition);

        SpawnBlockPlayer(plusBlockPosition);

        SpawnPlus(plusBlockPosition, Vector2Int.up);
        SpawnPlus(plusBlockPosition, Vector2Int.down);
        SpawnPlus(plusBlockPosition, Vector2Int.left);
        SpawnPlus(plusBlockPosition, Vector2Int.right);
    }

    void SpawnPlus(Vector2Int origin, Vector2Int direction)
    {
        Vector2Int newPosition = origin + direction;

        if (plusBlocks.Contains(newPosition)) return;
        if (ContainsBlockPosition(newPosition)) return;

        plusBlocks.Add(newPosition);

        Transform newPlusBlock = Instantiate(plusBlock, body).transform;
        newPlusBlock.localPosition = (Vector2)newPosition;

        PlusBlock plusBlockComponent = newPlusBlock.GetComponent<PlusBlock>();

        if(plusBlockComponent != null)
        {
            plusBlockComponent.position = newPosition;
            plusBlockComponent.engine = this;
        }
    }
}
