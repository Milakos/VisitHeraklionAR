using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputIntro : MonoBehaviour
{
   // Events ...........................................................
    #region Events
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    public Action OnLanguageChange;
    #endregion Events
    //..................................................................
    private TouchInputs touchInputs;
    private Camera mainCamera;

    //Boolean Checks
    public bool pressed;
    public bool zoom;
    public void Awake()
    {
        mainCamera = Camera.main;
        touchInputs = new TouchInputs();
    }
    private void OnEnable()
    {
        touchInputs.Enable();

        touchInputs.TouchActions.TouchPress.started += ctx => StartTouch(ctx);     
        touchInputs.TouchActions.TouchPress.canceled += ctx => EndTouch(ctx);    

        touchInputs.TouchActions.space.performed += SpaceEvent;       
    }
    private void OnDisable()
    {
        touchInputs.Disable();
    }

#region Event Methods 
    private void SpaceEvent(InputAction.CallbackContext context)
    {
        if(OnLanguageChange != null)
        {
            OnLanguageChange();
        }        
    }
    private void StartTouch(InputAction.CallbackContext context)
    {       
        if(OnStartTouch != null) 
        {
            OnStartTouch(PrimaryTouch(), (float)context.startTime);
            pressed = true;
        }
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        if(OnEndTouch != null) 
        {       
            OnEndTouch(PrimaryTouch(), (float)context.time);
            pressed = false;
        }     
    }
    
#endregion Event Methods

    public Vector2 PrimaryTouch()
    {
        return Utils.ScreenToWorld(mainCamera, touchInputs.TouchActions.PrimaryFingerPosition.ReadValue<Vector2>());       
    }
    public Vector2 SecondaryTouch()
    {
        return Utils.ScreenToWorld(mainCamera, touchInputs.TouchActions.SecondaryFingerPosition.ReadValue<Vector2>());
    }
}
