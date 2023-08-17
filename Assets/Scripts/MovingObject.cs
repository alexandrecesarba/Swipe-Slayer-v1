using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingObject : MonoBehaviour
{
    [SerializeField] public Tilemap groundTilemap;
    [SerializeField] public Tilemap collisionTilemap;


    public GameObject Move(Vector2 direction){
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 targetPosition = transform.position + (Vector3)direction;

        if (CanMove(targetPosition, out GameObject hitObject))
        {
            transform.position += (Vector3)direction;
        }
        else if (hitObject != null)
        {
            Debug.Log("OBJETO: " + hitObject);
            return hitObject;
        }
        return null;
    }



    private bool CanMove(Vector3 targetPosition, out GameObject enemy) {
        
        enemy = null; // Inicializa o parâmetro de saída

        Vector3Int targetGridPosition = groundTilemap.WorldToCell(targetPosition);
        TileBase targetGroundTile = groundTilemap.GetTile(targetGridPosition);
        TileBase targetCollisionTile = collisionTilemap.GetTile(targetGridPosition);
        

        if (targetGroundTile == null || targetCollisionTile != null)
            return false;

        if (groundTilemap.GetTile(targetGridPosition) == null)
            return true;

        // Verifique se há um inimigo no tile para onde o jogador está se movendo
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f);

        foreach (Collider2D collider in colliders) {
                // Retorna o GameObject do inimigo e impede o movimento depois de atacar
                enemy = collider.gameObject;
                return false;
            
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
