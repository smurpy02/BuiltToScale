using UnityEngine;

public class EditorBlockCross : MonoBehaviour
{
    public EditorBlock block;

    void OnMouseDown()
    {
        block.OnCross();
    }
}
