using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[DefaultExecutionOrder(ARUpdateOrder.k_TrackedImageManager)]
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracker : MonoBehaviour
{
    public ARSession arSession;
    public ARTrackedImageManager imageManager;
    [SerializeField] GameObject prefabs;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, ARAnchor> imageAnchors = new Dictionary<string, ARAnchor>();
    private Dictionary<string, bool> imageTracked = new Dictionary<string, bool>();
    public Action<bool> EnableFilters;
    public GameObject go;
    public bool notTracketYet;
    GameObject holder;

    private void Start() 
    {
        notTracketYet = false;
    }
    private void OnEnable()
    {
        StartCoroutine(RestartARSession());
        imageManager.trackedImagesChanged += ImageChange;
        InitializeTrackedImages();
        notTracketYet = false; 
    }
    private IEnumerator RestartARSession()
    {
        arSession.Reset();
        yield return null;
    }
    private void OnDisable() 
    {
        imageManager.trackedImagesChanged -= ImageChange;
    }
    private void InitializeTrackedImages()
    {
        foreach (ARTrackedImage trackedImage in imageManager.trackables)
        {
            if (!spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                go = Instantiate(prefabs, trackedImage.transform);

                spawnedPrefabs.Add(trackedImage.referenceImage.name, go);
                notTracketYet = true;
                EnableFilters?.Invoke(true);
                Debug.Log("AR: Added In Start And Initialized"); 
                imageTracked[trackedImage.referenceImage.name] = true;
            }
        }
    }
    private void ImageChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (!spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                go = Instantiate(prefabs, trackedImage.transform);
                go.name = trackedImage.referenceImage.name;               
                spawnedPrefabs.Add(trackedImage.referenceImage.name, go);      
                notTracketYet = true;
                EnableFilters?.Invoke(true);
                Debug.Log("AR: Added"); 
            }          
        }
        //Handle updated images
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {              
                go.SetActive(true); 
                go.transform.position = trackedImage.transform.position;
                go.transform.rotation = trackedImage.transform.rotation;

                imageTracked[trackedImage.referenceImage.name] = false;
                notTracketYet = true;
                EnableFilters?.Invoke(true);
                Debug.Log("AR: Updated in Tracking"); 
            }
            // else if (trackedImage.trackingState == TrackingState.Limited)
            // {
            //     go.SetActive(false);
            //     Debug.Log("Not tracked");
            // }           
  
        }
        //Handle removed images
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedPrefabs[trackedImage.referenceImage.name]);
                spawnedPrefabs.Remove(trackedImage.referenceImage.name);
                ARAnchor anchor;
                
                if (imageAnchors.TryGetValue(trackedImage.referenceImage.name, out anchor))
                {
                    Destroy(anchor);
                    GameObject go = spawnedPrefabs[trackedImage.referenceImage.name];
                    go.SetActive(false);
                    imageAnchors.Remove(trackedImage.referenceImage.name);
                    // imageTracked[trackedImage.referenceImage.name] = true;
                }
                                // Remove from imageTracked dictionary to allow re-tracking in the future
                if (imageTracked.ContainsKey(trackedImage.referenceImage.name))
                {
                    imageTracked.Remove(trackedImage.referenceImage.name);
                }
            }
        }
    
    }
    public void DestoryImageObject()
    {     
        EnableFilters?.Invoke(false);
        notTracketYet = false;   
        go.SetActive(false);          
        DestroyImageObject();
    }
    public void DestroyImageObject()
    {
        foreach (var prefab in spawnedPrefabs.Values)
        {
            Destroy(prefab);
        }

        spawnedPrefabs.Clear();
    }

}

