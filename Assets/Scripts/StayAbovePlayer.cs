using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAbovePlayer : MonoBehaviour
{
    public ExpansionManager expansion;

    private void Update()
    {
        bool positionUnasigned = true;
        int highestPosition = 0;

        foreach(Vector2Int blockPositions in expansion.blocks.Keys)
        {
            if(positionUnasigned)
            {
                highestPosition = blockPositions.y;
                positionUnasigned = false;
            }
            else
            {
                if(blockPositions.y > highestPosition)
                {
                    highestPosition = blockPositions.y;
                }
            }
        }

        Vector2 position = transform.localPosition;
        position.y = highestPosition + 1;
        transform.localPosition = position;
    }
}
