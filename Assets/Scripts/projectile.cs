using UnityEngine;

public class projectile : MonoBehaviour
{
    public float projectileSpeed = 8f;

    public Vector2 direction;

    public float lifetime = 2f;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * projectileSpeed * Time.deltaTime);
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player 1") || other.CompareTag("Player 2"))
        {
            double confidence = other.GetComponent<confidence>().getMultiplier();
            other.GetComponent<confidence>().resetMultiplier();
            
            
            Debug.Log("Player hit!");
        }
        
        
        
        Destroy(gameObject);
    }
}
