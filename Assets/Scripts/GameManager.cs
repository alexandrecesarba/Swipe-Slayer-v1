using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : SingletonPersistent<GameManager>
{
    public float levelStartDelay = .5f;
    public float turnDelay = 0f;
    public static GameManager instance = null;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    public GameObject levelImage;
    private int level = 1;
    private bool enemiesMoving;
    private bool doingSetup;

    private List<Enemy> enemies;
    public bool shouldChangeLevel = false;

    public LevelManager levelManager;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Assign enemies to a new List of Enemy objects.
        enemies = new List<Enemy>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        StartCoroutine(levelManager.TurnLoop());
        Debug.Log("START!!!");
    }

    //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number. (DESCOMENTAR QUANDO TIVER MAIS UMA SCENE PRONTA)
        // level++;
    }

    //Initializes the game for each level.
    void InitGame()
    {
        doingSetup = true;
        enemies.Clear();
        // levelImage = GameObject.Find("LevelImage");
     
        if (levelImage != null)
        {
            levelImage.SetActive(true);
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Level " + level;
        }
        else
        {
            Debug.LogError("LevelImage is not assigned!");
        }
        Invoke("HideLevelImage", levelStartDelay);

    }


    private void HideLevelImage(){
        levelImage.SetActive(false);
        doingSetup = false;
    
    }

   

  void Update()
    {
        // if (enemies.Count == 0 || shouldChangeLevel)
        // {
        //     shouldChangeLevel = false; 
        //     OnLevelWasLoaded(level); 
        //     Debug.Log("Terminou o antigo");
        //     return;
        // }


        // if (playersTurn || enemiesMoving || doingSetup)
        //     return;

        // StartCoroutine(MoveEnemies());
    }

    //Call this to add the passed in Enemy to the List of Enemy objects.
    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }

        public void RemoveEnemyFromList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Remove(script);
    }


    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {

        //Enable black background image gameObject.
        levelText.text = "You Died";
        levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    //Coroutine to move enemies in sequence.
//  public IEnumerator MoveEnemies()
//     {
//         enemiesMoving = true;
//         List<Enemy> enemiesCopy = new List<Enemy>(enemies);

//         for (int i = 0; i < enemiesCopy.Count; i++)
//         {
//             yield return new WaitForSeconds(turnDelay);
            
//             // Verificar se o inimigo ainda está na lista original
//             if (enemies.Contains(enemiesCopy[i])) 
//             {
//                 enemiesCopy[i].Play();
//             }
//         }
//         playersTurn = true;
//         enemiesMoving = false;
//     }

}