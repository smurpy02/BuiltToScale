using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player;
    public Transform teleport;

    private void Update()
    {
        if(player.position.y < -5)
        {
            player.position = teleport.position;
        }
    }
}
