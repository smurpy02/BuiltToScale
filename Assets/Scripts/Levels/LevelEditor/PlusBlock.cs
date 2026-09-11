using UnityEngine;

public class PlusBlock : MonoBehaviour
{
    public BlockBodyEditor engine;
    public Vector2Int position;

    public void Plus()
    {
        if (engine != null) engine.SpawnNewBlock(position);

        Destroy(gameObject);
    }
}
