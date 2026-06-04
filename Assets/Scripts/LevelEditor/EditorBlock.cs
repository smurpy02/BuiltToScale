using UnityEngine;
using UnityEngine.EventSystems;

public class EditorBlock : MonoBehaviour
{
    public GameObject block;
    public BlockBodyEditor editor;

    void OnEnable()
    {
        editor = BlockBodyEditor.spawnedLastBlock;

        if (editor == null) Destroy(gameObject);
    }

    void OnMouseDown()
    {
        editor.RemoveBlock(Vector2Int.RoundToInt(block.transform.localPosition), block.transform);
        Destroy(block);
    }
}
