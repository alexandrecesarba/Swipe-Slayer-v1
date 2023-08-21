using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{

    [SerializeField]
    private Object persistentScene;

    void Awake()
    {
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    [ContextMenu("Change Scene")] 
    public void NextScene()
    {
        SceneManager.UnloadSceneAsync("FirstScene");
        SceneManager.LoadSceneAsync("SecondScene", LoadSceneMode.Additive);
    }
}
