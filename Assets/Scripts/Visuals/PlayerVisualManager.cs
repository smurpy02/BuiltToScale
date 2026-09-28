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
        scaleParent.DOPunchScale(new Vector3(-0.2f, .2f), 0.3f, 2, 1).OnComplete(() => scaleParent.localScale = Vector3.one);
    }

    void Land()
    {
        scaleParent.DOPunchScale(new Vector3(.25f, -.3f), 0.3f, 2, 1).OnComplete(() => scaleParent.localScale = Vector3.one);
    }
}
