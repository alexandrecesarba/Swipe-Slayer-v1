using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    public int damageAmount = 1;
    [HideInInspector] public Damageable damageable;
    private Enemy enemy;
    public int maxTilesDistance = 2; // Alcance máximo do projétil em tiles
    private Vector3 startingPosition;

   public void SetDirection(Vector2 dir)
{
    direction = dir;
    startingPosition = transform.position;
    // Determina se a direção é principalmente horizontal ou vertical
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    {
        // Movimento horizontal
        direction = new Vector2(Mathf.Sign(direction.x), 0);
    }
    else
    {
        // Movimento vertical
        direction = new Vector2(0, Mathf.Sign(direction.y));
    }
}


    void Update()
    {
        // Move o objeto na direção e velocidade especificadas
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Verifica se o projétil atingiu o alcance máximo
        if (Vector3.Distance(startingPosition, transform.position) >= maxTilesDistance)
        {
            Debug.LogWarning("Entrou aqui");
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogWarning("COLIDIU");

        if (other.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            damageable.TakeDamage(damageAmount);
        }
    }
}
