using System.Collections;
using UnityEngine;

public class confidence : MonoBehaviour
{
    public Transform otherCharacter;

    private movement_controller controller;
    
    private double confidenceMultiplier = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<movement_controller>();
        StartCoroutine(IncreaseConfidence());
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, otherCharacter.position);

        
    }
    
    

    private IEnumerator IncreaseConfidence()
    {
        while (true)
        {
            confidenceMultiplier += 0.1;
            yield return new WaitForSeconds(1f);
        }
    }

    public void resetMultiplier()
    {
        confidenceMultiplier = 1;
    }
    
    public double getMultiplier()
    {
        return confidenceMultiplier;
    }
}
