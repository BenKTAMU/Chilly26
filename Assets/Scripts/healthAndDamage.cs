using UnityEngine;
using UnityEngine.UI;

public class healthAndDamage : MonoBehaviour
{

    public float health = 100.0f;
    public float total_health = 100f;

    public float hit_amount = 5.0f;
    public float smack_amount = 50.0f;

    public float hit_impulse = 5f;
    public float smack_impulse = 10f;

    public Image healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = total_health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hit(Vector2 directionToMe)
    {
        Vector2 newPos = gameObject.GetComponent<Rigidbody2D>().transform.position;
        newPos.y += 0.1f;
        gameObject.GetComponent<Rigidbody2D>().transform.position = newPos;
        gameObject.GetComponent<Rigidbody2D>().AddForce(directionToMe * hit_impulse + Vector2.up * 0.1f, ForceMode2D.Force);
        health -= hit_amount;
        healthBar.fillAmount = health / total_health;
    }

    public void smack(Vector2 directionToMe)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(directionToMe * smack_impulse, ForceMode2D.Force);
        health -= smack_amount;
        healthBar.fillAmount = health / total_health;
    }
}
