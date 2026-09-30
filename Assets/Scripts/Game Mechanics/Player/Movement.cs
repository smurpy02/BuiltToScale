using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Player player;

    [Header("Physical Components")]
    public Rigidbody2D body2D;
    public Transform body;

    [Header("Physics Parameters")]
    public float speed, jumpForce;

    [Header("Other")]
    public LayerMask jumpingMask;

    [Header("Events")]
    public Action jump, land;

    List<Transform> groundChecks = new List<Transform>();
    bool groundedLastFrame = true, isGrounded;
    float notGrounded = 0;

    private void Update()
    {
        PhysicsChecks();
    }

    public void AddGroundCheck(Transform groundCheck) => groundChecks.Add(groundCheck);
    public void RemoveGroundCheck(Transform groundCheck) => groundChecks.Remove(groundCheck);

    public void HorizontalInput(float horizontal)
    {
        HorizontalMovement(horizontal);
    }

    public void Jump()
    {
        if (CheckJump())
        {
            jump.Invoke();
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

        foreach (var square in groundChecks)
        {
            var hit = Physics2D.BoxCast(square.transform.position, Vector2.one * .8f, 0, Vector2.down, .1f, jumpingMask);

            groundedCheck |= hit.collider != null;
        }

        isGrounded = groundedCheck;

        if (!isGrounded && groundedLastFrame) notGrounded = 0;
        if (!isGrounded) notGrounded += Time.deltaTime;

        if (isGrounded && !groundedLastFrame && notGrounded > .1f)
        {
            land.Invoke();
            PlayerAudioManager.Land();
        }

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
