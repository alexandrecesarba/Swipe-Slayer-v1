using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Enemy : MonoBehaviour {
 
    public Transform target;
    public MovingObject movement;
    public GameObject circlePrefab;
    public int attackPoints = 2;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.instance.AddEnemyToList(this);
        movement = GetComponent<MovingObject>();

        Damageable damage = GetComponent<Damageable>();
        damage.OnDeath += HandleEnemyDeath;
    }


    public void MoveEnemy() {
		Vector3 posDif = transform.position - target.position;
        float absX = Mathf.Abs(posDif.x);
        float absY = Mathf.Abs(posDif.y);
        Vector2 moveDirection;

        if (absX > absY || absY == 0)
        {
            //mover na direção X
            moveDirection = posDif.x < 0 ? Vector2.right : Vector2.left;
        }

        else {
            //mover na direção Y
            moveDirection = posDif.y < 0 ? Vector2.up : Vector2.down;
        }

        //  _ = Instantiate(circlePrefab, transform.position + (Vector3)moveDirection, Quaternion.identity);
        GameObject playerObject = movement.Move(moveDirection);
        // Debug.Log("Entro aqui!!");

        // Se atacou o player, chame a função do script "Damageable"
        if (playerObject != null && playerObject.GetComponent<Damageable>() != null && playerObject.tag == "Player")
        {
            Debug.Log("Entro aqui!!");
            Damageable playerDamage = playerObject.GetComponent<Damageable>();
            if (playerDamage != null) 
            {
                playerDamage.TakeDamage(attackPoints);
            } else
            {
                Debug.Log("Deu Ruim");
            }
        }
        GameManager.instance.playersTurn = true;
        
    }

    public void HandleEnemyDeath() 
    {
        GameManager.instance.RemoveEnemyFromList(this);
    }
 
}