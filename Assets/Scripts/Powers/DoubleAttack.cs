using System.Collections;
using UnityEngine;

public class DoubleAttack : Attack, IPower
{
    public int attackPoints = 1;
    readonly float interval = 1f;

    public float Duration { get; set;} = 2f;

    public void Activate(GameObject user, Vector2 direction)
    {
        MovingObject movement = user.GetComponent<MovingObject>();
        movement.EvaluateMove((Vector2)user.transform.position + direction, out GameObject hit);
        if (hit != null)
        {
            StartCoroutine(ExecuteDoubleAttack(interval, hit, user));
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

}
