using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;

    public InputActionReference horizontalMove;
    public InputActionReference jump;

    public float speed;
    public float jumpForce;

    public static List<Transform> groundChecks = new List<Transform>();

    public Transform body;

    public bool invert = false;

    public LayerMask jumpingMask;

    private void Update()
    {
        float horizontal = horizontalMove.action.ReadValue<float>() * (invert ? -1 : 1);

        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * speed;
        rb.velocity = velocity;

        Jumping();
    }

    void Jumping()
    {
        if (jump.action.IsPressed() && rb.velocity.y <= 0.2f)
        {
            foreach (Transform square in body)
            {
                RaycastHit2D hit = Physics2D.BoxCast(square.transform.position, Vector2.one * 0.8f, 0, Vector2.down, 0.1f, jumpingMask);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    rb.velocity += Vector2.up * jumpForce;
                    return;
                }
            }
        }
    }

    private void OnDestroy()
    {
        groundChecks.Clear();
    }
}
