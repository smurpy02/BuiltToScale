using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternMatcher : MonoBehaviour
{
    public Transform matchBody;
    public Transform playerBody;

    //List<Transform> matchPositions = new List<Transform>();
    //List<Transform> playerPositions = new List<Transform>();

    //private void Start()
    //{
    //    matchBody.GetComponentsInChildren(matchPositions);
    //}

    private void Update()
    {
        //playerBody.GetComponentsInChildren(playerPositions);

        if (PositionsMatch())
        {
            SceneManager.LoadScene(0);
        }
    }

    bool PositionsMatch()
    {
        Debug.Log("Position Matcher");

        if (matchBody.childCount != playerBody.childCount)
        {
            Debug.Log("Wrong Length");
            return false;
        }

        Debug.Log("--- Possible Positions ---");
        foreach (Transform playerPosition in playerBody)
        {
            Debug.Log("Player Pos " + playerPosition);
            List<Vector2> playerOffsets = new List<Vector2>();

            Debug.Log("-- Other Positions --");
            foreach (Transform otherPlayerPosition in playerBody)
            {
                Debug.Log("Other Pos " + otherPlayerPosition);
                if (otherPlayerPosition != playerPosition)
                {
                    Debug.Log("Valid Offset " + (playerPosition.position - otherPlayerPosition.position));
                    playerOffsets.Add(playerPosition.position - otherPlayerPosition.position);
                }
            }

            bool validMatch = true;

            validMatch &= Vector2.Distance(playerPosition.position, matchBody.GetChild(0).position) < 0.1f;

            foreach (Transform child in matchBody)
            {
                if (child != matchBody.GetChild(0))
                {
                    Vector2 offset = matchBody.GetChild(0).position - child.position;
                    Debug.Log("Offset " + offset);

                    validMatch &= playerOffsets.Contains(offset);
                }
            }

            if (validMatch)
            {
                return true;
            }
        }

        return false;
    }
}
