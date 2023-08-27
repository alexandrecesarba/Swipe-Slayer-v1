using UnityEngine;

public class Enemy : MonoBehaviour, IUnit
{
    public Transform target;
    public MovingObject movement;
    public GameObject circlePrefab;

    public Melee meleeComponent;

    public bool IsPlaying {get; set;}
    public bool CanPlay {get; set;}

    void Start() 
    {
        this.CanPlay = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.instance.AddEnemyToList(this);
        movement = GetComponent<MovingObject>();

        Damageable damage = GetComponent<Damageable>();
        // damage.OnDeath += HandleEnemyDeath;

        meleeComponent = GetComponent<Melee>();  
        if (meleeComponent == null)
            meleeComponent = gameObject.AddComponent<Melee>();
    }

    public void Play() 
    {
		Vector3 posDif = transform.position - target.position;
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

        GameObject targetObject = movement.Move(moveDirection);
        if (targetObject != null)
        {
            meleeComponent.ExecuteAttack(moveDirection);
        }
        
        IsPlaying = false;
    }

    // public void HandleEnemyDeath() 
    // {
    //     GameManager.instance.RemoveEnemyFromList(this);
    // }
}
