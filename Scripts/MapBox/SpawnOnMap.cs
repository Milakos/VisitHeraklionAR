using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;
using System.Collections;
using PointsOfInterests;

[Serializable] // User - Player location properties
public class User 
{
	[field: SerializeField] const string Name = "User Player";
	[SerializeField] public GameObject userObject;
	[HideInInspector] public GameObject Border;
	[HideInInspector] public GameObject Dot;
	[SerializeField] public float _spawnScale = 1f;
	[Range(0, 3000)] public float RadiusScale;
}
public class SpawnOnMap : MonoBehaviour
{
	// General Properties
	[SerializeField] AbstractMap _map;	
	[SerializeField] User user;
	public GameObject _user;
	public float offset = 10f;

	// List of Point of Interests
	[SerializeField] public DataMarker[] firtsRoutePOIs;
	[SerializeField] DataMarker[] secondRoutePOIs;
	[SerializeField] DataMarker[] thirdRoutePOIs;
	[SerializeField] DataMarker[] forthRoutePOIs;
	[SerializeField] DataMarker[] fifthRoutePOIs;

	public List<DataMarker[]> markersList = new List<DataMarker[]>();
	// Properties of SpawnedObjects
	public Vector2d[] _locations;
	public List<GameObject> objs = new List<GameObject>();
	// Vector2d[] _locationsSecond;
	[SerializeField] public List<GameObject> _spawnedObjects = new List<GameObject>();
	[HideInInspector] public List<GameObject> _spawnedObjectsSecond = new List<GameObject>();
	[HideInInspector] public List<GameObject> _spawnedObjectsThird = new List<GameObject>();
	[HideInInspector] public List<GameObject> _spawnedObjectsForth = new List<GameObject>();
	[HideInInspector] public List<GameObject> _spawnedObjectsFifth = new List<GameObject>();
	private const float delayTime = 1f;
	// Events
	public Action spriteEvent;

	public float clampMin;
	public float clampMax;

	int maxPoint = 49;

    public float ScaleFactor;

    private void Awake() 
	{	
		// These must be implemented Only in Awake function
		Addressables.InitializeAsync();
		InitializeMarkers(firtsRoutePOIs);
		InitializeMarkers(secondRoutePOIs);
		InitializeMarkers(thirdRoutePOIs);
		InitializeMarkers(forthRoutePOIs);
		InitializeMarkers(fifthRoutePOIs);		
		
	}
	private void OnEnable() 
	{
		UtilsHomeBar.SendDistance += HanldeRadiusBasedOnSlider;
	}
    public void HanldeRadiusBasedOnSlider(float ob)
    {
        user.RadiusScale = ob;
    }
    void Start()
	{
		Input.compass.enabled = true;
		
		_locations = new Vector2d[maxPoint];

		_user = Instantiate(user.userObject);
		UserSpawn(_user);
		user.Border = GameObject.Find("Border"); 
		user.Dot = GameObject.Find("DotUser"); 
		user.Border.transform.localScale = Vector3.one * _map.WorldRelativeScale * user.RadiusScale;
		user.Dot.transform.localScale = new Vector3(5f,5f,5f );

		StartCoroutine(DelayLoadingPois(delayTime, firtsRoutePOIs, _spawnedObjects,0));		
		StartCoroutine(DelayLoadingPois(delayTime, secondRoutePOIs, _spawnedObjectsSecond, firtsRoutePOIs.Length));
		StartCoroutine(DelayLoadingPois(delayTime, thirdRoutePOIs, _spawnedObjectsThird, firtsRoutePOIs.Length + secondRoutePOIs.Length));
		StartCoroutine(DelayLoadingPois(delayTime, forthRoutePOIs, _spawnedObjectsForth, firtsRoutePOIs.Length + secondRoutePOIs.Length + thirdRoutePOIs.Length));
		StartCoroutine(DelayLoadingPois(delayTime, fifthRoutePOIs, _spawnedObjectsFifth, firtsRoutePOIs.Length + secondRoutePOIs.Length + thirdRoutePOIs.Length + forthRoutePOIs.Length));		
	}


	/// <summary>
	/// This Functions is responsible for Instantiating the POis for the first time in the provided locations
	/// in a specific amount of time with the usage of a delay due to a execution order callback. Also stores the
	/// spawned data in the appropriate List of instnciated objects. Adjusting the scale the map Elevation
	/// </summary>
	/// <param name="delay"></param>
	/// <param name="dataMarkers"></param>
	/// <param name="obj"></param>
	/// <returns></returns>
	IEnumerator DelayLoadingPois(float delay, DataMarker[] dataMarkers, List<GameObject> obj, int index)
    {
        yield return new WaitForSeconds(delay);

        for (int j = 0; j < dataMarkers.Length; j++)
        {
			
            var instance = Instantiate(dataMarkers[j].markerObject, this.transform);
			
			// _map.VectorData.AddPointsOfInterestSubLayer(instance, index);
			// _map.VectorData.SpawnPrefabAtGeoLocation(instance, 
			// _locations[j], 
			// null,
			//  false,
			//    instance.GetComponent<PointOfInterest>().name);
			
            dataMarkers[j].locations = FindObjectOfType<PointOfInterest>().POIlocation;

			//Adjusted the Y coordinate of the POI sprites to be a bit above of the TileMap Sprites
            float elevation = _map.QueryElevationInUnityUnitsAt(_locations[j]);
            float adjustedElevationOffset = elevation + offset;

            var locationString = dataMarkers[j].locations;
            _locations[index + j] = Conversions.StringToLatLon(locationString);		
			

            Vector3 position = _map.GeoToWorldPosition(_locations[j], false);
			

            instance.transform.localPosition = position;
            position.y = adjustedElevationOffset;
            instance.transform.position = position;
            instance.transform.localScale = new Vector3(dataMarkers[j]._spawnScale, dataMarkers[j]._spawnScale, dataMarkers[j]._spawnScale);

            obj.Add(instance);
			objs.Add(instance);
        }
		spriteEvent?.Invoke();	
    }
	private void LateUpdate()
    {
		UserSpawn(_user);
        user.Border.transform.localScale = Vector3.one * _map.WorldRelativeScale * user.RadiusScale;
		user.Dot.transform.localScale = new Vector3(5f,5f,5f );
		LateUpdatePoisHandler();
    }
	/// <summary>
	/// This functions handles the location of the user based on his/hers GPS that momment and store - updates the
	/// map values based on users location. It is used in Late Update
	/// </summary>
	/// <param name="userInstance"></param>
    private void UserSpawn(GameObject userInstance)
	{
		float lon = Input.location.lastData.longitude;
		float lan = Input.location.lastData.latitude;
		var gps = new Vector2d(lon, lan);

		gps = Conversions.StringToLatLon(gps.ToString());

		userInstance.transform.position = _map.GeoToWorldPosition(gps, true);
		userInstance.transform.localScale = new Vector3(user._spawnScale, user._spawnScale, user._spawnScale);
		// Vector3 currentBorder = user.Border.transform.position;
		// currentBorder.z = 1f;
		// Vector3 currentDot = user.Dot.transform.position;
		// currentDot.z = 1f;
	}
	/// <summary>
	/// A Function that will be used foreach path differently. Updates the location of spawned Pois based on the 
	/// dataMarker that represents the Route, A List that these objects are Stored, and An integer that repesents the
	/// overral count of th Spawned objects List
	/// </summary>
	/// <param name="counter"></param>
	/// <param name="spawnedPOIs"></param>
	/// <param name="data"></param>
    private void LateUpdatePoisHandler()
    {
		float currentZoomLevel = _map.Zoom;

        for (int j = 0; j < objs.Count; j++)
        {
            float elevation = _map.QueryElevationInUnityUnitsAt(_locations[j]);
            float adjustedElevationOffset = elevation + offset;

            var spawnedObject = objs[j];
            var location = _locations[j];

			Vector3 position = _map.GeoToWorldPosition(location, false);
			spawnedObject.transform.localPosition = position;
			spawnedObject.transform.position = position;
			position.y = adjustedElevationOffset;
			// spawnedObject.transform.localScale = Vector3.one * _map.WorldRelativeScale * ScaleFactor;

			//new Vector3(ScaleFactor,ScaleFactor,ScaleFactor);
			// spawnedObject.transform.localScale = UpdateScale(_map.AbsoluteZoom);
			// spawnedObject.transform.localScale = new Vector3(1f,1f,1f );
			// spawnedObject.transform.GetChild(0).transform.localScale = new Vector3(1f,1f,1f );
			// spawnedObject.GetComponent<BoxCollider>().size = UpdateScale(_map.AbsoluteZoom);
			
			// float scaleFactor = CalculateScaleFactor(currentZoomLevel);
        	// spawnedObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			
			var scaleFactor = Mathf.Pow(2, (_map.InitialZoom - _map.AbsoluteZoom));
			spawnedObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
	Vector3 UpdateScale(float zoom)
	{
		Vector3 maxScale = new Vector3(clampMax,clampMax, clampMax);
		Vector3 minScale = new Vector3(clampMin, clampMin, clampMin);
		float t = Mathf.InverseLerp(0, 16.9f, zoom);
		Vector3 scale = Vector3.Lerp(maxScale, minScale, t);
		return scale;
	}

	private float CalculateScaleFactor(float zoomLevel)
	{
		// Adjust this function to determine how the scale changes with zoom level
		// You can use any logic that suits your needs based on the zoom level
		// This is just an example
		float scaleFactor = ScaleFactor  *  zoomLevel; // Adjust as needed
		return Mathf.Clamp(scaleFactor, clampMin, clampMax); // Clamp the scale factor to a reasonable range
	}
	/// <summary>
	/// A Method that initialize the Async addressable system. It is implemented for avoiding repeatence 
	/// and to have a more flexible implementation due to enabling callbacks in Awake Start and On Enable
	/// </summary>
	/// <param name="markers"></param>
	void InitializeMarkers(DataMarker[] markers)
	{
		for (int i = 0; i < markers.Length; i++)
		{
			markers[i].Initialize();
		}
	}

}
