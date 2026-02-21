using System;
using System.Collections;
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
    
    private float leftPower = -1.0f;
    private float rightPower = -1.0f;

    public Transform start_position;
    private healthAndDamage health;
    private confidence confidence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        meleeAttack = GetComponent<meleeAttack>();
        rangedAttack = GetComponent<rangedAttack>();
        health = GetComponent<healthAndDamage>();
        confidence = GetComponent<confidence>();
    }

    public void Reset()
    {
        rb.transform.position = start_position.position;
        health.health = health.total_health;
    }

    void Rumble(float lowFreq, float highFreq, float duration)
    {
        Gamepad gp = null;
        if (Gamepad.all.Count >= 1 && player1) gp = Gamepad.all[0];
        if (Gamepad.all.Count >= 2 && !player1) gp = Gamepad.all[1];
        if (gp == null) return;
        StartCoroutine(RumbleCoroutine(lowFreq, highFreq, duration, gp));
    }
    
    private IEnumerator RumbleCoroutine(float lowFreq, float highFreq, float duration, Gamepad gp)
    {
        gp.SetMotorSpeeds(lowFreq, highFreq);
        yield return new WaitForSeconds(duration);
        gp.SetMotorSpeeds(0, 0);
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
        if (health.health <= 0)
        {
            GameObject.FindWithTag("Player 1").GetComponent<movement_controller>().Reset();
            GameObject.FindWithTag("Player 2").GetComponent<movement_controller>().Reset();
        }

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

            if (gp.xButton.isPressed)
            {
                meleeAttack.Hit(amount);
            }

            if (gp.rightTrigger.isPressed)
            {
                Vector2 direction = new Vector2();
                direction.x = x;
                direction.y = gp.leftStick.up.magnitude;
                if (direction.y == 0) direction.y = -gp.leftStick.down.magnitude;

                rangedAttack.SetAim(direction);
                rangedAttack.Shoot();

                leftPower = 0.5f;
                rightPower = 0f;
                
                
            }
            
            
            Debug.Log(rightPower + " " +leftPower);
            if (amount.x < 0)
            {
                leftPower = rightPower;
                rightPower = leftPower;
            }
            if (rightPower > -0.01)
            {
                gp.SetMotorSpeeds(leftPower, rightPower);

                if (leftPower > 0)
                {
                    leftPower -= 0.2f;
                }
                else 
                {
                    leftPower = 0;
                    rightPower += 0.1f;
                    if (rightPower > 0.5f)
                    {
                        rightPower = -1.0f;
                        
                    }
                }
            }
            else
            {
                gp.SetMotorSpeeds(0, 0);
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

            if ((Keyboard.current.digit1Key.isPressed && player1) || (Keyboard.current.periodKey.isPressed && !player1))
            {
                meleeAttack.Hit(direction);
            }

            if ((Keyboard.current.digit2Key.isPressed && player1) || (Keyboard.current.slashKey.isPressed && !player1))
            {
                rangedAttack.SetAim(direction);
                rangedAttack.Shoot();
            }

            MovePlayer(direction);
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
