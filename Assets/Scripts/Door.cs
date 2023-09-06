using UnityEngine;

public class Door : MonoBehaviour {

    #region Variables
    public bool isOpen = false;
    public Sprite openSprite;
    public Sprite closedSprite;
    #endregion

    public void Open()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }
    public void Close()
    {
        isOpen = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }


}
