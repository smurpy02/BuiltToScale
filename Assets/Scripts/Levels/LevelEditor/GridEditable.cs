using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridEditable : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap tilemap;
    public TileBase square;

    [HideInInspector]
    public List<Vector3Int> addedTiles = new List<Vector3Int>(), removedTiles = new List<Vector3Int>();

    [Header("Settings")]
    public bool editable = false;

    bool placeMode = false;

    void Update()
    {
        if (!editable) return;

        UpdateTiles();
    }

    void UpdateTiles()
    {
        if (!Input.GetMouseButton(1)) return;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int position = tilemap.WorldToCell(worldPosition);

        if (Input.GetMouseButtonDown(1))
        {
            placeMode = tilemap.GetTile(position) == null;
        }

        if (placeMode) PlaceTile(position);
        else RemoveTile(position);
    }

    public void PlaceTile(Vector3Int position)
    {
        ChangeTile(position, square);

        if (removedTiles.Contains(position)) removedTiles.Remove(position);
        if(!addedTiles.Contains(position)) addedTiles.Add(position);
    }

    public void RemoveTile(Vector3Int position)
    {
        ChangeTile(position, null);

        if (addedTiles.Contains(position)) addedTiles.Remove(position);
        if (!removedTiles.Contains(position)) removedTiles.Add(position);
    }

    void ChangeTile(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
    }
}
