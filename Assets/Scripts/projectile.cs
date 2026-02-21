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
}
