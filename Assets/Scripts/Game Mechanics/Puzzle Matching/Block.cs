using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Vector2Int position;
    public Transform transform;

    public Block(Transform transform, Vector2Int position)
    {
        this.position = position;
        this.transform = transform;
    }
}
