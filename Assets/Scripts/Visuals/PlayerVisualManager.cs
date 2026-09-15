using DG.Tweening;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    public Transform body;
    public Movement movement;

    void Start()
    {
        movement.jump += Jump;
        movement.land += Land;
    }

    void Jump()
    {
        //body.DOPunchScale(new Vector3(0.2f, 1, 1), 0.45f, 5, 1);
    }

    void Land()
    {
        //body.DOPunchScale(new Vector3(1.3f, .4f, 1), 0.4f, 10, 0.1f);
    }
}
