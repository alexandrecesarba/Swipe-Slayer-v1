using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

    #region Variables
    private Damageable playerDamageable;
    private HealthBar healthBar;
    #endregion

    #region Unity Methods

    void Start()
    {
        playerDamageable = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        healthBar = GetComponent<HealthBar>();
        if (playerDamageable != null && healthBar != null) {
            playerDamageable.healthBar = GetComponent<HealthBar>();
            healthBar.SetHealth(playerDamageable.CurrentHealth);
            healthBar.SetMaxHealth(playerDamageable.maxHealth);
        }
    }

    #endregion
}
