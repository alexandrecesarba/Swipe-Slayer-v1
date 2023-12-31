using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MovementResult
{
    Moved,
    Blocked,
    Hit,
    Unable
}

public class MovingObject : MonoBehaviour
{

    #region Events
    public delegate void HitHandler(GameObject hitObject);
    public event HitHandler OnHit;
    public delegate void MoveHandler();
    public event MoveHandler OnMove;
    public event MoveHandler OnMoveEnd;

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

    #region Unity Methods
    void Update()
    {
        if (isMoving)
        {
            Debug.Log("isMoving:" + isMoving);
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);
            if (transform.position == movePoint)
            {
                isMoving = false;
                OnMoveEnd?.Invoke();
                Debug.Log("OnMoveEnded");
            }
        }
    }

    void Start()
    {
        // circleRedGO = GameObject.Find("CircleRed");
    }
    #endregion

    public MovementResult AttemptMove(Vector2 direction){
     // Normaliza e arredonda a direção para garantir movimento ortogonal
        Vector2 normalizedDirection = NormalizeDirection(direction);

        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3 targetPosition = transform.position + (Vector3)normalizedDirection;



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

    private Vector2 NormalizeDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(direction.y));
        }
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
        return EvaluateMove(targetPosition, out _);
        
    }

    public GameObject AttemptMoveInTiles(Vector2 directionVector, int numberOfTiles, out int tilesMoved)
    {
        Vector2 tilemapCellSize = groundTilemap.cellSize;
        Vector3 targetPosition = transform.position;

        MovementResult moveCondition = MovementResult.Blocked;
        
        GameObject hitObject = null;
        tilesMoved = 0;

        // Verificar se o movimento é válido
        for (int i = 0; i < numberOfTiles; i++){
            targetPosition += (Vector3)(directionVector * tilemapCellSize);
            moveCondition = EvaluateMove(targetPosition, out hitObject);
            if (moveCondition == MovementResult.Moved){
                movePoint = targetPosition;
                tilesMoved++;
            }
            else{
                break;
            }
        }
        isMoving = true;
        OnMove?.Invoke();
        Debug.Log("OnMove");
        // circleRedGO.transform.position = targetPosition;
        return hitObject;

    }

    public MovementResult EvaluateMove(Vector3 targetPosition, out GameObject hit)
    {
        if (CanMove(targetPosition, out GameObject hitObject))
        {
            hit = null;
            return MovementResult.Moved;
        }
        else if (hitObject != null)
        {
            // Podemos fazer algo em relação ao objeto atingido, se necessário.
            Debug.Log("HIT: " + hitObject.name);
            OnHit?.Invoke(hitObject);
            hit = hitObject;

            return MovementResult.Hit;
        }
        hit = null;
        return MovementResult.Unable;
    }

    public bool CanMove(Vector3 targetPosition, out GameObject hit) {
        hit = null; // Inicializa o parâmetro de saída
        
        // Se já estiver movendo, não poderá mover
        if (isMoving) {
            return false;
        }

        Vector3Int targetGridPosition = groundTilemap.WorldToCell(targetPosition);
        TileBase targetGroundTile = groundTilemap.GetTile(targetGridPosition);
        TileBase targetCollisionTile = collisionTilemap.GetTile(targetGridPosition);
        
        // Se não tiver chão, ou houver algum tile da camada de colisão, retorna falso
        if (targetGroundTile == null || targetCollisionTile != null){
            Debug.Log("Não tem ground");
            return false;
        }

        // Verifique se há um inimigo no tile para onde o jogador está se movendo
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.2f);

        foreach (Collider2D collider in colliders) {
                // Retorna o GameObject do inimigo e impede o movimento depois de atacar
                if (collider.CompareTag("BlocksMovement") || collider.CompareTag("Player") || collider.CompareTag("Enemy") || collider.CompareTag("Interactable"))
                {
                    hit = collider.gameObject;
                    return false;
                }
            
        }

        return true;
    }

    public void SetGround(Tilemap ground, Tilemap collision)
    {
        groundTilemap = ground;
        collisionTilemap = collision;

    }


}
