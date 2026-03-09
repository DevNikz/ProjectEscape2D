#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputActions playerInputActions;
    [SerializeField] bool allowInputRead;
    public bool IsInputAllowed() { return allowInputRead; }
    public void SetInputAllowed(bool value) { allowInputRead = value; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();

        SetInputAllowed(true);
    }

    public Vector2 GetMouseScreenPosition()
    {
        #if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
        #else
        return Input.mousePosition;
        #endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
        if(allowInputRead) {
    #if USE_NEW_INPUT_SYSTEM
            return playerInputActions.UI.Click.WasPressedThisFrame();
    #else
            return Input.GetMouseButtonDown(0);
    #endif
        }
        else return false;
    }

    public bool IsMouseButtonUpThisFrame()
    {
        if(allowInputRead) {
    #if USE_NEW_INPUT_SYSTEM
        return playerInputActions.UI.Click.WasReleasedThisFrame();
    #else
        return Input.GetMouseButtonUp(0);
    #endif
        }
        else return false;
    }
}