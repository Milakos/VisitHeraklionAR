using UnityEngine;

public class GyroscopeTest : MonoBehaviour
{
    public bool gyroEnabled;
    private Gyroscope gyro;

    public GameObject cameraContainer;
    Quaternion rot;
    // Quaternion initialRotation;
    public  Quaternion accumulatedRotation = Quaternion.identity;
    void OnEnable()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);
        gyroEnabled = EnableGyro();
    }
    private void OnDisable() 
    {
        gyroEnabled = false; 
        cameraContainer.transform.DetachChildren();
        // transform.SetParent(this.transform);   
        Destroy(cameraContainer);
    }
    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0,0,1,0);
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.pressed == true)
        {
            gyroEnabled = false;
            accumulatedRotation = transform.localRotation;
            
            RotateCameraWithTouch();
        }
        else
        {
            gyroEnabled = true;
            // accumulatedRotation = transform.localRotation;
            if(gyroEnabled)
            {
                transform.localRotation = accumulatedRotation;
                transform.localRotation = gyro.attitude * rot;
                // accumulatedRotation = gyro.attitude * rot;
            }
        }
    }

    private void RotateCameraWithTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                        Vector2 touchDeltaPosition = touch.deltaPosition;
                        float rotationSpeed = 0.05f;

                        // Rotate the camera based on touch movement
                        transform.Rotate(Vector3.up, -touchDeltaPosition.x * rotationSpeed, Space.World);
                        transform.Rotate(Vector3.right, touchDeltaPosition.y * rotationSpeed, Space.World);

                                    break;
                    case TouchPhase.Ended:
                        
                    break;
                }
        }
    }
}
