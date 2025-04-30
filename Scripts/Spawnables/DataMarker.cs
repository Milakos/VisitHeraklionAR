using Mapbox.Unity.Utilities;
using PointsOfInterests;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataMarker : MonoBehaviour
{
    [SerializeField] public string pathRef;
    	
	[Header("Location")] 
	[HideInInspector] [Geocode] public string locations;
	[HideInInspector] public GameObject markerObject;
	[HideInInspector] public float _spawnScale = 1.5f;
    public void Initialize()
    {		
        // Addressables.LoadAssetAsync<GameObject>(path).Completed += OnMarkerObjectLoaded;
    	Addressables.LoadAssetAsync<GameObject>(pathRef).Completed += OnMarkerObjectLoaded;		
    }
    private void OnMarkerObjectLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            markerObject = obj.Result;

    		// name = obj.Result.name;
    		locations = obj.Result.GetComponent<PointOfInterest>().POIlocation;
        }
        else
        {
            Debug.LogError($"Failed to load marker object at path: {pathRef}. Error: {obj.OperationException}");
        }
    }
}
