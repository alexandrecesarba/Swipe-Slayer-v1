using System.Collections;
using UnityEngine;

public class Hole : MonoBehaviour, IPickup
{

    #region Variables
    [SerializeField]
    readonly float duration = 1;
    #endregion

    #region Unity Methods



    #endregion
    public void Activate(GameObject unit)
    {
        DamageAndMove(unit);
    }

    private IEnumerator DamageAndMove(GameObject unit)
    {
        if (unit.TryGetComponent<Damageable>(out Damageable damageComponent))
        {
            damageComponent.TakeDamage(1);
            if (unit.TryGetComponent<MovingObject>(out MovingObject movementComponent))
            {
                yield return new WaitForSeconds(duration);
                movementComponent.movePoint = movementComponent.lastPosition;
            }
        }
    }
}
