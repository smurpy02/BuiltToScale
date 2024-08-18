using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseColours : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    public List<Transform> elements = new List<Transform>();
    Dictionary<Transform, Color> colorPairs = new Dictionary<Transform, Color>();

    private void Awake()
    {
        if(colors.Count < elements.Count)
        {
            return;
        }

        foreach(Transform element in elements)
        {
            colorPairs.Add(element, ChooseRandomColour());
        }
    }

    private void Update()
    {
        foreach(Transform element in elements)
        {
            foreach(Renderer renderer in element.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = colorPairs[element];
            }
        }
    }

    public Color ChooseRandomColour()
    {
        Color color = colors[Random.Range(0, colors.Count)];
        colors.Remove(color);
        return color;
    }
}
