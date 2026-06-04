using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExpansionEngine))]
public class BlockBodyEditor : MonoBehaviour
{
    public static BlockBodyEditor spawnedLastBlock;

    Dictionary<Vector2Int, GameObject> plusBlocks = new Dictionary<Vector2Int, GameObject>();
    ExpansionEngine engine;

    public GameObject plusBlock;

    void Start()
    {
        engine = GetComponent<ExpansionEngine>();

        engine.SpawnBlockPlayer(Vector2Int.zero);

        SpawnPlus(Vector2Int.up);
        SpawnPlus(Vector2Int.down);
        SpawnPlus(Vector2Int.left);
        SpawnPlus(Vector2Int.right);
    }

    public void SpawnNewBlock(Vector2Int plusBlockPosition)
    {
        if(plusBlocks.ContainsKey(plusBlockPosition)) plusBlocks.Remove(plusBlockPosition);

        spawnedLastBlock = this;
        engine.SpawnBlockPlayer(plusBlockPosition);

        SpawnPlus(plusBlockPosition + Vector2Int.up);
        SpawnPlus(plusBlockPosition + Vector2Int.down);
        SpawnPlus(plusBlockPosition + Vector2Int.left);
        SpawnPlus(plusBlockPosition + Vector2Int.right);
    }

    void SpawnPlus(Vector2Int newPosition)
    {
        if (plusBlocks.ContainsKey(newPosition)) return;
        if (engine.ContainsBlockPosition(newPosition)) return;

        GameObject newPlusBlock = Instantiate(plusBlock, engine.body);
        newPlusBlock.transform.localPosition = (Vector2)newPosition;

        plusBlocks.Add(newPosition, newPlusBlock);

        PlusBlock plusBlockComponent = newPlusBlock.GetComponent<PlusBlock>();

        if(plusBlockComponent != null)
        {
            plusBlockComponent.position = newPosition;
            plusBlockComponent.engine = this;
        }
    }

    void ValidatePlusBlock(Vector2Int position)
    {
        if(!plusBlocks.ContainsKey(position)) return;

        if (!HasBlockNeighbours(position))
        {
            Destroy(plusBlocks[position]);
            plusBlocks.Remove(position);
        }
    }

    bool HasBlockNeighbours(Vector2Int position)
    {
        bool valid = false;

        if (engine.ContainsBlockPosition(position + Vector2Int.up)) valid = true;
        if (engine.ContainsBlockPosition(position + Vector2Int.down)) valid = true;
        if (engine.ContainsBlockPosition(position + Vector2Int.left)) valid = true;
        if (engine.ContainsBlockPosition(position + Vector2Int.right)) valid = true;

        return valid;
    }

    public void RemoveBlock(Vector2Int position, Transform transform)
    {
        engine.Break(position, transform);

        ValidatePlusBlock(position + Vector2Int.up);
        ValidatePlusBlock(position + Vector2Int.down);
        ValidatePlusBlock(position + Vector2Int.left);
        ValidatePlusBlock(position + Vector2Int.right);

        if (HasBlockNeighbours(position))
        {
            SpawnPlus(position);
        }
    }
}
