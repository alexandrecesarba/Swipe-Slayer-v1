using System.Linq;
using UnityEngine;

public class Key : MonoBehaviour, IPickup {

    #region Variables
    [SerializeField]
    Door[] doors;
    #endregion

    public void Activate(GameObject _)
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
        Destroy(gameObject);
    }
}
