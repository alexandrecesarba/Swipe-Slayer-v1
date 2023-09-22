using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [HideInInspector] public Vector2 LastRaycastDirection {get; private set;}

    [HideInInspector] public RaycastHit2D ray {get; private set;}

    public bool CheckLineOfSight(Transform target)
    {
        if (target == null)
            return false;

        Vector2 direction = (target.position - transform.position);
        ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);

        // Armazena a ultima posição do raycast
        LastRaycastDirection = direction;

        if (ray.collider != null)
        {
            // Debug.Log("Ray hit: " + ray.collider.gameObject.name);

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
