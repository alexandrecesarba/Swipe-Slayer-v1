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
    

    void Awake()
    {
     
    }

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        StartCoroutine(levelManager.TurnLoop());
    }

    void OnLevelWasLoaded()
    {
        //level++; // Uncomment when you have more than one scene
        levelManager = FindObjectOfType<LevelManager>();

        if (levelManager != null)
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            StartCoroutine(levelManager.TurnLoop());
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

        // Reinitialize LevelManager
        levelManager.Reinitialize();
        StartCoroutine(levelManager.TurnLoop());
    }


    void Update()
    {
        // Game update logic here
    }


    public void GameOver()
    {
        // Game over logic here
        enabled = false;
    }

    public void LevelEnded()
    {
        StopCoroutine(levelManager.TurnLoop());
    }
}
