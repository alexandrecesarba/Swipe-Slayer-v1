using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : SingletonPersistent<SceneInstantiate>
{

    [SerializeField]
    private Object persistentScene;

    void Awake()
    {
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }

    [ContextMenu("ChangeScene")]
    public void NextScene(){
        // GameManager.Instance.LevelEnded();

        SceneManager.UnloadSceneAsync("Fase1");
        SceneManager.LoadSceneAsync("Fase2", LoadSceneMode.Additive);
    }
}
