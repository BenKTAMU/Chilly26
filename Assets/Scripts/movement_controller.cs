using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class movement_controller : MonoBehaviour
{
    public float acceleration = 5f;
    public float jump_force = 1f;
    public float max_speed = 2f;
    public float max_jump_time = 0.3f;

    public bool player1 = true;

    public GameObject arm;
    private SpriteRenderer asr;

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
    private SpriteRenderer sr;
    private Animator animator;
    private Animator arm_animator;

    public AudioSource soundeffects;

    public AudioClip hit_ground;
    public AudioClip shoot;

    private Vector2 last_direction;

    public UnityEngine.UI.Image healthBar;

    public TextMeshProUGUI scoreText;
    int player_one_score = 0;
    int player_two_score = 0;


    private int swing_frames = 0;

    public GameObject winnerPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        meleeAttack = GetComponent<meleeAttack>();
        rangedAttack = GetComponent<rangedAttack>();
        health = GetComponent<healthAndDamage>();
        confidence = GetComponent<confidence>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        arm.transform.SetParent(transform, worldPositionStays: true);
        asr = arm.GetComponent<SpriteRenderer>();
        arm_animator = arm.GetComponent<Animator>();

        facing_left = !player1;
    }

    void PauseAndShowScore()
    {
        winnerPanel.SetActive(true);

    }

    public void Reset()
    {
        rb.transform.position = start_position.position;
        rb.linearVelocity = Vector2.zero;

        health.health = health.total_health;
        rb.linearVelocity = Vector2.zero;
        healthBar.fillAmount = 1;
        confidence.resetMultiplier();
    }


    private long rumbling_count = 0;
    public void Rumble(float lowFreq, float highFreq, float duration)
    {
        Gamepad gp = null;
        if (Gamepad.all.Count >= 1 && player1) gp = Gamepad.all[0];
        if (Gamepad.all.Count >= 2 && !player1) gp = Gamepad.all[1];
        if (gp == null) return;
        StartCoroutine(RumbleCoroutine(lowFreq, highFreq, duration, gp));
    }

    private IEnumerator RumbleCoroutine(float lowFreq, float highFreq, float duration, Gamepad gp)
    {
        Interlocked.Increment(ref rumbling_count);
        gp.SetMotorSpeeds(lowFreq, highFreq);
        yield return new WaitForSeconds(duration);
        Interlocked.Decrement(ref rumbling_count);
        gp.SetMotorSpeeds(0, 0);
    }

    private bool facing_left = false;
    void MovePlayer(Vector2 amount)
    {
        rb.AddForce(amount.x * acceleration * Vector2.right);
        if (Math.Abs(rb.linearVelocityX) > max_speed)
            rb.linearVelocity = new Vector2(max_speed * Math.Sign(rb.linearVelocityX), rb.linearVelocityY);


        if (amount.x < -0.1)
        {
            if (!facing_left)
            {
                facing_left = true;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else if (amount.x > 0.1)
        {
            if (facing_left)
            {
                transform.Rotate(new Vector3(0, -180, 0));
                facing_left = false;
            }
        }


        if (amount.x != 0) last_direction.x = amount.x;

        if (amount.y > 0)
        {
            if (isGrounded)
            {
                arm.transform.localPosition = new Vector2(-0.197f, -0.184f);
                animator.Play("Jump Up");
                if (swing_frames <= 0)
                    arm_animator.Play("Jump Up");
                rb.linearVelocityY = jump_force;
                startJump = Time.time;
                Rumble(1.0f, 0.0f, 0.05f);
            }
            else
            {
                if (Time.time - startJump < max_jump_time)
                {
                    rb.linearVelocityY = jump_force;
                }
            }
        }
        else if (rb.linearVelocityY < -0.1)
        {
            if (swing_frames <= 0)
                arm_animator.Play("Jump Down");
            animator.Play("Jump Down");
            arm.transform.localPosition = new Vector2(-0.475f, -0.141f);
        }
        else
        {
            if (Math.Abs(rb.linearVelocityX) > 0.1)
            {
                if (swing_frames <= 0)
                {
                    arm_animator.Play("Running");
                    arm.transform.localPosition = new Vector2(0.287f, 0.086f);
                }
                animator.Play("Running");
            }
            else
            {
                if (swing_frames <= 0)
                {
                    arm_animator.Play("Idle");
                    arm.transform.localPosition = new Vector2(-0.206f, -0.168f);
                }
                animator.Play("Idle");
            }
        }


    }

    void FixedUpdate()
    {
        if (swing_frames > 0) swing_frames--;

        if (health.health <= 0)
        {
            if (player1)
            {
                player_two_score++;
                scoreText.text = player_one_score + " - " + player_two_score;
            }
            else
            {
                player_one_score++;
                scoreText.text = player_one_score + " - " + player_two_score;
            }
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
                arm.transform.localPosition = new Vector2(-0.16f, 0.38f);
                arm_animator.Play("Hit");
                swing_frames = 20;

                meleeAttack.Hit(facing_left ? Vector2.left : Vector2.right);
                Rumble(0.0f, 0.7f, 0.1f);
            }

            if (gp.rightTrigger.isPressed && confidence.getMultiplier() == 5)
            {
                Vector2 direction = new Vector2();
                direction.x = x;
                direction.y = gp.leftStick.up.magnitude;
                if (direction.y == 0) direction.y = -gp.leftStick.down.magnitude;

                rangedAttack.SetAim(facing_left ? Vector2.left : Vector2.right);
                if (rangedAttack.Shoot())
                {
                    soundeffects.PlayOneShot(shoot);
                    arm.transform.localPosition = new Vector2(-0.122f, 0.359f);
                    arm_animator.Play("Throw");
                    swing_frames = 20;
                }

                leftPower = 0.5f;
                rightPower = 0f;
                StartCoroutine(ShootRumbleStop(gp, 1.0f));
            }


            if (rightPower > -0.01)
            {
                if (facing_left)
                    gp.SetMotorSpeeds(rightPower, leftPower * 1.5f);
                else gp.SetMotorSpeeds(leftPower, rightPower);

                if (leftPower > 0)
                {
                    leftPower -= 0.2f;
                }
                else
                {
                    leftPower = -1f;
                    rightPower += 0.1f;
                    if (rightPower > 0.5f)
                    {
                        rightPower = -1.0f;
                        gp.SetMotorSpeeds(0, 0);
                    }
                }
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
                arm.transform.localPosition = new Vector2(-0.122f, 0.359f);
                meleeAttack.Hit(facing_left ? Vector2.right : Vector2.left);
                arm_animator.Play("Hit");
                swing_frames = 20;
            }

            if (((Keyboard.current.digit2Key.isPressed && player1) || (Keyboard.current.slashKey.isPressed && !player1)) && confidence.getMultiplier() > 4.9)
            {
                rangedAttack.SetAim(facing_left ? Vector2.left : Vector2.right);
                if (rangedAttack.Shoot())
                {
                    soundeffects.PlayOneShot(shoot);
                    arm.transform.localPosition = new Vector2(-0.122f, 0.359f);
                    arm_animator.Play("Throw");
                    swing_frames = 20;
                }
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

    IEnumerator ShootRumbleStop(Gamepad gp, float delay)
    {
        yield return new WaitForSeconds(delay);
        leftPower = -1f;
        rightPower = -1f;
        gp.SetMotorSpeeds(0, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (collision.GetContact(0).normal.y > 0)
            { // only jump if player collides with ground not walls
                isGrounded = true;
                soundeffects.PlayOneShot(hit_ground);
                Rumble(Math.Abs(collision.relativeVelocity.y) / 50, 0.0f, Math.Abs(collision.relativeVelocity.y) / 200);
                if (Math.Abs(rb.linearVelocity.x) < 0.1)
                {
                    animator.Play("Idle");
                    arm_animator.Play("Idle");
                    //arm.transform.localPosition = new Vector2(-0.206f, -0.168f);
                }
                else
                {
                    arm_animator.Play("Running");
                    animator.Play("Running");
                    arm.transform.localPosition = new Vector2(0.287f, 0.086f);
                }
            }
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
