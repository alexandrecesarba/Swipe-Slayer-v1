using System.Collections;
using UnityEngine;

public class DoubleBoots : MonoBehaviour, IPower {

    public float Duration { get; set;} = .5f;
    
    public void Activate(GameObject user, Vector2 direction)
    {
        Debug.LogWarning("Activating DoubleBoots!");
        StartCoroutine(DoubleMove(user, direction));
    }

    IEnumerator DoubleMove(GameObject user, Vector2 direction)
    {
        user.GetComponent<MovingObject>().AttemptMoveInTiles(direction, 1, out _);
        while (user.GetComponent<MovingObject>().isMoving)
        {
            yield return null;
        }
        user.GetComponent<MovingObject>().AttemptMoveInTiles(direction, 1, out _);
    }
}
