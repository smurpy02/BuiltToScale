using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager instance;

    [Header("Save Data")]
    public Transform player;
    public ExpansionEngine playerEngine;

    public Transform pattern;
    public ExpansionEngine patternEngine;

    public GridEditable grid;

    [Header("UI")]
    public Toggle snapToGridToggle;

    [Header("Values")]
    public bool snapToGrid;

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

    public void SaveLevelData()
    {
        LevelData levelData = new LevelData();

        levelData.playerPosition = player.position;
        levelData.playerSquares = playerEngine.GetPositions();

        levelData.patternPosition = pattern.position;
        levelData.patternSquares = patternEngine.GetPositions();

        levelData.addedTiles = grid.addedTiles;
        levelData.removedTiles = grid.removedTiles;

        string levelJson = JsonUtility.ToJson(levelData);

        PlayerPrefs.SetString("TestLevelData", levelJson);

        Debug.Log("[Level Editor Manager] Saved Level Data");
    }

    public void TestLevel()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void UpdateSnapToGrid()
    {
        snapToGrid = snapToGridToggle.isOn;
    }
}
