using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 10;
    [HideInInspector]
    public int CurrentHealth {get; private set;}
    public HealthBar healthBar;
    public delegate void DeathAction(IUnit unit);
    public event DeathAction OnDeath;

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int amount)
    {
        if (CurrentHealth + amount <= maxHealth){
            CurrentHealth += amount;
        } else {
            CurrentHealth = maxHealth;
        }
        healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(this.GetComponent<IUnit>());
        Destroy(gameObject);
    }
}
