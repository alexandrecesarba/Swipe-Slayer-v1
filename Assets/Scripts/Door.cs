using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    #region Variables
    public bool isOpen = false;
    public Sprite openSprite;
    public Sprite closedSprite;
    #endregion

    public void Open()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
        Debug.Log("Opening Door");
    }
    public void Close()
    {
        isOpen = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        Debug.Log("Closing Door");
    }

    public void Interact(GameObject _)
    {
        if (isOpen){
            Debug.Log("Porta Aberta! Entrando...");
        } else {
            Debug.Log("Porta Trancada!");
        }
    }
}
