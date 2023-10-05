using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IUnit
{
    // Variáveis públicas para configuração no Editor
    public Transform target;
    public MovingObject movement;
    public GameObject bullet;
    public float startShotCooldown;
    public Transform attackPlayer;
    public int turnsToWait = 0;

    // Variáveis privadas para controle interno
    private int currentTurnsToWait;
    private int maxTurnsToWait;
    private float shotCooldown;
    private RaycastHandler raycastHandler;
    private AStar pathfinding;
    private List<Vector3Int> path;
    private int pathIndex;

    // Propriedades para controle de estado
    public bool IsPlaying { get; set; }
    public bool CanPlay { get; set; }

    // Inicializa variáveis e componentes
    void Start()
    {
        this.CanPlay = true;
        currentTurnsToWait = turnsToWait;
        maxTurnsToWait = turnsToWait;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GetComponent<MovingObject>();
        shotCooldown = startShotCooldown;

        // Inicializa a referência para o script RaycastHandler
        raycastHandler = GetComponent<RaycastHandler>();
        if (raycastHandler == null)
            raycastHandler = gameObject.AddComponent<RaycastHandler>();

        // Inicializa o sistema de pathfinding
        pathfinding = new AStar(movement.groundTilemap, movement.collisionTilemap);
    }

    // Atualiza a variável path a cada FixedUpdate
    private void FixedUpdate()
    {
        Vector3Int start = movement.groundTilemap.WorldToCell(transform.position);
        Vector3Int end = movement.groundTilemap.WorldToCell(target.position);
        path = pathfinding.FindPath(start, end);
        pathIndex = 0;
    }

    // Atira na direção especificada
    private void Shoot(Vector2 direction)
    {
        if (shotCooldown <= 0)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
            EnemyBullet bulletScript = bulletInstance.GetComponent<EnemyBullet>();
            bulletScript.SetDirection(direction);

            shotCooldown = startShotCooldown;
        }
        else
        {
            shotCooldown -= Time.deltaTime;
        }
    }

    // Controla o comportamento do inimigo durante sua "jogada"
    public IEnumerator Play(float time)
    {
        if (currentTurnsToWait >= 1)
        {
            currentTurnsToWait--;
        }
        else
        {
            yield return new WaitForSeconds(time / 2);

            if (path != null && path.Count > 0 && pathIndex < path.Count)
            {
                Vector3 nextStep = movement.groundTilemap.CellToWorld(path[pathIndex]);
                movement.AttemptMove((nextStep - transform.position).normalized);
                pathIndex++;
            }
            else
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                EnemyBullet bulletPrefabScript = bullet.GetComponent<EnemyBullet>();

                if (raycastHandler.HasLineOfSightTo(target) && distanceToTarget <= bulletPrefabScript.MaxDistance)
                {
                    Vector2 raycastDirection = raycastHandler.LastRaycastDirection;
                    Shoot(raycastDirection);
                }
            }

            currentTurnsToWait = maxTurnsToWait;
        }

        yield return new WaitForSeconds(time / 2);
        IsPlaying = false;
    }
}
