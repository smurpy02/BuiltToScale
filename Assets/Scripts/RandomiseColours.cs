using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseColours : MonoBehaviour
{
    public List<Color> colors = new List<Color>();

    public Transform player;
    public Transform level;
    public Transform pattern;

    Color playerColor;
    Color levelColor;
    Color patternColor;

    private void Awake()
    {
        if(colors.Count < 3)
        {
            return;
        }

        playerColor = ChooseRandomColour();
        levelColor = ChooseRandomColour();
        patternColor = ChooseRandomColour();

        foreach(Renderer sprite in player.GetComponentsInChildren<Renderer>())
        {
            sprite.material.color = playerColor;
        }

        foreach(Renderer sprite in level.GetComponentsInChildren<Renderer>())
        {
            sprite.material.color = levelColor;
        }

        foreach(Renderer sprite in pattern.GetComponentsInChildren<Renderer>())
        {
            sprite.material.color = patternColor;
        }
    }

    private void Update()
    {
        foreach (Renderer sprite in player.GetComponentsInChildren<Renderer>())
        {
            sprite.material.color = playerColor;
        }
    }

    public Color ChooseRandomColour()
    {
        Color color = colors[Random.Range(0, colors.Count)];
        colors.Remove(color);
        return color;
    }
}
