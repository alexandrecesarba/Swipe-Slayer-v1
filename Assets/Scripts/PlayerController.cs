using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;


    private PlayerMovement controls;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerMovement();
    }


    // This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        controls.Enable();
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        controls.Disable();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    void Start()
    {
        controls.Main.Movement.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
    }


    private void Move(Vector2 direction){
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 targetPosition = transform.position + (Vector3)direction;

        GameObject enemyObject = null; // Inicializa o GameObject do inimigo

        if (CanMove(targetPosition, out enemyObject)) {
            transform.position += (Vector3)direction;
        }
        else if (enemyObject != null) {
            // Se atacou um inimigo, chame a função do script "EnemyScript"
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            if (enemyScript != null) {
                enemyScript.TakeDamage(2); // Supondo que existe uma função TakeDamage no script do inimigo
            } else {
                Debug.Log("Deu Ruim");
            }
        }
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
            if (collider.tag == "Enemy") {
                // Retorna o GameObject do inimigo e impede o movimento depois de atacar
                enemy = collider.gameObject;
                Debug.Log("Atacou um inimigo!");
                return false;
            }
        }

        return true;
    }

    private void MovePlayer(Vector2 direction) {
        if (GameManager.instance.playersTurn){
            Move(direction);
            // GameManager.instance.playersTurn = false;
        }
        Debug.Log(GameManager.instance.playersTurn);
    }


}
