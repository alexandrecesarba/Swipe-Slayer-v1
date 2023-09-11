using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPersistent<GameManager>
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0f;
    [HideInInspector] public bool playersTurn = true;

    private int level = 1;
    private bool enemiesMoving;
    private bool doingSetup;

    private List<Enemy> enemies;
    public bool shouldChangeLevel = false;

    public LevelManager levelManager;
    public LevelLoader levelLoader;

    private Coroutine turnLoop;
    

    void Awake()
    {
    }

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // levelLoader = GetComponent<LevelLoader>();
        turnLoop = StartCoroutine(levelManager.TurnLoop());
        Debug.Log("StartCoroutine(levelManager.Turnloop())");
    }

    public void SetUpNewLevel()
    {
        InputManager.Instance.Reload();
        levelManager.levelOver = false;
        Debug.Log("OnLevelWasLoaded");
        //level++; // Uncomment when you have more than one scene
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        if (levelManager != null)
        {   
            turnLoop = StartCoroutine(levelManager.TurnLoop());
            Debug.Log("StartCoroutine(levelManager.Turnloop())");
        }
        else
        {
            Debug.Log("LevelManager not Found!");
        }
        // Reactivate events
        SwipeDetection swipeDetection = FindObjectOfType<SwipeDetection>();
        if (swipeDetection != null)
        {
            //swipeDetection.ReactivateEvents();
        }
        else
        {
            Debug.LogError("SwipeDetection not found");
        }
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }


    public void GameOver()
    {
        // Game over logic here
        enabled = false;
    }

    public void LevelEnded()
    {
        levelManager.EndLevel();
        Debug.Log("Ending level");
        StopCoroutine(turnLoop);
        Debug.Log("Stopping Coroutine");
    }
}
