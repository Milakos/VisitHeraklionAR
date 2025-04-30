using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARLocation;
using Mapbox.Utils;
using PointsOfInterests;
using UnityEngine;

public class ARBoundHandler : Singleton<ARBoundHandler>
{
    [HideInInspector] public string globalMessageString = " "; // Global string that holds the messages that will be displayed at info toast message
    [HideInInspector] public InfoPointState infoPointState = new InfoPointState(); // Info point state 
    [HideInInspector] protected ImageTracker imageTracker {get; private set;} // Image tracker Script reference for handle behaviour when user is at info point
    [HideInInspector] protected ARElement aRElement {get; private set;}  
    public ARCameraHandler cameraHandler;
    LocationInfo locInfo; // User location Info

    [Header ("Properties")]
    public float minimumDistanceRender = 100f; // min distance value for pois calculations
    [Range(1, 100)] public float Radius; // Property of Generic Radius 
    [HideInInspector] public Plane[] frustumPlanes; // the camera planes
    private Vector2d locInfoVector;
    // -------- Indicator ----- \\
    [Header ("Group Bubble")]
    [SerializeField] private GameObject GroupIndicatorPrefab; // Prefab to indicate overlapping POIs
    
    private HashSet<GameObject> activationInProgress = new HashSet<GameObject>(); // A HashSet to keep track of objects already in the activation process
    [HideInInspector] public List<GameObject> objectsGreaterThanMin = new List<GameObject>(); // List to store the gameObjects that are beyond 300  meters
    [HideInInspector] public List<GameObject> prefabs = new List<GameObject>(); // List of bubble prefabs 
    [HideInInspector] public List<PointOfInterest> points = new List<PointOfInterest>();  // list of point of interests objects that contains data
    [HideInInspector] public List<GameObject> objectsLessThanMin = new List<GameObject>(); // List to store the gameObjects that are until 300  meters
    public Dictionary<Vector2d, GameObject> NearlatlongList = new Dictionary<Vector2d, GameObject>(); // Dictionary that stores the visible objects to be projected
    private Vector2d nearestLatLong; // property that updates the nearest poi based on position and NearlatlongList
    public Dictionary<Vector2d, GameObject> groupCoordsList = new Dictionary<Vector2d, GameObject>(); // Dictionary that stores the visible objects to be projected in a group
    private KeyValuePair<Vector2d, GameObject> nearestLatLongEntry; // property used for calculations of the nearest poi when Group is activated;
    public Dictionary<Vector2d, GameObject> NearlatlongListArrow = new Dictionary<Vector2d, GameObject>(); // Dictionary that stores the non-visible objects and < 300m to be projected in as arrow targets

    // ----- Reset Properties ---- \\
    [Header ("Reset Properties")]
    [SerializeField] private double accuracyThreshold = 5f; 
    [SerializeField] private float restartCooldownTimer = 0f; // Tracks cooldown time
    // [SerializeField] private float restartCooldownTimerEnable = 0f; // Tracks cooldown time
    // public const float restartCooldownDuration = 0f; // Cooldown duration in seconds

    // ----- Activate Properties ---- \\
    public Action<bool, GameObject> OnHotspotActivated;
    [HideInInspector] public bool activated {get;  set;} = false;
    [HideInInspector] public bool isInRadius {get; set;} = false;  

    [HideInInspector] private bool restartGPS {get; set;} = false;
    private float lastNoPointTimestamp = -1f; // Initialize to -1 to indicate no prior timestamp

    public override void Awake()
    {
        aRElement = FindObjectOfType<ARElement>();
        base.Awake();
    }
    private void Start()
    {
        cameraHandler.arCamera = ARLocationManager.Instance.MainCamera;
        GetComponent<WebMapLoader>().inita += AssignSigns;
        
        // ARLocationManager.Instance.OnTrackingLost.AddListener(TrackLost);

        ARLocationManager.Instance.OnTrackingLost.AddListener(() =>
        {
            TrackLost();
            // FindObjectOfType<ARSession>().Reset();
            Debug.Log("Tracking lost! Inline listener triggered.");
        });

        ARLocationManager.Instance.OnTrackingRestored.AddListener(TrackRestored);
        
        restartGPS = true;
        
        imageTracker = FindObjectOfType<ImageTracker>();
        StartCoroutine(FindObjectsOfTypePoints(3f));
    }
    private void TrackRestored()
    {
        restartGPS = true;
    }
    private void TrackLost()
    {
        globalMessageString = TextLibrary.GpsText.GetTranslatedText();
        UIExtentions.Display(aRElement.infoHolder, true);  
        restartGPS = false;
        DisableARVisibleObjects();
    }

    IEnumerator FindObjectsOfTypePoints(float delay)
    {
        yield return new WaitForSeconds(delay);

        var _points = FindObjectsOfType<PointOfInterest>().ToList();

        foreach (PointOfInterest point in _points)
        {
            if (point.isInMorePaths == false)
            {
                points.Add(point);
            }
        }
        _points.Clear();
    }
    private void AssignSigns()
    {
        var signs = FindObjectsOfType<assignNameTMP>();

        foreach (var item in signs)
        {
            prefabs.Add(item.gameObject);          
        }
    }

    void Update()
    {
        if (aRElement.enabled && aRElement.initialized)
        {
            locInfo = Input.location.lastData;
            var locInfoLat = locInfo.latitude;
            var locInfoLon = locInfo.longitude;
            locInfoVector = new Vector2d(locInfoLat, locInfoLon);           

            if(restartGPS == true)
            { 
                cameraHandler.elapsedTime += Time.deltaTime;
                cameraHandler.InitializeCameraClipPlanes();
            }
            if (infoPointState.isAtInfoPointState == false)
            {
                // CalculateAverageLatLong();
                CalculateNearestLatLong();                     // Calculate the nearest point of interest
                aRElement.imageTrack.SetActive(false);  //Deactivate imagetarcking mode
                
                DisplayARPOIHandler();
                
                // This block will run every frame (or conditionally, depending on your setup)
                if (NearlatlongListArrow.Count == 0 
                && NearlatlongList.Count == 0 
                && objectsLessThanMin.Count == 0
                && objectsGreaterThanMin.Count == 0 
                && cameraHandler.CameraInitialized == true)
                {
                    // Check if 5 seconds have passed since the last message was displayed
                    if (lastNoPointTimestamp < 0 || Time.time - lastNoPointTimestamp >= 5f)
                    {
                        lastNoPointTimestamp = Time.time; // Update the timestamp
                        globalMessageString = TextLibrary.FarAwayText.GetTranslatedText();
                        UIExtentions.Display(aRElement.infoHolder, true);
                    }
                }
                else
                {
                    // Reset the timestamp if the condition is no longer met
                    lastNoPointTimestamp = -1f;
                }
            }
            else
            {
                WhenInInfoPointHandler();
            }
        }
        else
        {
            DisableARVisibleObjects();
        }
    }
    private void DisplayARPOIHandler()
    {
        if (cameraHandler.CameraInitialized == true)
        {
            frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cameraHandler.arCamera);

            RetunNearestVector(locInfoVector);
            Debug.LogWarning($"POI: <300 {objectsLessThanMin.Count} and >300 {objectsGreaterThanMin.Count} and NLLL {NearlatlongList.Count} and Group {groupCoordsList.Count} and arrow {NearlatlongListArrow.Count}");

            if (!activated && !isInRadius)
            {
                if (NearlatlongList.ContainsKey(nearestLatLong))
                {
                    NearlatlongList.TryGetValue(nearestLatLong, out GameObject value);

                    if (NearlatlongList.Count > 1)
                    {
                        // Deactivate all other targets
                        foreach (var kvp in NearlatlongList)
                        {
                            if (!UIExtentions.IsApproximatelyEqual(kvp.Key, nearestLatLong)) // Use approximate equality
                            {
                                kvp.Value.GetComponent<assignNameTMP>().children.SetActive(false);
                                Debug.Log($"ABVA: Deactivating {kvp.Value.name}, coord: {kvp.Key} and narest is {nearestLatLong}");
                            }
                            else
                            {
                                Debug.Log($"ABVA: FalseDeactivating {kvp.Value.name}, coord: {kvp.Key} and nearest is {nearestLatLong}");
                            }
                        }
                    }
                    if (objectsGreaterThanMin.Contains(value))
                    {
                        // Only start coroutine if it's not already in progress for this object
                        if (!activationInProgress.Contains(value))
                        {
                            activationInProgress.Add(value);
                            StartCoroutine(ActivateNearestAfterDelay(value));
                        }
                    }
                    else
                    {
                        if (value.GetComponent<assignNameTMP>().activeDisplay == false)
                        {
                            value.GetComponent<assignNameTMP>().children.SetActive(true);
                            value.GetComponent<assignNameTMP>().activeDisplay = true;
                            Debug.Log($"POI : nearest is {value}");
                        }
                    }
                }
            }
        }
    }
    private IEnumerator ActivateNearestAfterDelay(GameObject value)
    {
        float delay = 2f; // 2-second delay
        yield return new WaitForSeconds(delay);

        // Check objectsGreaterThanMin after the delay
        if (objectsGreaterThanMin.Count <= 1)
        {
            if (value.GetComponent<assignNameTMP>().activeDisplay == false)
            {
                value.GetComponent<assignNameTMP>().children.SetActive(true);
                value.GetComponent<assignNameTMP>().activeDisplay = true;
                Debug.Log($"POI : nearest is {value}");
            }
        }
        else
        {
            Debug.LogWarning($"POI: Skipping activation for {value.name}, as objectsGreaterThanMin count is > 1");
        }

        // Remove the object from the "in-progress" set after activation is completed
        activationInProgress.Remove(value);
    }
    private void WhenInInfoPointHandler()
    {
        foreach (GameObject item in prefabs)
        {
            var obj = item.GetComponent<assignNameTMP>();

            if (obj.children.activeSelf)
            {
                obj.children.SetActive(false);
            }
            if(obj.hasBeenSeen)
            {
                obj.hasBeenSeen = false;
            }
            if(obj.activeDisplay)
            {
                obj.activeDisplay = false;   
            }
            if(activated == true)
            {
                if(obj.thisActivated)
                {
                    Deactivate(obj.gameObject); 
                }                       
            }
        }

        if(GroupIndicatorPrefab.activeSelf)
        {
            GroupIndicatorPrefab.SetActive(false); // Deactivate Group Bubble if it is enabled;
        }
        

        if(imageTracker != null && imageTracker.enabled)
        {
            if (imageTracker.notTracketYet == false)
            {
                globalMessageString = TextLibrary.infoPointBannerText.GetTranslatedText();
                UIExtentions.Display(aRElement.infoHolder, true);
                aRElement.imageTrack.SetActive(true);
            }
            else
            {
                aRElement.imageTrack.SetActive(false);
            }
        }
        else
        {
            Debug.Log("ImageTracker in NUll");
        }
    }
    

    public void DisableARVisibleObjects()
    {
        foreach (GameObject poi in prefabs)
        {
            var obj = poi.GetComponent<assignNameTMP>();

            if (obj.children.activeSelf)
            {
                obj.children.SetActive(false);
            }
            if(obj.hasBeenSeen)
            {
                obj.hasBeenSeen = false;
            }
            if(obj.activeDisplay)
            {
                obj.activeDisplay = false;   
            }
            if(activated == true)
            {
                if(obj.thisActivated)
                {
                    Deactivate(obj.gameObject); 
                }                       
            }
        }

        if(GroupIndicatorPrefab.activeSelf)
        {
            GroupIndicatorPrefab.SetActive(false); // Deactivate Group Bubble if it is enabled;
        }
      
        ClearAll();
        cameraHandler.arCamera.farClipPlane = 1f;
        cameraHandler.adjustmentStarted = false;
        cameraHandler.CameraInitialized = false;
        isInRadius = false;
    }
    void ClearAll()
    {
        groupCoordsList.Clear();
        NearlatlongListArrow.Clear();
        NearlatlongList.Clear();
        objectsLessThanMin.Clear();
        objectsGreaterThanMin.Clear();
        activationInProgress.Clear();
        nearestLatLong = new Vector2d(Mathd.Infinity, Mathd.Infinity);
    }
    
    #region ActivationHandler
    public void Deactivate(GameObject poi)
    {
        if (OnHotspotActivated != null)
        {
            FindObjectOfType<ObjectSelectionManager>().DeselectAllObjects();
            activated = false;
            OnHotspotActivated?.Invoke(false, poi);
        }
    }
    public void Activate(GameObject poi)
    {
        if (OnHotspotActivated != null)
        {
            MatchPOIwithARSigns(poi);
            activated = true;
            OnHotspotActivated?.Invoke(true, poi);
        }
    }  
    public void MatchPOIwithARSigns(GameObject poi)
    {
        var point = poi.GetComponent<assignNameTMP>();

        foreach (var item in points)
        {
            if (!item.isInMorePaths)
            {
                if (point.path == item.TranlatePathToInt() && point.ID == item.routeIndex)
                {
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(item);
                    item.HandlePOISelectionFromButtonOrTouch(item);
                }
            }
        }
    }
    #endregion ActivationHandler

    #region Calculations
    public Vector2d RetunNearestVector(Vector2d locInfoVector)
    {
        if (NearlatlongList.Count == 0)
        {
            Debug.LogWarning("The list is empty.");
            return Vector2d.zero; // Return a default value if the list is empty
        }

        // Initialize with the first element in the dictionary
        List<Vector2d> keys = new List<Vector2d>(NearlatlongList.Keys);
        nearestLatLong = keys[0];
        double shortestDistance = UIExtentions.DistanceBetweenLatLong(locInfoVector, nearestLatLong);

        // Start loop from the second element
        for (int i = 1; i < keys.Count; i++)
        {
            var currentLatLong = keys[i];
            double dist = UIExtentions.DistanceBetweenLatLong(locInfoVector, currentLatLong); // Calculate the distance for each point

            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestLatLong = currentLatLong; // Update the nearest point
            }
        }

        return nearestLatLong;
    }
    public Vector2d CalculateAverageLatLong()
    {
        Vector2d sum = Vector2d.zero;
        int count = groupCoordsList.Count;

        // Check if there are any coordinates in the list
        if (count == 0)
        {
            Debug.LogWarning("No coordinates in the list.");
            return Vector2d.zero;
        }

        // Iterate through the dictionary keys (Vector2d coordinates)
        foreach (KeyValuePair<Vector2d, GameObject> latlong in groupCoordsList)
        {
            sum += latlong.Key; // Sum the latitudes and longitudes
        }

        // Calculate the average
        Vector2d average = sum / count;

        Debug.Log($"The average Vector2d Coordinates Is {average}");
        return average;
        
    }
    public Vector2d CalculateNearestLatLong()
    {
        if (groupCoordsList == null || groupCoordsList.Count == 0)
        {
            Debug.LogWarning("No coordinates in the dictionary.");
            return Vector2d.zero;
        }

        Vector2d currentLocation = new Vector2d(Input.location.lastData.longitude, Input.location.lastData.longitude);
        
        // Assume the first point is the nearest initially
        var enumerator = groupCoordsList.GetEnumerator();
        enumerator.MoveNext(); // Move to the first element
        nearestLatLongEntry = enumerator.Current;

        double shortestDistance = UIExtentions.DistanceBetweenLatLong(currentLocation, nearestLatLongEntry.Key); // Calculate distance for the first point
        
        // Iterate through the dictionary
        foreach (var entry in groupCoordsList)
        {
            Vector2d currentLatLong = entry.Key;
            double distance = UIExtentions.DistanceBetweenLatLong(currentLocation, currentLatLong); // Calculate distance

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestLatLongEntry = entry; // Update nearest point
            }
        }
        return nearestLatLongEntry.Key; // Return the nearest point (latitude and longitude)
    }
 
    #endregion Calculations
}