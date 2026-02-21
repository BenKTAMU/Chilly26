using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class rangedAttack : MonoBehaviour
{
    
    public GameObject projectilePrefab;

    public Transform launchPoint;

    public float shotCooldown = 0.5f;

    private float shotTimer;
    
    private Vector2 aimDirection = Vector2.right;

    
    
    
    
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

        Aim();

        if (Input.GetKeyDown(KeyCode.E) && shotTimer <= 0  || Input.GetKeyDown(KeyCode.RightShift) && shotTimer <= 0)
        {
            Shoot();
            shotTimer = shotCooldown;
        }
    }

    void Aim()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
        }
    }
    
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        projectile.GetComponent<projectile>().direction = aimDirection;
    }


}
