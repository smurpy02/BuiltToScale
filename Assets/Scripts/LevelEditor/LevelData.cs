using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData
{
    // Player Info
    public Vector2 playerPosition;
    public List<Vector2Int> playerSquares;

    public Vector2 patternPosition;
    public List<Vector2Int> patternSquares;

    // Grid Info
    public List<Vector3Int> addedTiles;
    public List<Vector3Int> removedTiles;

    // Puzzle Components
    public List<PuzzleComponentData> puzzleComponents;
}

[Serializable]
public struct CloneComponentData
{
    public GameObject prefab;

    public Vector2 clonePosition;
    public List<Vector2Int> cloneSquares;

    public Vector2 clonePatternPosition;
    public List<Vector2Int> clonePatternSquares;
}

[Serializable]
public struct PuzzleComponentData
{
    public Vector2 position;
    public GameObject prefab;

    public static PuzzleComponentData New(Vector2 position, GameObject prefab)
    {
        PuzzleComponentData data = new PuzzleComponentData();

        data.position = position;
        data.prefab = prefab;

        return data;
    }
}