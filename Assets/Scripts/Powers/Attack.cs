using UnityEngine;

public class Attack : MonoBehaviour {
    private int attackPoints;

    #region Variables

    #endregion

    public void ExecuteAttack(GameObject attacker, GameObject targetObject)
    {
        if (targetObject != null)
        {
            Damageable damageableComponent = targetObject.GetComponent<Damageable>();

            if (damageableComponent != null && targetObject.tag != attacker.tag)  // Evite atacar objetos com a mesma tag
            {
                damageableComponent.TakeDamage(attackPoints);
                Debug.Log(targetObject.name + " foi atacado! Dano: " + attackPoints);
            }
            else if (damageableComponent == null) 
            {
                Debug.LogWarning("Não foi possível acessar o script Damageable");
            }
            else if (targetObject.tag == attacker.tag) 
            {
                Debug.LogWarning("Tentando atacar objeto com a mesma tag: " + attacker.tag + targetObject.name);
            }
        }
        else
        {
            Debug.LogWarning("Target Object é NULO!");
        }
    }
}
