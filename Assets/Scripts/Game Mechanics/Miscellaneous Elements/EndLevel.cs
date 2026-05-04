using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public Transform player, teleport;

    private void Update()
    {
        if(player.position.y < -5) player.position = teleport.position;
    }
}
