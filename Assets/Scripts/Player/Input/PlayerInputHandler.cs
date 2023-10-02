using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class PlayerInputHandler : MonoBehaviour
{
    #region Events
    public delegate void TouchEvent(Vector2 position, float time);
    public event TouchEvent TouchStarted;
    public event TouchEvent TouchEnded;
    public delegate void SwipeHandler(Vector2 direction);
    public event SwipeHandler OnSwipe;
    #endregion

    [SerializeField] private PlayerMovement playerControls;

    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .93f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Vector2 touchLastPos;
    private double touchStartTime;
    private double touchEndTime;


    private float swipeDistance;
    private float swipeTime;
    private Camera  mainCamera;
    
    [SerializeField]
    private float swipeMinimumDistance = .2f;
    [SerializeField]
    private float swipeMaximumTime = 1f;

    private void Awake() 
    {
        // playerControls = new PlayerMovement();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        // playerControls.Enable();
        TouchSimulation.Enable();

        // UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();

        // playerControls.Disable();
        TouchSimulation.Disable();

        // UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        // playerControls.Touch.PrimaryContact.started += ctx => StartTouch(ctx);
        // playerControls.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx);

    }
    public void Reload()
    {
        mainCamera = Camera.main;
    }


    public async void OnStartTouch(InputAction.CallbackContext context)
    {
        if (context.started){
            Debug.LogWarning("STARTED");
            await Task.Delay(50);
            touchStartTime = context.startTime;
            touchStartPos = PrimaryPosition(touchLastPos);
            Debug.Log("touchStartPos = PrimaryPosition(touchStartPos); = " + touchStartPos);
            TouchStarted?.Invoke(PrimaryPosition(touchStartPos), (float)context.startTime);
        }
        else if (context.canceled) {
            Debug.LogWarning("CANCELED");
            await Task.Delay(50);
            touchEndTime = context.time;
            touchEndPos = PrimaryPosition(touchEndPos);
            SwipeEnd(touchEndPos, (float)context.time);
            // TouchEnded?.Invoke(PrimaryPosition(touchEndPos), (float)context.time);
        }
        else {
            Debug.Log("BUGOU AQUI!!!!!");
        }
    }

    public void OnEndTouch(InputAction.CallbackContext context)
    {
        Debug.LogWarning("END TOUCH");
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            touchEndPos = context.ReadValue<Vector2>();
            touchLastPos = context.ReadValue<Vector2>();
            // Debug.Log("TOUCH MOVED: " + touchEndPos);
            // Faça algo com a posição do toque atual

            // Calcule a diferença entre a posição inicial e a posição atual
            // Vector2 swipeDelta = touchEndPos - touchStartPos;

            // Determine se o swipe atende aos critérios desejados (por exemplo, um movimento mínimo)
            // if (swipeDelta.magnitude > threshold)
            {
                // Um swipe válido foi detectado; faça algo com ele
            }
        }
        else if (context.started)
        {
            touchStartPos = context.ReadValue<Vector2>();
            Debug.Log("TOUCH STARTED: " + touchStartPos);
            // Armazene a posição inicial do toque
        }
    }
    // public async void StartTouch(InputAction.CallbackContext context)
    // {
    //     await Task.Delay(50);
    //     OnStartTouch?.Invoke(PrimaryPosition(context), (float)context.startTime);
    //     // Debug.Log("TOQUE INICIAL" + PrimaryPosition());
    // }
    // public async void EndTouch(InputAction.CallbackContext context)
    // {
    //     await Task.Delay(50);
    //     OnEndTouch?.Invoke(PrimaryPosition(context), (float)context.time);
    //     // Debug.Log("TOQUE FINAL" + PrimaryPosition());
    // }

    // private void FingerDown(Finger finger)
    // {
    //     OnStartTouch?.Invoke(finger.screenPosition, Time.time);
    // }

    // void Update()
    // {
    //     Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers);
    //     foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
    //     {
    //         Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
    //     }
    // }

    private void SwipeEnd(Vector2 position, float time)
    {
        // trail.SetActive(false);
        // StopCoroutine(coroutine);
        // endPosition = position;
        // endTime = time;
        Debug.Log("TOQUE FINAL: " + position + "| TIME: " + time);

        Collider2D hitColliders = Physics2D.OverlapCircle(position, 0f);
        // if (hitColliders != null){
        //     clickEndObject = hitColliders.GetComponent<IClickable>();
        //     if (clickEndObject != null && clickEndObject == clickStartObject)
        //     {
        //         clickEndObject.OnClick();
        //     } else{
        //         DetectSwipe();
        //     }
        // }
        DetectSwipe();
    }

    private void DetectSwipe() {
        Debug.Log("Detecting Swipe...");
        swipeDistance = Vector3.Distance(touchStartPos, touchEndPos);
        swipeTime = (float)(touchEndTime - touchStartTime);
        // Debug.Log("DIST : (" + endPosition + " - " + startPosition + " = " + (float)swipeDistance + "| TIME : (" + endTime + " - " + startTime + " = " + swipeTime);
        if (swipeDistance >= swipeMinimumDistance &&
         swipeTime <= swipeMaximumTime) {
            Debug.Log("LINE START: " + touchStartPos + "LINE END: " + touchEndPos);
            Debug.DrawLine(touchStartPos, touchEndPos, Color.red, 5f);
            Vector3 direction = touchEndPos - touchStartPos;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            OnSwipe?.Invoke(SwipeDirection(direction2D));
        }
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

    public Vector2 PrimaryPosition(Vector2 pos) 
    {
        return Utils.ScreenToWorld(mainCamera, pos);
    }
}