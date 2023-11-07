using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public List<IUnit> units = new List<IUnit>();
    public float turnTime = .5f;
    private int currentUnitIndex;
    private int playerUnitIndex;
    private int playCount = 0;
    public bool levelOver = false;
    [SerializeField]
    public Material highlightMaterial;
    [SerializeField]
    public float maxTurnTime;
    [SerializeField]
    public TimeBar timeBar;
    [SerializeField]
    private Vector3 playerSpawn;
    [SerializeField]
    private Tilemap groundTileMap;
    [SerializeField]
    private Tilemap collisionTileMap;

    private int kills;

    private bool isInitialized = false;
    private bool hasReloadedGame = false;
    private LevelLoader levelLoader;


    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(InitializeAndStartLevel());
    }

    private IEnumerator InitializeAndStartLevel()
    {
        yield return StartCoroutine(Initialize());
        isInitialized = true;
        GameManager.Instance.SetUpNewLevel();
        StartCoroutine(TurnLoop());
    }

    public IEnumerator Initialize()
    {
        GameManager.Instance.player.transform.position = playerSpawn;
        GameManager.Instance.player.GetComponent<PlayerInputHandler>().Reload();
        units.Clear();
        units.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());
        currentUnitIndex = units.FindIndex(unit => unit is PlayerController);
        playerUnitIndex = currentUnitIndex;

        foreach (IUnit unit in units)
        {
            if (unit is MonoBehaviour unitBehaviour)
            {
                unitBehaviour.GetComponent<MovingObject>().SetGround(groundTileMap, collisionTileMap);
                unitBehaviour.GetComponent<Renderer>().material = highlightMaterial;
                if (unitBehaviour.TryGetComponent(out Damageable damageable))
                {
                    damageable.OnDeath += HandleUnitDied;
                }
            }
        }

        if (timeBar != null)
        {
            timeBar.SetMaxTime(maxTurnTime);
        }
        else
        {
            Debug.LogWarning("TimeBar is null.");
        }

        yield return null; // Pode ser substituído por uma espera específica, se necessário
    }

    public IEnumerator TurnLoop()
    {
         // Aguarde até que a inicialização esteja completa
        while (!isInitialized)
        {
            yield return null;
        }


        yield return new WaitForSeconds(1f);
        while (!levelOver)
        {
            IUnit currentUnit = units[currentUnitIndex];
            MonoBehaviour unitMB = (MonoBehaviour) units[currentUnitIndex];

            if (currentUnit.CanPlay)
            {
                currentUnit.IsPlaying = true;
                unitMB.GetComponent<Renderer>().material.SetFloat("_Outline_Thickness", 100);

                StartCoroutine(currentUnit.Play(turnTime));
                float startTime = Time.time;

                if (timeBar != null)
                {
                    if (currentUnitIndex == playerUnitIndex)
                    {
                        timeBar.gameObject.SetActive(true);
                    }

                    while (currentUnit.IsPlaying && Time.time - startTime < maxTurnTime)
                    {
                        timeBar.SetTime(maxTurnTime - (Time.time - startTime));
                        yield return null;
                    }

                    if (currentUnitIndex == playerUnitIndex)
                    {
                        timeBar.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogWarning("TimeBar is null.");
                }
                if (currentUnit.IsUsingCard)
                {
                    while (currentUnit.IsUsingCard)
                    {
                        yield return null;
                    }
                }
                currentUnit.IsPlaying = false;
            }

            unitMB.GetComponent<Renderer>().material.SetFloat("_Outline_Thickness", 0);

            currentUnitIndex++;
            if (currentUnitIndex >= units.Count)
            {
                currentUnitIndex = 0;
            }
            playCount++;
        }

        // Game over logic here
    }

    private void HandleUnitDied(IUnit unit)
    {
        if (currentUnitIndex > units.IndexOf(unit))
        {
            currentUnitIndex--;
        }
        units.Remove(unit);
        if (unit is PlayerController)
        {
            levelOver = true;
            if (!hasReloadedGame)
            {
                hasReloadedGame = true;
                levelLoader.ReloadGame();
            }
        }
    }

    public void EndLevel()
    {
        levelOver = true;
        GameManager.Instance.player.GetComponent<Damageable>().OnDeath -= HandleUnitDied;
    }
}
