// SceneInstantiate.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private Object persistentScene;
    private int currentPhase = 1;  // Start with Fase1

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }

    [ContextMenu("ChangeScene")]
    public void NextScene()
    {
        // Try to unload the current phase
        if (Application.CanStreamedLevelBeLoaded("Fase" + currentPhase))
        {
            SceneManager.UnloadSceneAsync("Fase" + currentPhase);
        }

        // Increment to the next phase
        currentPhase++;

        // Try to load the next phase if it exists
        if (Application.CanStreamedLevelBeLoaded("Fase" + currentPhase))
        {
            SceneManager.LoadSceneAsync("Fase" + currentPhase, LoadSceneMode.Additive);
        }
        else
        {
            // Handle the end of the game or loop back to the first phase, etc.
            Debug.Log("No more phases. Game Over or Loop back.");
        }
    }
}
