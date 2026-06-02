using UnityEngine;

public class PlusBlock : MonoBehaviour
{
    public EditorExpansionEngine engine;
    public Vector2Int position;

    public void Plus()
    {
        Debug.Log("[Plus Block] Plus");

        if (engine != null) engine.SpawnNewBlock(position);

        Destroy(gameObject);
    }
}
