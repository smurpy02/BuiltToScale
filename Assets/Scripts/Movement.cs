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

    [Header("Audio")]
    public AudioSource jumpAudio;
    public AudioSource landingAudio;

    bool groundedLastFrame = true;
    bool landed;

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
        bool grounded = false;

        foreach (Transform square in body)
        {
            RaycastHit2D hit = Physics2D.BoxCast(square.transform.position, Vector2.one * 0.8f, 0, Vector2.down, 0.1f, jumpingMask);

            grounded |= hit.collider != null;
        }

        if(grounded && !groundedLastFrame)
        {
            if(landed) landingAudio.Play();
            landed = true;
        }

        if (jump.action.IsPressed() && rb.velocity.y <= 0.2f)
        {
            if (grounded)
            {
                jumpAudio.Play();
                rb.velocity += Vector2.up * jumpForce;
                return;
            }
        }

        groundedLastFrame = grounded;
    }

    private void OnDestroy()
    {
        groundChecks.Clear();
    }
}
