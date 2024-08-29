using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spinner : MonoBehaviour
{
    List<Transform> squares = new List<Transform>();
    float currentAngle = 0;
    bool stoppedMovement;

    public bool spin;
    public InputActionReference spinInput;
    public GameObject spinPrompt;

    //public Movement movement;
    //public ExpansionManager expansionManager;
    //public Rigidbody2D body;

    //private void Start()
    //{
    //    movement = FindObjectOfType<Movement>();
    //    expansionManager = FindObjectOfType<ExpansionManager>();
    //    body = movement.rb;
    //}

    public void AddSquare(Transform square)
    {
        if (squares.Contains(square))
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
        spinPrompt.SetActive(squares.Count > 0);

        spin |= spinInput.action.WasPressedThisFrame();

        if (spin && !stoppedMovement)
        {
            spin = false;
            currentAngle += 90;
            StartCoroutine(Rotate());
        }
    }

    IEnumerator Rotate()
    {
        Dictionary<Transform, Transform> originalParentOfBlock = new Dictionary<Transform, Transform>();

        foreach (Transform square in squares)
        {
            if (square != null)
            {
                originalParentOfBlock.Add(square, square.parent);
            }
        }

        if (squares.Count > 0)
        {
            foreach (Transform square in originalParentOfBlock.Keys)
            {
                square.parent = transform;

                foreach (Collider2D col in square.GetComponentsInChildren<Collider2D>())
                {
                    col.enabled = false;
                }
            }

            ToggleSystems(false);

            stoppedMovement = true;
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = currentAngle;
        yield return transform.DORotate(rotation, 1f).WaitForCompletion();

        if (stoppedMovement)
        {
            Debug.Log("Resetting Blocks");

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

            ToggleSystems(true);

        }

        stoppedMovement = false;
    }
    //public Vector3 GetRotatedPointDelta(Vector3 startPosition, Vector3 rotationCentre, Vector3 rotationAxis, float angle)
    //{
    //    Quaternion q = Quaternion.AngleAxis(angle, rotationAxis);
    //    Vector3 localPosition = startPosition - rotationCentre;
    //    return (q * localPosition) - localPosition;
    //}

    void ToggleSystems(bool enable)
    {
        foreach (Movement movement in FindObjectsOfType<Movement>())
        {
            movement.enabled = enable;
            if (!enable) movement.rb.velocity = Vector3.zero;
        }

        foreach (ExpansionManager expansion in FindObjectsOfType<ExpansionManager>())
        {
            expansion.enabled = enable;
            if (enable) expansion.UpdateAfterSpin();
        }

        foreach (PatternMatcher pattern in FindObjectsOfType<PatternMatcher>())
        {
            pattern.enabled = enable;
        }
    }
}
