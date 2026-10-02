using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
}