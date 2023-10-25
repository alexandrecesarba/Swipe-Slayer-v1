using UnityEngine;
using UnityEngine.Tilemaps;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private bool useRaycast = true; // Variável para decidir se o raycast será utilizado

    [HideInInspector] public Vector2 LastRaycastDirection { get; private set; }
    [HideInInspector] public RaycastHit2D ray { get; private set; }

    public bool HasLineOfSightTo(Transform target, int maxTilesDistance = -1)
    {
        if (!useRaycast || target == null) // Verifica se o raycast está habilitado
            return false;

        Vector2 direction = (target.position - transform.position);
        float maxDistance = maxTilesDistance > 0 ? groundTilemap.cellSize.x * maxTilesDistance : Mathf.Infinity;

        // Ignora o próprio collider do inimigo
        Collider2D selfCollider = GetComponent<Collider2D>();
        if (selfCollider != null)
        {
            selfCollider.enabled = false;
        }

        ray = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

        // Reativa o próprio collider do inimigo
        if (selfCollider != null)
        {
            selfCollider.enabled = true;
        }

        // Armazena a última posição do raycast
        LastRaycastDirection = direction;

        if (ray.collider != null)
        {
            Debug.LogWarning("Raycast hit: " + ray.collider.name); // Adicionado para depuração

            if (ray.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, direction * maxDistance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * maxDistance, Color.red);
            }
        }
        return false;
    }
}
