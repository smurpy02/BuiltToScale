using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using DG.Tweening;

public class PatternMatcher : MonoBehaviour
{
    static PatternMatcher leader;
    static List<PatternMatcher> matchers = new List<PatternMatcher>();

    public Player player;
    public Pattern pattern;

    public float matchDistance;

    bool matched;
    Transform originBlock;

    void Start()
    {
        if (leader == null) leader = this;
    }

    private void FixedUpdate()
    {
        if (leader == this) CheckMatch();
    }

    #region Validate Match
    void CheckMatch()
    {
        if (matched) return;

        foreach (PatternMatcher matcher in matchers)
        {
            if (!matcher.CheckPositions() || matcher.originBlock == null) return;
        }

        CompleteMatch();
    }

    bool CheckPositions()
    {
        if(pattern.body.childCount != player.body.childCount) return false;

        foreach(Transform playerOrigin in player.body)
        {
            if (CheckPlayerOrigin(playerOrigin))
            {
                originBlock = playerOrigin;
                return true;
            }
        }

        return false;
    }

    bool CheckPlayerOrigin(Transform playerOrigin)
    {
        Vector2 patternOrigin = pattern.body.GetChild(0).position;

        if (Vector3.Distance(patternOrigin, playerOrigin.position) > matchDistance) return false;

        foreach(Transform patternBlock in pattern.body)
        {
            var offset = (Vector2)patternBlock.position - patternOrigin;

            Vector2Int playerSquare = Vector2Int.RoundToInt((Vector2)playerOrigin.localPosition + offset);

            if(!player.engine.ContainsBlockPosition(playerSquare)) return false;
        }

        return true;
    }
    #endregion

    #region Match Success
    void CompleteMatch()
    {
        matched = true;

        var playerPosition = player.engine.body.position;
        var cameraTransform = Camera.main.transform;

        cameraTransform.DOMove(new Vector3(playerPosition.x, playerPosition.y, cameraTransform.position.z), .6f).SetEase(Ease.OutBack);

        Camera.main.DOOrthoSize(3.5f, 1f).SetEase(Ease.InOutSine);

        foreach (PatternMatcher matcher in matchers) matcher.SnapIntoPlace();

        //set scene as complete in PlayerPrefs

        LevelLoader.LoadNextLevel();
    }

    public void SnapIntoPlace()
    {
        Vector2 newPosition = player.body.position + pattern.body.GetChild(0).position - originBlock.position;
        player.body.position = newPosition;

        player.movement.enabled = false;

        foreach (Transform block in pattern.body)
        {
            Transform highlight = block.Find("Highlight");
            if(highlight != null) highlight.gameObject.SetActive(true);
        }

        LevelAudioManager.Match();
        player.body2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    #endregion

    void OnDisable()
    {
        matchers.Remove(this);
    }

    void OnEnable()
    {
        matchers.Add(this);
    }
}
