using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Sprite[] sprites; // Um array de sprites que você pode atribuir no Inspector.

    private Image[] images; // Um array para armazenar as referências das imagens.

    private void Start()
    {
        images = GetComponentsInChildren<Image>(); // Obtém todas as imagens dentro do objeto atual.

        // Preenche as imagens com os sprites, mas não mais do que o número de sprites disponíveis.
        for (int i = 0; i < images.Length; i++)
        {
            if (i < sprites.Length)
            {
                images[i].sprite = sprites[i];
                images[i].enabled = true; // Ativa a imagem.
            }
            else
            {
                images[i].enabled = false; // Desativa a imagem se não houver sprite correspondente.
            }
        }
    }
}
