using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class MapBoxScript : MonoBehaviour
{
    VisualElement mapElementBackground;
    VisualElement root;
    public UIDocument uIDocument;

    

    public string accessToken;
    public float centerLatitude = -33.8873f;
    public float centerLongitude = 151.2189f;
    public float zoom = 12.0f;
    public int bearing = 0;
    public int pitch = 0;
    public enum style {Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets, NavigationDay, NavigationNight};
    public style mapStyle = style.Streets;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    private int mapWidth = 414;
    private int mapHeight = 736;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11",  "navigation-day-v1", "navigation-night-v1"};
    private string url = "";
    private bool mapIsLoading = false; 
    private bool updateMap = true;

    private string accessTokenLast;
    private float centerLatitudeLast = -33.8873f;
    private float centerLongitudeLast = 151.2189f;
    private float zoomLast = 12.0f;
    private int bearingLast = 0;
    private int pitchLast = 0;
    private style mapStyleLast = style.Streets;
    private resolution mapResolutionLast = resolution.low;

    //Start is called before the first frame update
    void Start()
    {
        root = uIDocument.rootVisualElement;
        mapElementBackground = root.Query<VisualElement>("BackGround");
        
        StartCoroutine(GetMapbox());
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast, centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom || bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
                    mapElementBackground = root.Query<VisualElement>("BackGround");
            StartCoroutine(GetMapbox());
            updateMap = false;
        }
    }


    IEnumerator GetMapbox()
    {
        url = "https://api.mapbox.com/styles/v1/mapbox/" + styleStr[(int)mapStyle] + "/static/" + centerLongitude + "," + centerLatitude + "," + zoom + "," + bearing + "," + pitch + "/" + mapWidth + "x" + mapHeight + "?" + "access_token=" + accessToken;
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            mapIsLoading = false;

            mapElementBackground.style.backgroundImage = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            centerLatitudeLast = centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
        }
    }
}




