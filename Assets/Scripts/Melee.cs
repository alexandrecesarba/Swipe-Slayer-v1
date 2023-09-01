using UnityEngine;

[RequireComponent(typeof(MovingObject))]
public class Melee : MonoBehaviour
{
    public int attackPoints = 3;
    private MovingObject movingObject;

    private void Start()
    {
        movingObject = GetComponent<MovingObject>();
        movingObject.OnHit += ExecuteAttack;
    }

    public void ExecuteAttack(GameObject targetObject)
    {
        if (targetObject != null)
        {
            Damageable damageableComponent = targetObject.GetComponent<Damageable>();
            if (damageableComponent != null && targetObject.tag != gameObject.tag)  // Evite atacar objetos com a mesma tag
            {
                damageableComponent.TakeDamage(attackPoints);
                Debug.Log(targetObject.name + " foi atacado!");
            }
            else
            {
                Debug.Log("Erro: Não foi possível acessar o script Damageable ou tentando atacar objeto com a mesma tag");
            }
        }
    }
}
