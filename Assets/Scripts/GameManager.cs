using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Time to wait before starting level, in seconds.
    public float levelStartDelay = 2f;
    //Delay between each Player turn.
    public float turnDelay = 0.1f;
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    //Boolean to check if it's players turn, hidden in inspector but public.
    [HideInInspector] public bool playersTurn = true;

    //Current level number, expressed in game as "Day 1".
    private int level = 1;
    //List of all Enemy units, used to issue them move commands.
    private List<Enemy> enemies;
    //Boolean to check if enemies are moving.
    private bool enemiesMoving;


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

    //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {


    }


    //Update is called every frame.
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (playersTurn || enemiesMoving)

            //If any of these are true, return and do not start MoveEnemies.
            return;

        //Start moving enemies.
        StartCoroutine(MoveEnemies());
        // MoveEnemies();
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
        // levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    //Coroutine to move enemies in sequence.
    public IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100 ms).
        // yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in first level):
        // if (enemies.Count == 0)
        // {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            // yield return new WaitForSeconds(turnDelay);
        // }
        // Debug.Log(enemies.Count);
        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++)
        {
            //Wait for Enemy's moveTime before moving next Enemy, 
            yield return new WaitForSeconds(1f);

            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            enemies[i].MoveEnemy();
            // Debug.Log(enemies[i].target.transform.position);

 
        }
        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;
        // return;
    }
}
