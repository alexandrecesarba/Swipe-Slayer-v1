using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string targetTag = "Player"; // Tag do alvo a ser verificado
    [SerializeField] private bool drawRaycast = true; // Controla se o raycast deve ser desenhado ou não

    public Vector2 LastRaycastDirection { get; private set; }
    public RaycastHit2D Ray { get; private set; }

    public bool HasLineOfSightTo(Transform target)
    {
        if (target == null)
            return false;

        Vector2 direction = (target.position - transform.position);
        Ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);

        // Armazena a última direção do raycast
        LastRaycastDirection = direction;

        if (Ray.collider != null)
        {
            if (drawRaycast)
            {
                Color rayColor = Ray.collider.CompareTag(targetTag) ? Color.green : Color.red;
                Debug.DrawRay(transform.position, direction, rayColor);
            }

            return Ray.collider.CompareTag(targetTag);
        }
        return false;
    }
}
