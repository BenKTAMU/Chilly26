using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    private BoxCollider2D hitbox;
    public SpriteRenderer sr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
    }
    public void Hit()
    {
        Invoke("ActivateHitbox", 0.2f);
        Invoke("DeactivateHitbox", 0.4f);
    }

    void ActivateHitbox()
    {
        hitbox.gameObject.SetActive(true);
        sr.enabled = true;

    }

    void DeactivateHitbox()
    {
        hitbox.gameObject.SetActive(false);
        sr.enabled = false;
    }
}