using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternMatcher : MonoBehaviour
{
    public Transform matchBody;
    public Transform playerBody;

    public float matchDistance;

    bool sceneLoaded;

    private void Update()
    {
        //playerBody.GetComponentsInChildren(playerPositions);

        if (PositionsMatch() && !sceneLoaded)
        {
            sceneLoaded = true;
            FindObjectOfType<Movement>().enabled = false;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SceneTransition.TransitionScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                Vector2 newPosition = playerBody.position + matchBody.GetChild(0).position - playerPosition.position;
                playerBody.position = newPosition;
                return true;
            }
        }

        return false;
    }
}
