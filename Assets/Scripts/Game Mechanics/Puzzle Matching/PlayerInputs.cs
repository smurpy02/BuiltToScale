using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference horizontalMove;
    public InputActionReference jump;
    public InputActionReference up, down, left, right;

    [Header("Engines")]
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
