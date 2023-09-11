using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    #region Variables

    public Animator transition;
    public float transitionTime = 3f;

    #endregion

    #region Unity Methods

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        Debug.Log("Loading Next Level...");

        yield return new WaitForSeconds(transitionTime);

        GameManager.Instance.LevelEnded();
        SceneManager.UnloadSceneAsync(levelIndex - 1);
        Debug.Log("Unloading scene, Index: " + (levelIndex - 1));
        var asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone){
            Debug.Log("Loading scene, Index: " + levelIndex);
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));
        GameManager.Instance.SetUpNewLevel();
        transition.SetTrigger("End");

    }
}
