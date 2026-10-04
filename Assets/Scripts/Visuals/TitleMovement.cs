using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopShake();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartShake();
    }

    void Start()
    {
        StartShake();
    }

    void StartShake()
    {
        transform.DOShakePosition(10, .2f, 4, 80, false, false).SetLoops(-1);
    }

    void StopShake()
    {
        transform.DOKill();
    }
}
