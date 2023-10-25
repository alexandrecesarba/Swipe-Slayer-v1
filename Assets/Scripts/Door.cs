using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    #region Variables
    public bool isOpen = false;
    [SerializeField]
    private Sprite openSprite;
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    public LevelLoader levelLoader;
    #endregion

    #region Unity Methods
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
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

    public void Interact(GameObject _)
    {
        if (isOpen){
            // LevelLoader.Instance.LoadNextLevel();
            if (levelLoader == null) {
            } else {
                // GameManager.Instance.LevelEnded();
                levelLoader.LoadNextLevel();
            };
        } else {
            Debug.Log("Porta Trancada!");
        }
    }

}
