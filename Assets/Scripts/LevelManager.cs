using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<IUnit> units = new();
    public float turnTime = .5f;
    private int currentUnitIndex;
    private int playerUnitIndex;
    private int playCount = 0;
    public bool gameOver = false;
    [SerializeField]
    public Material highlightMaterial;
    [SerializeField]
    public float maxTurnTime;
    [SerializeField]
    public TimeBar timeBar;
    private int kills;


    void Start()
    {
        currentUnitIndex = 0;
        timeBar.SetMaxTime(maxTurnTime);
        units.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());
        currentUnitIndex = units.FindIndex(unit => unit is PlayerController);
        playerUnitIndex = currentUnitIndex;

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
    }


    public IEnumerator TurnLoop()
    {
        while (!gameOver)
        {
            Debug.Log("Units Count: " + units.Count);
            Debug.Log("Current Index: " + currentUnitIndex);
            IUnit currentUnit = units[currentUnitIndex];
            MonoBehaviour unitMB = (MonoBehaviour) units[currentUnitIndex];
            

            if (currentUnit.CanPlay)
            {
                currentUnit.IsPlaying = true;
                unitMB.GetComponent<Renderer>().material.SetFloat("_Outline_Thickness", 100);
                StartCoroutine(currentUnit.Play(turnTime));
                float startTime = Time.time;
                if (currentUnitIndex == playerUnitIndex){
                    timeBar.gameObject.SetActive(true);
                }

                // Espera até que a unidade termine de jogar ou após um tempo limite de 10 segundos
                while (currentUnit.IsPlaying && Time.time - startTime < maxTurnTime)
                {
                    timeBar.SetTime(maxTurnTime - (Time.time - startTime));
                    yield return null; // Aguarda um frame antes de verificar novamente
                }
                currentUnit.IsPlaying = false;
                if (currentUnitIndex == playerUnitIndex){
                    timeBar.gameObject.SetActive(false);
                }
            }
            // yield return new WaitForSeconds(turnTime);
            unitMB.GetComponent<Renderer>().material.SetFloat("_Outline_Thickness", 0);
            currentUnitIndex++;
            if (currentUnitIndex >= units.Count)
            {
                currentUnitIndex = 0;
            }
            playCount++;
        }

        // Game over logic
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
            gameOver = true;
        }
        
    }

    public bool IsGameOver()
    {
        // Implementar a lógica para verificar se o jogo terminou.
        // Isso pode ser quando não há mais inimigos, o jogador foi derrotado, etc.
        return false;
    }


}
