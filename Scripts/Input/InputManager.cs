using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    // Events ...........................................................
    #region Events
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    public delegate void StartZoomEvent(Vector2 position, float time);
    public event StartZoomEvent OnStartZoom;

    public delegate void EndZoomEvent(Vector2 position, float time);
    public event EndZoomEvent OnEndZoom;
    public Action OnLanguageChange;
    #endregion Events
    //..................................................................
    private TouchInputs touchInputs;
    private Camera mainCamera;

    //Boolean Checks
    public bool pressed;
    public bool zoom;
    public override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        touchInputs = new TouchInputs();
    }
    private void OnEnable()
    {
        touchInputs.Enable();

        touchInputs.TouchActions.TouchPress.started += ctx => StartTouch(ctx);     
        touchInputs.TouchActions.TouchPress.canceled += ctx => EndTouch(ctx); 

        touchInputs.TouchActions.SecondaryTouchContact.started += ctx => ZoomStart(ctx);  
        touchInputs.TouchActions.SecondaryTouchContact.canceled += ctx => ZoomEnd(ctx);     

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
    private void ZoomStart(InputAction.CallbackContext  ctx)
    {
        zoom = true;
        if(OnStartZoom != null) 
        {
            OnStartZoom(SecondaryTouch(), (float)ctx.startTime);        
        }
    }
    private void ZoomEnd(InputAction.CallbackContext  ctx)
    {
        zoom = false;
        if(OnEndZoom != null) 
        {
            OnEndZoom(SecondaryTouch(), (float)ctx.time);            
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
    public float CalculateZoom()
    {
        float distance;
        return distance = (PrimaryTouch() - SecondaryTouch()).magnitude;
    }
}
