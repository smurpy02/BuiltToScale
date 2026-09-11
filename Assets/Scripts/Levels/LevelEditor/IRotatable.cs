using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class IRotatable : MonoBehaviour
{
    public void Rotate()
    {
        transform.Rotate(0, 0, 90);
    }
}