using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference horizontalMove;
    public InputActionReference jump;
    public InputActionReference up, down, left, right;
    public InputActionReference spin;

    [Header("Engines")]
    public Movement movement;
    public ExpansionEngine expansion;

    void Update()
    {
        // Horizontal Movement
        float horizontal = horizontalMove.action.ReadValue<float>();
        movement.HorizontalInput(horizontal);

        // Vertical Movement
        if (jump.action.IsPressed()) movement.Jump();

        // Expansion
        CheckExpand(up, Vector2Int.up);
        CheckExpand(down, Vector2Int.down);
        CheckExpand(left, Vector2Int.left);
        CheckExpand(right, Vector2Int.right);

        // Puzzle Object Interaction
        if (spin.action.WasPressedThisFrame()) Spinner.TriggerSpin();
    }

    void CheckExpand(InputActionReference input, Vector2Int direction)
    {
        if (input.action.WasPressedThisFrame()) expansion.Expand(direction);
    }
}
