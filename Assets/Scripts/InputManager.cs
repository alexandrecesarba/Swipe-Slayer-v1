using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnEndTouch;
    #endregion

    private PlayerMovement playerControls;
    private Camera  mainCamera;

    private void Awake() 
    {
        playerControls = new PlayerMovement();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        playerControls.Enable();
        TouchSimulation.Enable();

        // UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();

        playerControls.Disable();
        TouchSimulation.Disable();

        // UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        playerControls.Touch.PrimaryContact.started += ctx => StartTouch(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx);

    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(PrimaryPosition(), (float)context.startTime);
        Debug.Log("Touch started" + PrimaryPosition());
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(PrimaryPosition(), (float)context.time);
        Debug.Log("Touch ended" + PrimaryPosition());
    }

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

    public Vector2 PrimaryPosition() 
    {
        return Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
