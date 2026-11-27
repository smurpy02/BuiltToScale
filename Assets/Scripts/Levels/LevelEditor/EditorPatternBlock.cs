using UnityEngine;
using UnityEngine.UI;

public class EditorPatternBlock : MonoBehaviour
{
    public BlockState state;

    public Toggle goldToggle;

    public void ToggleGold() => ToggleGold(goldToggle.isOn);

    void ToggleGold(bool gold) => state = gold ? BlockState.Gold : BlockState.Basic;
}
