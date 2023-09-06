using UnityEngine;
[CreateAssetMenu(fileName = "pickupBuffs", menuName = "Pickup/New")]
public class PickUp : ScriptableObject
{
    public float heal;
    public float maxHealth;
    public float damage;
    public float maxDamage;
}
