using UnityEngine;

public interface IPower
{
    void Activate(GameObject user, Vector2 direction);
    float Duration{get; set;}
}
