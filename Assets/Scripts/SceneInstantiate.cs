// SceneInstantiate.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private Object persistentScene;

    void Awake()
    {
        Debug.Log("persistentScene.name = " + persistentScene.name);
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }
}
