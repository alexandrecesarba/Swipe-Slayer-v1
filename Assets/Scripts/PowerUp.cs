using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPickup {

    #region Variables
    public PowerUpData data;
    private float heal;
    private float maxHealth;
    private float damage;
    private float maxDamage;
    #endregion

    #region Unity Methods
    void Start() {
        this.heal = data.heal;
        this.maxHealth = data.maxHealth;
        this.damage = data.damage;
        this.maxDamage = data.maxDamage;
    }
    #endregion

    public void Activate(GameObject unit)
    {
        Damageable damageComponent = unit.GetComponent<Damageable>();
        damageComponent?.RestoreHealth(3);
        Debug.Log("Power Picked Up!");
        Destroy(gameObject);
    }

}
