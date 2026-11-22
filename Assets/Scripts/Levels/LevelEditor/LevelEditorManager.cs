using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;
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
    public TMP_Dropdown chapterDropdown;
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

    int overwriteChapter, overwriteLevel;
    string overwriteLevelName;

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
        SetupChapterDropdown();
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

    public void DeleteLevel(int chapterNumber, int levelNumber)
    {
        var holder = LevelMemoryManager.GetChapterLevelHolder();

        if (holder.chapters.Count <= chapterNumber) return;
        if (holder.chapters[chapterNumber].levels.Count <= levelNumber) return;

        holder.chapters[chapterNumber].levels.RemoveAt(levelNumber);

        RefreshLevelList();
        SelectChapter(chapterNumber);
    }

    public void Overwrite(LevelData levelData, int chapter, int level)
    {
        overwriteLevelName = levelData.levelName;
        overwriteChapter = chapter;
        overwriteLevel = level;

        overwriteText.text = $"Are you sure you'd like to Overwrite {overwriteLevelName}?";
        confirmOverwrite.SetActive(true);
    }

    public void ConfirmOverwrite()
    {
        confirmOverwrite.SetActive(false);

        DeleteLevel(overwriteChapter, overwriteLevel);

        SaveLevelData(overwriteChapter, overwriteLevelName, overwriteLevel);

        RefreshLevelList();

        SelectChapter(overwriteChapter);
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

        int chapter = chapterDropdown.value;

        SaveLevelData(chapter, levelName);
        RefreshLevelList();
        SelectChapter(chapter);
    }

    void SaveLevelData(int chapter, string levelName, int insertLevel = -1)
    {
        levelData = new LevelData(levelName);

        SavePlayerAndPattern();
        SaveGridTiles();
        SavePuzzleComponents();

        string levelJson = JsonUtility.ToJson(levelData);

        var holder = LevelMemoryManager.GetChapterLevelHolder();
        var chapterDetails = holder.chapters[chapter];

        var levelDetails = new LevelDetails(chapterDetails, levelJson);

        if (insertLevel < 0) chapterDetails.levels.Add(levelDetails);
        else chapterDetails.levels.Insert(insertLevel, levelDetails);
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
        chapterDropdown.ClearOptions();

        var saveData = LevelMemoryManager.GetChapterLevelHolder();

        List<string> chapters = new List<string>();

        foreach(var chapter in saveData.chapters)
        {
            chapters.Add(chapter.chapterName);
        }

        chapterDropdown.AddOptions(chapters);
        chapterDropdown.value = 0;

        SelectChapter(0);
    }

    void SelectChapter(int chapter)
    {
        var saveData = LevelMemoryManager.GetChapterLevelHolder();

        levelInfoBlocks.ForEach(block => Destroy(block));
        levelInfoBlocks.Clear();

        var levels = saveData.chapters[chapter].levels;

        int levelNumber = 0;
        foreach(var level in levels)
        {
            SpawnLevelInfoBlock(level, chapter, levelNumber);
            levelNumber++;
        }
    }

    void SetupChapterDropdown()
    {
        chapterDropdown.onValueChanged.AddListener(SelectChapter);
    }
    void SpawnLevelInfoBlock(LevelDetails level, int chapterNumber, int levelNumber)
    {
        var levelData = LevelMemoryManager.GetLevelData(level.json);

        var infoBlock = Instantiate(levelInfoBlock, levelInfoBlockContainer);
        var infoBlockUI = infoBlock.GetComponent<LevelInfoBlock>();
        levelInfoBlocks.Add(infoBlock);

        if(infoBlockUI == null)
        {
            Debug.LogWarning("Level Info Block does not contain LevelInfoBlock component");
            return;
        }

        infoBlockUI.Initiate(levelData, this, chapterNumber, levelNumber);
    }
    #endregion
}

public class LevelSaveLocationData
{
    public List<int> SaveLocations;
}