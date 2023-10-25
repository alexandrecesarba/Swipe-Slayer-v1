using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private List<IUnit> units = new List<IUnit>();
    private int currentUnitIndex;
    private int playerUnitIndex;
    private int playCount = 0;
<<<<<<< Updated upstream:Assets/Scripts/LevelManager.cs
    private int kills;
    private Transform player;
 
=======
    private Card cardSelected;

>>>>>>> Stashed changes:Assets/Scripts/Managers/LevelManager.cs
    public bool levelOver = false;

    [SerializeField]
    private Vector3 playerSpawn = Vector3.zero;
    [SerializeField]
    public float turnTime = .5f;
    [SerializeField]
    public Material highlightMaterial;
    [SerializeField]
    public float maxTurnTime;
    [SerializeField]
    public TimeBar timeBar;

    void Start()
    {
        Reinitialize();
    }

    public void Reinitialize()
    {
        units.Clear();
        units.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());
        currentUnitIndex = units.FindIndex(unit => unit is PlayerController);
        playerUnitIndex = currentUnitIndex;
        try
        {
            player = GameManager.Instance.player.transform;

            // tilemaps do player para os existentes

            // inimigos tem que target o player
        }
        catch
        {
            Debug.LogWarning("Não foi possível localizar o player do GameObject");
        }

        foreach (IUnit unit in units)
        {
            if (unit is MonoBehaviour unitBehaviour)
            {
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
    }

    public IEnumerator TurnLoop()
    {
        Debug.Log("levelOver: " + levelOver);
        yield return new WaitForSeconds(1f);
        while (!levelOver)
        {
            Debug.Log("levelOver: " + levelOver + "| currentUnitIndex: "+currentUnitIndex+ "units count: " + units.Count + "| currentSceneIndex: " 
            + SceneManager.GetActiveScene().buildIndex);
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
        }
    }

    public void EndLevel()
    {
        levelOver = true;
    }
}
