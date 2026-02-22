using UnityEngine;

public class projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;

    public Vector2 direction;

    public float lifetime = 2f;

    public string sender;

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
        if (other.CompareTag(sender)) return;
        if (other.CompareTag("Player 1") || other.CompareTag("Player 2"))
        {
            double confidence = other.GetComponent<confidence>().getMultiplier();
            other.GetComponent<confidence>().resetMultiplier();

            other.GetComponent<healthAndDamage>().smack(other.gameObject.transform.position - gameObject.transform.position);
        }



        Destroy(gameObject);
    }
}
