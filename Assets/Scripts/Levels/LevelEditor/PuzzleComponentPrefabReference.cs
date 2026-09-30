using UnityEngine;

[CreateAssetMenu(fileName = "new puzzle reference", menuName = "Puzzle Reference")]
public class PuzzleComponentPrefabReference : ScriptableObject
{
    public int id;
    public GameObject prefab, editablePrefab;
}