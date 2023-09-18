using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IUnit
{
    public Transform target;
    public MovingObject movement;
    public GameObject circlePrefab;

    public Melee meleeComponent;

    private bool hasLineOfSight = false;

    public GameObject bullet;
    private float shotCooldown;
    public float startShotCooldown;
    public Transform attackPlayer;

    public bool IsPlaying { get; set; }
    public bool CanPlay { get; set; }

    void Start()
    {
        this.CanPlay = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GetComponent<MovingObject>();

        shotCooldown = startShotCooldown;

        Damageable damage = GetComponent<Damageable>();

        meleeComponent = GetComponent<Melee>();
        if (meleeComponent == null)
            meleeComponent = gameObject.AddComponent<Melee>();
    }

    private void FixedUpdate()
    {
        hasLineOfSight = CheckLineOfSight();
    }

    private bool CheckLineOfSight()
    {
        if (target == null)
            return false;

    Vector2 direction = (target.transform.position - transform.position);
    RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);

    if (ray.collider != null)
    {
        Debug.Log("Ray hit: " + ray.collider.gameObject.name);  // Debugging line

        if (ray.collider.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, direction, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, direction, Color.red);
        }
    }
    return false;
    }


   private void Shoot(Vector2 direction)
{
    if (shotCooldown <= 0)
    {
        GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
        EnemyBullet bulletScript = bulletInstance.GetComponent<EnemyBullet>();
        bulletScript.SetDirection(direction.normalized);
        shotCooldown = startShotCooldown;
    }
    else
    {
        shotCooldown -= Time.deltaTime;
    }
}

    public IEnumerator Play(float time)
    {
        yield return new WaitForSeconds(time / 2);
        
		Vector2 posDif = new Vector2(transform.position.x - target.position.x, transform.position.y - target.position.y);
        // transform.up = posDif; 
         
        float absX = Mathf.Abs(posDif.x);
        float absY = Mathf.Abs(posDif.y);
        Vector2 moveDirection;

        if (absX > absY || absY == 0)
        {
            moveDirection = posDif.x < 0 ? Vector2.right : Vector2.left;
        }
        else 
        {
            moveDirection = posDif.y < 0 ? Vector2.up : Vector2.down;
        }

        movement.AttemptMove(moveDirection);
        yield return new WaitForSeconds(time/2);
        IsPlaying = false;

        if (hasLineOfSight)
        {
            Shoot(posDif);
        }
    }

}


    