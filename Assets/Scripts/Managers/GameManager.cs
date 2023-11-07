using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0f;
    [HideInInspector] public bool playersTurn = true;

    private int level = 1;
    public GameObject player;
    public List<Card> deck = new List<Card>();
    public List<Card> hands = new();
    public List<Vector3> cardSlots = new();
    public bool[] availableCardSlots;

    public bool shouldChangeLevel = false;

    public LevelManager levelManager;
    public LevelLoader levelLoader;

    private Coroutine turnLoop;
    

    void Awake()
    {
        player = GameObject.FindWithTag("Player");

        cardSlots.Add(new Vector3(-2.5f, -4.5f, -1));
        cardSlots.Add(new Vector3(-0.83f, -4.5f, -1));
        cardSlots.Add(new Vector3(0.83f, -4.5f, -1));
        cardSlots.Add(new Vector3(2.5f, -4.5f, -1));

        for (int i = 0; i < 3; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Cards/DoubleBootsCard"));
            cardObject.SetActive(false);
            deck.Add(cardObject.GetComponent<Card>());
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Cards/DoubleAttackCard"));
            cardObject.SetActive(false);
            deck.Add(cardObject.GetComponent<Card>());
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Cards/NormalAttackCard"));
            cardObject.SetActive(false);
            deck.Add(cardObject.GetComponent<Card>());
        }

        deck.Shuffle();

        // Debug.LogWarning("DECK INIT: =============================================");

        // for (int i = 0; i < 8; i++)
        // {
        //     Debug.LogWarning(deck[i].name);
        // }
    }

    void Start()
    {
        if (player == null)
            Debug.LogWarning("Não foi possível encontrar o Player no GameManager");
        // Debug.LogWarning("Active scene: " + SceneManager.GetActiveScene().name);
        // levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
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

        Draw4Cards();

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
        hands.Shuffle();
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
