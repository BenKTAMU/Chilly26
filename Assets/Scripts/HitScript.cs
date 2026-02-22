using UnityEngine;

public class HitScript : MonoBehaviour
{
    public GameObject parent;

    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player 1" && other.tag != "Player 2") return;
        if (!sr.enabled) return;

        GameObject player = other.gameObject;
        player.GetComponent<healthAndDamage>().hit(other.gameObject.transform.position - parent.transform.position);
        player.GetComponent<confidence>().resetMultiplier();
    }
}
