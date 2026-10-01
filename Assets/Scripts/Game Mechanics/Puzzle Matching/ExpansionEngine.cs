using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpansionEngine : MonoBehaviour
{
    public Transform body, pivot;
    public GameObject blockObject, breakBlock;
    public LayerMask expansionMask;

    Dictionary<Vector2Int, Block> blocks = new Dictionary<Vector2Int, Block>();
    Block highestBlock;

    Block CreateNewBlock(Vector2Int position)
    {
        Transform transform = Instantiate(blockObject, pivot).transform;
        transform.localPosition = (Vector2)position;

        HandleNewBlock(transform);

        Block block = Block.Create(transform, position);

        if (highestBlock == null) highestBlock = block;
        else if (position.y > highestBlock.position.y) highestBlock = block;

        return block;
    }

    #region Virtual Functions
    protected virtual void HandleNewBlock(Transform blockTransform) { }
    protected virtual void HandleRemovedBlock(Transform blockTransform) { }
    #endregion

    #region Modify Body
    void OffsetPivot(Vector2 direction)
    {
        pivot.parent = null;

        body.position += (Vector3)(direction / 2);

        pivot.parent = body;
    }

    void ProcessNewBlock(Block block, Block newBlock)
    {
        var blockPhysics = block.transform.GetComponent<BlockPhysics>();

        if (blockPhysics != null) blockPhysics.BlockAdded(block.position, newBlock.position);

        var newBlockPhysics = newBlock.transform.GetComponent<BlockPhysics>();

        if (newBlockPhysics != null) newBlockPhysics.BlockAdded(newBlock.position, block.position);
    }

    //Spawn block relative to player
    public void SpawnBlockPlayer(Vector2Int position) // INPUT: Position relative to Player
    {
        if (blocks.ContainsKey(position)) return;

        var newBlock = CreateNewBlock(position);

        blocks.Add(position, newBlock);

        bool blockIncrementsX = true, blockIncrementsY = true, blockDecrementsX = true, blockDecrementsY = true;

        foreach (var otherBlock in blocks.Values)
        {
            ProcessNewBlock(otherBlock, newBlock);

            if (otherBlock != newBlock)
            {
                if (newBlock.position.x <= otherBlock.position.x) blockIncrementsX = false;
                if (newBlock.position.y <= otherBlock.position.y) blockIncrementsY = false;

                if (newBlock.position.x >= otherBlock.position.x) blockDecrementsX = false;
                if (newBlock.position.y >= otherBlock.position.y) blockDecrementsY = false;
            }
        }

        if (blockIncrementsX) OffsetPivot(Vector2Int.right);
        if (blockIncrementsY) OffsetPivot(Vector2Int.up);
        if (blockDecrementsX) OffsetPivot(Vector2Int.left);
        if (blockDecrementsY) OffsetPivot(Vector2Int.down);
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
        HandleRemovedBlock(blockTransform);

        bool blockIncrementsX = true, blockIncrementsY = true, blockDecrementsX = true, blockDecrementsY = true;

        foreach (var otherBlock in blocks.Values)
        {
            var physics = otherBlock.transform.GetComponent<BlockPhysics>();

            if (physics != null) physics.BlockRemoved(otherBlock.position, position);

            if (otherBlock != block)
            {
                if (block.position.x <= otherBlock.position.x) blockIncrementsX = false;
                if (block.position.y <= otherBlock.position.y) blockIncrementsY = false;

                if (block.position.x >= otherBlock.position.x) blockDecrementsX = false;
                if (block.position.y >= otherBlock.position.y) blockDecrementsY = false;
            }
        }

        if (block == highestBlock) ReconfigureHighestBlock();

        if (blockIncrementsX) OffsetPivot(Vector2Int.left);
        if (blockIncrementsY) OffsetPivot(Vector2Int.down);
        if (blockDecrementsX) OffsetPivot(Vector2Int.right);
        if (blockDecrementsY) OffsetPivot(Vector2Int.up);
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

            RaycastHit2D hit = Physics2D.BoxCast((Vector2)pivot.position + newPosition, Vector2.one * 0.8f, 0, direction, 0f, expansionMask);

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

    #region External Functions
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
    #endregion
}