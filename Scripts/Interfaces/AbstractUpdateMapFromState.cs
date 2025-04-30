using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public static class AbstractUpdateMapFromState
{
    public static Vector2d vec = new Vector2d(35.33897, 25.134);

    public static Vector2d vec0 = new Vector2d(35.3450355529785, 25.1310939788818);
    public static Vector2d vec1 = new Vector2d(35.3397331237793, 25.1312465667725);
    public static Vector2d vec2 = new Vector2d(35.341609954834, 25.1318778991699);
    public static Vector2d vec3 = new Vector2d(35.3408164978027, 25.1331253051758);
    public static Vector2d vec4 = new Vector2d(35.3416061401367, 25.1323413848877);
    public static List<Vector2d> averageCoordinate = new List<Vector2d>();

    public static void UpdateMapFromState()
    {
        var map = GameObject.Find("MapManager").GetComponent<AbstractMap>();
        
        if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("Unable to determine device location");
            map.UpdateMap(vec, 15);
        }
        else if (Input.location.status == LocationServiceStatus.Running)
        {
            float lat = Input.location.lastData.latitude;
            float lon = Input.location.lastData.longitude;
            Vector2d vec1 = new Vector2d(lat, lon);
            map.UpdateMap(vec1, 15);
        }
    }
    public static void UpdateMapToSelectedRoute(int index)
    {
        AddTheCoordinates();
        var map = GameObject.Find("MapManager").GetComponent<AbstractMap>();
        map.UpdateMap(averageCoordinate[index], 14);
    }
    public static void AddTheCoordinates()
    {
        averageCoordinate.Add(vec0);
        averageCoordinate.Add(vec1);
        averageCoordinate.Add(vec2);
        averageCoordinate.Add(vec3);
        averageCoordinate.Add(vec4);
    }
    public static void ResetMap()
    {
        var map = GameObject.Find("MapManager").GetComponent<AbstractMap>();
        float lat = 35.338f;
        float lon = 25.134f;
        Vector2d vec2 = new Vector2d(lat, lon);
        map.UpdateMap(vec2, 15);
    }
}
