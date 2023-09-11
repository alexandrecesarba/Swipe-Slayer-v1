using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    #region Variables

    public Animator transition;
    public float transitionTime = 3f;
    public int currentBuildIndex = 0;
    private int lastSceneBuildIndex = 0;

    #endregion

    #region Unity Methods

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void LoadNextLevel()
    {
        lastSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentBuildIndex = lastSceneBuildIndex + 1;
        Debug.Log("Current Scene: "+SceneManager.GetActiveScene().name+". Loading: " + currentBuildIndex);
        if (currentBuildIndex > 4)
        {
            currentBuildIndex = 0;
        }
        if (lastSceneBuildIndex > 4)
        {
            lastSceneBuildIndex = 0;
        }
        StartCoroutine(LoadLevel(currentBuildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        Debug.Log("Loading Next Level...");

        yield return new WaitForSeconds(transitionTime);

        GameManager.Instance.LevelEnded();
        Debug.Log("Unloading scene, Index: " + (lastSceneBuildIndex));
        SceneManager.UnloadSceneAsync(lastSceneBuildIndex);
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
