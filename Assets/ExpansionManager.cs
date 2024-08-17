using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    void Expand(Vector2Int direction)
    {
        List<Block> newBlocks = new List<Block>();

        foreach(Block block in blocks.Values)
        {
            Vector2Int newPosition = block.position + direction;

            bool validPosition = true;

            validPosition &= !blocks.ContainsKey(newPosition);

            RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + newPosition, Vector2.one * 0.9f, 0, direction, 0f, 3);

            validPosition &= hit.collider == null;

            if(validPosition)
            {
                newBlocks.Add(CreateNewBlock(newPosition));
            }
        }

        foreach(Block block in newBlocks)
        {
            blocks.Add(block.position, block);
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

        Movement.groundChecks.Add(newBlock.transform.Find("GroundCheck"));

        return (newBlock);
    }
}