using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IUnit
{
    // Variáveis públicas para configuração no Editor
    public Transform target;
    public MovingObject movement;
    public GameObject circlePrefab;
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
    private bool shouldMove = true; // Começa com movimento
    private bool hasLineOfSight = false;
    public int rangedAttackPoints;


    // Propriedades para controle de estado
    public bool IsPlaying { get; set; }
    public bool CanPlay { get; set; }

    private Vector3Int lastTargetPosition; // Armazena a última posição do alvo


    // Inicializa variáveis e componentes
    void Start()
    {
        this.CanPlay = true;
        currentTurnsToWait = turnsToWait;
        maxTurnsToWait = turnsToWait;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GetComponent<MovingObject>();
        shotCooldown = startShotCooldown;

                // Inicializa o componente Melee se ele não existir
        Melee meleeComponent = GetComponent<Melee>();
        if (meleeComponent == null)
            meleeComponent = gameObject.AddComponent<Melee>();

        // Inicializa a referência para o script RaycastHandler
        raycastHandler = GetComponent<RaycastHandler>();
        if (raycastHandler == null)
            raycastHandler = gameObject.AddComponent<RaycastHandler>();

        // Inicializa o sistema de pathfinding
        pathfinding = new AStar(movement.groundTilemap, movement.collisionTilemap);

        lastTargetPosition = movement.groundTilemap.WorldToCell(target.position); // Inicializa com a posição atual do alvo

    }

    // Atualiza a variável path a cada FixedUpdate
    private void FixedUpdate()
    {
        Vector3Int currentTargetPosition = movement.groundTilemap.WorldToCell(target.position);

        // Verifica se o caminho atual terminou ou se o destino mudou
        if (pathIndex >= path?.Count || currentTargetPosition != lastTargetPosition)
        {
            Vector3Int startPos = movement.groundTilemap.WorldToCell(transform.position);
            Vector3Int targetPos = movement.groundTilemap.WorldToCell(target.position);
            path = pathfinding.FindPath(startPos, targetPos);
            pathIndex = 0;
            lastTargetPosition = currentTargetPosition; // Atualiza a última posição do alvo
        }
    }

    // Atira na direção especificada
    private void Shoot(Vector2 direction)
    {
        if (shotCooldown <= 0)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
            EnemyBullet bulletScript = bulletInstance.GetComponent<EnemyBullet>();
            bulletScript.SetDirection(direction);

            bulletScript.OnMaxDistanceReached += HandleBulletMaxDistanceReached;

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

            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            EnemyBullet bulletPrefabScript = bullet.GetComponent<EnemyBullet>();

            // Verifica se o inimigo pode atacar
            if (hasLineOfSight && distanceToTarget <= bulletPrefabScript.MaxDistance)
            {
                Vector2 raycastDirection = raycastHandler.LastRaycastDirection;
                Shoot(raycastDirection);
            }
            // Caso contrário, move o inimigo
            else if (path != null && path.Count > 0 && pathIndex < path.Count)
            {
                Vector3 nextStep = movement.groundTilemap.CellToWorld(path[pathIndex]);
                movement.AttemptMove((nextStep - transform.position).normalized);
                pathIndex++;
            }
          
            currentTurnsToWait = maxTurnsToWait;
        }

        yield return new WaitForSeconds(time / 2);
        IsPlaying = false;
    }

     // Método que será chamado quando o projétil atingir sua distância máxima
    private void HandleBulletMaxDistanceReached()
    {
 
        Debug.Log("Bullet reached its max distance!");

      
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        bulletScript.OnMaxDistanceReached -= HandleBulletMaxDistanceReached;
     
    }
}
