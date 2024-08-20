using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResetTilemapCollider : MonoBehaviour
{
    public TilemapCollider2D col;

    private void Awake()
    {
        col.enabled = false;
        col.enabled = true;
    }
}
