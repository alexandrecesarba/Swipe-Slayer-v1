using UnityEngine;
using System.Collections;


public class DamageFlash : MonoBehaviour
{
    public Material damageMaterial; // Material de dano.
    public float flashDuration = 0.15f; // Tempo de piscar em branco.

    private Renderer rend;
    private Material defaultMaterial;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultMaterial = rend.material;
    }

    public void Flash()
    {
        StartCoroutine(FlashDamageMaterial());
    }

    private IEnumerator FlashDamageMaterial()
    {
        rend.material = damageMaterial;
        yield return new WaitForSeconds(flashDuration);
        rend.material = defaultMaterial;
    }
}
