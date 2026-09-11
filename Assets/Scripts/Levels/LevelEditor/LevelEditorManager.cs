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
    public GameObject levelInfoBlock;
    public Transform levelInfoBlockContainer;

    [Header("Values")]
    public bool snapToGrid;

    List<GameObject> levelInfoBlocks = new List<GameObject>();
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
        RefreshLevelList();
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

    public void UpdateSnapToGrid()
    {
        snapToGrid = snapToGridToggle.isOn;
    }

    #region Edit Data
    public void ClearLevelData()
    {
        LevelMemoryManager.InitLevelList();
        RefreshLevelList();
    }

    public void DeleteLevel(int levelNumber)
    {
        var levelSaveData = LevelMemoryManager.GetLocationData();

        if (!levelSaveData.SaveLocations.Contains(levelNumber)) return;

        levelSaveData.SaveLocations.Remove(levelNumber);

        LevelMemoryManager.SetLocationData(levelSaveData);
        LevelMemoryManager.DeleteLevel(levelNumber);

        RefreshLevelList();
    }
    #endregion

    #region Save Data
    public void SaveLevelData()
    {
        string levelName = levelNameInput.text;

        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogWarning("Level name empty");
            return;
        }

        var levelLocation = LevelMemoryManager.RegisterLevel(levelName);

        SaveLevelData(levelLocation, levelName);
        RefreshLevelList();
    }

    void SaveLevelData(string location, string levelName)
    {
        levelData = new LevelData(levelName);

        SavePlayerAndPattern();
        SaveGridTiles();
        SavePuzzleComponents();

        string levelJson = JsonUtility.ToJson(levelData);

        PlayerPrefs.SetString(location, levelJson);
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

        foreach (var component in puzzleComponents)
        {
            levelData.puzzleComponents.Add(PuzzleComponentData.New(component.Item1.position, component.Item2));
        }
    }
    #endregion

    #region Load Data
    void RefreshLevelList()
    {
        levelInfoBlocks.ForEach(block => Destroy(block));
        levelInfoBlocks.Clear();

        var saveData = LevelMemoryManager.GetLocationData();
        if (saveData == null) return;

        saveData.SaveLocations.ForEach(levelNumber => SpawnLevelInfoBlock(levelNumber));
    }

    void SpawnLevelInfoBlock(int levelNumber)
    {
        var levelData = LevelMemoryManager.GetLevelData(levelNumber);

        var infoBlock = Instantiate(levelInfoBlock, levelInfoBlockContainer);
        var infoBlockUI = infoBlock.GetComponent<LevelInfoBlock>();
        levelInfoBlocks.Add(infoBlock);

        if(infoBlockUI == null)
        {
            Debug.LogWarning("Level Info Block does not contain LevelInfoBlock component");
            return;
        }

        infoBlockUI.Initiate(levelData, levelNumber);
    }
    #endregion
}

public class LevelSaveLocationData
{
    public List<int> SaveLocations;
}