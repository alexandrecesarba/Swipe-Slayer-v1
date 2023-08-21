using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class TestTouch : MonoBehaviour
{
    private InputManager inputManager;
    private Camera cameraMain;
    void Awake()
    {
        inputManager = InputManager.Instance;
        cameraMain = Camera.main;
    }

    /// This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        inputManager.OnStartTouch += Move;
    }

    void OnDisable()
    {
        inputManager.OnStartTouch -= Move;
    }

    public void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }
}
