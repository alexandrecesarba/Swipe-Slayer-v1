using UnityEngine;
using UnityEngine.Tilemaps;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private bool useRaycast = true;

    [HideInInspector] public Vector2 LastRaycastDirection { get; private set; }
    [HideInInspector] public RaycastHit2D ray { get; private set; }

    public bool HasLineOfSightTo(Transform target, int maxTilesDistance = -1)
    {
        if (!useRaycast || target == null)
            return false;

        Vector2 direction = (target.position - transform.position).normalized; // Normaliza a direção
        float maxDistance = maxTilesDistance > 0 ? groundTilemap.cellSize.x * maxTilesDistance : Mathf.Infinity;

        // Ignora o próprio collider do inimigo
        Collider2D selfCollider = GetComponent<Collider2D>();
        bool originalState = true;
        if (selfCollider != null)
        {
            originalState = selfCollider.enabled;
            selfCollider.enabled = false;
        }

        // Realiza o raycast
        ray = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

        // Reativa o próprio collider do inimigo
        if (selfCollider != null)
        {
            selfCollider.enabled = originalState;
        }

        LastRaycastDirection = direction;

        // Verifica se o raycast atingiu o jogador
        if (ray.collider != null && ray.collider.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, direction * ray.distance, Color.green);
            return true;
        }

        Debug.DrawRay(transform.position, direction * maxDistance, Color.red);
        return false;
    }
}
