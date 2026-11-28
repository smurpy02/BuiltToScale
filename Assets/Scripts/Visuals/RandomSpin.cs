using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class RandomSpin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpinRandom());
    }

    IEnumerator SpinRandom()
    {
        yield return new WaitForSeconds(Random.Range(4, 10));

        yield return transform.DORotate(new Vector3(0, 0, 360), .8f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutElastic).WaitForCompletion();

        StartCoroutine(SpinRandom());
    }
}
