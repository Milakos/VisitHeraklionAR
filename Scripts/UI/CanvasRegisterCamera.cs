
using ARLocation;
using UnityEngine;

public class CanvasRegisterCamera : MonoBehaviour
{
    public Canvas canvas;
    Camera arCamera;
    private void Start() 
    {
        arCamera = ARLocationManager.Instance.MainCamera;

        canvas.worldCamera = arCamera;
    }
}
