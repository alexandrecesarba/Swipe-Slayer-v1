using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    public int damageAmount; 

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void Update()
    {
        // Move o objeto na direção e velocidade especificadas
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogWarning("COLIDIU");

        if (other.collider.CompareTag("Player"))
        {

            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }

}
