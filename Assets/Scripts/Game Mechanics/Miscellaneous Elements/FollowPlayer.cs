using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if(Mathf.Pow(transform.position.x - player.position.x, 2f) > 4)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        }
    }
}
