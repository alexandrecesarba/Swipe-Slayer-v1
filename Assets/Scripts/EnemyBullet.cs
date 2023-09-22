using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    private RaycastHandler raycastHandler; 


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
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.back, GetComponent<CircleCollider2D>().radius);
    }
}
