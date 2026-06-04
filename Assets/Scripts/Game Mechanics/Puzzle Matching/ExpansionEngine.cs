using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;

public class ExpansionEngine : MonoBehaviour
{
    public Transform body;
    public GameObject blockObject, breakBlock;
    public LayerMask expansionMask;

    Dictionary<Vector2Int, Block> blocks = new Dictionary<Vector2Int, Block>();
    Block highestBlock;

    Block CreateNewBlock(Vector2Int position)
    {
        Transform transform = Instantiate(blockObject, body).transform;
        transform.localPosition = (Vector2)position;
        Movement.groundChecks.Add(transform.Find("GroundCheck"));

        Block block = Block.Create(transform, position);

        if (highestBlock == null) highestBlock = block;
        else if (position.y > highestBlock.position.y) highestBlock = block;

        return block;
    }

    #region Modify Body
    //Spawn block relative to player
    public void SpawnBlockPlayer(Vector2Int position) // INPUT: Position relative to Player
    {
        if (blocks.ContainsKey(position)) return;

        blocks.Add(position, CreateNewBlock(position));
    }

    //Spawn block relative to world
    public void SpawnBlockGlobal(Vector2 position) // INPUT: World Position
    {
        SpawnBlockPlayer(Vector2Int.RoundToInt(position - (Vector2)transform.position));
    }

    //Break block relative to player
    public void Break(Vector2Int position, Transform blockTransform)
    {
        Block block = blocks[position];

        Instantiate(breakBlock, blockTransform.position, Quaternion.identity).GetComponentInChildren<Renderer>().material.color = blockTransform.GetComponentInChildren<Renderer>().material.color;

        blocks.Remove(position);

        if (block == highestBlock) ReconfigureHighestBlock();
    }

    public void ReconfigBlockPositions()
    {
        List<Vector2Int> blockPositions = blocks.Keys.ToList();

        foreach (Vector2Int blockPosition in blockPositions)
        {
            blocks[blockPosition].position = Vector2Int.RoundToInt(blocks[blockPosition].transform.localPosition);
        }

        ReconfigureHighestBlock();
    }

    public void Expand(Vector2Int direction)
    {
        Block[] blockCopy = new Block[blocks.Count];
        blocks.Values.CopyTo(blockCopy, 0);

        bool expansionSuccessful = false;

        foreach (Block block in blockCopy)
        {
            Vector2Int newPosition = block.position + direction;

            if (blocks.ContainsKey(newPosition)) continue;

            RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + newPosition, Vector2.one * 0.8f, 0, direction, 0f, expansionMask);

            if (hit.collider != null)
            {
                TopHat topHat = hit.transform.GetComponentInChildren<TopHat>();
                if (topHat != null) topHat.ExpandIntoTophat(this);

                continue;
            }

            expansionSuccessful = true;
            SpawnBlockPlayer(newPosition);
        }

        if (expansionSuccessful) PlayerAudioManager.Pop();
    }

    void ReconfigureHighestBlock()
    {
        highestBlock = null;

        foreach (Block currentBlock in blocks.Values)
        {
            if (highestBlock == null) highestBlock = currentBlock;
            else if (currentBlock.position.y > highestBlock.position.y) highestBlock = currentBlock;
        }
    }
    #endregion

    public Vector2 GetHighestBlock()
    {
        return highestBlock.position;
    }

    public bool ContainsBlockPosition(Vector2Int position)
    {
        return blocks.ContainsKey(position);
    }

    public List<Vector2Int> GetPositions()
    {
        return blocks.Keys.ToList();
    }
}