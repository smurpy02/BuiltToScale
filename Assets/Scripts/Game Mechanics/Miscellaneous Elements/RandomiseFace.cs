using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseFace : MonoBehaviour
{
    public SpriteRenderer renderer;

    public List<Sprite> sprites;

    void OnEnable()
    {
        int random = Random.Range(0, sprites.Count);

        renderer.sprite = sprites[random];
    }
}
