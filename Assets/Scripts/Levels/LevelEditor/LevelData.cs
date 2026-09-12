using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData
{
    public LevelData(string levelName) => this.levelName = levelName;

    // General Info
    public string levelName;

    // Player Info
    public Vector2 playerPosition;
    public List<Vector2Int> playerSquares;

    public Vector2 patternPosition;
    public List<Vector2Int> patternSquares;

    // Grid Info
    public List<Vector3Int> addedTiles;
    public List<Vector3Int> removedTiles;

    // Puzzle Components
    [SerializeReference]
    public List<PuzzleComponentData> puzzleComponents;
}

[Serializable]
public class CloneComponentData : PuzzleComponentData
{
    public Vector2 clonePosition;
    public List<Vector2Int> cloneSquares;

    public Vector2 clonePatternPosition;
    public List<Vector2Int> clonePatternSquares;

    public CloneComponentData(GameObject prefab) : base(prefab) { }
}

[Serializable]
public class PuzzleComponentData
{
    public Vector2 position;
    public GameObject prefab;

    public PuzzleComponentData(GameObject prefab)
    {
        this.prefab = prefab;
    }
}