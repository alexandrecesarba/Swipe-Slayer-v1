using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

    #region Variables
    private Damageable playerDamageable;
    #endregion

    #region Unity Methods

    void Start()
    {
        playerDamageable = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        if (playerDamageable != null) {
            playerDamageable.healthBar = GetComponent<HealthBar>();
        }
    }

    void Update()
    {
        
    }

    #endregion
}
