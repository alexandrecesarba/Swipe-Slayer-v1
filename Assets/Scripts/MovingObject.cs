using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MovementResult
{
    Moved,
    Blocked,
    Hit
}

public class MovingObject : MonoBehaviour
{

    #region Events
    public delegate void HitHandler(GameObject hitObject);
    public event HitHandler OnHit;
    #endregion

    [SerializeField] 
    public Tilemap groundTilemap;
    [SerializeField] 
    public Tilemap collisionTilemap;

    [SerializeField] 
    public float moveSpeed = 10f;

    private Vector3 movePoint;
    public bool isMoving;
    // private GameObject circleRedGO;


    public MovementResult AttemptMove(Vector2 direction){
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 targetPosition = transform.position + (Vector3)direction;
    
        if (CanMove(targetPosition, out GameObject hitObject))
        {
            isMoving = true;
            movePoint = targetPosition;
        }
        else if (hitObject != null)
        {
            Debug.Log("HIT: " + hitObject.name);
            OnHit?.Invoke(hitObject);
            return MovementResult.Hit;
        }
        return MovementResult.Blocked;
    }

    public MovementResult AttemptMoveInTiles(GridDirection direction, int numberOfTiles)
    {
        Vector2 tilemapCellSize = groundTilemap.cellSize;

        // Calcular o deslocamento total baseado na direção e no tamanho do tile
        Vector2 moveVector = Vector2.zero;

        switch (direction)
        {
            case GridDirection.Up:
                moveVector = new Vector2(0f, tilemapCellSize.y * numberOfTiles);
                break;
            case GridDirection.Down:
                moveVector = new Vector2(0f, -tilemapCellSize.y * numberOfTiles);
                break;
            case GridDirection.Right:
                moveVector = new Vector2(tilemapCellSize.x * numberOfTiles, 0f);
                break;
            case GridDirection.Left:
                moveVector = new Vector2(-tilemapCellSize.x * numberOfTiles, 0f);
                break;
            case GridDirection.UpRight:
                moveVector = new Vector2(tilemapCellSize.x * numberOfTiles, tilemapCellSize.y * numberOfTiles);
                break;
            case GridDirection.UpLeft:
                moveVector = new Vector2(-tilemapCellSize.x * numberOfTiles, tilemapCellSize.y * numberOfTiles);
                break;
            case GridDirection.DownRight:
                moveVector = new Vector2(tilemapCellSize.x * numberOfTiles, -tilemapCellSize.y * numberOfTiles);
                break;
            case GridDirection.DownLeft:
                moveVector = new Vector2(-tilemapCellSize.x * numberOfTiles, -tilemapCellSize.y * numberOfTiles);
                break;
        }

        // Verificar se o movimento é válido
        Vector3 targetPosition = transform.position + (Vector3)moveVector;
        return EvaluateMove(targetPosition);
        
    }

    public MovementResult AttemptMoveInTiles(Vector2 directionVector, int numberOfTiles, out int tilesMoved)
    {
        Vector2 tilemapCellSize = groundTilemap.cellSize;
        
        Vector3 targetPosition = transform.position;

        MovementResult moveCondition = MovementResult.Blocked;
        
        tilesMoved = 0;

        // Verificar se o movimento é válido
        for (int i = 0; i < numberOfTiles; i++){
            targetPosition += (Vector3)(directionVector * tilemapCellSize);
            Debug.Log("targetPosition + 1 ->" + i); 
            moveCondition = EvaluateMove(targetPosition);
            Debug.Log(moveCondition);
            if (moveCondition == MovementResult.Moved){
                movePoint = targetPosition;
                tilesMoved++;
                Debug.Log("CHECKED, NEXT TILE");
            }
            else{
                Debug.Log("CANT MOVE ANYMORE, i = " + i);
                break;
            }
        }
        isMoving = true;
        // circleRedGO.transform.position = targetPosition;
        return moveCondition;

    }

    public MovementResult EvaluateMove(Vector3 targetPosition)
    {
        if (CanMove(targetPosition, out GameObject hitObject))
        {
            return MovementResult.Moved;
        }
        else if (hitObject != null)
        {
            // Podemos fazer algo em relação ao objeto atingido, se necessário.
            Debug.Log("HIT: " + hitObject.name);
            OnHit?.Invoke(hitObject);
            return MovementResult.Hit;
        }
        return MovementResult.Blocked;
    }

    public bool CanMove(Vector3 targetPosition, out GameObject hit) {
        
        hit = null; // Inicializa o parâmetro de saída

        Vector3Int targetGridPosition = groundTilemap.WorldToCell(targetPosition);
        TileBase targetGroundTile = groundTilemap.GetTile(targetGridPosition);
        TileBase targetCollisionTile = collisionTilemap.GetTile(targetGridPosition);
        

        if (targetGroundTile == null || targetCollisionTile != null){
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
   void Start()
    {
        // circleRedGO = GameObject.Find("CircleRed");
    }

}
