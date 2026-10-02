using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExpansionEngine))]
public class BlockBodyEditor : MonoBehaviour
{
    public static BlockBodyEditor spawnedLastBlock;

    Dictionary<Vector2Int, GameObject> plusBlocks = new Dictionary<Vector2Int, GameObject>();
    ExpansionEngine engine;
    int blocksSpawned;

    public GameObject plusBlock;

    void OnEnable()
    {
        engine = GetComponent<ExpansionEngine>();

        engine.SpawnBlock += (position, block) => SpawnAround(position, block);
        engine.SpawnBlockPlayer(Vector2Int.zero);
    }

    void OnDisable()
    {
        if (engine != null) engine.SpawnBlock -= (position, block) => SpawnAround(position, block);
    }

    public void SpawnNewBlock(Vector2Int plusBlockPosition)
    {
        spawnedLastBlock = this;
        engine.SpawnBlockPlayer(plusBlockPosition);
    }

    void SpawnAround(Vector2Int position, Transform block)
    {
        blocksSpawned++;

        var editorBlock = block.GetComponent<EditorBlock>();

        if (editorBlock != null)
        {
            editorBlock.editor = this;
            if (blocksSpawned == 1) editorBlock.RemoveCross();
        }

        if (plusBlocks.ContainsKey(position)) RemovePlusBlock(position);

        SpawnPlus(position + Vector2Int.up);
        SpawnPlus(position + Vector2Int.down);
        SpawnPlus(position + Vector2Int.left);
        SpawnPlus(position + Vector2Int.right);
    }

    void RemovePlusBlock(Vector2Int position)
    {
        Destroy(plusBlocks[position]);
        plusBlocks.Remove(position);
    }

    void SpawnPlus(Vector2Int newPosition)
    {
        if (plusBlocks.ContainsKey(newPosition)) return;
        if (engine.ContainsBlockPosition(newPosition)) return;

        GameObject newPlusBlock = Instantiate(plusBlock, engine.pivot);
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
