using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    #region Variables

    #endregion

    #region Unity Methods

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     // Debug.Log("Power Picked Up!");

    //     if (other.CompareTag("Player"))
    //     {
    //         Pickup();

    //     }
    // }

    #endregion

    public void Pickup(GameObject unit)
    {
        Damageable damageComponent = unit.GetComponent<Damageable>();
        damageComponent?.RestoreHealth(3);
        Debug.Log("Power Picked Up!");
        Destroy(gameObject);
    }

}
