using UnityEngine;

public class healthAndDamage : MonoBehaviour
{

    public double health = 100.0;
    public double total_health = 100;

    public double hit_amount = 5;
    public double smack_amount = 50;

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
        Debug.Log("Hit: " + health);
    }

    public void smack()
    {
        health -= smack_amount;
        Debug.Log("Smacked: " + health);
    }
}
