using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkIntoPlace : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector3.one * 1.2f;
    }

    private void Update()
    {
        transform.localScale = Vector2.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * 2f);
    }
}
