using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPersistent<GameManager>
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0f;
    [HideInInspector] public bool playersTurn = true;
    public GameObject player;

    private int level = 1;
<<<<<<< Updated upstream:Assets/Scripts/GameManager.cs
=======
    public GameObject player;
    private List<Card> deck = new();
    public List<Card> hands = new();
    public List<Vector3> cardSlots = new();
    public bool[] availableCardSlots;
>>>>>>> Stashed changes:Assets/Scripts/Managers/GameManager.cs

    private List<Enemy> enemies;
    public bool shouldChangeLevel = false;

    public LevelManager levelManager;
    public LevelLoader levelLoader;

    private Coroutine turnLoop;
    

    void Awake()
    {
<<<<<<< Updated upstream:Assets/Scripts/GameManager.cs
=======
        player = GameObject.FindWithTag("Player");

        cardSlots.Add(new Vector3(-2.5f, -7, -1));
        cardSlots.Add(new Vector3(-0.83f, -7, -1));
        cardSlots.Add(new Vector3(0.83f, -7, -1));
        cardSlots.Add(new Vector3(2.5f, -7, -1));

        for (int i = 0; i < 2; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Cards/DoubleBootsCard"));
            cardObject.SetActive(false);
            cardObject.name = "Card " + i;
            deck.Add(cardObject.GetComponent<Card>());
            Debug.LogWarning("Added card to Deck!  -> " + deck[i]);
        }

        for (int i = 0; i < 6; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Cards/DoubleAttackCard"));
            cardObject.SetActive(false);
            cardObject.name = "Card " + i;
            deck.Add(cardObject.GetComponent<Card>());
            Debug.LogWarning("Added card to Deck!  -> " + deck[i]);
        }

        Debug.LogWarning("DECK INIT: =============================================");

        for (int i = 0; i < 8; i++)
        {
            Debug.LogWarning(deck[i].name);
        }
>>>>>>> Stashed changes:Assets/Scripts/Managers/GameManager.cs
    }

    void Start()
    {
<<<<<<< Updated upstream:Assets/Scripts/GameManager.cs
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // levelLoader = GetComponent<LevelLoader>();
        turnLoop = StartCoroutine(levelManager.TurnLoop());
        Debug.Log("StartCoroutine(levelManager.Turnloop())");

        player = GameObject.FindWithTag("Player");
=======
        if (player == null)
        {
            Debug.LogWarning("Não foi possível encontrar o Player no GameManager");
        }
        

        // Debug.LogWarning("Active scene: " + SceneManager.GetActiveScene().name);
        // levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        // levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // // levelLoader = GetComponent<LevelLoader>();
        // turnLoop = StartCoroutine(levelManager.TurnLoop());
        // Debug.Log("StartCoroutine(levelManager.Turnloop())");
>>>>>>> Stashed changes:Assets/Scripts/Managers/GameManager.cs
    }

    public void SetUpNewLevel()
    {
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
<<<<<<< Updated upstream:Assets/Scripts/GameManager.cs
=======
        }
    }

    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            // IPower randPower = deck[Random.Range(0, deck.Count)];
            // Card randCard = new(randPower);
            Card newCard = deck[0];
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    newCard.gameObject.SetActive(true);
                }
            }
        }
>>>>>>> Stashed changes:Assets/Scripts/Managers/GameManager.cs
    }

    public void ReplaceCards(int amount)
    {
        if (deck.Count > amount)
        {
            for (int i = 0; i < amount; i++)
            {

            }
        }
    }
    [ContextMenu("DiscardCards")]
    public void DiscardCards()
    {
        int total = hands.Count;
        for (int i =0; i < total; i++)
        {
            deck.Add(hands[0]);
            hands[0].gameObject.SetActive(false);
            hands.RemoveAt(0);
            Debug.LogWarning("DISCARDING CARD | DECK SIZE -> " + deck.Count + ". HANDS SIZE -> " + hands.Count);
        }
        hands.Clear();
    }

    public void DrawCards(int amount)
    {
        if (deck.Count > amount)
        {
            for (int i = 0; i < amount; i++)
            {
                // GameObject.Instantiate(Card, new Vector3());;
                hands.Add(deck[0]);
                deck[0].movePoint = cardSlots[i];
                deck[0].gameObject.SetActive(true);
                deck.RemoveAt(0);
            }
        } else{
            Debug.LogWarning("DECK IS EMPTY!");
        }
    }

    [ContextMenu("Draw4Cards")]
    public void Draw4Cards()
    {
        if (deck.Count > 4)
        {
            for (int i = 0; i < 4; i++)
            {
                deck[i].movePoint = cardSlots[i]; // Usar o índice i para acessar a posição correta
                deck[i].gameObject.SetActive(true);
                Debug.LogWarning("DRAWED CARD -> " + deck[i].name + "| POSITION -> " + deck[i].movePoint);
                hands.Add(deck[i]);
                Debug.LogWarning("REMOVED " + deck[i]);
            }

            // Remover as cartas do deck após o loop
            deck.RemoveRange(0, 4);

            Debug.LogWarning("========DECK:");
            for (int j = 0; j < deck.Count; j++)
            {
                Debug.LogWarning(deck[j].name);
            }

            Debug.LogWarning("DECK SIZE -> " + deck.Count + ". HANDS SIZE -> " + hands.Count);
        }
        else
        {
            Debug.LogWarning("DECK IS EMPTY! DECK COUNT ->" + deck.Count);
        }
    }

}
