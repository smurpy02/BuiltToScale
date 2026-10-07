using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public InputActionReference toggleCanvas;
    public Toggle snapToGridToggle;
    public TMP_InputField levelNameInput;
    public GameObject levelInfoBlock, levelEditorCanvas, confirmOverwrite;
    public Transform levelInfoBlockContainer;
    public TextMeshProUGUI overwriteText;

    [Header("Generation")]
    public GenerateEditableLevel generateEditable;

    [Header("Values")]
    public bool snapToGrid;

    List<GameObject> levelInfoBlocks = new List<GameObject>();
    List<PuzzleComponentInstance> puzzleComponents = new List<PuzzleComponentInstance>();
    LevelData levelData;

    string overwriteLevelLocation, overwriteLevelName;

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

    void Update()
    {
        if (toggleCanvas.action.WasPressedThisFrame()) levelEditorCanvas.SetActive(!levelEditorCanvas.activeSelf);
    }

    public void AddPuzzleComponent(PuzzleComponentInstance puzzleComponent)
    {
        puzzleComponents.Add(puzzleComponent);
    }

    public void ClearComponents()
    {
        foreach (var component in puzzleComponents)
        {
            Destroy(component.transform.gameObject);
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

    public void Overwrite(int levelNumber)
    {
        var data = LevelMemoryManager.GetLevelData(levelNumber);

        overwriteLevelLocation = LevelMemoryManager.GetLevelLocation(levelNumber);
        overwriteLevelName = data.levelName;

        overwriteText.text = $"Are you sure you'd like to Overwrite {overwriteLevelName}?";
        confirmOverwrite.SetActive(true);
    }

    public void ConfirmOverwrite()
    {
        confirmOverwrite.SetActive(false);

        SaveLevelData(overwriteLevelLocation, overwriteLevelName);

        RefreshLevelList();
    }
    #endregion

    #region Save Data
    void SaveLevelData()
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

    public void SaveLevelToChapter()
    {
        SaveLevelData(LevelMemoryManager.GetLevelLocation(-1), levelNameInput.text);

        LevelMemoryManager.SaveLevelToPersistentPath(1, -1);
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
            levelData.puzzleComponents.Add(component.GetPuzzleComponent());
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

        infoBlockUI.Initiate(levelData, levelNumber, this);
    }
    #endregion
}

public class LevelSaveLocationData
{
    public List<int> SaveLocations;
}