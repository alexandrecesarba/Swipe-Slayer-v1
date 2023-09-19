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

    // Variáveis privadas para controle interno
    private bool hasLineOfSight = false;
    private float shotCooldown;
    private RaycastHandler raycastHandler; 

    // Propriedades para controle de estado
    public bool IsPlaying { get; set; }
    public bool CanPlay { get; set; }

    // Inicializa variáveis e componentes
    void Start()
    {
        this.CanPlay = true;
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
        hasLineOfSight = raycastHandler.CheckLineOfSight(target);
    }

    // Verifica se tem linha de visão para o alvo
    private bool CheckLineOfSight()
    {
        return raycastHandler.CheckLineOfSight(target);
    }

    // Atira na direção especificada
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

    // Controla o comportamento do inimigo durante sua "jogada"
    public IEnumerator Play(float time)
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


        yield return new WaitForSeconds(time / 2);
        IsPlaying = false;

        if (hasLineOfSight)
        {
            Vector2 raycastDirection = raycastHandler.LastRaycastDirection;
            Shoot(raycastDirection); // invertido, pois é do inimigo ao jogador
        }
    }
}
