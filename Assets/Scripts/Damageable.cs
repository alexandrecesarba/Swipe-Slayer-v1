using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public HealthBar healthBar;
    public delegate void DeathAction(IUnit unit);
    public event DeathAction OnDeath;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int amount)
    {
        if (currentHealth + amount <= maxHealth){
            currentHealth += amount;
        } else {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
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
