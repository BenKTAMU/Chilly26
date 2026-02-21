using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement_controller : MonoBehaviour
{
    public float acceleration = 5f;
    public float jump_force = 1f;
    public float max_speed = 2f;
    public float max_jump_time = 0.3f;

    public bool player1 = true;

    private Rigidbody2D rb;

    private bool isGrounded = false;

    private float startJump = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        int direction = 0;
        if ((Keyboard.current.aKey.isPressed && player1) || (Keyboard.current.leftArrowKey.isPressed && !player1))
        {
            direction = -1;
        }
        else if ((Keyboard.current.dKey.isPressed && player1) || (Keyboard.current.rightArrowKey.isPressed && !player1))
        {
            direction = 1;
        }

        if ((Keyboard.current.wKey.isPressed && player1) || (Keyboard.current.upArrowKey.isPressed && !player1))
        {
            if (isGrounded)
            {
                rb.linearVelocityY = jump_force;
                //rb.AddForce(jump_force * Vector2.up, ForceMode2D.Impulse);
                startJump = Time.time;
            }
            else
            {
                if (Time.time - startJump < max_jump_time)
                    rb.linearVelocityY = jump_force;
            }
        }

        rb.AddForce(direction * acceleration * Vector2.right);
        if (Math.Abs(rb.linearVelocityX) > max_speed)
            rb.linearVelocity = new Vector2(max_speed * Math.Sign(rb.linearVelocityX), rb.linearVelocityY);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (collision.GetContact(0).normal.y > 0) // only jump if player collides with ground not walls
                isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }
}
