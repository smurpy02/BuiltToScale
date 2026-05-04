using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Movement")]
    public InputActionReference horizontalMove, jump;

    [Header("Expansion")]
    public InputActionReference up, down, left, right;

    public Movement movement;
    public ExpansionEngine expansion;

    void Update()
    {
        float horizontal = horizontalMove.action.ReadValue<float>();
        movement.HorizontalInput(horizontal);

        if (jump.action.IsPressed()) movement.Jump();

        CheckExpand(up, Vector2Int.up);
        CheckExpand(down, Vector2Int.down);
        CheckExpand(left, Vector2Int.left);
        CheckExpand(right, Vector2Int.right);
    }

    void CheckExpand(InputActionReference input, Vector2Int direction)
    {
        if (input.action.WasPressedThisFrame()) expansion.Expand(direction);
    }
}
