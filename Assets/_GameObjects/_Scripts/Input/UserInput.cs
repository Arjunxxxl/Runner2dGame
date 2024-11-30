using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] private float moveDir; // Indicates if the player is moving right/Left
    [SerializeField] private bool isJumping = false; // Indicates if the player is jumping

    private Vector2 startInputPosition1; // Starting position of the input (mouse or touch)
    private Vector2 currentInputPosition1; // Current position of the input (mouse or touch)
    private Vector2 startInputPosition2; // Starting position of the input (mouse or touch)
    private Vector2 currentInputPosition2; // Current position of the input (mouse or touch)
    private Vector2 dragDelta1;
    private Vector2 swipeDelta1;
    private Vector2 swipeDelta2;
    private bool isDragging1 = false;
    private bool isDragging2 = false;
    
    [Space]
    
    public bool useMouseControl = false;

    public float MoveDir => moveDir;
    public bool IsJumping => isJumping;
    
    #region SingleTon
    public static UserInput Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    void Update()
    {
        HandleTouchInput(); // Mobile input
        //HandleMouseInput(); // Mouse input
        //HandleDesktopInput(); // Desktop input
    }

    private void HandleTouchInput()
    {
        #if UNITY_ANDROID
        
        // Reset jumping flag after one frame
        isJumping = false;
        
        if (Input.touchCount > 0)
        {
            Touch touch1 = Input.GetTouch(0);

            switch (touch1.phase)
            {
                case TouchPhase.Began:
                    // Touch started
                    startInputPosition1 = touch1.position;
                    isDragging1 = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging1)
                    {
                        currentInputPosition1 = touch1.position;
                        dragDelta1 = currentInputPosition1 - startInputPosition1;
                        
                        // Horizontal drag for left/right movement
                        if (Mathf.Abs(dragDelta1.x) > Mathf.Abs(dragDelta1.y))
                        {
                            moveDir = dragDelta1.x > 0 ? 1 : -1;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging1)
                    {
                        currentInputPosition1 = touch1.position;
                        swipeDelta1 = currentInputPosition1 - startInputPosition1;

                        // Detect upward swipe to jump (independent of horizontal dragging)
                        if (Mathf.Abs(swipeDelta1.y) > Mathf.Abs(swipeDelta1.x) && swipeDelta1.y > 100f)
                        {
                            isJumping = true;
                        }

                        // Reset drag state
                        isDragging1 = false;
                        moveDir = 0;
                    }
                    break;
            }
            
            if (Input.touchCount > 1)
            {
                Touch touch2 = Input.GetTouch(1);

                switch (touch2.phase)
                {
                    case TouchPhase.Began:
                        // Touch started
                        startInputPosition2 = touch2.position;
                        isDragging2 = true;
                        break;

                    case TouchPhase.Moved:
                        if (isDragging2)
                        {
                            currentInputPosition2 = touch2.position;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (isDragging2)
                        {
                            currentInputPosition2 = touch2.position;
                            swipeDelta2 = currentInputPosition2 - startInputPosition2;

                            // Detect upward swipe to jump (independent of horizontal dragging)
                            if (Mathf.Abs(swipeDelta2.y) > Mathf.Abs(swipeDelta2.x) && swipeDelta2.y > 100f)
                            {
                                isJumping = true;
                            }

                            // Reset drag state
                            isDragging2 = false;
                        }
                        break;
                }
            }
        }
        else
        {
            // Reset jumping flag after one frame
            isJumping = false;
        }
        
#endif
    }

    private void HandleMouseInput()
    {
        #if UNITY_EDITOR
        
        if (!useMouseControl)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            startInputPosition1 = Input.mousePosition;
            isDragging1 = true;
        }

        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            if (isDragging1)
            {
                currentInputPosition1 = Input.mousePosition;
                Vector2 delta = currentInputPosition1 - startInputPosition1;

                // Horizontal drag for left/right movement
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    moveDir = delta.x > 0 ? 1 : -1;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            if (isDragging1)
            {
                currentInputPosition1 = Input.mousePosition;
                Vector2 swipeDelta = currentInputPosition1 - startInputPosition1;

                // Detect upward swipe to jump
                if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x) && swipeDelta.y > 100f)
                {
                    isJumping = true;
                }

                // Reset drag state
                isDragging1 = false;
                moveDir = 0;
            }
        }
        else
        {
            // Reset jumping flag after one frame
            isJumping = false;
        }
        
        #endif
    }

    private void HandleDesktopInput()
    {
        #if UNITY_EDITOR

        if (useMouseControl)
        {
            return;
        }
        
        // Horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDir = horizontalInput;

        // Jumping
        isJumping = verticalInput > 0 || Input.GetKey(KeyCode.Space);
        
        #endif
    }
}
