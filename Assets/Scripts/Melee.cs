using UnityEngine;

[RequireComponent(typeof(MovingObject))]
public class Melee : MonoBehaviour
{
    public int attackPoints = 3;
    private MovingObject movingObject;

    private void Start()
    {
        movingObject = GetComponent<MovingObject>();
    }

    public void ExecuteAttack(Vector2 direction)
    {
        GameObject targetObject = movingObject.Move(direction);

        if (targetObject != null)
        {
            Damageable damageableComponent = targetObject.GetComponent<Damageable>();
            if (damageableComponent != null && targetObject.tag != gameObject.tag)  // Evite atacar objetos com a mesma tag
            {
                damageableComponent.TakeDamage(attackPoints);
            }
            else
            {
                Debug.Log("Erro: Não foi possível acessar o script Damageable ou tentando atacar objeto com a mesma tag");
            }
        }
    }
}
