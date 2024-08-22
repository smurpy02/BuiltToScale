using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternMatcher : MonoBehaviour
{
    public Transform matchBody;
    public Transform playerBody;

    public float matchDistance;

    public AudioSource pop;

    bool sceneLoaded;

    public bool fit = false;

    public bool levelSelecter = false;
    public int levelToLoad;
    public TextMeshProUGUI levelText;

    Transform originBlock;

    private void Update()
    {
        //playerBody.GetComponentsInChildren(playerPositions);

        fit = PositionsMatch();

        //if (PositionsMatch() && !sceneLoaded)
        //{
            //fit = true;

            bool snapIntoPlace = true;

            foreach(PatternMatcher matcher in FindObjectsOfType<PatternMatcher>())
            {
                //Debug.Log(matcher.gameObject.name);
                snapIntoPlace &= matcher.fit;
            }

        snapIntoPlace |= levelSelecter;
        snapIntoPlace &= !sceneLoaded;
        snapIntoPlace &= originBlock != null;

            if (!snapIntoPlace)
            {
                return;
            }

            //Debug.Log("pop");

            Vector2 newPosition = playerBody.position + matchBody.GetChild(0).position - originBlock.position;
            playerBody.position = newPosition;

            sceneLoaded = true;
            FindObjectOfType<Movement>().enabled = false;

            foreach(Transform block in matchBody)
            {
                Transform highlight = block.Find("Highlight");
                highlight.gameObject.SetActive(true);
            }

        Debug.Log("Pop " + levelSelecter);

            pop.Play();
            playerBody.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        int scene = levelSelecter ? levelToLoad : SceneManager.GetActiveScene().buildIndex + 1;
            SceneTransition.TransitionScene(scene);
        //}
        //else
        //{
        //    fit = false;
        //}
    }

    bool PositionsMatch()
    {
        if (matchBody.childCount != playerBody.childCount)
        {
            return false;
        }

        foreach (Transform playerPosition in playerBody)
        {
            List<Vector2Int> playerOffsets = new List<Vector2Int>();

            foreach (Transform otherPlayerPosition in playerBody)
            {
                if (otherPlayerPosition != playerPosition)
                {
                    playerOffsets.Add(Vector2Int.RoundToInt(playerPosition.position - otherPlayerPosition.position));
                }
            }

            bool validMatch = true;

            validMatch &= Vector2.Distance(playerPosition.position, matchBody.GetChild(0).position) < matchDistance;

            foreach (Transform child in matchBody)
            {
                if (child != matchBody.GetChild(0))
                {
                    Vector2Int offset = Vector2Int.RoundToInt(matchBody.GetChild(0).position - child.position);

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
}
