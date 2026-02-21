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
        if (direction != Vector2.zero) aimDirection = direction.normalized;
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

    public void Shoot()
    {
        if (shotTimer > 0) return;
        shotTimer = shotCooldown;

        Vector3 aimDirection3 = aimDirection * startOffsetAmount;

        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position + aimDirection3, Quaternion.identity);
        Debug.Log(projectile + ", " + aimDirection);
        projectile.GetComponent<projectile>().direction = aimDirection;
    }
}
