using UnityEngine;
using UnityEngine.EventSystems;

public class EditorBlock : MonoBehaviour
{
    [HideInInspector] public BlockBodyEditor editor;
    public GameObject cross;

    public void OnCross()
    {
        editor.RemoveBlock(Vector2Int.RoundToInt(transform.localPosition), transform);
        Destroy(gameObject);
    }

    public void RemoveCross()
    {
        cross.SetActive(false);
    }
}