using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class confidence : MonoBehaviour
{
    public Transform otherCharacter;

    private movement_controller controller;
    
    private double confidenceMultiplier = 0.0;
    
    public Image confidenceBar;

    private Coroutine curr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<movement_controller>();
       // curr = StartCoroutine(IncreaseConfidence());
    }

    // Update is called once per frame
    void Update()
    {

        confidenceMultiplier += Time.deltaTime;
        confidenceBar.fillAmount = (float)(confidenceMultiplier / 5.0);
        if (confidenceMultiplier > 5) confidenceMultiplier = 5;

    }
    
    

    // private IEnumerator IncreaseConfidence()
    // {
    //     while (true)
    //     {
    //         confidenceMultiplier += 0.3f;
    //             confidenceBar.fillAmount = (float)(confidenceMultiplier / 5.0);
    //             if (confidenceMultiplier > 5) confidenceMultiplier = 5;
    //         yield return new WaitForSeconds(1f);
    //     }
    // }
    //
    

    public void resetMultiplier()
    {
        confidenceMultiplier = 0.0;
        confidenceBar.fillAmount = 0;
    }

    // private IEnumerator reset()
    // {
    //     StopCoroutine(curr);
    //     confidenceMultiplier = 0.0f;
    //     yield return new WaitForSeconds(1f);
    //     curr = StartCoroutine(IncreaseConfidence());
    // }
    //
    public double getMultiplier()
    {
        return confidenceMultiplier;
    }
}
