using System;
using UnityEngine;

[Serializable]
public class ARCameraHandler
{
    [Header ("Camera")]
    public  Camera arCamera; // Camera of AR Session
    [SerializeField] private float targetFarClipPlane = 2000f; // Target value
    [SerializeField] private float targetFarClipPlane360 = 10000f; // Target value
    public float duration = 3f; // Time to reach the target
    public float startDelay = 4f; // Delay before starting
    public float elapsedTime = 0f; // Tracks total elapsed time
    public bool adjustmentStarted = false;
    [HideInInspector] public bool CameraInitialized = false;
    public void InitializeCameraClipPlanes()
    {
        // Wait for the delay before starting
        if (adjustmentStarted == false && elapsedTime >= startDelay)
        {
            adjustmentStarted = true;
            elapsedTime = 0f; // Reset elapsed time for the adjustment
        }

        // Perform the adjustment
        if (adjustmentStarted && arCamera.farClipPlane < targetFarClipPlane)
        {
            float t = elapsedTime / duration; // Calculate interpolation factor (0 to 1)
            arCamera.farClipPlane = Mathf.Lerp(arCamera.farClipPlane, targetFarClipPlane, t);

            Debug.LogWarning($"Camera farClipPlane: {arCamera.farClipPlane}");
        }

        // Ensure it stops exactly at 2000
        if (adjustmentStarted && arCamera.farClipPlane >= targetFarClipPlane)
        {
            if(!UtilsHomeBar.isIn360)
            {
                arCamera.farClipPlane = targetFarClipPlane;
                CameraInitialized = true;
            }
            else
            {
                arCamera.farClipPlane = targetFarClipPlane360;
            }
        }
    }
}
