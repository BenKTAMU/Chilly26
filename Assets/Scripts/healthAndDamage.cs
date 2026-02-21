using UnityEngine;

public class healthAndDamage : MonoBehaviour
{

    public double health = 100.0;

    public double hit_amount = 5;
    public double smack_amount = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
