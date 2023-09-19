using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    public bool CheckLineOfSight(Transform target)
    {
        if (target == null)
            return false;

        Vector2 direction = (target.position - transform.position);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);

        if (ray.collider != null)
        {
            Debug.Log("Ray hit: " + ray.collider.gameObject.name);

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
