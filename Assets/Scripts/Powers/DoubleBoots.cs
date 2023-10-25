using UnityEngine;

public class DoubleBoots : MonoBehaviour, IPower {
    
    public void Activate(GameObject user, Vector2 direction)
    {
        user.GetComponent<MovingObject>().AttemptMoveInTiles(direction, 2, out _);
    }

}
