// SceneInstantiate.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private Object persistentScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }

    void Start() 
    {
        _ = GameManager.Instance;
    }
}
