using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour, IUnit
{
    #region Variables
    [SerializeField] private PlayerInputHandler swipeDetection;
    // private PlayerMovement controls;
    private Damageable damage;
    private MovingObject movement;
    private Melee playerMelee; // Adicione esta linha
    private Interacts interaction;

    public Card cardSelected;
    public bool IsPlaying {get; set;}
    public bool CanPlay {get; set;}
    public bool IsUsingCard {get; set;}
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Awake()
    {
        // controls = new PlayerMovement();
        GameManager.Instance.player = gameObject;
        DontDestroyOnLoad(this);
        // swipeDetection = swipeDetection.GetComponent<SwipeDetection>();
    }

    // This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        // controls.Enable();
        this.CanPlay = true; // Mover para aqui.
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        // swipeDetection.OnSwipe -= HandleSwipe;
        // controls.Disable();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    void Start()
    {
        try
        {
            // swipeDetection = GameObject.Find("SwipeDetection").GetComponent<SwipeDetection>();
            // swipeDetection = SwipeDetection.Instance;
            swipeDetection = GetComponent<PlayerInputHandler>();
            swipeDetection.OnSwipe += HandleSwipe;
        }
        catch
        {
            Debug.LogWarning("Não foi possível carregar o swipe detection");
        }
        // controls.Main.Movement.performed += ctx => StartCoroutine(MovePlayer(ctx.ReadValue<Vector2>()));
        movement = this.GetComponent<MovingObject>();
        
        playerMelee = GetComponent<Melee>();
        damage = GetComponent<Damageable>();
        interaction = GetComponent<Interacts>();
        // swipeDetection = SwipeDetection.Instance;

        // damage.OnDeath += HandlePlayerDeath;
        // this.CanPlay = true;
    }



    // private void HandlePlayerDeath(IUnit unit)
    // {
        
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {  
            case "Collectible":
                IPickup pickUp = other.GetComponent<IPickup>();
                pickUp?.Activate(gameObject);
                break;
            case "Interactable":
                interaction?.Interact(other.gameObject);
                Debug.Log("Interacting with "+ other.gameObject.name);
                break;
            default:
            Debug.Log("NO TAG MATCH: " + other.tag);
                break;
        }
        // if (other.CompareTag("Exit"))
        // {
        //     LevelManager.Instance.unlocked?.ChangeScene();
        // }
    }
    
    #endregion
    
    private IEnumerator MovePlayer(Vector2 direction)
    {
        if (IsPlaying)
        {
            GameObject hit = movement.AttemptMoveInTiles(direction, 1, out _);
            IsPlaying = false;
            
            yield return new WaitForSeconds(1f);
        }
    
    }
    
    public void HandleSwipe(Vector2 direction) {
        Debug.Log("Handling Swipe on PlayerController...");
        if (cardSelected == null)
        {
            StartCoroutine(MovePlayer(direction));
        } else {
            StartCoroutine(PlayCard(cardSelected, direction));

        }
    }

    private IEnumerator PlayCard(Card cardSelected, Vector2 direction)
    {
        IsPlaying = false;
        IsUsingCard = true;
        cardSelected.Activate(this.gameObject, direction);
        cardSelected.UnselectCard();

        yield return new WaitForSeconds(cardSelected.power.Duration);
        IsUsingCard = false;
    }

    public IEnumerator Play(float time)
    {
        yield return null;
        //implementar lógica para quando estiver na vez do jogador
    }

}
