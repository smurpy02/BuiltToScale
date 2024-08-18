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

    private void Update()
    {
        float horizontal = horizontalMove.action.ReadValue<float>();

        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * speed;
        rb.velocity = velocity;

        Jumping();
    }

    void Jumping()
    {
        if (jump.action.IsPressed() && rb.velocity.y <= 0.2f)
        {
            Debug.Log("jump time");

            foreach(Transform square in body)
            {
                RaycastHit2D hit = Physics2D.BoxCast(square.transform.position, Vector2.one * 0.9f, 0, Vector2.down, 0.2f, 3);

                if (hit.collider != null)
                {
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
