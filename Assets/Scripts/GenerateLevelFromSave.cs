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

    public static LevelData levelData;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if (levelData == null) return;

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
        foreach(var component in levelData.puzzleComponents)
        {
            var componentObject = Instantiate(component.prefab, component.position, Quaternion.identity);

            if(component is CloneComponentData) GenerateClone(componentObject, component as CloneComponentData);
        }
    }

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

        if(matcher == null)
        {
            Debug.Log("Couldn't find Clone's pattern matcher");
            return;
        }

        ConfigureExpansionEngine(matcher.player.transform, data.clonePosition, data.cloneSquares, matcher.player.engine);
        ConfigureExpansionEngine(matcher.pattern.transform, data.clonePatternPosition, data.clonePatternSquares, matcher.pattern.engine);
    }
}