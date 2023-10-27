using System.Collections;
using UnityEngine;

public class NormalAttack : Attack, IPower {

    public int attackPoints = 1;
    public float Duration { get; set;} = 1f;

    public void Activate(GameObject user, Vector2 direction)
    {
        MovingObject movement = user.GetComponent<MovingObject>();
        movement.EvaluateMove((Vector2)user.transform.position + direction, out GameObject hit);
        if (hit != null)
        {
            StartCoroutine(ExecuteNormalAttack(hit, user));
        }
        else{
            Debug.LogWarning("NO HIT FOUND");
        }
    }

    #region Variables

    #endregion

    IEnumerator ExecuteNormalAttack(GameObject hit, GameObject user)
    {
        ExecuteAttack(user, hit);
        yield return null;
    }
    
}
