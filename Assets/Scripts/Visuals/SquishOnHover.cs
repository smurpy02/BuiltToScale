using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquishOnHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        transform.DOShakeScale(.8f, .2f, 8, 80).OnComplete(() => transform.DOScale(Vector3.one, .3f));
    }
}
