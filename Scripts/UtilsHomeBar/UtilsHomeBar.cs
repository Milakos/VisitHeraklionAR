using System;
using System.Collections.Generic;
using PointsOfInterests;
using UnityEngine;
using UnityEngine.UIElements;

public class UtilsHomeBar : MonoBehaviour
{
    public Sprite espa;
    public Sprite espaG;
    public Sprite mayor;
    public Sprite mayorG;
    public Sprite Forth;
    public Sprite ForthG;
    public VisualTreeAsset visualTree;
    public static Sprite pointer;
    public Sprite pointerSprite;
    public static Sprite pointerBlue;
    public Sprite pointerSpriteBlue;
    public Sprite bluecard;
    public Sprite whiteCard;
    public static Sprite blueButton;
    public static Sprite greyButton;
    public Sprite kiosk;
    public Sprite gastronomy;
    public static Sprite Kiosk;
    public static Sprite Gastronomy;

    public Sprite pauseSprite;
    public Sprite resumeSprite;
    public static Sprite pauseSpriteStatic;
    public static Sprite resumeSpriteStatic;
    public static VisualTreeAsset visualTreeStatic;
    public Button homeBarAR;
    public Button homeBarMap;
    public Button homeBarRoads;
    public Button homeBarSettings;
    public Button homeBarHome;
    VisualElement root;
    private const long animationDelayButton = 100; // delay in ms

    // Color Presets ......................................................................
    private static Color pressedFilterButtonColor = new Color(255/255f, 195/255f, 83/255f);
    private static Color idleButtonColor = new Color(1,1,1);
    private static Color appBlueColor = new Color(17/255f, 57/255f, 132/255f);
    // .........................................................................
    
    // Events ..................................................................
    public static Action<PointOfInterest.FilterID, bool> FilterEvent;
    public static Action<ARButton.FilterID, bool> FilterEventAR;
    public static Action<GroupInfoPoint.FilterID, bool> FilterEventARGroup;
    public static Action CompassEvent;
    public static Action<float> SendDistance;
    public static Action SendPath;
    public static Action<int> SendPathIndex;
    public static Action RestorePath;
    public static Action<bool> setActiveToFalse;
    public static Action<int> POIselectedButton;
     public static Action<int> poiSelectStart;
    public static Action<bool> poiAdd;
    public static Action<bool> poiSub;
    public static Action deselectObject;
    public static Action playAudio;
    public static Action stopAudio;
    public static Action pauseAudio;
    public static Action resumeAudio;
    public static Action< int> swipe;
    public static Action<int > ScrollLeftRight;
    public  static Action< int> swipeAR;
    public static Action <bool > enableLandscapeAction;

    public static Action stateAction;
    public static Action<bool> toggleFiltersOpacity;

    public static Action <bool> toggleNumberSprite;
    // public static Action load360Material;
    public static bool popUpEnabled;
    [SerializeField] public static bool mapViewCheck;
    [SerializeField] public static bool isInMap;
    [SerializeField] public static bool isInRoutes;
    [SerializeField] public static bool fullScreenCheck = false;
    [SerializeField] public static bool infoCheck32;
    public static bool isIn360;
    public static bool ar;
    public static bool exitFromInfo = false;
    public static bool isInReccommendedRoutes;
    // .........................................................................
    public List<Vector2> StaticscrollPositions = new List<Vector2>();
    public static List<Vector2> scrollPositions = new List<Vector2>();
    public static Action resetColorObjects;

    public static Action MakeChevronsBlue;
    public static Sprite chevronblue;
    public static Sprite chevronyellow;
    [SerializeField] public Sprite blueChevron;
    [SerializeField] public Sprite YellowChevron;
    private void Start() 
    {
        chevronblue = blueChevron;
        chevronyellow = YellowChevron;
        visualTreeStatic = visualTree; 
        blueButton = bluecard;
        greyButton = whiteCard;
        pointer = pointerSprite;
        pointerBlue = pointerSpriteBlue;

        resumeSpriteStatic = resumeSprite;
        pauseSpriteStatic = pauseSprite;

        Kiosk = kiosk;
        Gastronomy = gastronomy;

        scrollPositions = StaticscrollPositions;
    }
    /// <summary>
    /// Initializes the home bar buttons by assigning them from the root visual element.
    /// </summary>
    /// <param name="rootElement">The root visual element.</param>
    public void InitializeHomeBarButtons(VisualElement rootElement)
    {
        root = rootElement;

        homeBarHome = root.Q<Button>("HomeButton");
        homeBarRoads = root.Q<Button>("MonumentButton");
        homeBarAR = root.Q<Button>("ARButton");
        homeBarMap = root.Q<Button>("MapButton");
        homeBarSettings = root.Q<Button>("SettingsButton");
        
    } 
    #region Static Events Region
    /// <summary>
    /// Triggers the filter event, indicating a change in the state of a filter.
    /// </summary>
    /// <param name="filterID">The ID of the filter being affected.</param>
    /// <param name="active">True if the filter is activated, false if deactivated.</param>
    public static void TriggerFilterEvent(PointOfInterest.FilterID filterID, bool active)
    {
        FilterEvent?.Invoke(filterID, active);
    }
    public static void TriggerFilterEventAR(ARButton.FilterID filterID, bool active)
    {
        FilterEventAR?.Invoke(filterID, active);
    }
    public static void TriggerFilterEventARGroups(GroupInfoPoint.FilterID filterID, bool active)
    {
        FilterEventARGroup?.Invoke(filterID, active);
    }
    public static void TriggerCompassEvent()
    {
        CompassEvent?.Invoke();
    }
    public static void TriggerSendDistance(float distance)
    {
        SendDistance?.Invoke(distance);
    }
    public static void TriggerNavigationRoute()
    {      
        SendPath?.Invoke();
    }
    public static void SendPathInt(int path)
    {
        SendPathIndex?.Invoke(path);
    }
    public static void RestorePathPOI()
    {
        RestorePath?.Invoke();
    }
    public static void SetActiveToFalse(bool check)
    {
        setActiveToFalse?.Invoke(check);
    }
    public static void SelectedPOIButton(int buttonIndex)
    {
        POIselectedButton?.Invoke(buttonIndex);
    }
    public static void SelectNewPOI(bool add)
    {
        poiAdd?.Invoke(add);       
    }
    internal static void SelectPOIFromStart(int v)
    {
        poiSelectStart?.Invoke(v);
    }
    public static void DeselectPOI()
    {
        deselectObject?.Invoke();
    }
    public static void PlayAudio()
    {
        playAudio?.Invoke();
    }
    public static void StopAudio()
    {
        stopAudio?.Invoke();
    }
    public static void PauseAudio()
    {
        pauseAudio?.Invoke();
    }
    public static void ResumeAudio()
    {
        resumeAudio?.Invoke();
    }
    public static void Swipe(int direction)
    {
        swipe?.Invoke(direction);
    }
    public static void SwipeRR(int direction)
    {
        ScrollLeftRight?.Invoke(direction);
    }
    public static void SwipeAR(int direction)
    {
        swipeAR?.Invoke(direction);
    }
    public static void ToggleNumberAndSpriteAction(bool toggle)
    {
        toggleNumberSprite?.Invoke(toggle);
    }
    public static void EnableLandscape(bool enable)
    {
        enableLandscapeAction?.Invoke(enable);
    }
    public static void ResetColorsofPOIS()
    {
        resetColorObjects?.Invoke();
    }
    public static void StateARAction()
    {
        stateAction?.Invoke();
    }
    public static void MakeChevronBlue()
    {
        MakeChevronsBlue?.Invoke();
    }
    #endregion Static Events Region

    #region ButtonGenericFunctionality
    /// <summary>
    /// Reduces the opacity of a button and schedules a delayed action to restore it.
    /// </summary>
    /// <param name="buttonName">The button to modify.</param>
    /// <param name="root">The root visual element containing the button.</param>
    /// <param name="pressed">True if the button is pressed, false otherwise.</param>
    public static void ReduceOpacity(Button buttonName, VisualElement root, bool pressed, float x, float y, float max) 
    {
        Button btn = root.Q<Button>(buttonName.name);

        ChangeButtonColor(btn, pressed);
           
        btn.style.width = x;
        btn.style.height = y + max;
        // Schedule a delayed action to restore opacity
        root.schedule.Execute(() => 
        {
            // btn.style.backgroundColor = Color.white;
            btn.style.width = x;
            btn.style.height = y;

        }).StartingIn(animationDelayButton);
        
    }
    /// <summary>
    /// Changes the color of a button based on its pressed state.
    /// </summary>
    /// <param name="btn">The button to modify.</param>
    /// <param name="buttonPressed">True if the button is pressed, false otherwise.</param>
    public static void ChangeButtonColor(Button btn, bool buttonPressed)
    {
        btn.style.backgroundColor = (buttonPressed ? pressedFilterButtonColor : idleButtonColor);
    }
    public static void ReduceFilterOpacity(Button buttonName,  bool pressed, float y, float max) 
    {
        ChangeButtonColor(buttonName, pressed);
           
        // buttonName.style.height = y + max;
        // // Schedule a delayed action to restore opacity
        // buttonName.schedule.Execute(() => 
        // {
        //     // btn.style.backgroundColor = Color.white;
        //     buttonName.style.height = y;

        // }).StartingIn(animationDelayButton);
        
    }
    #endregion ButtonGenericFunctionality
    // public static IEnumerator EnableWayPointPath()
	// {
    //     var wayPointPath = FindObjectOfType<WayPointPath>();
    //     if(wayPointPath.enabled == true)
    //         wayPointPath.enabled = false;
	// 	yield return new WaitForSeconds(1);		
	// 	// wayPointPath._waypoints.Add(_user.transform);
	// 	wayPointPath.enabled = true;
	// }


}
