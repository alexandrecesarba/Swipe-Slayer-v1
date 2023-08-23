using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Enemy : MonoBehaviour 
{
    public Transform target;
    public MovingObject movement;
    public GameObject circlePrefab;

    public Melee meleeComponent;  

    void Start() 
    {


        target = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.instance.AddEnemyToList(this);
        movement = GetComponent<MovingObject>();

        Damageable damage = GetComponent<Damageable>();
        damage.OnDeath += HandleEnemyDeath;

        meleeComponent = GetComponent<Melee>();  
        if (meleeComponent == null)
            meleeComponent = gameObject.AddComponent<Melee>();
    }

    public void MoveEnemy() 
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
        
        GameManager.instance.playersTurn = true;
    }

    public void HandleEnemyDeath() 
    {
        GameManager.instance.RemoveEnemyFromList(this);
    }
}
