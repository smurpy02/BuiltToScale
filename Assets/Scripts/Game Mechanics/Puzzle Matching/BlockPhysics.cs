using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPhysics : MonoBehaviour
{
    public GameObject softHighlight;
    public bool shrinkIntoPlace;

    void Start()
    {
        if (!shrinkIntoPlace) return;

        transform.localScale = Vector3.one * 1.3f;
        transform.DOScale(Vector3.one, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Breaker")
        {
            transform.parent.parent.GetComponentInChildren<ExpansionEngine>().Break(Vector2Int.RoundToInt(transform.localPosition), transform);
            Destroy(gameObject);
        }

        if(collision.tag == "Spinner")
        {
            softHighlight.SetActive(true);
            collision.GetComponent<Spinner>().AddSquare(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spinner")
        {
            softHighlight.SetActive(false);
            collision.GetComponent<Spinner>().RemoveSquare(transform);
        }
    }
}
