using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHat : MonoBehaviour
{
    public List<Transform> playerSquares = new List<Transform>();
    public GameObject spotlight;
    public Transform partner;
    public Vector2 outDir;

    bool _touchingPlayer { get { return playerSquares.Count > 0; } }

    List<ExpansionManager> expanders = new List<ExpansionManager>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            playerSquares.Add(collision.transform);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && !playerSquares.Contains(collision.transform))
        {
            playerSquares.Add(collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (playerSquares.Contains(collision.transform))
        {
            playerSquares.Remove(collision.transform);
        }
    }

    public void ExpandIntoTophat(ExpansionManager expander)
    {
        if (!_touchingPlayer)
        {
            return;
        }

        expanders.Add(expander);
    }

    void Update()
    {
        foreach (ExpansionManager expander in expanders)
        {
            expander.SpawnBlock((Vector2)partner.position + outDir);
        }

        spotlight.SetActive(_touchingPlayer || partner.GetComponentInChildren<TopHat>()._touchingPlayer);

        expanders.Clear();
    }
}
