using UnityEngine;
using UnityEngine.Tilemaps;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Tilemap groundTilemap;

    [HideInInspector] public Vector2 LastRaycastDirection { get; private set; }
    [HideInInspector] public RaycastHit2D ray { get; private set; }

    public bool HasLineOfSightTo(Transform target, int maxTilesDistance = -1)
    {
        if (target == null)
            return false;

        Vector2 direction = (target.position - transform.position);
        float maxDistance = maxTilesDistance > 0 ? groundTilemap.cellSize.x * maxTilesDistance : Mathf.Infinity;

        // if (maxDistance == Mathf.Infinity)
        //     Debug.Log("INFINITO");
            
        ray = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

        // Armazena a última posição do raycast
        LastRaycastDirection = direction;

        if (ray.collider != null)
        {
            if (ray.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }
        return false;
    }
}
