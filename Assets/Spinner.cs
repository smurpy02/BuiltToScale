using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    List<Transform> squares = new List<Transform>();
    float currentAngle = 0;
    bool stoppedMovement;

    public bool spin;
    public Movement movement;
    public ExpansionManager expansionManager;

    private void Start()
    {
        movement = FindObjectOfType<Movement>();
        expansionManager = FindObjectOfType<ExpansionManager>();
    }

    public void AddSquare(Transform square)
    {
        if(squares.Contains(square))
        {
            return;
        }

        squares.Add(square);
    }

    public void RemoveSquare(Transform square)
    {
        if (!squares.Contains(square))
        {
            return;
        }

        squares.Remove(square);
    }

    public void Spin()
    {
        if (stoppedMovement)
        {
            return;
        }

        spin = true;
    }

    private void Update()
    {
        if (spin)
        {
            spin = false;
            currentAngle += 90;
            StartCoroutine(Rotate());
        }
    }

    IEnumerator Rotate()
    {
        Dictionary<Transform, Transform> originalParentOfBlock = new Dictionary<Transform, Transform>();

        foreach(Transform square in squares)
        {
            originalParentOfBlock.Add(square, square.parent);
        }

        if (squares.Count > 0)
        {
            foreach(Transform square in originalParentOfBlock.Keys)
            {
                square.parent = transform;

                foreach(Collider2D col in square.GetComponentsInChildren<Collider2D>())
                {
                    col.enabled = false;
                }

                //Vector3 targetRotation = square.rotation.eulerAngles;
                //targetRotation.z += 90;

                //square.DORotate(targetRotation, 1f);

                //Vector2 targetPosition = square.position + GetRotatedPointDelta(square.position, transform.position, Vector3.forward, 90);

                //square.DOMove(targetPosition, 1f);
            }

            movement.enabled = false;
            expansionManager.enabled = false;
            stoppedMovement = true;
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = currentAngle;
        yield return transform.DORotate(rotation, 1f).WaitForCompletion();

        if (stoppedMovement)
        {
            Debug.Log("Resetting Blocks");

            movement.enabled = true;
            expansionManager.enabled = true;

            foreach (Transform square in originalParentOfBlock.Keys)
            {
                square.parent = originalParentOfBlock[square];

                foreach (Collider2D col in square.GetComponentsInChildren<Collider2D>())
                {
                    col.enabled = true;
                }

                Vector3 localPosition = (Vector2)Vector2Int.RoundToInt(square.localPosition);
                localPosition.z = 0;
                square.localPosition = localPosition;
            }

            expansionManager.UpdateAfterSpin();
        }

        stoppedMovement = false;
    }
    public Vector3 GetRotatedPointDelta(Vector3 startPosition, Vector3 rotationCentre, Vector3 rotationAxis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, rotationAxis);
        Vector3 localPosition = startPosition - rotationCentre;
        return (q * localPosition) - localPosition;
    }
}
