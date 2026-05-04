using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Player player;

    [Header("Physical Components")]
    public Rigidbody2D body2D;
    public Transform body;
    public static List<Transform> groundChecks = new List<Transform>();

    [Header("Physics Parameters")]
    public float speed, jumpForce;

    [Header("Other")]
    public LayerMask jumpingMask;

    bool groundedLastFrame = true, isGrounded;

    private void Update()
    {
        PhysicsChecks();
    }

    public void HorizontalInput(float horizontal)
    {
        HorizontalMovement(horizontal);
    }

    public void Jump()
    {
        if (CheckJump())
        {
            PlayerAudioManager.Jump();
            Vector2 velocity = body2D.linearVelocity;
            velocity.y = jumpForce;
            body2D.linearVelocity = velocity;
        }
    }

    void HorizontalMovement(float horizontal)
    {
        if (player.invert) horizontal *= -1;

        Vector2 velocity = body2D.linearVelocity;
        velocity.x = horizontal * speed;
        body2D.linearVelocity = velocity;
    }

    void PhysicsChecks()
    {
        bool groundedCheck = false;

        foreach (Transform square in body)
        {
            RaycastHit2D hit = Physics2D.BoxCast(square.transform.position, Vector2.one * 0.8f, 0, Vector2.down, 0.1f, jumpingMask);

            groundedCheck |= hit.collider != null;
        }

        isGrounded = groundedCheck;

        if (isGrounded && !groundedLastFrame) PlayerAudioManager.Land();

        groundedLastFrame = isGrounded;
    }

    bool CheckJump()
    {
        return isGrounded && body2D.linearVelocity.y <= .2f;
    }

    private void OnDestroy()
    {
        groundChecks.Clear();
    }
}
