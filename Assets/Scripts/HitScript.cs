using UnityEngine;

public class HitScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = other.gameObject;
        Debug.Log(player.tag);
        player.GetComponent<healthAndDamage>().hit();
        player.GetComponent<confidence>().resetMultiplier();
    }
}
