using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAbovePlayer : MonoBehaviour
{
    public ExpansionManager expansion;

    private void Update()
    {
        bool positionUnasigned = true;
        Vector2 highestPosition = Vector2.zero;

        foreach(Vector2Int blockPositions in expansion.blocks.Keys)
        {
            if(positionUnasigned)
            {
                highestPosition = blockPositions;
                positionUnasigned = false;
            }
            else
            {
                if(blockPositions.y > highestPosition.y)
                {
                    highestPosition = blockPositions;
                }
            }
        }

        Vector2 position = highestPosition;
        position.y = highestPosition.y + 1;
        transform.localPosition = position;
    }
}
