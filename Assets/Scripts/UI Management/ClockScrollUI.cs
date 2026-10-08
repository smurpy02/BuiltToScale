using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClockScrollUI : MonoBehaviour
{
    public GameObject leftArrow, rightArrow;
    public Transform pivot;
    public float incrementAmount;
    public Action shiftLeft, shiftRight;

    List<float> points = new();
    float zRotation = 0;

    public void ResetRotation()
    {
        zRotation = 0;
        UpdateRotation(false);
    }

    public void ClearPoints() => points.Clear();

    public void AddPoint()
    {
        if (points.Contains(zRotation)) zRotation += incrementAmount;
        UpdateRotation(false);
        points.Add(zRotation);
    }

    public void ShiftRight()
    {
        if (TryRotate(zRotation + incrementAmount)) shiftRight?.Invoke();
    }

    public void ShiftLeft()
    {
        if (TryRotate(zRotation - incrementAmount)) shiftLeft?.Invoke();
    }

    bool TryRotate(float newRotation)
    {
        if (!points.Contains(newRotation)) return false;

        zRotation = newRotation;
        UpdateRotation();
        return true;
    }

    void UpdateRotation(bool animate = true)
    {
        if (animate)
        {
            pivot.DORotate(Vector3.forward * zRotation, .15f).SetEase(Ease.InOutSine);
        }
        else
        {
            pivot.rotation = Quaternion.Euler(0, 0, zRotation);
        }

        leftArrow.SetActive(points.Contains(zRotation - incrementAmount));
        rightArrow.SetActive(points.Contains(zRotation + incrementAmount));
    }
}
