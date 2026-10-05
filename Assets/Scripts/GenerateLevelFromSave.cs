using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateLevelFromSave : MonoBehaviour
{
    public Player player;
    public Pattern pattern;

    public GridEditable grid;

    public ListOfComponentReferences components;

    public static LevelData levelData;

    protected virtual void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is Null. Grabbing Default");
            levelData = LevelMemoryManager.GetLevelData(0);
            if (levelData == null) return;
        }

        GenerateGrid();
        GeneratePlayer();
        GeneratePattern();
        GeneratePuzzleComponents();
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
        ConfigureExpansionEngine(player.transform, levelData.playerPosition, levelData.playerSquares, player.engine);
    }

    void GeneratePattern()
    {
        ConfigureExpansionEngine(pattern.transform, levelData.patternPosition, levelData.patternSquares, pattern.engine);
    }

    void GeneratePuzzleComponents()
    {
        foreach (var component in levelData.puzzleComponents)
        {
            var prefab = GetPrefabFromID(component.id);

            if (prefab.IsUnityNull())
            {
                Debug.LogWarning($"Puzzle Component with ID {component.id} cannot be found or is null");
                return;
            }

            var componentObject = Instantiate(prefab, component.position, Quaternion.identity);

            OnComponentSpawned(component, componentObject);

            if (component is CloneComponentData) GenerateClone(componentObject, component as CloneComponentData);
            if (component is TextComponentData) GenerateText(componentObject, component as TextComponentData);
        }
    }

    protected virtual void OnComponentSpawned(PuzzleComponentData component, GameObject componenetObject) { }

    GameObject GetPrefabFromID(int id)
    {
        foreach (var prefab in components.components) if (prefab.id == id) return GetPrefabFromReference(prefab);

        return null;
    }

    protected virtual GameObject GetPrefabFromReference(PuzzleComponentPrefabReference prefab) => prefab.prefab;

    public void GoToEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    void ConfigureExpansionEngine(Transform target, Vector3 position, List<Vector2Int> squares, ExpansionEngine engine)
    {
        target.position = position;

        squares.ForEach(squarePosition => engine.SpawnBlockPlayer(squarePosition));
    }

    void GenerateClone(GameObject cloneInstance, CloneComponentData data)
    {
        var matcher = cloneInstance.GetComponent<PatternMatcher>();

        if (matcher == null)
        {
            Debug.LogWarning("Couldn't find Clone's pattern matcher");
            return;
        }

        ConfigureExpansionEngine(matcher.player.transform, data.clonePosition, data.cloneSquares, matcher.player.engine);
        ConfigureExpansionEngine(matcher.pattern.transform, data.clonePatternPosition, data.clonePatternSquares, matcher.pattern.engine);
    }

    void GenerateText(GameObject textInstance, TextComponentData textComponentData)
    {
        var text = textInstance.GetComponentInChildren<TextMeshProUGUI>();

        if (text == null)
        {
            Debug.LogWarning("Couldn't find Text Component");
            return;
        }

        text.text = textComponentData.text;
    }
}