using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
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
    public TMP_InputField levelNameInput;

    [Header("Values")]
    public bool snapToGrid;

    const string levelNameListLocation = "ListOfLevelNames", levelNamePrefix = "CustomLevel_";

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

        if (!PlayerPrefs.HasKey(levelNameListLocation)) return;
        var jsonList = PlayerPrefs.GetString(levelNameListLocation);
        var saveData = JsonUtility.FromJson<LevelSaveLocationData>(jsonList);

        Debug.Log(saveData.SaveLocations.Count);

        saveData.SaveLocations.ForEach(level => Debug.Log($"{levelNamePrefix}{level}"));
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
        string levelName = levelNameInput.text;

        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Level name empty");
            return;
        }

        RegisterLevel(levelName);
    }

    void RegisterLevel(string levelName)
    {
        if (!PlayerPrefs.HasKey(levelNameListLocation))
        {
            InitLevelList();
        }

        var jsonList = PlayerPrefs.GetString(levelNameListLocation);
        var levelSaveData = JsonUtility.FromJson<LevelSaveLocationData>(jsonList.ToString());

        var levelNumber = 0;
        while (levelSaveData.SaveLocations.Contains(levelNumber)) levelNumber++;
        levelSaveData.SaveLocations.Add(levelNumber);

        string levelLocation = levelNamePrefix + levelNumber;
        var newJsonList = JsonUtility.ToJson(levelSaveData);

        PlayerPrefs.SetString(levelNameListLocation, newJsonList.ToString());

        SaveLevelData(levelLocation, levelName);
    }

    public void ClearLevelData()
    {
        InitLevelList();
    }

    void InitLevelList()
    {
        var levelList = new LevelSaveLocationData();
        var jsonList = JsonUtility.ToJson(levelList);

        PlayerPrefs.SetString(levelNameListLocation, jsonList.ToString());
    }

    void SaveLevelData(string location, string levelName)
    {
        levelData = new LevelData(levelName);

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

public class LevelSaveLocationData
{
    public List<int> SaveLocations;
}