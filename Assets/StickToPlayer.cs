using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlayer : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        Vector3 position = player.position;
        position.z = transform.position.z;
        transform.position = position;
    }
}
