using UnityEngine;
using System.Collections;

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
    private int currentTurnsToWait;
    private int maxTurnsToWait;

    // Variáveis privadas para controle interno
    private bool hasLineOfSight = false;
    private float shotCooldown;
    private RaycastHandler raycastHandler; 
    public int rangedAttackPoints;

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

        // Inicializa o componente Melee se ele não existir
        Melee meleeComponent = GetComponent<Melee>();
        if (meleeComponent == null)
            meleeComponent = gameObject.AddComponent<Melee>();

        // Inicializa a referência para o script RaycastHandler
        raycastHandler = GetComponent<RaycastHandler>();
        if (raycastHandler == null)
            raycastHandler = gameObject.AddComponent<RaycastHandler>();
    }

    // Atualiza a variável hasLineOfSight a cada FixedUpdate
    private void FixedUpdate()
    {
        hasLineOfSight = raycastHandler.HasLineOfSightTo(target);
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
        if(currentTurnsToWait >= 1)
        {
            currentTurnsToWait--;
        }
        else 
        {
            yield return new WaitForSeconds(time / 2);

            Vector2 posDif = new Vector2(transform.position.x - target.position.x, transform.position.y - target.position.y);
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

            if (hasLineOfSight)
            {
                Vector2 raycastDirection = raycastHandler.LastRaycastDirection;
                Shoot(raycastDirection); // invertido, pois é do inimigo ao jogador
            }

            currentTurnsToWait = maxTurnsToWait;
        }

        yield return new WaitForSeconds(time / 2);
        IsPlaying = false;
    }
}
