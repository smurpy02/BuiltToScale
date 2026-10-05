using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<ButtonHover> otherButtons;
    public Vector3 awakePosition;

    void Awake()
    {
        awakePosition = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = awakePosition;
        transform.DOShakePosition(.2f, 3);
        transform.DOScale(1.1f, .25f);

        otherButtons.ForEach(button => button.transform.DOLocalMoveX(button.awakePosition.x + ((button.transform.localPosition.x > transform.localPosition.x) ? 10 : -10), .25f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1, .4f);
        transform.localPosition = awakePosition;

        otherButtons.ForEach(button => button.transform.DOLocalMove(button.awakePosition, .1f));
    }
}
