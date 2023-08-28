using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingObject : MonoBehaviour
{
    [SerializeField] 
    public Tilemap groundTilemap;
    [SerializeField] 
    public Tilemap collisionTilemap;

    [SerializeField] 
    public float moveSpeed = 10f;

    private Vector3 movePoint;
    public bool isMoving;

    public GameObject Move(Vector2 direction){
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 targetPosition = transform.position + (Vector3)direction;
    
        if (CanMove(targetPosition, out GameObject hitObject))
        {
            isMoving = true;
            movePoint = transform.position + (Vector3)direction;
        }
        else if (hitObject != null)
        {
            Debug.Log("OBJETO: " + hitObject);
            return hitObject;
        }
        return null;
    }



    public bool CanMove(Vector3 targetPosition, out GameObject hit) {
        
        hit = null; // Inicializa o parâmetro de saída

        Vector3Int targetGridPosition = groundTilemap.WorldToCell(targetPosition);
        TileBase targetGroundTile = groundTilemap.GetTile(targetGridPosition);
        TileBase targetCollisionTile = collisionTilemap.GetTile(targetGridPosition);
        

        if (targetGroundTile == null || targetCollisionTile != null){
            Debug.Log("targetGroundTile == null || targetCollisionTile != null");
            Debug.Log(targetCollisionTile);
            return false;
        }

        if (groundTilemap.GetTile(targetGridPosition) == null){
            return true;
        }
        // Verifique se há um inimigo no tile para onde o jogador está se movendo
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f);

        foreach (Collider2D collider in colliders) {
                // Retorna o GameObject do inimigo e impede o movimento depois de atacar
                hit = collider.gameObject;
                Debug.Log("COLIDER");
                return false;
            
        }

        return true;
    }

    void Update()
    {
        if (isMoving)
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
            if (transform.position == movePoint)
                isMoving = false;
    }

}
