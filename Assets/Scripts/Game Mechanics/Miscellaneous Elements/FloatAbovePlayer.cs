using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAbovePlayer : MonoBehaviour
{
    public ExpansionEngine expansion;

    private void Update()
    {
        Vector2 position = expansion.GetHighestBlock();
        position.y += 1;
        transform.localPosition = position;
    }
}
