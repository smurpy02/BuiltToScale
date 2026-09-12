using NUnit.Framework;
using UnityEngine;

public class PlayerVisualElements : MonoBehaviour
{
    public GameObject
        left,
        right,
        up,
        down,
        TL,
        TR,
        BL,
        BR;

    bool
        TLActive,
        TRActive,
        BLActive,
        BRActive;

    public void BlockRemoved(Vector2Int myPosition, Vector2Int blockPosition)
    {
        var direction = blockPosition - myPosition;

        if (direction.Equals(Vector2Int.left)) { left.SetActive(false); BL.SetActive(false); TL.SetActive(false); }
        if (direction.Equals(Vector2Int.right)) { right.SetActive(false); BR.SetActive(false); TR.SetActive(false); }
        if (direction.Equals(Vector2Int.up))    { up.SetActive(false); TL.SetActive(false); TR.SetActive(false); }
        if (direction.Equals(Vector2Int.down))  { down.SetActive(false); BL.SetActive(false); BR.SetActive(false); }

        if (direction.Equals(Vector2Int.left + Vector2Int.up))      { TLActive = false; TL.SetActive(false); }
        if (direction.Equals(Vector2Int.left + Vector2Int.down))    { BLActive = false; BL.SetActive(false); }
        if (direction.Equals(Vector2Int.right + Vector2Int.up))     { TRActive = false; TR.SetActive(false); }
        if (direction.Equals(Vector2Int.right + Vector2Int.down))   { BRActive = false; BR.SetActive(false); }
    }

    public void BlockAdded(Vector2Int myPosition, Vector2Int blockPosition)
    {
        var direction = blockPosition - myPosition;

        if (direction.Equals(Vector2Int.left)) left.SetActive(true);
        if (direction.Equals(Vector2Int.right)) right.SetActive(true);
        if (direction.Equals(Vector2Int.up)) up.SetActive(true);
        if (direction.Equals(Vector2Int.down)) down.SetActive(true);
        
        if (direction.Equals(Vector2Int.left + Vector2Int.up))      TLActive = true;
        if (direction.Equals(Vector2Int.left + Vector2Int.down))    BLActive = true;
        if (direction.Equals(Vector2Int.right + Vector2Int.up))     TRActive = true;
        if (direction.Equals(Vector2Int.right + Vector2Int.down))   BRActive = true;

        TL.SetActive(TLActive && up.activeSelf && left.activeSelf);
        BL.SetActive(BLActive && down.activeSelf && left.activeSelf);
        TR.SetActive(TRActive && up.activeSelf && right.activeSelf);
        BR.SetActive(BRActive && down.activeSelf && right.activeSelf);
    }
}
