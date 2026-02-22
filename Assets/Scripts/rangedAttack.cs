using UnityEditor;
using UnityEngine;


public class rangedAttack : MonoBehaviour
{

    public GameObject projectilePrefab;

    public Transform launchPoint;

    public float shotCooldown = 0.5f;

    private float shotTimer;

    private Vector2 aimDirection = Vector2.right;

    public float startOffsetAmount = 1;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }
    }

    public void SetAim(Vector2 direction)
    {
        if (direction.x != 0)
        {
            aimDirection = direction;
            aimDirection.y = 0;
            aimDirection = aimDirection.normalized;
        }
    }

    /*void Aim()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
        }
    }*/

    public bool Shoot()
    {
        if (GetComponent<confidence>().getMultiplier() < 4.9) return false;

        if (shotTimer > 0) return false;
        shotTimer = shotCooldown;

        Vector3 aimDirection3 = aimDirection * startOffsetAmount * 3;

        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position + aimDirection3, Quaternion.identity);
        projectile.GetComponent<projectile>().direction = aimDirection;
        projectile.GetComponent<projectile>().sender = gameObject.tag;

        GetComponent<confidence>().resetMultiplier();

        return true;
    }
}
