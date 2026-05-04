using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkIntoPlace : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector3.one * 1.3f;
        transform.DOScale(Vector3.one, 0.2f);
    }
}
