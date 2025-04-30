using System.Globalization;
using ARLocation;
using Mapbox.Utils;
using TMPro;
using UnityEngine;

public class IndicatorTest : MonoBehaviour
{
    public Animator animator;
    readonly int SignClose = Animator.StringToHash("CloseGroupIndicator"); // String hash for Animator Clip
    public GameObject child;
    public float scale_factor;
    private float timer;
    private Vector3 temp_scale;
    public float heightOffset = 1.0f;
    Vector2d dist;
    private Vector2d currentDist = new Vector2d(0, 0); // Current distance to nearest location
    private Vector2d prevDist = new Vector2d(0, 0);
    public SpriteRenderer sprite;
    [SerializeField] TMP_Text Pointname;
    TextClassTranslate pointTextTranslate = new TextClassTranslate("Σημεία", "Points");
    [SerializeField] TMP_Text Minutes;
    TextClassTranslate minutesTextTranslate = new TextClassTranslate("λεπτά", "minutes");
    [SerializeField] TMP_Text Meters;
    TextClassTranslate metersTextTranslate = new TextClassTranslate("μ", "m");
    Camera arCamera;

    public bool isVisible { get; private set; }
    public bool GroupBubbleActive = false;

    ARElement aRElement;

    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable() 
    {
        aRElement.whenIn360 += WhenIndicatorIn360;
    }
    private void OnDisable() 
    {
        aRElement.whenIn360 -= WhenIndicatorIn360;
    }
    void Start()
    {
        arCamera = ARLocationManager.Instance.MainCamera;
        if(child.activeSelf)
            child.SetActive(false);
    }
    void Update()
    {  
        dist = ARBoundHandler.Instance.CalculateNearestLatLong();
        gameObject.transform.LookAt(arCamera.transform);
        
        if (aRElement.enabled && aRElement.initialized)
        {
            if(ARBoundHandler.Instance.cameraHandler.CameraInitialized)
            {
                if (ARBoundHandler.Instance.objectsGreaterThanMin.Count >= 2 
                && !ARBoundHandler.Instance.activated 
                && !ARBoundHandler.Instance.isInRadius 
                && ARBoundHandler.Instance.groupCoordsList.Count >= 2
                && ARBoundHandler.Instance.NearlatlongList.Count == 0)
                { 
                    CalculateNearestDistanceToMinutes();            
                    AdjustIndicatorScale();
                    
                    if(UIExtentions.Vector2dDistance(currentDist, dist) > 0.001)
                    {     
                        if(GroupBubbleActive == false)               
                            InvokeNewLocation();
                    }                   
                    if(UIExtentions.IsVisibleFromCamera(this.gameObject, ARBoundHandler.Instance.frustumPlanes))
                    {    
                        isVisible = true; 

                        Minutes.text = CalculateNearestDistanceToMinutes().ToString() + " " + minutesTextTranslate.GetTranslatedText();
                        Pointname.text =  ARBoundHandler.Instance.objectsGreaterThanMin.Count.ToString() + " " + pointTextTranslate.GetTranslatedText();
                        Meters.text = UserToPointDistanceCoordinates();
                        ARBoundHandler.Instance.groupCoordsList.TryGetValue(ARBoundHandler.Instance.CalculateNearestLatLong(), out GameObject value);  
                        sprite.sprite = value.GetComponent<assignNameTMP>().sprite.sprite;         
                    }
                    else
                    {
                        isVisible = false;             
                    }                     
                    
                }
                else
                {
                    if (child.activeSelf == true)
                    {
                        animator.Play(SignClose);   
                        isVisible = false;
                        currentDist = new Vector2d(0, 0);
                    }
                }                                             
            }
        }
    }
    private void InvokeNewLocation()
    {  
        GetComponent<PlaceAtLocation>().PlacementOptions.HideObjectUntilItIsPlaced = true;  
        var newLocation = new Location()
        {
            Latitude = dist.x,
            Longitude = dist.y,
            Altitude = 1,
            AltitudeMode = AltitudeMode.GroundRelative
        };
        GetComponent<PlaceAtLocation>().Location = newLocation;  
        currentDist = dist; 
    }
    public void AdjustIndicatorScale()
    {
        if (timer > 1)
        {
            timer = 0;
            
            temp_scale = new Vector3(UserToPointDistance() * scale_factor, UserToPointDistance() * scale_factor + heightOffset, 1);
            
            if (Vector3.Distance(gameObject.transform.localScale, temp_scale) < 0.01f) // Check if the scale has almost reached the target
            {       
                if(isVisible == true)
                {
                    if(child.activeSelf == false && GroupBubbleActive == false)
                    {
                        child.SetActive(true);
                    }              
                } 
                else
                {  
                    currentDist = new Vector2d(0, 0);
                    animator.Play(SignClose);  
                }          
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, temp_scale, Time.deltaTime * 80.0f);
    }
    private void WhenIndicatorIn360(bool obj)
    {
        child.SetActive(obj);
    }
    
    //........................PURE FUNCTIONS.........................\\
    private float CalculateNearestDistanceToMinutes()
    {
        float averageWalkSpeed = 1.4f;
        float distance = UserToPointDistance();
        return Mathf.Round((distance / averageWalkSpeed) / 60);
    }
    private float UserToPointDistance()
    {
        Vector3 userPosition = arCamera.transform.position;
        Vector3 pointPosition = gameObject.transform.position;
        float distance = Vector3.Distance(userPosition, pointPosition);
        return distance;
    }
    private string UserToPointDistanceCoordinates()
    {
        var dist = ARBoundHandler.Instance.CalculateNearestLatLong();
        var myPosition = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);

        return UIExtentions.DistanceBetweenLatLong(myPosition, dist).ToString("0", CultureInfo.InvariantCulture) + metersTextTranslate.GetTranslatedText();;
    }
}
