using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    private BoxCollider2D hitbox;
    public SpriteRenderer sr;

    private Vector2 direction = Vector2.right;
    private bool didOffset = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
    }
    public void Hit(Vector2 direction)
    {
        Invoke("ActivateHitbox", 0.2f);
        Invoke("DeactivateHitbox", 0.4f);

        if (!didOffset)
        {
            direction.y = 0;
            if (direction.normalized != Vector2.zero) this.direction = direction.normalized;
        }

    }

    void ActivateHitbox()
    {
        //hitbox.gameObject.transform.localPosition = direction;
        /*if (!didOffset)
        {
            Vector3 direction3 = direction;
            hitbox.gameObject.transform.position += direction3;
            didOffset = true;
        }*/
        hitbox.gameObject.SetActive(true);
        sr.enabled = true;
    }

    void DeactivateHitbox()
    {
        /*if (didOffset)
        {
            Vector3 direction3 = direction;
            hitbox.gameObject.transform.position -= direction3;
            didOffset = false;
        }*/
        hitbox.gameObject.SetActive(false);
        sr.enabled = false;
    }
}