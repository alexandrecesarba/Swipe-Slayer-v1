using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private string persistentSceneName = "Managers";

    void Start()
    {
        StartCoroutine(LoadManagerScene());
    }

    IEnumerator LoadManagerScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(persistentSceneName, LoadSceneMode.Additive);

        // Aguarde até que a cena seja completamente carregada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName(persistentSceneName);
        if (loadedScene.isLoaded)
        {
            Debug.Log("Scene loaded: " + persistentSceneName);
        }
        else
        {
            Debug.LogError("CENA NÃO CARREGOU E POSSUI O NOME: " + persistentSceneName);
        }
    }
}
