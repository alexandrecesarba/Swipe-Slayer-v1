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
        // Debug.LogWarning("Active scene: " + SceneManager.GetActiveScene().name);
        // levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        // levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // // levelLoader = GetComponent<LevelLoader>();
        // turnLoop = StartCoroutine(levelManager.TurnLoop());
        // Debug.Log("StartCoroutine(levelManager.Turnloop())");
    }

    public void SetUpNewLevel()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        InputManager.Instance.Reload();
        levelManager.levelOver = false;
        Debug.Log("OnLevelWasLoaded");
        //level++; // Uncomment when you have more than one scene

        if (levelManager != null)
        {   
            turnLoop = StartCoroutine(levelManager.TurnLoop());
            Debug.Log("StartCoroutine(levelManager.Turnloop())");
        }
        else
        {
            Debug.Log("LevelManager not Found!");
        }
    }


    public void GameOver()
    {
        // Game over logic here
        enabled = false;
    }

    public void LevelEnded()
    {
        if (levelManager != null)
        {
        levelManager.EndLevel();
        Debug.Log("Ending level");
        StopCoroutine(turnLoop);
        Debug.Log("Stopping Coroutine");
        }
    }
}
