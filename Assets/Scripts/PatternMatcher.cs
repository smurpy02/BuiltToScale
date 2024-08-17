using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternMatcher : MonoBehaviour
{
    public Transform matchBody;
    public Transform playerBody;

    public float matchDistance;

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
        if (matchBody.childCount != playerBody.childCount)
        {
            return false;
        }

        foreach (Transform playerPosition in playerBody)
        {
            List<Vector2> playerOffsets = new List<Vector2>();

            foreach (Transform otherPlayerPosition in playerBody)
            {
                if (otherPlayerPosition != playerPosition)
                {
                    playerOffsets.Add(playerPosition.position - otherPlayerPosition.position);
                }
            }

            bool validMatch = true;

            validMatch &= Vector2.Distance(playerPosition.position, matchBody.GetChild(0).position) < matchDistance;

            foreach (Transform child in matchBody)
            {
                if (child != matchBody.GetChild(0))
                {
                    Vector2 offset = matchBody.GetChild(0).position - child.position;

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
