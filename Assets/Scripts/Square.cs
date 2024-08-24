using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject softHighlight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger " + gameObject.name);
        if(collision.tag == "Breaker")
        {
            transform.parent.parent.GetComponentInChildren<ExpansionManager>().Break(Vector2Int.RoundToInt(transform.localPosition), transform);
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
