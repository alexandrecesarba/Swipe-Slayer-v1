using UnityEngine;
[CreateAssetMenu(fileName = "powerUps", menuName = "PowerUp/New")]
public class PowerUpData : ScriptableObject
{
    public float heal;
    public float maxHealth;
    public float damage;
    public float maxDamage;
}
