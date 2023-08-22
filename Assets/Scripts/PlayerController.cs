using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public SwipeDetection swipeDetection;
    private PlayerMovement controls;
    private MovingObject movingObject;
    public int attackPoints = 3;

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
        movingObject = this.GetComponent<MovingObject>();
        swipeDetection.OnSwipe += HandleSwipe;
    }


    private IEnumerator MovePlayer(Vector2 direction) {
        if (GameManager.instance.playersTurn){
            GameObject enemyObject = movingObject.Move(direction);
            // Se atacou um inimigo, chame a função do script "EnemyScript"
            if (enemyObject != null){
                Damageable enemyScript = enemyObject.GetComponent<Damageable>();
                if (enemyScript != null) {
                    enemyScript.TakeDamage(attackPoints); // Supondo que existe uma função TakeDamage no script do inimigo
                } else {
                    Debug.Log("Deu Ruim");
                }
            }

            GameManager.instance.playersTurn = false;

            yield return new WaitForSeconds(1f);

        }
        // Debug.Log(GameManager.instance.playersTurn);
    }
    
    public void HandleSwipe(Vector2 direction) {
        Debug.Log("Handling Swipe on PlayerController...");
        StartCoroutine(MovePlayer(direction));
    }


}
