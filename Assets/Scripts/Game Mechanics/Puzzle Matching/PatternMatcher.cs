using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternMatcher : MonoBehaviour
{
    public static PatternMatcher instance;
    public static List<PatternMatcher> listeners = new List<PatternMatcher>();
    public static PatternMatcher selectedLevel;

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
        if(instance == null) instance = this;

        fit = PositionsMatch();

        if (!instance == this)
        {
            return;
        }

        if (!ValidMatch())
        {
            return;
        }

        if (levelSelecter)
        {
            selectedLevel.SnapIntoPlace();
            selectedLevel.TransitionScene();
            return;
        }

        foreach (PatternMatcher matcher in listeners)
        {
            matcher.SnapIntoPlace();
        }

        if(!levelSelecter) PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 1);

        TransitionScene();
    }

    public void SnapIntoPlace()
    {
        Vector2 newPosition = playerBody.position + matchBody.GetChild(0).position - originBlock.position;
        playerBody.position = newPosition;

        playerBody.GetComponentInParent<Movement>().enabled = false;

        foreach (Transform block in matchBody)
        {
            Transform highlight = block.Find("Highlight");
            highlight.gameObject.SetActive(true);
        }

        pop.Play();
        playerBody.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    bool ValidMatch()
    {
        bool snapIntoPlace = true;

        foreach (PatternMatcher matcher in listeners)
        {
            snapIntoPlace &= matcher.fit;
        }

        snapIntoPlace |= levelSelecter;
        snapIntoPlace &= !sceneLoaded;
        snapIntoPlace &= originBlock != null;

        return snapIntoPlace;
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
                if (levelSelecter) selectedLevel = this;

                return true;
            }
        }

        return false;
    }

    void TransitionScene()
    {
        sceneLoaded = true;
        int scene = levelSelecter ? levelToLoad : SceneManager.GetActiveScene().buildIndex + 1;
        SceneTransition.TransitionScene(scene);
    }

    void OnDisable()
    {
        instance = null;
        listeners.Remove(this);
    }

    void OnEnable()
    {
        if (instance == null) instance = this;
        listeners.Add(this);
    }
}
