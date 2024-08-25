using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaySoundOnHover : MonoBehaviour, IPointerEnterHandler
{
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.Play();
    }
}
