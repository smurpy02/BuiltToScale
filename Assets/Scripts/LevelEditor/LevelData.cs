using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData
{
    public Vector2 playerPosition;
    public List<Vector2Int> playerSquares;

    public Vector2 patternPosition;
    public List<Vector2Int> patternSquares;

    public List<Vector3Int> addedTiles;
    public List<Vector3Int> removedTiles;
}
