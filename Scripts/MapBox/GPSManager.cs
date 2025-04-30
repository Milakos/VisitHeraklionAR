using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class GPSManager : MonoBehaviour
{
    LocationService service;
    private void Start() 
    {
        StartCoroutine(location());    
    }

    private IEnumerator location()
    {

#if UNITY_EDITOR
        yield return new WaitWhile(() => !UnityEditor.EditorApplication.isRemoteConnected);
        yield return new WaitForSecondsRealtime(5f);
#endif

        if (!Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) 
        {
            Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");
        

        // Start service before querying location
        Input.location.Start(500f, 500f);

        // Wait until service initializes
        int maxWait = 15;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

#if UNITY_EDITOR
        int editorMaxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Stopped && editorMaxWait > 0) {
            yield return new WaitForSecondsRealtime(1);
            editorMaxWait--;
        }
#endif

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            // print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude);
        
            //  Debug.LogFormat("Location service live. status {0}", UnityEngine.Input.location.status);
            // Access granted and location value could be retrieved
            Debug.LogFormat("Location: " 
                + UnityEngine.Input.location.lastData.latitude + " " 
                + UnityEngine.Input.location.lastData.longitude + " " 
                + UnityEngine.Input.location.lastData.altitude + " " 
                + UnityEngine.Input.location.lastData.horizontalAccuracy + " " 
                + UnityEngine.Input.location.lastData.timestamp);

                // float lat = Input.location.lastData.latitude;
                // float lon = Input.location.lastData.longitude;
        }

        // Stop service if there is no need to query location updates continuously
        // Input.location.Stop();
    }
}



