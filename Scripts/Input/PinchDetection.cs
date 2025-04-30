using System;
using System.Collections;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.UIElements;

public class PinchDetection : MonoBehaviour
{
    public static VisualElement zoomElement;
    private InputManager inputManager;
    public AbstractMap map;
    float previousDistance = 0f;
    public float _zoomSpeed = 0.25f;
    public float _zoomFactor;
    public float newZoom;
    
    private void Awake() 
    {
        inputManager = InputManager.Instance;      
    }
    private void OnEnable() 
    {
        inputManager.OnStartZoom += ZoomStart;   
        inputManager.OnEndZoom += ZoomEnd;
    }
    private void OnDisable() 
    {
        inputManager.OnStartZoom -= ZoomStart;   
        inputManager.OnEndZoom -= ZoomEnd;
    }
    private void ZoomEnd(Vector2 position, float time)
    {
        StopCoroutine(ZoomDetection());
        print("ZoomEnd");
    }
    private void ZoomStart(Vector2 position, float time)
    {
        StartCoroutine(ZoomDetection());
    }
    IEnumerator ZoomDetection()
    {
        while(inputManager.zoom == true)
        {
            inputManager.pressed = false;

            if(inputManager.CalculateZoom() > previousDistance)
            {         
                ZoomElement(inputManager.CalculateZoom() * _zoomFactor, zoomElement);
                // ZoomMapUsingTouchOrMouse(inputManager.CalculateZoom() * _zoomFactor);                      
                // print("ZoomIN " + inputManager.CalculateZoom() * _zoomFactor);
            }
            else if(inputManager.CalculateZoom() < previousDistance)
            {
                ZoomElement(-inputManager.CalculateZoom() * _zoomFactor, zoomElement);
                // ZoomMapUsingTouchOrMouse(-inputManager.CalculateZoom() * _zoomFactor);    
                // print("ZoomOut "+ inputManager.CalculateZoom());
            }
            previousDistance = inputManager.CalculateZoom();
            yield return null;
        }
    }
    void ZoomMapUsingTouchOrMouse(float zoomFactor)
    {
        float zoom = Mathf.Max(0f, Mathf.Min(map.Zoom + zoomFactor * _zoomSpeed, 21.0f));

        if (Math.Abs(zoom - map.Zoom) > 0.0f)
        {
            map.UpdateMap(map.CenterLatitudeLongitude, zoom);
            newZoom = map.Zoom;
        }
    }
    void ZoomElement(float zoomFactor, VisualElement zoomElement)
    {
        // Adjust the scale of the VisualElement based on the zoomFactor
        Vector3 newScale = zoomElement.transform.scale + new Vector3(zoomFactor, zoomFactor, 0f);
        zoomElement.transform.scale = newScale;
    }
}
