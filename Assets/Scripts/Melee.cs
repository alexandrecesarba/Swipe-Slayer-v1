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
        GameObject enemyObject = movingObject.Move(direction);
        // Se atacou um inimigo, chame a função do script "Damageable"
        if (enemyObject != null)
        {
            Damageable enemyScript = enemyObject.GetComponent<Damageable>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackPoints);
            }
            else
            {
                Debug.Log("Erro: Não foi possível acessar o script Damageable");
            }
        }
    }
}
