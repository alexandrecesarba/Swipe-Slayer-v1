using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    #region Variables

    public Animator transition;
    public float transitionTime = 10f;
    public int currentBuildIndex = 0;
    private int lastSceneBuildIndex = 0;
    private int levelCount = 9;
    private int firstLevelBuildIndex = 2;

    #endregion

    #region Unity Methods

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public void LoadNextLevel()
    {
        // LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lastSceneBuildIndex = currentBuildIndex;
        currentBuildIndex = lastSceneBuildIndex + 1;

        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name+". To load: " + currentBuildIndex);

        if (currentBuildIndex - firstLevelBuildIndex >= levelCount)
        {
            currentBuildIndex = firstLevelBuildIndex;
        }
        if (lastSceneBuildIndex - firstLevelBuildIndex >= levelCount)
        {
            lastSceneBuildIndex = firstLevelBuildIndex;
        }
        StartCoroutine(LoadLevel(currentBuildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        GameManager.Instance.LevelEnded();
        GameManager.Instance.DiscardCards();

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Unloading scene, Index: " + (lastSceneBuildIndex));
        SceneManager.UnloadSceneAsync(lastSceneBuildIndex);

        Debug.Log("Loading scene, Index: " + levelIndex);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone){
            yield return null;
        }

        Debug.Log("Setting active scene: " + levelIndex);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));
        // GameManager.Instance.SetUpNewLevel();
        transition.SetTrigger("End");

    }

    public void LoadGame (int sceneIndex)
    {
        lastSceneBuildIndex = currentBuildIndex;
        currentBuildIndex = lastSceneBuildIndex + 1;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Unloading scene, Index: " + (lastSceneBuildIndex));
        SceneManager.UnloadSceneAsync(lastSceneBuildIndex);

        Debug.Log("Loading Scene, Index: " + sceneIndex);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }

        // Debug.Log("Setting active scene: " + sceneIndex);
        // SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        transition.SetTrigger("End");

    }

    }
}
