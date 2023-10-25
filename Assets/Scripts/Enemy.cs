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
    private List<Vector2> path;
    private int pathIndex;
    private bool shouldMove = true; // Começa com movimento
    private bool hasLineOfSight = false;
    public int rangedAttackPoints;

    // Propriedades para controle de estado
    public bool IsPlaying { get; set; }
    public bool CanPlay { get; set; }

    private Vector2 lastTargetPosition; // Armazena a última posição do alvo

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

        lastTargetPosition = new Vector2(
            movement.groundTilemap.WorldToCell(target.position).x, 
            movement.groundTilemap.WorldToCell(target.position).y
        );
    }

    // Atualiza a variável path a cada FixedUpdate
    private void FixedUpdate()
    {
        Vector2 currentTargetPosition = new Vector2(target.position.x, target.position.y);

        if (pathIndex >= path?.Count || currentTargetPosition != lastTargetPosition)
        {
            StopCoroutine("CalculatePath");
            StartCoroutine(CalculatePath(currentTargetPosition));
        }

        // Atualiza a variável hasLineOfSight
        hasLineOfSight = raycastHandler.HasLineOfSightTo(target);
    }

    private IEnumerator CalculatePath(Vector2 targetPos)
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        List<Vector2> newPath = pathfinding.FindPath(startPos, targetPos);

        yield return null;

        path = newPath;
        pathIndex = 0;
        lastTargetPosition = targetPos;
    }

    // Atira na direção especificada
    private void Shoot(Vector2 direction)
    {
        if (shotCooldown <= 0)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
            Debug.Log("Bullet instantiated"); // Adicionado para depuração

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
                Vector2 nextStep = path[pathIndex];
                movement.AttemptMoveInTiles((nextStep - new Vector2(transform.position.x, transform.position.y)).normalized, 1, out _);
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
