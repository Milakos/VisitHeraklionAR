using System;
using ARLocation;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public static class UIExtentions
{
    //The Preset Color of Cyan Blue
    public static Color cyanBlue = new Color(133/255f, 204/255f, 211/255f, 1);
    public static Color white = new Color(255/255f, 255/255f, 255/255f, 1);
    public static Color whiteAlpha05 = new Color(255/255f, 255/255f, 255/255f, 1/3);
    public static Color whiteAlpha00 = new Color(255/255f, 255/255f, 255/255f, 0);
    public static Color whiteAlpha01 = new Color(255/255f, 255/255f, 255/255f, 1);
    public static Color BlueDark = new Color(17/255f, 57/255f, 132/255f, 1);
    public static Color BlueAlpha05 = new Color(17/255f, 57/255f, 132/255f, 1/2);
    public static Color grey = new Color(70/255f, 70/255f, 70/255f, 1);
    public static Color greyButtonFilter = new Color(70/255f, 70/255f, 70/255f, 0.3f);
    public static Color grey05 = new Color(190/255f, 190/255f, 190/255f, 80f);
    public static Color grey08 = new Color(210/255f, 210/255f, 210/255f, 60f);
    public static Color greyTutorial = new Color(210/255f, 210/255f, 210/255f, 237/255f);
    public static Color yellow = new Color(255/255f,195/255f, 83/255f, 1);
    public static LayerMask mask = LayerMask.GetMask("POI");
    
    /// <summary>
    /// Sets the display style of the VisualElement based on the provided boolean value.
    /// Sets to Flex if it is true and to none if it is false
    /// </summary>
    /// <param name="element">The VisualElement to modify.</param>
    /// <param name="enabled">The boolean value determining the display style (Flex or None).</param>
    public static void Display(this VisualElement element, bool enabled)
    {
        if(element == null) return;
        element.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
    }
    /// <summary>
    ///  Sets the picking mode of the VisualElement based on the provided boolean value.
    ///  Sets the Picking mode to Ignore if it is true and to Position if it is false
    /// </summary>
    /// <param name="element"></param>
    /// <param name="ignored"></param>
    public static void Ignore(this VisualElement element, bool ignored)
    {
        if(element == null)return;

        element.pickingMode = (ignored ? PickingMode.Ignore : PickingMode.Position);
    }
    /// <summary>
    /// Sets the usage hints of the VisualElement based on the provided boolean value.
    /// Sets to MaskContainer if it is true and to None if it is false
    /// </summary>
    /// <param name="element"></param>
    /// <param name="ignoredMask"></param>
    public static void MaskIgnore(this VisualElement element, bool ignoredMask)
    {
        if(element == null) return;
        element.usageHints = (ignoredMask ? UsageHints.MaskContainer : UsageHints.None);
    }
    /// <summary>
    ///  Checks if the selected locale in Unity's Localization system is set to English.
    /// </summary>
    /// <returns> True if the selected locale is English; otherwise, false </returns>
    public static bool IsEnglish() => LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.GetLocale("en");
    
    
    static double  tolerance = 0.00001; // Adjust the tolerance based on your precision needs
    public static bool IsApproximatelyEqual(Vector2d a, Vector2d b)
    {
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance;
    }
    public static Vector2d CalculateCoordinates(GameObject poi)
    {
        var x = poi.GetComponent<PlaceAtLocation>().LocationOptions.LocationInput.Location.Latitude;
        var y = poi.GetComponent<PlaceAtLocation>().LocationOptions.LocationInput.Location.Longitude;
        Vector2d latlong = new Vector2d(x, y);
        return latlong;
    }
    public static bool IsVisibleFromCamera(GameObject obj, Plane[] frustumPlanes)
    {
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.min; // Bottom-left near
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z); // Bottom-left far
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z); // Top-left near
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z); // Top-left far
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z); // Bottom-right near
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z); // Bottom-right far
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z); // Top-right near
        corners[7] = bounds.max; // Top-right far

        // Check if each corner of the bounds is inside the frustum
        foreach (var corner in corners)
        {
            // bool isInside = true;

            foreach (var plane in frustumPlanes)
            {
                // If the corner is behind any plane, it is outside
                if (plane.GetDistanceToPoint(corner) < 0)
                {
                    return false;
                }
            }

        }
        return true;
    }
    public static bool IsHalfVisibleFromCamera(GameObject obj, Plane[] frustumPlanes)
    {
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        
        Vector3[] corners = new Vector3[8]
        {
            bounds.min, // Bottom-left near
            new Vector3(bounds.min.x, bounds.min.y, bounds.max.z), // Bottom-left far
            new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), // Top-left near
            new Vector3(bounds.min.x, bounds.max.y, bounds.max.z), // Top-left far
            new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), // Bottom-right near
            new Vector3(bounds.max.x, bounds.min.y, bounds.max.z), // Bottom-right far
            new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), // Top-right near
            bounds.max  // Top-right far
        };

        // Check if all corners are outside the frustum
        foreach (var corner in corners)
        {
            bool isInside = true;

            // Check each plane to see if the corner is outside
            foreach (var plane in frustumPlanes)
            {
                if (plane.GetDistanceToPoint(corner) < 0)
                {
                    isInside = false; // Corner is behind this plane
                    break;
                }
            }

            if (isInside)
            {
                // At least one corner is inside, so the object is NOT completely outside
                return false;
            }
        }

        // If all corners are outside, return true
        return true;
    }
    //Compare Vectors2d
    public static double Vector2dDistance(Vector2d a, Vector2d b)
    {
        return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
    }
    // Calculate Distance with FLOAT
    public static float CalculateDistance(float lat_1, float lat_2, float long_1, float long_2)
    {
        int R = 6371;
        var lat_rad_1 = Mathf.Deg2Rad * lat_1;
        var lat_rad_2 = Mathf.Deg2Rad * lat_2;
        var d_lat_rad = Mathf.Deg2Rad * (lat_2 - lat_1);
        var d_long_rad = Mathf.Deg2Rad * (long_2 - long_1);
        var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var total_dist = R * c * 1000; // convert to meters
        return (int)total_dist;
    }
    // Calculate Distance with Vector2d
    public static double DistanceBetweenLatLong(Vector2d point1, Vector2d point2)
    {
        double R = 6371000; // Radius of the Earth in meters
        double lat1Rad = DegreesToRadians(point1.x);
        double lat2Rad = DegreesToRadians(point2.x);
        double deltaLat = DegreesToRadians(point2.x - point1.x);
        double deltaLon = DegreesToRadians(point2.y - point1.y);

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = R * c; // Distance in meters
        return distance;
    }
    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}

