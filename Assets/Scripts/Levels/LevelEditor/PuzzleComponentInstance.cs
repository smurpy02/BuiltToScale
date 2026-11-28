using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PuzzleComponentInstance
{
    public Transform transform;

    protected PuzzleComponentData puzzleComponent;

    public PuzzleComponentInstance(PuzzleComponentData puzzleComponent, Transform transform)
    {
        this.puzzleComponent = puzzleComponent;
        this.transform = transform;
    }

    public PuzzleComponentData GetPuzzleComponent()
    {
        puzzleComponent.position = transform.position;

        if (puzzleComponent is CloneComponentData) ConfigureCloneData();
        if (puzzleComponent is TextComponentData) ConfigureTextData();
        if (puzzleComponent is MidasComponentData) ConfigureMidasData();
        if(puzzleComponent is SnakeComponentData) ConfigureSnakeData();

        return puzzleComponent;
    }

    void ConfigureCloneData()
    {
        var cloneComponent = puzzleComponent as CloneComponentData;

        var matcher = transform.GetComponent<PatternMatcher>();

        if (matcher == null)
        {
            Debug.LogWarning("Couldn't find Clone's pattern matcher");
            return;
        }

        cloneComponent.clonePosition = matcher.player.transform.position;
        cloneComponent.cloneSquares = matcher.player.engine.GetPositions();

        cloneComponent.clonePatternPosition = matcher.pattern.transform.position;
        cloneComponent.clonePatternSquares = matcher.pattern.engine.GetPositions();

        puzzleComponent = cloneComponent;
    }

    void ConfigureTextData()
    {
        var textComponent = puzzleComponent as TextComponentData;

        var inputField = transform.GetComponentInChildren<TMP_InputField>();

        if(inputField == null)
        {
            Debug.LogWarning("Couldn't find Input Field");
            return;
        }

        textComponent.text = inputField.text;

        puzzleComponent = textComponent;
    }

    void ConfigureMidasData()
    {
        var midasComponent = puzzleComponent as MidasComponentData;
        var matcher = GameObject.FindObjectsByType<PatternMatcher>(0)[0];

        List<Vector2Int> goldBlocks = new();

        foreach (Transform block in matcher.pattern.body)
        {
            var editorPattern = block.GetComponent<EditorPatternBlock>();

            if (editorPattern == null) continue;

            if (editorPattern.state == BlockState.Gold) goldBlocks.Add(Vector2Int.RoundToInt(block.localPosition));
        }

        midasComponent.goldBlocks = goldBlocks;

        puzzleComponent = midasComponent;
    }

    void ConfigureSnakeData()
    {
        var snakeComponent = puzzleComponent as SnakeComponentData;

        var inputField = transform.GetComponentInChildren<TMP_InputField>();

        if (inputField == null)
        {
            Debug.LogWarning("Couldn't find Input Field");
            return;
        }

        if(!int.TryParse(inputField.text, out snakeComponent.countdown))
        {
            snakeComponent.countdown = 3;
        }

        puzzleComponent = snakeComponent;
    }
}