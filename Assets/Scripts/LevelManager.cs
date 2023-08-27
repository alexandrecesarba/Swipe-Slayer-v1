using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<IUnit> units = new();
    public float turnTime = 1f;
    private int currentUnitIndex;
    private int playCount = 0;
    public bool gameOver = false;


    void Start()
    {
        currentUnitIndex = 0;
        
        units.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());
        Debug.Log(units.Count);
        // currentUnitIndex = units.FindIndex(unit => unit is PlayerController);

        foreach (IUnit unit in units)
        {
            if (unit is MonoBehaviour unitBehaviour)
            {
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
            IUnit currentUnit = units[currentUnitIndex];
            Debug.Log("CURRENT UNIT: " + currentUnit);
            

            if (currentUnit.CanPlay)
            {
                currentUnit.IsPlaying = true;
                currentUnit.Play();
                float startTime = Time.time;

                // Espera até que a unidade termine de jogar ou após um tempo limite de 10 segundos
                while (currentUnit.IsPlaying && Time.time - startTime < 5f)
                {
                    yield return null; // Aguarda um frame antes de verificar novamente
                }
            }
            yield return new WaitForSeconds(turnTime);
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
