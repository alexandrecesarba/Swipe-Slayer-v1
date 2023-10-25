using System.Collections;
using UnityEngine;

public class DoubleAttack : MonoBehaviour, IPower
{
    public int attackPoints = 5;

    public void Activate(GameObject user, Vector2 direction)
    {
        MovingObject movement = user.GetComponent<MovingObject>();
        movement.EvaluateMove((Vector2)user.transform.position + direction, out GameObject hit);
        if (hit != null)
        {
            StartCoroutine(ExecuteDoubleAttack(1f, hit, user));
        }
        else{
            Debug.LogWarning("NO HIT FOUND");
        }

    }

    IEnumerator ExecuteDoubleAttack(float waitTime, GameObject hit, GameObject attacker)
    {
        ExecuteAttack(attacker, hit);
        yield return new WaitForSeconds(waitTime);
        ExecuteAttack(attacker, hit);
    }

    public void ExecuteAttack(GameObject attacker, GameObject targetObject)
    {
        if (targetObject != null)
        {
            Damageable damageableComponent = targetObject.GetComponent<Damageable>();

            if (damageableComponent != null && targetObject.tag != attacker.tag)  // Evite atacar objetos com a mesma tag
            {
                damageableComponent.TakeDamage(attackPoints);
                Debug.Log(targetObject.name + " foi atacado!");
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
