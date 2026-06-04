using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [Header("Settings")]
    public bool invert;

    [Header("Inputs")]
    public InputActionReference horizontalMove;
    public InputActionReference jump;
    public InputActionReference up, down, left, right;
    public InputActionReference retry;
    public InputActionReference spin;

    [Header("Engines")]
    public Movement movement;
    public ExpansionEngine expansion;

    bool reloadingScene;

    void Update()
    {
        // Retry
        if (retry.action.WasPressedThisFrame() && !reloadingScene) ReloadScene();

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
        if (invert) direction.x *= -1;

        if (input.action.WasPressedThisFrame()) expansion.Expand(direction);
    }

    void ReloadScene()
    {
        reloadingScene = true;
        LevelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex, 0.8f);
    }
}
