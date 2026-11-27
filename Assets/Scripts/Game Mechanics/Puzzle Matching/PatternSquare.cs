using UnityEngine;

public class PatternSquare : MonoBehaviour
{
    public GameObject goldCover;
    public BlockState state = BlockState.Basic;

    public void Gold()
    {
        goldCover.SetActive(true);
        state = BlockState.Gold;
    }
}
