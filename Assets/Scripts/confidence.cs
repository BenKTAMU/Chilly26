using UnityEngine;

public class confidence : MonoBehaviour
{
    public Transform otherCharacter;
    
    private movement_controller controller;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector2.Distance(transform.position, otherCharacter.position);
        if (controller.player1)
        {
            Debug.Log("Player 1: " + distance);
        }
        else
        {
            Debug.Log("Player 2: " + distance);
        }
    }
}
