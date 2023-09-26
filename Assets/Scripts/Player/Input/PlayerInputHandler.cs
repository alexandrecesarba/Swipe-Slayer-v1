using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class PlayerInputHandler : MonoBehaviour
{
    #region Events
    // public delegate void TouchEvent(Vector2 position, float time);
    // public event TouchEvent OnStartTouch;
    // public event TouchEvent OnEndTouch;
    #endregion

    // private PlayerMovement playerControls;
    private Camera  mainCamera;

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


    public void OnStartTouch(InputAction.CallbackContext context)
    {
        if (context.canceled){
            Debug.LogWarning("CANCELED");
        }
        else {
            Debug.LogWarning("STARTED");
        }
    }

    public void OnEndTouch(InputAction.CallbackContext context)
    {
        Debug.LogWarning("END TOUCH");
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

    public Vector2 PrimaryPosition(InputAction.CallbackContext context) 
    {
        return Utils.ScreenToWorld(mainCamera, context.ReadValue<Vector2>());
    }
}