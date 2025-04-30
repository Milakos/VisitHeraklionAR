using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTEST : MonoBehaviour
{
    public Camera arCamera;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public GameObject objectToInstantiate;
    GameObject instance;
    
    public event Action OnPlaneDetected;
    private bool planeDetected = false;
    bool planeInView = false;
    private bool completed = false;

    public GameObject tap;
    public GameObject track;
    private List<ARPlane> trackedPlanes = new List<ARPlane>();
    private void OnEnable()
    {
        OnPlaneDetected += HandlePlaneDetected;
    }
    private void OnDisable()
    {
        OnPlaneDetected -= HandlePlaneDetected;
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }
    void Update()
    {
        if(completed == false)
        {
            UpdateObjectVisibility();    
        } 
        else
        {
            planeManager.planesChanged -= OnPlanesChanged;
            planeManager.enabled = false;
            planeDetected = false;
            tap.SetActive(false);
            track.SetActive(false);
        }      
        OnScreenTouch();
    }
    public void EnablePlaneDetection(Toggle toggle)
    {
        completed = false;
        planeManager.enabled = toggle.isOn;
        if(toggle.isOn == true)
        {
            planeManager.planesChanged += OnPlanesChanged; 
        }
        else if(toggle.isOn == false)
        {
            planeManager.planesChanged -= OnPlanesChanged; 
        }
    }  
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            trackedPlanes.Add(plane);

            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                OnPlaneDetected?.Invoke();
                tap.SetActive(true);
                break;
            }
        }
    }
    private void HandlePlaneDetected()
    {
        Debug.Log("Plane detected! Touch the screen to place an object.");
        planeDetected = true;
    }

    public void OnScreenTouch()
    {
        if (planeDetected && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touchPosition = Input.GetTouch(0).position;
            var hitResults = new List<ARRaycastHit>();

            if (raycastManager.Raycast(touchPosition, hitResults, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hitResults[0].pose;
                tap.SetActive(false);
                
                instance = Instantiate(objectToInstantiate, hitPose.position, hitPose.rotation);
                
                // instance.transform.LookAt(arCamera.gameObject.transform);
                Vector3 directionToCamera = arCamera.gameObject.transform.position - instance.transform.position;
                
                // Eliminate the x and z components of the direction
                directionToCamera.y = 0;

                // Calculate the rotation to look at the camera on the y-axis only
                Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);

                // Apply the y-axis only rotation to the instance
                instance.transform.rotation = lookRotation;
                
                instance.transform.parent = null;
                
                foreach (var item in planeManager.trackables)
                {
                    item.gameObject.SetActive(false);
                }
                completed = true;
            }
        }
    }
    void UpdateObjectVisibility()
    {   
        foreach (var plane in trackedPlanes)
        {
            if (IsPlaneInView(plane))
            {
                planeInView = true;
                break;
            }
        }
        track.SetActive(!planeInView);
        tap.SetActive(planeInView);
    }
    bool IsPlaneInView(ARPlane plane)
    {
        Vector3 screenPoint = arCamera.WorldToViewportPoint(plane.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
    public void Reset()
    {
        Destroy(instance);
        completed = true;
        planeDetected = false;
        planeInView = false;
    }
}
