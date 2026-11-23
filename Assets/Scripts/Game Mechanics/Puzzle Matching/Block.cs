using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Block
{
    public Vector2Int position;
    public Transform transform;
    public BlockState state;

    public Block(Transform transform, Vector2Int position)
    {
        this.position = position;
        this.transform = transform;

        state = BlockState.Basic;
    }
}

public enum BlockState
{
    Basic,
    Petrified,
    Gold
}