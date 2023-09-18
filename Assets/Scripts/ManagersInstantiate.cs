using System.Collections;
using UnityEngine;

public class ManagersInstantiate : MonoBehaviour {

    #region Variables
    private LevelLoader levelLoader;
    #endregion

    #region Unity Methods

    void Start()
    {
        Debug.LogWarning("Loading Managers.");
        _ = GameManager.Instance;
        _ = InputManager.Instance;
        _ = SwipeDetection.Instance;

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        StartCoroutine(WaitForManagers());

    }


    #endregion
    private IEnumerator WaitForManagers()
    {
        yield return new WaitForSeconds(2f);
        levelLoader.LoadGame(2);
        
    }
}
