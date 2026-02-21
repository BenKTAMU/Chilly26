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

    private meleeAttack meleeAttack;
    private rangedAttack rangedAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        meleeAttack = GetComponent<meleeAttack>();
        rangedAttack = GetComponent<rangedAttack>();
    }

    void MovePlayer(Vector2 amount)
    {
        rb.AddForce(amount.x * acceleration * Vector2.right);
        if (Math.Abs(rb.linearVelocityX) > max_speed)
            rb.linearVelocity = new Vector2(max_speed * Math.Sign(rb.linearVelocityX), rb.linearVelocityY);

        if (amount.y > 0)
        {
            if (isGrounded)
            {
                rb.linearVelocityY = jump_force;
                startJump = Time.time;
            }
            else
            {
                if (Time.time - startJump < max_jump_time)
                {
                    rb.linearVelocityY = jump_force;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Gamepad gp = null;
        if (Gamepad.all.Count >= 1 && player1) gp = Gamepad.all[0];
        if (Gamepad.all.Count >= 2 && !player1) gp = Gamepad.all[1];

        if (gp != null)
        {
            float x = gp.leftStick.right.magnitude;
            if (x == 0) x = -gp.leftStick.left.magnitude;
            Vector2 amount = new Vector2(x, 0);

            if (gp.aButton.isPressed)
            {
                amount.y = 1;
            }

            if (gp.bButton.isPressed)
            {
                meleeAttack.Hit();
            }

            if (gp.rightShoulder.isPressed)
            {
                Vector2 direction = new Vector2();
                direction.x = x;
                direction.y = gp.leftStick.up.magnitude;
                if (direction.y == 0) direction.y = -gp.leftStick.down.magnitude;

                rangedAttack.SetAim(direction);
                rangedAttack.Shoot();
            }

            MovePlayer(amount);
        }
        else
        {
            Vector2 direction = new Vector2();
            if ((Keyboard.current.aKey.isPressed && player1) || (Keyboard.current.leftArrowKey.isPressed && !player1))
            {
                direction.x = -1;
            }
            else if ((Keyboard.current.dKey.isPressed && player1) || (Keyboard.current.rightArrowKey.isPressed && !player1))
            {
                direction.x = 1;
            }

            if ((Keyboard.current.wKey.isPressed && player1) || (Keyboard.current.upArrowKey.isPressed && !player1))
            {
                direction.y = 1;
            }
        }

        /*int direction = 0;
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
            */
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
