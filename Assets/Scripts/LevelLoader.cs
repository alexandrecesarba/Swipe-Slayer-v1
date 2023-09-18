using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    #region Variables

    public Animator transition;
    public float transitionTime = 3f;
    public int currentBuildIndex = 1;
    private int lastSceneBuildIndex = 0;
    private int levelCount = 5;
    private int firstLevelBuildIndex = 1;

    #endregion

    #region Unity Methods

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void LoadNextLevel()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lastSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentBuildIndex = lastSceneBuildIndex + 1;
        Debug.Log("Current Scene: "+SceneManager.GetActiveScene().name+". Loading: " + currentBuildIndex);
        if (currentBuildIndex > levelCount)
        {
            currentBuildIndex = firstLevelBuildIndex;
        }
        if (lastSceneBuildIndex > levelCount)
        {
            lastSceneBuildIndex = firstLevelBuildIndex;
        }
        StartCoroutine(LoadLevel(currentBuildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        GameManager.Instance.LevelEnded();
        transition.SetTrigger("Start");
        Debug.Log("Loading Next Level...");

        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Unloading scene, Index: " + (lastSceneBuildIndex));
        SceneManager.UnloadSceneAsync(lastSceneBuildIndex);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone){
            Debug.Log("Loading scene, Index: " + levelIndex);
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));
        // GameManager.Instance.SetUpNewLevel();
        transition.SetTrigger("End");

    }

    public void LoadGame (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        {
        transition.SetTrigger("Start");
        Debug.Log("Loading Next Level...");

        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Unloading scene, Index: " + (lastSceneBuildIndex));
        SceneManager.UnloadSceneAsync(lastSceneBuildIndex);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }

        Debug.Log("Setting active scene: " + sceneIndex);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        transition.SetTrigger("End");

    }

    }
}
