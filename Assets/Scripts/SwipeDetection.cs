using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private float minimumDistance = .2f;
    [SerializeField]
    private float maximumTime = 10f;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private float swipeDistance;
    private float swipeTime;

    void Awake()
    {
        inputManager = InputManager.Instance;
    }

    void OnEnable ()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }
    
    private void DetectSwipe() {
        swipeDistance = Vector3.Distance(startPosition, endPosition);
        swipeTime = endTime - startTime;
        Debug.Log("DIST : (" + endPosition + " - " + startPosition + " = " + (float)swipeDistance + "| TIME : (" + endTime + " - " + startTime + " = " + swipeTime);
        if (swipeDistance >= minimumDistance &&
         swipeTime <= maximumTime) {
            Debug.Log("LINE START: " + startPosition + "LINE END: " + endPosition);
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        }
    }
}

