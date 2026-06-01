using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spinner : MonoBehaviour
{
    public static List<Spinner> spinners = new List<Spinner>();

    List<Transform> squares = new List<Transform>();
    float currentAngle = 0;
    bool isSpinning;

    public GameObject spinPrompt;

    void OnEnable()
    {
        spinners.Add(this);
    }

    void OnDisable()
    {
        spinners.Remove(this);
    }

    public void AddSquare(Transform square)
    {
        if (squares.Contains(square)) return;

        squares.Add(square);
        spinPrompt.SetActive(true);
    }

    public void RemoveSquare(Transform square)
    {
        if (!squares.Contains(square)) return;

        squares.Remove(square);
        spinPrompt.SetActive(squares.Count > 0);
    }

    public static void TriggerSpin()
    {
        foreach (Spinner spinner in spinners) spinner.Spin();
    }

    public void Spin()
    {
        if (isSpinning) return;

        isSpinning = true;
        currentAngle += 90;
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        Dictionary<Transform, Transform> blockParents = new Dictionary<Transform, Transform>();

        #region Prespin
        foreach (Transform square in squares)
        {
            if (square != null) blockParents.Add(square, square.parent);
        }

        bool squaresModified = false;

        foreach (Transform square in blockParents.Keys)
        {
            if (square == null) continue;

            squaresModified = true;

            StickSquare(square, false, transform);
        }

        if (squaresModified)
        {
            ToggleSystems(false);
        }
        #endregion

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = currentAngle;
        LevelAudioManager.Spin();

        yield return transform.DORotate(rotation, 1f).SetEase(Ease.InCubic).WaitForCompletion();

        #region postSpin
        if (squaresModified)
        {
            foreach (Transform square in blockParents.Keys)
            {
                if (square == null) continue;

                StickSquare(square, true, blockParents[square]);
                LockSquareToGrid(square);
            }

            ToggleSystems(true);
        }

        isSpinning = false;
        #endregion
    }

    #region Manage Spin Physics
    void StickSquare(Transform square, bool enableColliders, Transform newParent)
    {
        square.parent = newParent;

        foreach (Collider2D col in square.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = enableColliders;
        }
    }

    void LockSquareToGrid(Transform square)
    {
        Vector3 localPosition = (Vector2)Vector2Int.RoundToInt(square.localPosition);
        localPosition.z = 0;
        square.localPosition = localPosition;
    }

    void ToggleSystems(bool enable)
    {
        foreach (PlayerInput playerInput in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
        {
            playerInput.enabled = enable;
        }

        foreach (Movement movement in FindObjectsByType<Movement>(FindObjectsSortMode.None))
        {
            movement.enabled = enable;
            if (!enable) movement.body2D.linearVelocity = Vector3.zero;
        }

        foreach (ExpansionEngine expansion in FindObjectsByType<ExpansionEngine>(FindObjectsSortMode.None))
        {
            expansion.enabled = enable;
            if (enable) expansion.ReconfigBlockPositions();
        }

        foreach (PatternMatcher pattern in FindObjectsByType<PatternMatcher>(FindObjectsSortMode.None))
        {
            pattern.enabled = enable;
        }
    }
    #endregion
}
