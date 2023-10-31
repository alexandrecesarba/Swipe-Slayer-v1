using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int CurrentHealth { get; private set; }
    public HealthBar healthBar;
    public delegate void DeathAction(IUnit unit);
    public event DeathAction OnDeath;

    private DamageFlash damageFlash; // Referência ao script DamageFlash.

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        // Obtenha a referência ao script DamageFlash no objeto atual.
        damageFlash = GetComponent<DamageFlash>();
    }

    public void TakeDamage(int damage)
    {
        Debug.LogWarning(gameObject.name + " TOMOU DANO!");
        CurrentHealth -= damage;
        healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Chame a função Flash do script DamageFlash para iniciar a animação.
            damageFlash.Flash();
        }
    }

    public void RestoreHealth(int amount)
    {
        if (CurrentHealth + amount <= maxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
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
