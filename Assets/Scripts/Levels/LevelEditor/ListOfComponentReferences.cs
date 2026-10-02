using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new list", menuName = "List of Components")]
public class ListOfComponentReferences : ScriptableObject
{
    public List<PuzzleComponentPrefabReference> components;
}
