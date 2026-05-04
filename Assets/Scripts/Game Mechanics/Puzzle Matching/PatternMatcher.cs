using System.Collections.Generic;
using UnityEngine;
using System;

public class PatternMatcher : MonoBehaviour
{
    public static PatternMatcher leader;
    public static List<PatternMatcher> matchers = new List<PatternMatcher>();

    public Player player;
    public Pattern pattern;

    public float matchDistance;

    bool matched;

    Transform originBlock;

    void Start()
    {
        if (leader == null) leader = this;
    }

    private void Update()
    {
        if (leader == this) CheckMatch();
    }

    void CheckMatch()
    {
        Debug.Log("Checking Matches1");
        if (matched) return;

        Debug.Log("Checking Matches2");
        foreach(PatternMatcher matcher in matchers)
        {
            if (!PositionsMatch() || matcher.originBlock == null) return;
        }
        Debug.Log("Checking Matches3");

        CompleteMatch();
    }

    void CompleteMatch()
    {
        Debug.Log("Completing Match");
        matched = true;

        foreach (PatternMatcher matcher in matchers) matcher.SnapIntoPlace();

        throw new NotImplementedException();

        //set scene as complete in PlayerPrefs

        //find the next level to load

        LevelLoader.LoadScene(0);
    }

    public void SnapIntoPlace()
    {
        Vector2 newPosition = player.body.position + pattern.body.GetChild(0).position - originBlock.position;
        player.body.position = newPosition;

        player.movement.enabled = false;

        foreach (Transform block in pattern.body)
        {
            Transform highlight = block.Find("Highlight");
            highlight.gameObject.SetActive(true);
        }

        LevelAudioManager.Match();
        player.body2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    bool PositionsMatch()
    {
        if (pattern.body.childCount != player.body.childCount)
        {
            return false;
        }

        foreach (Transform playerPosition in player.body)
        {
            List<Vector2Int> playerOffsets = new List<Vector2Int>();

            foreach (Transform otherPlayerPosition in player.body)
            {
                if (otherPlayerPosition != playerPosition)
                {
                    playerOffsets.Add(Vector2Int.RoundToInt(playerPosition.position - otherPlayerPosition.position));
                }
            }

            bool validMatch = true;

            validMatch &= Vector2.Distance(playerPosition.position, pattern.body.GetChild(0).position) < matchDistance;

            foreach (Transform child in pattern.body)
            {
                if (child != pattern.body.GetChild(0))
                {
                    Vector2Int offset = Vector2Int.RoundToInt(pattern.body.GetChild(0).position - child.position);

                    validMatch &= playerOffsets.Contains(offset);
                }
            }

            if (validMatch)
            {
                originBlock = playerPosition;

                return true;
            }
        }

        return false;
    }

    void OnDisable()
    {
        matchers.Remove(this);
    }

    void OnEnable()
    {
        matchers.Add(this);
    }
}
