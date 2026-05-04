using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Physical Components")]
    public Rigidbody2D bodyPhysics;
    public Transform bodyTransform;
    public static List<Transform> groundChecks = new List<Transform>();

    [Header("Physics Parameters")]
    public float speed, jumpForce;
    public bool invert = false;

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
            PlayerAudioManager.instance.Jump();
            bodyPhysics.linearVelocity += Vector2.up * jumpForce;
        }
    }

    void HorizontalMovement(float horizontal)
    {
        if (invert) horizontal *= -1;

        Vector2 velocity = bodyPhysics.linearVelocity;
        velocity.x = horizontal * speed;
        bodyPhysics.linearVelocity = velocity;
    }

    void PhysicsChecks()
    {
        bool groundedCheck = false;

        foreach (Transform square in bodyTransform)
        {
            RaycastHit2D hit = Physics2D.BoxCast(square.transform.position, Vector2.one * 0.8f, 0, Vector2.down, 0.1f, jumpingMask);

            groundedCheck |= hit.collider != null;
        }

        isGrounded = groundedCheck;

        if (isGrounded && !groundedLastFrame) PlayerAudioManager.instance.Land();

        groundedLastFrame = isGrounded;
    }

    bool CheckJump()
    {
        return isGrounded && bodyPhysics.linearVelocity.y <= .2f;
    }

    private void OnDestroy()
    {
        groundChecks.Clear();
    }
}
