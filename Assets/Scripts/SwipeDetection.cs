using System.Collections;
using UnityEngine;

// [DefaultExecutionOrder(-1)]
public class SwipeDetection : MonoBehaviour
{
    #region Events
    public delegate void SwipeHandler(Vector2 direction);
    public event SwipeHandler OnSwipe;
    #endregion

    [SerializeField]
    private float minimumDistance = .2f;
    [SerializeField]
    private float maximumTime = 10f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .93f;
    [SerializeField]
    private PlayerInputHandler playerInputHandler;
    // private GameObject trail;

    // private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private float swipeDistance;
    private float swipeTime;
    private IClickable clickStartObject;
    private IClickable clickEndObject;
    
    private Coroutine coroutine;

    void Start()
    {
        // DontDestroyOnLoad(gameObject);
        Debug.LogWarning("NEW SWIPE DETECTION");
    }

    void OnEnable ()
    {
        playerInputHandler.TouchStarted += SwipeStart;
        playerInputHandler.TouchEnded += SwipeEnd;
        // try
        // {
        //     // inputManager = InputManager.Instance;
        // }
        // catch
        // {
        //     Debug.LogWarning("Instancia de inputManager inalcançável");
        // }
        // Debug.LogWarning("Enabling Swipe Detection. inputManager: " + inputManager.name);
        // inputManager.OnStartTouch += SwipeStart;
        // inputManager.OnEndTouch += SwipeEnd;
    }

    void OnDisable()
    {
        Debug.LogWarning("Disabling Swipe Detection");
        // inputManager.OnStartTouch -= SwipeStart;
        // inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        Debug.Log("TOQUE INICIAL: " + position + "| TIME: " + time);
        Collider2D hitColliders = Physics2D.OverlapCircle(position, 0f);
        if (hitColliders != null){
            clickStartObject = hitColliders.GetComponent<IClickable>();
            clickStartObject?.OnClickStart();
        }
        // trail.SetActive(true);
        // trail.transform.position = position;
        // coroutine = StartCoroutine(Trail());
    }

    // private IEnumerator Trail() 
    // {
    //     while(true)
    //     {
    //         trail.transform.position = new Vector3(inputManager.PrimaryPosition().x, inputManager.PrimaryPosition().y, Camera.main.transform.position.z+1);
    //         yield return null;
    //     }
    // }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        // trail.SetActive(false);
        // StopCoroutine(coroutine);
        endPosition = position;
        endTime = time;
        Debug.Log("TOQUE FINAL: " + position + "| TIME: " + time);

        Collider2D hitColliders = Physics2D.OverlapCircle(position, 0f);
        if (hitColliders != null){
            clickEndObject = hitColliders.GetComponent<IClickable>();
            if (clickEndObject != null && clickEndObject == clickStartObject)
            {
                clickEndObject.OnClick();
            } else{
                DetectSwipe();
            }
        }
    }
    
    private void DetectSwipe() {
        swipeDistance = Vector3.Distance(startPosition, endPosition);
        swipeTime = endTime - startTime;
        // Debug.Log("DIST : (" + endPosition + " - " + startPosition + " = " + (float)swipeDistance + "| TIME : (" + endTime + " - " + startTime + " = " + swipeTime);
        if (swipeDistance >= minimumDistance &&
         swipeTime <= maximumTime) {
            Debug.Log("LINE START: " + startPosition + "LINE END: " + endPosition);
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            OnSwipe?.Invoke(SwipeDirection(direction2D));
        }
    }

        public void ReactivateEvents()
    {
        // inputManager = InputManager.Instance;
        // inputManager.OnStartTouch += SwipeStart;
        // inputManager.OnEndTouch += SwipeEnd;
    }


    private Vector2 SwipeDirection(Vector2 direction) 
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
            return Vector2.up;
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
            return Vector2.left;
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
            return Vector2.right;
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
            return Vector2.down;
        }
        else if (Vector2.Dot(new Vector2(-1,1).normalized, direction) > directionThreshold)
        {
            Debug.Log("Swipe UpLeft");
            return new Vector2(-1,1);
        }
        else if (Vector2.Dot(new Vector2(1,1).normalized, direction) > directionThreshold)
        {
            Debug.Log("Swipe UpRight");
            return new Vector2(1,1);
        }
        else if (Vector2.Dot(new Vector2(-1,-1).normalized, direction) > directionThreshold)
        {
            Debug.Log("Swipe DownLeft");
            return new Vector2(-1,-1);
        }
        else if (Vector2.Dot(new Vector2(1,-1).normalized, direction) > directionThreshold)
        {
            Debug.Log("Swipe DownRight");
            return new Vector2(1,-1);
        }
        else
        {
            Debug.Log("NO DIRECTION");
            return Vector2.zero;
        }
    }
}

