using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Vector2Int position;
    public Transform transform;

    public static Block Create(Transform transform, Vector2Int position)
    {
        Block newBlock = new Block();
        newBlock.position = position;
        newBlock.transform = transform;

        return newBlock;
    }
}
