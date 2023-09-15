using UnityEngine;

public class ManagersInstantiate : MonoBehaviour {

    #region Unity Methods

    void Awake() 
    {
        Debug.LogWarning("Loading Managers.");
        _ = GameManager.Instance;
        // _ = SwipeDetection.Instance;
        _ = InputManager.Instance;
        _ = SwipeDetection.Instance;
    }

    #endregion
}
