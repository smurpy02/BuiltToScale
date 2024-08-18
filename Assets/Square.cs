using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);

        if(collision.tag == "Breaker")
        {
            FindObjectOfType<ExpansionManager>().Break(Vector2Int.RoundToInt(transform.localPosition), transform);
            Destroy(gameObject);
        }
    }
}
