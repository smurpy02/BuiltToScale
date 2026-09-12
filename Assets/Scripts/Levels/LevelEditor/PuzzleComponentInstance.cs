using UnityEngine;

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

        return puzzleComponent;
    }

    void ConfigureCloneData()
    {
        var cloneComponent = puzzleComponent as CloneComponentData;

        var matcher = transform.GetComponent<PatternMatcher>();

        if (matcher == null)
        {
            Debug.Log("Couldn't find Clone's pattern matcher");
            return;
        }

        cloneComponent.clonePosition = matcher.player.transform.position;
        cloneComponent.cloneSquares = matcher.player.engine.GetPositions();

        cloneComponent.clonePatternPosition = matcher.pattern.transform.position;
        cloneComponent.clonePatternSquares = matcher.pattern.engine.GetPositions();

        puzzleComponent = cloneComponent;
    }
}