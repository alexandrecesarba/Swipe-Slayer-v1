using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Enemy : Damageable {
 
    public GameObject target;

    protected override void Start() {
        base.Start();
        target = GameObject.FindWithTag("Player");
        GameManager.instance.AddEnemyToList(this);
    }

    protected override void Die() {
        base.Die();
        GameManager.instance.RemoveEnemyFromList(this);
    }
    
    // public void TakeDamage (int damage) {

    //     currentHealth -= damage;

    //     if (currentHealth <= 0){
    //         Destroy (gameObject);
    //         GameManager.instance.RemoveEnemyFromList(this);
    //     }

    //     healthBar.SetHealth(currentHealth);
    // }

    public void MoveEnemy(Vector3 playerPos) {
		Vector3 posDif = transform.position - playerPos;
        float absX = Mathf.Abs(posDif.x);
        float absY = Mathf.Abs(posDif.y);

        if(absX > absY || absY == 0){
            //mover na direção X
            MoveEnemyInX(posDif);
        }

        else if(absX < absY || absX == 0){
            //mover na direção Y
            MoveEnemyInY(posDif);
        }
        GameManager.instance.playersTurn = true;
        
    }

    private void MoveEnemyInX(Vector3 posDif){
        if (posDif.x < 0){
            //enemy left of player
            transform.position += Vector3.right;
        }
            
		
        else if (posDif.x > 0){
            //enemy right of player
            transform.position += Vector3.left;
        }
    }

    private void MoveEnemyInY(Vector3 posDif){
        if (posDif.y < 0){
            //enemy down of player
            transform.position += Vector3.up;
        }
            
		
        else if (posDif.y > 0){
            //enemy up of player
            transform.position += Vector3.down;
        }
    }
}