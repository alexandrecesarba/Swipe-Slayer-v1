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
    public GameObject player;
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    public bool shouldChangeLevel = false;

    public LevelManager levelManager;
    public LevelLoader levelLoader;

    private Coroutine turnLoop;
    

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        if (player == null)
            Debug.LogWarning("Não foi possível encontrar o Player no GameManager");
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

        // InputManager.Instance.Reload();
        levelManager.levelOver = false;
        //level++; // Uncomment when you have more than one scene

        if (levelManager != null)
        {   
            turnLoop = StartCoroutine(levelManager.TurnLoop());
        }
        else
        {
            Debug.LogWarning("LevelManager not Found!");
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

    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
            
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                }
            }
        }
    }
}
