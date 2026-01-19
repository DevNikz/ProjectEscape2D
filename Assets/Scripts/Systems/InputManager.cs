#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputActions playerInputActions;

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
    #if USE_NEW_INPUT_SYSTEM
            return playerInputActions.UI.Click.WasPressedThisFrame();
    #else
            return Input.GetMouseButtonDown(0);
    #endif
    }
}