using UnityEngine;
using UnityEngine.UI;

public class healthAndDamage : MonoBehaviour
{

    public float health = 100.0f;
    public float total_health = 100f;

    public float hit_amount = 5.0f;
    public float smack_amount = 50.0f;
    
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

    public void hit()
    {
        health -= hit_amount;
        healthBar.fillAmount = health / total_health;
    }

    public void smack()
    {
        health -= smack_amount;
        healthBar.fillAmount = health / total_health;
    }
}
