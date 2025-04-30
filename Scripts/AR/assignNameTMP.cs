using ARLocation;
using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class assignNameTMP : MonoBehaviour
{
    public GameObject children;
    ARElement aRElement;
    private Camera arCamera;
    
    // Animation \\
    public Animator animator;
    readonly int SignClose = Animator.StringToHash("CloseBubble"); // String hash for Animator Clip

    public enum Model
    {
        Ariadne = 0, Dominic = 1
    }
    [Tooltip ("Which Model Should be projected")]
    public Model model;
    [Tooltip ("The ID of the poi or the number of the row that has in the json")]
    public int ID; 
    [Tooltip ("The number of the path that has in the json. Only from 0-4")]
    public int path;
    
    [Tooltip ("Property that inidicates the distance that user reached the destination")]
    public float personalRadius = 5f;
    [HideInInspector] public bool hasBeenSeen = false;
    [HideInInspector] public bool activeDisplay = false;
    [HideInInspector] public bool thisActivated = false;
    
    [Header ("Timestamp Properties")]
    [Tooltip ("ATTENTION : Only change this on Prefab not in variants")]
    [SerializeField] private float poiExecutionIntervalTimestamp = 3f;
    private float lastCheckTime = 0f;
    private float scale_factor = 0.08f; // Scale factor that scales accordingly to have the same size based on user location   
    private float timer;
    private Vector3 temp_scale;
    private float averageWalkSpeed = 1.4f;
    public const int RawZero = 0;

    [HideInInspector] public string globalName;
    string nameOfPOIGreek, nameOfPOIEng;  
  
    [Header ("Objects Refrences")]
    public TMP_Text distanceText;
    public TMP_Text Name;
    public TMP_Text minutes;     
    List<TMP_Text> texts = new List<TMP_Text>();
    [SerializeField] AssetReference mainSpritePath;
    public SpriteRenderer sprite;
 
    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();     
        animator = GetComponentInChildren<Animator>(); 
    }
    private void OnEnable()
    {
        timer = 2;
        arCamera = ARLocationManager.Instance.MainCamera;   
        nameOfPOIGreek = JSONTest.Instance.globalPath[path].greekTextsRoute[ID].Titles[RawZero].ToString();
        nameOfPOIEng = JSONTest.Instance.globalPath[path].englishTextsRoute[ID].Titles[RawZero].ToString();
        
        LoadSprite();     
        children.SetActive(false);
        aRElement.whenIn360 += WhenSignIsIn360;
    }
    private void OnDisable() 
    {
        aRElement.whenIn360 -= WhenSignIsIn360;
    }

    private void WhenSignIsIn360(bool obj)
    {
        children.SetActive(obj);
    }

    private void Start() 
    {
        texts.Add(Name);
        texts.Add(minutes);
        texts.Add(distanceText);
        
    }
    void Update()
    {     
        if (aRElement.enabled && aRElement.initialized)
        {
            if(ARBoundHandler.Instance.cameraHandler.CameraInitialized)
            {
                gameObject.transform.LookAt(arCamera.transform);
                Vector3 userPosition = arCamera.transform.position;
                Vector3 pointPosition = gameObject.transform.position;
                
                float distance = Vector3.Distance(userPosition, pointPosition);
                var distanceCoord = UIExtentions.CalculateDistance(Input.location.lastData.latitude, (float)this.GetComponent<PlaceAtLocation>().Location.Latitude,
                Input.location.lastData.longitude, (float)this.GetComponent<PlaceAtLocation>().Location.Longitude);
                var poiVector = UIExtentions.CalculateCoordinates(this.gameObject);
                
                if (Time.time >= lastCheckTime + poiExecutionIntervalTimestamp)
                {    
                    // Less to 300 meters object behviour        
                    if(distanceCoord <= ARBoundHandler.Instance.minimumDistanceRender)
                    {
                        // Check if the object is visible from camera
                        if(UIExtentions.IsVisibleFromCamera(this.gameObject, ARBoundHandler.Instance.frustumPlanes))
                        {
                            // Add the object to a list if it is visible and less than 300 meters
                            if(!ARBoundHandler.Instance.objectsLessThanMin.Contains(this.gameObject))
                            {
                                ARBoundHandler.Instance.objectsLessThanMin.Add(this.gameObject);

                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 0");
                            }
                            // Add the object to the display list
                            if(!ARBoundHandler.Instance.NearlatlongList.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.NearlatlongList.Add(poiVector, this.gameObject);
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 1");
                            }
                            
                            if(ARBoundHandler.Instance.NearlatlongListArrow.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.NearlatlongListArrow.Remove(poiVector);
                            }
                        }
                        // Check if the object is not visible from camera and is less than 300 meters
                        else if(UIExtentions.IsHalfVisibleFromCamera(this.gameObject, ARBoundHandler.Instance.frustumPlanes))
                        {
                            // Remove the object if it less than 300 meters and not visible from camera
                            if(ARBoundHandler.Instance.NearlatlongList.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.NearlatlongList.Remove(poiVector);
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 2");  
                            }
                            if(!ARBoundHandler.Instance.NearlatlongListArrow.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.NearlatlongListArrow.Add(poiVector, this.gameObject);
                            }
                            //Deactivates Object
                            if(children.activeSelf)
                            {
                                // children.SetActive(false);  
                                animator.Play(SignClose);
                                activeDisplay = false;
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 3");
                            }
                        }
                        //Remove the object from the list if previously was more than 300 meters and now is less
                        if(ARBoundHandler.Instance.objectsGreaterThanMin.Contains(this.gameObject))
                        {
                            ARBoundHandler.Instance.objectsGreaterThanMin.Remove(this.gameObject);  
                            Debug.LogWarning($"POI : {this.gameObject.name} is at state 4");
                        }
                        //Remove the object from group list if it was existed inside
                        if(ARBoundHandler.Instance.groupCoordsList.ContainsValue(this.gameObject))
                        {
                            ARBoundHandler.Instance.groupCoordsList.Remove(poiVector);
                            Debug.LogWarning($"POI : {this.gameObject.name} 4.5"); 
                        }
                    }
                    // Up to 300 meters object behviour
                    else
                    {
                        // Remove the Object if previously object was under 300 meters and user got more far up to 300 meters
                        if(ARBoundHandler.Instance.objectsLessThanMin.Contains(this.gameObject))
                        {
                            ARBoundHandler.Instance.objectsLessThanMin.Remove(this.gameObject);  
                            Debug.LogWarning($"POI : {this.gameObject.name} is at state 5");  
                        }
                        if(ARBoundHandler.Instance.NearlatlongListArrow.ContainsValue(this.gameObject))
                        {
                            ARBoundHandler.Instance.NearlatlongListArrow.Remove(poiVector);
                        }
                        // Check if the Object is Visible from Camera
                        if(UIExtentions.IsVisibleFromCamera(this.gameObject, ARBoundHandler.Instance.frustumPlanes))
                        {
                            // Add object to List if its position is more than 300 meters
                            if(!ARBoundHandler.Instance.objectsGreaterThanMin.Contains(this.gameObject))
                            {
                                ARBoundHandler.Instance.objectsGreaterThanMin.Add(this.gameObject);    
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 6");
                            }
                            // Add the object to display list if it is the only object more than 300 meters
                            if(!ARBoundHandler.Instance.NearlatlongList.ContainsValue(this.gameObject) && ARBoundHandler.Instance.objectsGreaterThanMin.Count <= 1)
                            {
                                ARBoundHandler.Instance.NearlatlongList.Add(poiVector, this.gameObject);
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 7");  
                            }
                            //Remove the Object from display list if there are more objects up to 300 meters
                            else if(ARBoundHandler.Instance.NearlatlongList.ContainsValue(this.gameObject) &&  ARBoundHandler.Instance.objectsGreaterThanMin.Count > 1)
                            {
                                ARBoundHandler.Instance.NearlatlongList.Remove(poiVector);
                                
                                if(children.activeSelf)
                                {
                                    // children.SetActive(false); 
                                    animator.Play(SignClose); 
                                    activeDisplay = false;
                                    Debug.LogWarning($"POI : {this.gameObject.name} is at state 10");
                                }
                            }
                           // Add the object to Group calculation List
                            else
                            {
                                if(!ARBoundHandler.Instance.groupCoordsList.ContainsValue(this.gameObject))
                                {
                                    ARBoundHandler.Instance.groupCoordsList.Add(poiVector, this.gameObject);
                                    Debug.LogWarning($"POI : {this.gameObject.name} Unknown Condition"); 
                                }
                            }
                        }
                        // Remove the object from all lists
                        else
                        {
                            
                            if(ARBoundHandler.Instance.objectsGreaterThanMin.Contains(this.gameObject))
                            {
                                ARBoundHandler.Instance.objectsGreaterThanMin.Remove(this.gameObject);      
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 8");
                            }
                            if(ARBoundHandler.Instance.NearlatlongList.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.NearlatlongList.Remove(poiVector);
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 9");                              
                            }
                            if(ARBoundHandler.Instance.groupCoordsList.ContainsValue(this.gameObject))
                            {
                                ARBoundHandler.Instance.groupCoordsList.Remove(poiVector);
                                Debug.LogWarning($"POI : {this.gameObject.name} 9.5"); 
                            }
                            if(children.activeSelf)
                            {
                                children.SetActive(false); 
                                // animator.Play(SignClose); 
                                activeDisplay = false;
                                Debug.LogWarning($"POI : {this.gameObject.name} is at state 10");
                            }
                        }
                    }
                    lastCheckTime = Time.time;
                }
                    
                if (timer > 1)
                {
                    timer = 0;
                
                    temp_scale = new Vector3(distance * scale_factor, distance * scale_factor, 1);
                    
                    distanceText.text = distanceCoord.ToString("0", CultureInfo.InvariantCulture) + TextLibrary.meterTextTranslate.GetTranslatedText();      
                }
                else
                {
                    timer += Time.deltaTime;
                }
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, temp_scale, Time.deltaTime * 1.2f); 
                
                float averageTime = Mathf.Round((float)distanceCoord / averageWalkSpeed / 60f);
                
                if(averageTime >= 1)
                {
                    minutes.text = averageTime + " " + TextLibrary.minutesTextTranslate.GetTranslatedText();
                }
                else
                {
                    minutes.text =  TextLibrary.minuteTextTranslate.GetTranslatedText();
                } 
                CheckStringTextsLocalization();
                Name.text = globalName;
            }
            else
            {
                children.SetActive(false);
                Debug.Log($"POI : notInitialized");
            }
            CalculateDistanceCameraToPoint();
        }
    }
    public void CalculateDistanceCameraToPoint()
    {     

        var distance = UIExtentions.CalculateDistance(Input.location.lastData.latitude, (float)this.gameObject.GetComponent<PlaceAtLocation>().Location.Latitude,
        Input.location.lastData.longitude, (float)this.gameObject.GetComponent<PlaceAtLocation>().Location.Longitude);

        if (hasBeenSeen == false)
        {
            if(this.gameObject.activeSelf == true)
            {
                if (distance < ARBoundHandler.Instance.Radius)
                {
                    ARBoundHandler.Instance.isInRadius = true;
                    
                    if(children.activeSelf == false)
                    {
                        children.SetActive(true);
                    }

                    if (distance < personalRadius)
                    {
                        if (!ARBoundHandler.Instance.activated)
                        {
                            ARBoundHandler.Instance.Activate(this.gameObject);
                            thisActivated = true;
                            hasBeenSeen = true;
                        }
                    }    
                    else
                    {
                        if(!ARBoundHandler.Instance.activated)
                        {
                            var name = globalName;
                            // globalMessageString = inRadiusText.GetTranslatedText(name);  
                            ARBoundHandler.Instance.globalMessageString = TextLibrary.InRadiusText.GetTranslatedText(name);
                            UIExtentions.Display(aRElement.infoHolder, true);  
                        }
                    }                              
                }
                else
                {
                    ARBoundHandler.Instance.isInRadius = false;
                }
            }
        }
        else
        {
            if (distance >= personalRadius + 2f)
            {
                if (distance > ARBoundHandler.Instance.Radius)
                {
                    hasBeenSeen = false;                                                           
                    ARBoundHandler.Instance.isInRadius = false; 
                }
                else
                {
                    if (ARBoundHandler.Instance.activated)
                    {
                        ARBoundHandler.Instance.Deactivate(this.gameObject);
                        thisActivated = false;
                    }
                }
            }
        }
    }
    private void CheckStringTextsLocalization()
    {
        globalName = UIExtentions.IsEnglish() ? nameOfPOIEng : nameOfPOIGreek;
    }
    private void LoadSprite()
    {
        Addressables.LoadAssetAsync<Sprite>(mainSpritePath).Completed += OnMainSpriteLoaded;        
    }
    private void OnMainSpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            sprite.sprite = obj.Result;
        }
        else
        {
            Debug.LogWarning($"Failed to load main sprite at path: {mainSpritePath}. Error: {obj.OperationException}");
        }
        Addressables.Release(obj); 
    }  
}
