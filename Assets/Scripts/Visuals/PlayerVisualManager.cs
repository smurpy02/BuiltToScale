using DG.Tweening;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    public Transform scaleParent;
    public Movement movement;

    void Start()
    {
        movement.jump += Jump;
        movement.land += Land;
    }

    void Jump()
    {
        scaleParent.localScale = Vector3.one;
        scaleParent.DOPunchScale(new Vector3(-0.2f, .2f), 0.3f, 2, 1).OnComplete(() => scaleParent.DOScale(Vector3.one, .1f));
    }

    void Land()
    {
        scaleParent.localScale = Vector3.one;
        scaleParent.DOPunchScale(new Vector3(.25f, -.3f), 0.3f, 2, 1).OnComplete(() => scaleParent.DOScale(Vector3.one, .1f));
    }
}
