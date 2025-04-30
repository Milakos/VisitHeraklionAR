using UnityEngine;
using UnityEngine.UIElements;

public class SwipeDetection : MonoBehaviour
{
    VisualElement zoomElement;
    VisualElement zoomElement_;
    private Vector3 initialScale;
    private float initialDistance;
    public UIDocument uIDocument;
    //Swipe Variables
    private Vector2 startPosition;
    private Vector2 endPosition;

    private Vector2 startPositionRec;
    private Vector2 endPositionRec;
    public delegate void SwipeEvents(int index); 
    public SwipeEvents SwipeRightAction;
    public SwipeEvents SwipeLeftAction;
    public float newZoom;
    public static float newZoomStatic;
    public float swipeRegionBottomOffsetFraction = 0.64f;
    StateTracker state;
    bool zooming = false;
    bool rec = false; 
    DeviceOrientationManager deviceOrientationManager;

    private void Awake() 
    {      
        deviceOrientationManager = GetComponent<DeviceOrientationManager>();

        zoomElement =  uIDocument.rootVisualElement.Q<VisualElement>("CulturalRoads").Q<VisualElement>("FS_Image");
        zoomElement_ = uIDocument.rootVisualElement.Q<VisualElement>("MapPage").Q<VisualElement>("FS_Image");

        initialScale = zoomElement.transform.scale;

        state = FindObjectOfType<StateTracker>();      
    }
    private void Update() 
    {   
        //Debug.LogWarning(UtilsHomeBar.mapViewCheck + " " +UtilsHomeBar.popUpEnabled + " " + UtilsHomeBar.fullScreenCheck + " " + UtilsHomeBar.infoCheck32);
        if(state.gameState == StateTracker.GameState.Routes && UtilsHomeBar.isInRoutes && !UtilsHomeBar.ar == true)
        {
            ZoomBehaviour(zoomElement);
            SwipeBehaviour(UtilsHomeBar.mapViewCheck, UtilsHomeBar.fullScreenCheck, UtilsHomeBar.infoCheck32);
        }
        else if (state.gameState == StateTracker.GameState.Map && UtilsHomeBar.isInMap && !UtilsHomeBar.ar == true )
        {
            if(UtilsHomeBar.isInReccommendedRoutes == false)
            {
                ZoomBehaviour(zoomElement_);
                SwipeBehaviour(UtilsHomeBar.popUpEnabled && !UtilsHomeBar.infoCheck32, UtilsHomeBar.fullScreenCheck, UtilsHomeBar.infoCheck32); 
                startPositionRec = Vector2.zero;
                endPositionRec = Vector2.zero;
            }
            else
            {
                
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startPositionRec = Input.GetTouch(0).position;
                }
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    endPositionRec = Input.GetTouch(0).position;
                    
                    if (endPositionRec.x < startPositionRec.x)
                    {
                        UtilsHomeBar.SwipeRR(1);  
                    }
                    else if (endPositionRec.x > startPositionRec.x)
                    {
                        UtilsHomeBar.SwipeRR(-1);
                    }
                }
            }
                  
        } 
        else if(state.gameState == StateTracker.GameState.Map && UtilsHomeBar.ar == true
            || state.gameState == StateTracker.GameState.Routes && UtilsHomeBar.ar == true
            || state.gameState == StateTracker.GameState.Home && UtilsHomeBar.ar == true
            || state.gameState == StateTracker.GameState.Settings && UtilsHomeBar.ar == true)
        {
            ARBehaviour();
        }
        else
        {
            Debug.Log("No swipe");
        }
        newZoomStatic = newZoom;
    }
    private void SwipeBehaviour(bool map, bool fs, bool info)
    {
        if (map || fs || info )
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPosition = Input.GetTouch(0).position;
            }
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endPosition = Input.GetTouch(0).position;

                if (map && IsInSwipeRegionPopUp(endPosition))
                {
                    ProcessSwipe(map);
                }
                if (info && IsInSwipeRegionInfo(endPosition))
                {
                    ProcessSwipe(info);
                }
                if (fs && zooming == false && deviceOrientationManager.isInLandcape == false)
                {
                    ProcessSwipe(fs);          
                }
            }
        }
    }
    public void ARBehaviour()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startPosition = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPosition = Input.GetTouch(0).position;
            
            if(UtilsHomeBar.infoCheck32 == false && FindObjectOfType<ARElement>().firstInitialized == false)
            {
                ProcessSwipeAR();
            }
            else
            {
                if (UtilsHomeBar.infoCheck32 == true && IsInSwipeRegionInfo(endPosition))
                {
                    ProcessSwipeAR();
                }
                else if(zooming == false && deviceOrientationManager.isInLandcape == false && UtilsHomeBar.fullScreenCheck == true)
                {
                    ProcessSwipeAR();
                }
            }
            
        }
    }

    void ProcessSwipe(bool isPopUpEnabled)
    {
        if(zooming == false)
        {
            float swipeDistanceX = endPosition.x - startPosition.x;

            if (swipeDistanceX < 0)
            {
                UtilsHomeBar.Swipe(1); 
                Debug.Log("SWIPE R");
            }
            else if (swipeDistanceX > 0)
            {
                UtilsHomeBar.Swipe(-1); 
                Debug.Log("SWIPE L");
            }
        }
    }
    void ProcessSwipeAR()
    {
        if (endPosition.x < startPosition.x)
        {
            UtilsHomeBar.SwipeAR(1); 
            Debug.Log("SWIPE R");
        }
        else if (endPosition.x > startPosition.x)
        {
            UtilsHomeBar.SwipeAR(-1); 
            Debug.Log("SWIPE L");
        }
    }
    bool IsInSwipeRegionPopUp(Vector2 touchPosition)
    {
        float screenHeight = Screen.height;
        float swipeRegionMaxY = screenHeight / 2f; 
        float swipeRegionBottomOffset = swipeRegionMaxY / 2f;
        return touchPosition.y < swipeRegionMaxY && touchPosition.y > swipeRegionBottomOffset;
    }
    bool IsInSwipeRegionInfo(Vector2 touchPosition)
    {
        float screenHeight = Screen.height;
        float swipeRegionMaxY = screenHeight / 2f; 
        float swipeRegionBottomOffset = screenHeight * swipeRegionBottomOffsetFraction;
        return touchPosition.y > swipeRegionMaxY && touchPosition.y > swipeRegionBottomOffset;
    }
#region ZoomRegion
    public void ZoomBehaviour(VisualElement fsImage)
    {
        if(UtilsHomeBar.fullScreenCheck == true)
        {
            if (Input.touchCount == 2)
            {
                Vector3 vector = new Vector3(1,1,1);

                zooming = true;
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Calculate the distance between the two touches in the current frame
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);

                // Check if the initial distance has been set
                if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                {
                    initialDistance = currentDistance;
                    initialScale = fsImage.transform.scale;
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    // Calculate the scaling factor
                    float scaleFactor = currentDistance / initialDistance;

                    // Apply the scaling transformation to the zoomElement
                    fsImage.transform.scale = initialScale * scaleFactor;
                }
                else if(touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
                {
                    fsImage.transform.scale = vector;       
                }
            } 
            zooming = false;      
        }
       
    }

#endregion ZoomRegion

    // public async Task delayTouch()
    // {
    //     await Task.Delay(60);
    //     rec = true;
    // }
}
