using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomiseColours : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    public List<Transform> elements = new List<Transform>();
    Dictionary<Transform, Color> colorPairs = new Dictionary<Transform, Color>();

    private void Awake()
    {
        if (colors.Count < elements.Count)
        {
            return;
        }

        foreach (Transform element in elements)
        {
            colorPairs.Add(element, ChooseRandomColour());
        }
    }

    private void Update()
    {
        foreach (Transform element in elements)
        {
            if (element != null)
            {
                foreach (Renderer renderer in element.GetComponentsInChildren<Renderer>())
                {
                    renderer.material.color = colorPairs[element];
                }

                foreach (Image image in element.GetComponentsInChildren<Image>())
                {
                    image.color = colorPairs[element];
                    image.material.color = Color.white;
                }
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
