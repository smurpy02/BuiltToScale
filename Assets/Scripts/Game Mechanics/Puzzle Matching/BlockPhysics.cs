using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockPhysics : MonoBehaviour
{
    public GameObject softHighlight;
    public Transform groundCheck;
    public SquareVisualManager elements;
    public ExpansionEngine engine;
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
            if(engine.Break(GetFixedPosition(), transform)) Destroy(gameObject);
        }

        if(collision.tag == "Spinner")
        {
            softHighlight.SetActive(true);
            collision.GetComponent<Spinner>().AddSquare(transform);
        }

        if(collision.tag == "Midas")
        {
            var block = engine.GetBlock(GetFixedPosition());

            if (block.IsUnityNull()) return;
            if (block.state != 0) return;

            block.state = BlockState.Gold;
            elements.Gold();
        }
    }

    Vector2Int GetFixedPosition() => Vector2Int.RoundToInt(transform.localPosition);

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spinner")
        {
            softHighlight.SetActive(false);
            collision.GetComponent<Spinner>().RemoveSquare(transform);
        }
    }

    public void BlockRemoved(Vector2Int myPosition, Vector2Int blockPosition) => elements?.BlockRemoved(myPosition, blockPosition);
    public void BlockAdded(Vector2Int myPosition, Vector2Int blockPosition) => elements?.BlockAdded(myPosition, blockPosition);
}
