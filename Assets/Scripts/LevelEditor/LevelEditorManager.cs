using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager instance;

    [Header("Spawn Components")]
    public Transform spawnComponentsPosition;

    [Header("Player and Pattern")]
    public Transform player;
    public ExpansionEngine playerEngine;

    public Transform pattern;
    public ExpansionEngine patternEngine;

    [Header("Grid Layout")]
    public GridEditable grid;

    [Header("UI")]
    public Toggle snapToGridToggle;

    [Header("Values")]
    public bool snapToGrid;

    List<(Transform, GameObject)> puzzleComponents = new List<(Transform, GameObject)>();
    LevelData levelData;

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        instance = null;
    }

    void Start()
    {
        UpdateSnapToGrid();
    }

    public void AddPuzzleComponent(Transform component, GameObject prefab)
    {
        puzzleComponents.Add((component, prefab));
    }

    public void ClearComponents()
    {
        foreach (var component in puzzleComponents)
        {
            Destroy(component.Item1.gameObject);
        }

        puzzleComponents.Clear();
    }

    public void TestLevel()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void UpdateSnapToGrid()
    {
        snapToGrid = snapToGridToggle.isOn;
    }

    #region Save Data
    public void SaveLevelData()
    {
        levelData = new LevelData();

        SavePlayerAndPattern();
        SaveGridTiles();
        SavePuzzleComponents();

        string levelJson = JsonUtility.ToJson(levelData);

        PlayerPrefs.SetString("TestLevelData", levelJson);

        Debug.Log("[Level Editor Manager] Saved Level Data");
    }

    void SavePlayerAndPattern()
    {
        levelData.playerPosition = player.position;
        levelData.playerSquares = playerEngine.GetPositions();

        levelData.patternPosition = pattern.position;
        levelData.patternSquares = patternEngine.GetPositions();
    }

    void SaveGridTiles()
    {
        levelData.addedTiles = grid.addedTiles;
        levelData.removedTiles = grid.removedTiles;
    }

    void SavePuzzleComponents()
    {
        levelData.puzzleComponents = new List<PuzzleComponentData>();

        foreach(var component in puzzleComponents)
        {
            levelData.puzzleComponents.Add(PuzzleComponentData.New(component.Item1.position, component.Item2));
        }
    }
    #endregion
}
