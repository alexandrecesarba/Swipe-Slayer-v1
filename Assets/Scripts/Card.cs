using UnityEngine;

public class Card : MonoBehaviour {

    #region Variables
    public IPower power;
    
    private PlayerController playerController;
    public Vector3 movePoint;

    bool selected;
    Vector2 currentVelocity;
    float smoothTime = .1f;
    #endregion


    #region Unity Methods

    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    void OnMouseDown()
    {
        Debug.Log("Carta!");
        SelectCard();
    }
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // movePoint = transform.position;
        power = GetComponent<IPower>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, movePoint) >= 0.05)
        {
            transform.position = Vector2.SmoothDamp(transform.position, movePoint, ref currentVelocity, smoothTime);
        }
    }
    void SelectCard()
    {
        if (GameManager.Instance.player.GetComponent<PlayerController>().cardSelected != null)
        {
            GameManager.Instance.player.GetComponent<PlayerController>().cardSelected.UnselectCard();
        }
        GameManager.Instance.player.GetComponent<PlayerController>().cardSelected = this;
        if(!selected){
            movePoint = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }

    public void UnselectCard()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().cardSelected = null;
        movePoint = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }

    public void Activate(GameObject user, Vector2 direction)
    {
        power.Activate(user, direction);
    }

    #endregion
}
