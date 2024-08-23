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

    public void AddSquare(Transform square)
    {
        squares.Add(square);
    }

    public void RemoveSquare(Transform square)
    {
        squares.Remove(square);
    }

    public void Spin()
    {
        spin = true;
        Debug.Log("spin");
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
        Transform[] blocksTransformed = new Transform[squares.Count];
        squares.CopyTo(0, blocksTransformed, 0, squares.Count);

        Debug.Log("blocks " + blocksTransformed.Length);

        if (squares.Count > 0)
        {
            foreach(Transform square in blocksTransformed)
            {
                foreach(Collider2D col in square.GetComponentsInChildren<Collider2D>())
                {
                    col.enabled = false;
                }

                Vector3 targetRotation = square.rotation.eulerAngles;
                targetRotation.z += 90;

                square.DORotate(targetRotation, 1f);

                Vector2 targetPosition = square.position + GetRotatedPointDelta(square.position, transform.position, Vector3.forward, 90);

                square.DOMove(targetPosition, 1f);
            }

            movement.enabled = false;
            stoppedMovement = true;
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = currentAngle;
        yield return transform.DORotate(rotation, 1f).WaitForCompletion();

        if (stoppedMovement)
        {
            Debug.Log("Resetting Blocks");

            movement.enabled = true;

            Debug.Log("blocks " + blocksTransformed.Length);

            foreach (Transform square in blocksTransformed)
            {
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
