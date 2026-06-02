using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class IDragable : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if(LevelEditorManager.instance != null) if (LevelEditorManager.instance.snapToGrid)
            {
                curPosition.x = Round(curPosition.x);
                curPosition.y = Round(curPosition.y);
                curPosition.z = Round(curPosition.z);
            }

        transform.position = curPosition;
    }

    float Round(float value)
    {
        value *= 2;
        value = Mathf.Round(value);
        value /= 2;

        return value;
    }

}