using UnityEngine;

public class DeviceOrientationManager : MonoBehaviour
{

    private DeviceOrientation portrait;
    public bool isInLandcape = false;
    void Start()
    {
        portrait = DeviceOrientation.Portrait;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Update()
    {
        if(UtilsHomeBar.fullScreenCheck == true)
        {
            if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                UtilsHomeBar.EnableLandscape(false);
                isInLandcape = true;
            }
            else if(Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
                UtilsHomeBar.EnableLandscape(false);
                isInLandcape = true;
            }
            else 
            {
                Screen.orientation = ScreenOrientation.Portrait;
                UtilsHomeBar.EnableLandscape(enable: true);
                isInLandcape = false;
            }
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
            isInLandcape = false;
        }
    }
}
