using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public SwipeDetection swipeDetection;
    private PlayerMovement controls;
    // private MovingObject movingObject;
    private Melee playerMelee; // Adicione esta linha

    

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerMovement();
        swipeDetection = swipeDetection.GetComponent<SwipeDetection>();
    }

    // This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        controls.Enable();
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        controls.Disable();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    void Start()
    {
        controls.Main.Movement.performed += ctx => StartCoroutine(MovePlayer(ctx.ReadValue<Vector2>()));
        // movingObject = this.GetComponent<MovingObject>();
        swipeDetection.OnSwipe += HandleSwipe;
        playerMelee = GetComponent<Melee>(); 
    }


   private IEnumerator MovePlayer(Vector2 direction)
    {
        if (GameManager.instance.playersTurn)
        {
            playerMelee.ExecuteAttack(direction); // Ataque usando o novo componente de ataque
            GameManager.instance.playersTurn = false;
            yield return new WaitForSeconds(1f);
        }
    
    }
    
    public void HandleSwipe(Vector2 direction) {
        Debug.Log("Handling Swipe on PlayerController...");
        StartCoroutine(MovePlayer(direction));
    }


}
