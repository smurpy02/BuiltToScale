using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GenerateLevelFromSave : MonoBehaviour
{
    public Player player;
    public Pattern pattern;

    public GridEditable grid;

    LevelData levelData;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        LoadLevelData();
        if (levelData == null) return;

        GenerateGrid();
        GeneratePlayer();
        GeneratePattern();
    }

    void LoadLevelData()
    {
        if (!PlayerPrefs.HasKey("TestLevelData"))
        {
            Debug.LogError("No Test Level Data");
            return;
        }

        string levelJson = PlayerPrefs.GetString("TestLevelData");

        levelData = JsonUtility.FromJson<LevelData>(levelJson);
    }

    void GenerateGrid()
    {
        foreach (Vector3Int position in levelData.addedTiles)
        {
            grid.PlaceTile(position);
        }

        foreach (Vector3Int position in levelData.removedTiles)
        {
            grid.RemoveTile(position);
        }
    }

    void GeneratePlayer()
    {
        if (levelData == null) return;

        player.transform.position = levelData.playerPosition;

        foreach (Vector2Int position in levelData.playerSquares)
        {
            player.engine.SpawnBlockPlayer(position);
        }
    }

    void GeneratePattern()
    {
        pattern.transform.position = levelData.patternPosition;

        foreach (Vector2Int position in levelData.patternSquares)
        {
            pattern.engine.SpawnBlockPlayer(position);
        }
    }

    public void GoToEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }
}
