using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class ExpansionManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Block> blocks = new Dictionary<Vector2Int, Block>();

    public Transform body;
    public Transform sourceBlock;

    public InputActionReference up;
    public InputActionReference down;
    public InputActionReference left;
    public InputActionReference right;

    public GameObject blockObject;

    public PolygonCollider2D polygonCollider;

    public AudioSource pop;

    public GameObject breakBlock;

    public List<Vector2Int> initialExpansions;

    public bool invert = false;

    public LayerMask expansionMask;

    static Vector2[] defaultPoints = new Vector2[]
    {
        new Vector2(0.45f, 0.45f),
        new Vector2(-0.45f, 0.45f),
        new Vector2(-0.45f, -0.45f),
        new Vector2(0.45f, -0.45f)
    };

    private void Start()
    {
        Block source = new Block();

        source.transform = sourceBlock;
        source.position = Vector2Int.zero;

        Movement.groundChecks.Add(source.transform.Find("GroundCheck"));

        blocks.Add(source.position, source);

        foreach(Vector2Int expansion in initialExpansions)
        {
            Expand(expansion, false);
        }
    }

    private void Update()
    {
        if (up.action.WasPressedThisFrame())
        {
            Expand(Vector2Int.up);
        }

        if (down.action.WasPressedThisFrame())
        {
            Expand(Vector2Int.down);
        }

        if (left.action.WasPressedThisFrame())
        {
            Expand(Vector2Int.left);
        }

        if (right.action.WasPressedThisFrame())
        {
            Expand(Vector2Int.right);
        }
    }

    void Expand(Vector2Int direction, bool playSound = true)
    {
        if (invert) direction.x *= -1;

        List<Block> newBlocks = new List<Block>();

        foreach(Block block in blocks.Values)
        {
            Vector2Int newPosition = block.position + direction;

            bool validPosition = true;

            validPosition &= !blocks.ContainsKey(newPosition);

            RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + newPosition, Vector2.one * 0.8f, 0, direction, 0f, expansionMask);

            validPosition &= hit.collider == null;

            if(hit.collider != null)
            {
                Debug.Log(hit.collider);
            }

            if(validPosition)
            {
                newBlocks.Add(CreateNewBlock(newPosition));
            }
        }

        foreach(Block block in newBlocks)
        {
            Debug.Log("new block " + block.position);
            blocks.Add(block.position, block);
        }

        if(newBlocks.Count > 0 && playSound)
        {
            pop.Play();
        }
    }

    Block CreateNewBlock(Vector2Int position)
    {
        Debug.Log(position);
        Block newBlock = new Block();
        newBlock.transform = Instantiate(blockObject, body).transform;
        newBlock.transform.localPosition = (Vector2)position;
        newBlock.position = position;

        Vector2[] colliderPoints = new Vector2[4];
        defaultPoints.CopyTo(colliderPoints, 0);

        for (int point = 0; point < colliderPoints.Length; point++)
        {
            colliderPoints[point] += position;
        }

        polygonCollider.pathCount++;
        polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPoints);
        newBlock.colliderIndex = polygonCollider.pathCount - 1;

        Movement.groundChecks.Add(newBlock.transform.Find("GroundCheck"));

        return (newBlock);
    }

    public void Break(Vector2Int position, Transform blockTransform)
    {
        Debug.Log("break " + blockTransform.name + " in position " + position);
        Block block = blocks[position];
        Debug.Log("block position " + block.position);
        Debug.Log("block collider index " + block.colliderIndex);

        for(int index = block.colliderIndex; index < polygonCollider.pathCount-1; index++)
        {
            polygonCollider.SetPath(index, polygonCollider.GetPath(index+1));
        }

        Debug.Log("paths reset");

        foreach (Block currentBlock in blocks.Values)
        {
            if(currentBlock.colliderIndex > block.colliderIndex)
            {
                currentBlock.colliderIndex--;
            }
        }

        Debug.Log("path indexes updated");

        polygonCollider.pathCount--;

        Debug.Log("lower polygon count " + polygonCollider.pathCount);

        Instantiate(breakBlock, blockTransform.position, Quaternion.identity).GetComponentInChildren<Renderer>().material.color = blockTransform.GetComponentInChildren<Renderer>().material.color;

        blocks.Remove(position);
    }

    public void UpdateAfterSpin()
    {
        List<Vector2Int> blockPositions = blocks.Keys.ToList();
        List<Block> newBlocks = new List<Block>();

        foreach(Vector2Int blockPosition in blockPositions)
        {
            Block block = blocks[blockPosition];
            block.position = Vector2Int.RoundToInt(block.transform.localPosition);
            blocks.Remove(blockPosition);
            newBlocks.Add(block);

            Vector2[] colliderPoints = new Vector2[4];
            defaultPoints.CopyTo(colliderPoints, 0);

            for (int point = 0; point < colliderPoints.Length; point++)
            {
                colliderPoints[point] += block.position;
            }

            polygonCollider.SetPath(block.colliderIndex, colliderPoints);
        }

        foreach(Block block in newBlocks)
        {
            blocks.Add(block.position, block);
        }
    }
}