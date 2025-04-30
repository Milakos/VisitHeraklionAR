using System.Collections;
using System.Collections.Generic;
using ARLocation;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

// namespace ARLocation.MapboxRoutes
// {
    public class SignArrow : MonoBehaviour

    {
        public enum TargetVisibilityState
        {
            None,
            Visible,
            OffUp,
            OffDown,
            OffLeft,
            OffRight
        }

        public enum ArrowDir
        {
            Left,
            Right
        }

        ARElement aRElement;
        public Sprite ArrowSprite;
        public ArrowDir NeutralArrowDirection;
        public float Margin = 20;
        public float MarginY = 700;
        private RectTransform indicator;
        private Canvas canvas;
        private Camera cam;
        private Transform camTransform;
        private Renderer targetRenderer;
        private TargetVisibilityState targetVisibility;
        private bool initialized;

        public TargetVisibilityState TargetVisibility => targetVisibility;

        public GameObject target;
        public float smoothSpeed;


        [Header ("Arrow")]
        // Dictionary<Vector2d, GameObject> NearlatlongListArrow = new Dictionary<Vector2d, GameObject>();
        [HideInInspector] public Vector2d nearestLatLongArrow;   
        private readonly float arrowDelay = 2.5f;
        private float lastArrowExecutionTime = 0f;  // To track the last time the coroutine was started
        [SerializeField] private float arrowExecutionIntervalTimeStamp = 3.0f;
        private bool arrowEnabled = false;
        public GameObject GroupIndicatorPrefab;
        LocationInfo locInfo;
        private Vector2d locInfoVector;

    // public LayerMask mask;
    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();
    }
    private void Start()
    {

        if (!initialized)
        {
            var canvasGo = new GameObject("[OnScreenTargetIndicatorCanvas]");
            int LayerIgnoreRaycast = LayerMask.NameToLayer("OnlyAR");
            aRElement.go = canvasGo;
            
            canvasGo.layer = LayerIgnoreRaycast;
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            

            var indicatorGo = new GameObject("[OnScreenTargetIndicatorImage]");
            indicatorGo.gameObject.layer = LayerIgnoreRaycast;
            indicatorGo.transform.parent = canvasGo.transform;

            var indicatorImage = indicatorGo.AddComponent<Image>();
            indicatorImage.sprite = ArrowSprite;
            indicator = indicatorGo.GetComponent<RectTransform>();

            cam = ARLocationManager.Instance.Camera;
            camTransform = cam.transform;

            initialized = true;
        }
    }

    bool isLeftOfCamera(Vector3 targetPos)
    {
        var camForward = camTransform.forward;
        var camPos = camTransform.position;
        return Vector3.Dot(Vector3.Cross(camForward, targetPos - camPos), (new Vector3(0, 1, 0))) < 0;
    }

    bool isBehindCamera(Vector3 targetPos)
    {
        var camForward = camTransform.forward;
        var relative = targetPos - camTransform.position;

        return Vector3.Dot(camForward, relative) < 0;
    }

    bool isVisible(Vector3 targetPos)
    {
        if (isBehindCamera(targetPos))
        {
            return false;
        }

        if (targetRenderer != null)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), targetRenderer.bounds);
        }
        else
        {
            var p = cam.WorldToScreenPoint(targetPos);
            return (p.x >= 0 && p.x <= Screen.width) && (p.y >= 0 && p.y <= Screen.height);
        }
    }
    private void Update() 
    {
        locInfo = Input.location.lastData;
        var locInfoLat = locInfo.latitude;
        var locInfoLon = locInfo.longitude;
        locInfoVector = new Vector2d(locInfoLat, locInfoLon);
        
        if (aRElement.enabled && aRElement.initialized)
        {
            if(ARBoundHandler.Instance.cameraHandler.CameraInitialized)
            {
                RetunNearestVectorArrow(locInfoVector);
                Arrow();
            }
        }

        if(target != null)
        {
            Debug.Log(target + " " + target.transform.position + " " + target.activeSelf);

            bool isLeft = isLeftOfCamera(target.transform.position);
            bool isBehind = isBehindCamera(target.transform.position);
            bool isFront = !isBehind;
            bool isRight = !isLeft;

            if (isVisible(target.transform.position))
            {
                this.indicator.gameObject.SetActive(false);
                targetVisibility = TargetVisibilityState.Visible;
                return;
            }
            else
            {
                this.indicator.gameObject.SetActive(true);
            }

            var p = cam.WorldToScreenPoint(target.transform.position);

            if (p.x < 0)
            {
                targetVisibility = TargetVisibilityState.OffLeft;
            }
            else if (p.x >= Screen.width)
            {
                targetVisibility = TargetVisibilityState.OffRight;
            }
            else if (p.y < 0)
            {
                targetVisibility = isBehind ? TargetVisibilityState.OffUp : TargetVisibilityState.OffDown;
            }
            else
            {
                targetVisibility = isBehind ? TargetVisibilityState.OffDown : TargetVisibilityState.OffUp;
            }
            // Vector3 targetPosition = p;
            
            p.x = Mathf.Clamp(p.x, Margin, Screen.width - Margin);
            p.y = Mathf.Clamp(p.y, MarginY, Screen.height - MarginY);

            if (isBehind)
            {
                p.y = Screen.height - p.y;

                if (isLeft)
                {
                    p.x = Margin;
                    targetVisibility = TargetVisibilityState.OffLeft;
                }
                else
                {
                    p.x = Screen.width - Margin;
                    targetVisibility = TargetVisibilityState.OffRight;
                }
            }
            
            // indicator.position = p;
            Quaternion targetRotation = Quaternion.identity;

            switch (targetVisibility)
            {
                case TargetVisibilityState.OffLeft:
                    if (NeutralArrowDirection == ArrowDir.Right)
                    {
                        indicator.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                        indicator.position = p;
                    }
                    else
                    {
                        indicator.rotation = Quaternion.identity;
                        indicator.position = p;
                    }
                    break;

                case TargetVisibilityState.OffRight:
                    if (NeutralArrowDirection == ArrowDir.Right)
                    {
                        indicator.rotation = Quaternion.identity;
                        indicator.position = p;
                    }
                    else
                    {
                        indicator.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                        indicator.position = p;
                    }
                    break;

                case TargetVisibilityState.OffUp:
                    if (NeutralArrowDirection == ArrowDir.Right)
                    {
                        indicator.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    }
                    else
                    {
                        indicator.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                    }
                    break;

                case TargetVisibilityState.OffDown:
                    if (NeutralArrowDirection == ArrowDir.Right)
                    {
                        indicator.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                    }
                    else
                    {
                        indicator.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    }
                    break;

                default:
                    indicator.rotation = Quaternion.identity;
                    break;
            }
            indicator.rotation = Quaternion.Lerp(indicator.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
            
    }
    private void Arrow()
    {
        if (Time.time >= lastArrowExecutionTime + arrowExecutionIntervalTimeStamp)  // Check the time interval
        {
            if (!ARBoundHandler.Instance.isInRadius & !ARBoundHandler.Instance.activated)
            {
                if (ARBoundHandler.Instance.NearlatlongList.Count == 0 
                && GroupIndicatorPrefab.activeSelf == false) 
                {
                    if(!arrowEnabled)
                    {
                        StartCoroutine(ActivateArrows(arrowDelay));                                        
                    }
                }
                else
                {
                    if(!ARBoundHandler.Instance.activated)
                    {
                        UIExtentions.Display(aRElement.infoHolder, false);  
                    }
                    
                    target = null;
                    aRElement.go.SetActive(false);   
                    arrowEnabled = false;
                    Debug.Log($"Arrow Disabled");       
                }               
            }
            else
            {
                target = null;
                aRElement.go.SetActive(false);  
                arrowEnabled = false;
            }
            lastArrowExecutionTime = Time.time;
        }   
    }
    public IEnumerator ActivateArrows(float delayTime)
    {     
        yield return new WaitForSeconds(delayTime);  
        
        if (ARBoundHandler.Instance.NearlatlongList.Count == 0 
        && GroupIndicatorPrefab.activeSelf == false)
        {         
            if (ARBoundHandler.Instance.NearlatlongListArrow.ContainsKey(nearestLatLongArrow))
            {
                ARBoundHandler.Instance.NearlatlongListArrow.TryGetValue(nearestLatLongArrow, out GameObject value);
                aRElement.go.SetActive(true); 
                ARBoundHandler.Instance.globalMessageString = TextLibrary.arrowText.GetTranslatedText();
                UIExtentions.Display(aRElement.infoHolder, true); 
                target = value;
                arrowEnabled = true; 
                Debug.Log($"Arrow Enabled");
            }                 
        }  
        else
        {
            if(!ARBoundHandler.Instance.activated)
            {
                UIExtentions.Display(aRElement.infoHolder, false);  
            }
            
            target = null;
            aRElement.go.SetActive(false);   
            arrowEnabled = false;
            Debug.Log($"Arrow Disabled");
        }                 
    }
    private Vector2d RetunNearestVectorArrow(Vector2d locInfoVector)
    {
        if (ARBoundHandler.Instance.NearlatlongListArrow.Count == 0)
        {
            Debug.LogWarning("The list is empty.");
            return Vector2d.zero; // Return a default value if the list is empty
        }

        // Initialize with the first element in the dictionary
        List<Vector2d> keys = new List<Vector2d>(ARBoundHandler.Instance.NearlatlongListArrow.Keys);
        nearestLatLongArrow = keys[0];
        double shortestDistance = UIExtentions.DistanceBetweenLatLong(locInfoVector, nearestLatLongArrow);

        // Start loop from the second element
        for (int i = 1; i < keys.Count; i++)
        {
            var currentLatLong = keys[i];
            double dist = UIExtentions.DistanceBetweenLatLong(locInfoVector, currentLatLong); // Calculate the distance for each point

            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestLatLongArrow = currentLatLong; // Update the nearest point
            }
        }

        return nearestLatLongArrow;
    }


  

    
    
    }
// }

