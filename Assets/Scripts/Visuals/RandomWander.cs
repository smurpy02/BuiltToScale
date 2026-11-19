using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RandomWander : MonoBehaviour
{
    public Transform centre;
    public float wanderRadius;

    public void Start()
    {
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        Vector2 target = (Vector2)centre.position + (Random.insideUnitCircle * wanderRadius);

        float distance = Vector2.Distance(transform.position, target);

        var wobble = transform.DOShakeScale(3f, new Vector3(0, .05f, 0), 6, 0, false).OnComplete(() => transform.localScale=Vector3.one).SetLoops(-1);

        yield return transform.DOMove(target, distance * .7f, false).SetEase(Ease.OutFlash).WaitForCompletion();

        wobble.Kill();
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(.5f);

        StartCoroutine(Wander());
    }
}
