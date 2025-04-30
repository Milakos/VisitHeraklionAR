using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointsOfInterests;
using UnityEngine;
using UnityEngine.UIElements;
public class MapState : PlayerBaseState
{
    public MapState(PlayerStateMachine stateMachine) : base(stateMachine){}
    
    // Home bar Buttons
    Button home;
    Button map;
    Button settings;
    Button augmentedReality;
    Button routes;

    // Filter Buttons
    ScrollView FilterScrollView;
    Button squareButton;
    Button museumButton;
    Button monumentButton;
    Button templeButton;
    Button playGroundButton;
    Button recreationGroundButton;
    Button shoppingStreetButton;
    Button culturalFacilityButton;
    Button gpsButton;
    Button routeButton;
    Button retargetButton;
    Button mapInfoButton;

    //Distance Page
    VisualElement distancePage;
    Button distanceAcceptButton;
    Button distanceExitButton;
    Slider distanceSlider;
    Label distanceLabel;
    float currentValue;
    float previousValue;
    bool distancePagePressed = false;

    //Recommended Routes
    VisualElement RecommendedRoutesPage;
    ScrollView ScrollViewRB;
    List<VisualElement> RBList = new List<VisualElement>();
    List<Button> reccomendedScrollViewButtons = new List<Button>();
    Button recommendedAcceptButton;
    List<Button> exitbuttonrecommended = new List<Button>();
    List<Button> recommendedRoutesButtons = new List<Button>();

    //POP UP
    VisualElement PopUp;
    Translate popUpUnseenTransform = new Translate(0, 423, 0);
    Translate popUpVisibleTransfrom = new Translate(0, 24, 0);
    Button exitPopUp;
    Button googleButton;
    ScrollView scrollView;

    public Label popUpTitle;

    VisualElement scrollFilterView;
    VisualElement scrollContainer;
    Label popUpFilterID;
    VisualElement filterIDIconGrey;
    VisualElement PointerContainer;

    // Tutorial
    VisualElement TutorialHolder;
    Color tintBackgroundFadeOut = new Color(0,0,0,0);
    Color tintBackgroundFadeIn = new Color(255,255,255,255);

    // Root
    VisualElement root;
    VisualElement mapPage;
    SO_PageData mapPageData;
    private Dictionary<Button, PointOfInterest.FilterID> buttonFilterMap = new Dictionary<Button, PointOfInterest.FilterID>();
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacksFilter = new Dictionary<Button, EventCallback<ClickEvent>>();
    List<Button> elements = new List<Button>();
    VisualElement mapViewPage;
   

    // ---------------------------AUDIO STATE ---------------------------
    public enum AudioState
    {
        Playing,
        Paused
    }
    // Maintain a variable to track the current state
    private AudioState currentAudioState;
    // ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::                       

    //////INFO PROPERTIES\\\\\\\
    VisualElement infoRoutesMap;
    VisualElement background;
    Button back;
    Button goTo360;
    ScrollView scrollViewMap;
    VisualElement PointerTab;
    List<Label> titleList = new List<Label>();
    Button previous;
    Button next;
    VisualElement filterIconGrey;
    Label routeNameText;
    Label routeIndex;
    Label routeCounter;
    Label filtertext;
    Label address;
    Label tel ;
    Label email;
    Label ticket;
    Label descPoint;
    Button audio;
    Button audioPause;
    Button GooglePin;
    Button infoButton;

    Button web;
    Button telephone;
    Button googlemap;

    VisualElement websiteButton;
    VisualElement Earth;
    VisualElement Telephone;
    VisualElement Ticket;
    //////INFO PRoperties|||||||||

    // FULLSCREEN
    List<Sprite> fullScreenSprites = new List<Sprite>();
    Button fullScreenButton;
    Button fullScreenButtonReset;
    VisualElement fullScreen;
    Button fsNext;
    Button fsBack;
    VisualElement fsImage;
    Label fsPoiIndex;
    Label fsRouteCounter;
    VisualElement fsResetRegion;
    VisualElement fsNPRegion;
    VisualElement View360Page;
    Button back360;
    Label Title360;
    //FULLSCREEN
    
    LayerMask mask ;
    Camera camera = Camera.main;
    GameObject mapManager;
    SelectPath selectPath = new SelectPath();
    StartState startState = new StartState();
    public static StartState staticStartState;

    // StartRoadTrip
    VisualElement buttonSpawnPoint;
    Button _3backButton;
    VisualElement startRoadTripPage;    
    Label pointText; 
    Label pathText;
    Label routeText;
    Button listView;
    Button mapView;
    Button start;
    Button end;
    VisualElement startRoadTripElement;
    ScrollView infoAndButtonArea;
    VisualElement backGround;
    VisualElement Fader;

    VisualElement PointerCounterHolderLabel;
    Label pointC;
    Label pointS;

    // Modal
    VisualElement modal;
    Button yes;
    Button no;
    Button exitModal;
    bool googlemapenable = false;
    Label modalTitle;  
    Label modaldescription;
    TextClassTranslate modalTitleText = new TextClassTranslate("Έχετε ήδη ξεκινήσει μια διαδρομή", "You have already began a tour!");
    TextClassTranslate modalDescriptionText = new TextClassTranslate("Είστε σίγουροι ότι θέλετε να αποχωρήσετε από την ξενάγηση;", "Are you sure of leaving the tour");

    TextClassTranslate modalTitleGoogleText = new TextClassTranslate("Έχετε ήδη ξεκινήσει μια διαδρομή", "You have already began a tour!");
    TextClassTranslate modalDescriptionGoogleText = new TextClassTranslate("Πρόκειται να αποχωρήσετε από την εφαρμογή. Είστε σίγουροι ότι θέλετε να αποχωρήσετε;", "You are about to leave this app. Are you sure of leaving the app?");

    
    // Properties int and float
    private const float _width = 65;
    private const float _height = 50f;
    private const float maximizer = 2.5f;
    private const int Row = 0;
    private int currentImageIndex = 0;
    private int totalImagesCounter = 0;
    public int indexbtn { get; private set; }
    int pathIndex;
    int indexOfSumPOIS;
    bool homecheck;
    bool infoCHeck;
    
    bool settingscheck;
    bool arCheck;
    bool[] mainButtonCheck = new bool[5];
    bool cameFromMapView = false;

    bool previousCheck;
    bool nextCheck; 

    bool hmbtn = false;
    bool infobtn = false;
    bool arbtn= false;
    bool setbtn= false;

    //Events
    public static Action tutorialMap;
    public static Action load360Material;   
    public static Action unload360Material;
    public static Action resetButtonColor;
    public static Action exit;

    int curreRRIndex = 0;
    int newIndex = 0;
    bool isInRec = false;

    private List<Button> seenButtons = new List<Button>();
    private List<Button> unseenButtons = new List<Button>();
    private int currentButtonIndex = -1;

    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();
    
    public override void Enter()
    {
        mask = LayerMask.GetMask("POI");
        var slider = GameObject.Find("UIDocument").GetComponent<CustomSlider>();
        var mapTutorial = GameObject.Find("UIDocument").GetComponent<MapTutorial>();
        GameObject.Find("GameManager").gameObject.GetComponent<StateTracker>().gameState = StateTracker.GameState.Map;
        mapManager = GameObject.Find("MapManager");
        slider.enabled = false;    
        Input.compass.enabled = true;
        selectPath.SelectUnselect(SelectPath.Select.Unselected);

        if(mapTutorial.initialized == false)
        {
            UtilsHomeBar.mapViewCheck = false;
            mapTutorial.enabled = true;
            
        }
        else
        {
            UtilsHomeBar.mapViewCheck = true;
        }

        mapPageData = stateMachine.mapPageData;
        root = stateMachine.root;
        mapPage = root.Q("MapPage");
        UIExtentions.Display(mapPage, true);
        
        background = mapPage.Q<VisualElement>("BackGround");
        mapViewPage = mapPage.Q<VisualElement>("MapView");
        UIExtentions.Display(mapViewPage, true);
        startRoadTripPage = mapPage.Q<VisualElement>("StartRoadTripPage");
        UIExtentions.Display(startRoadTripPage, false);

        home = mapPage.Q<Button>("HomeButton");
        map = mapPage.Q<Button>("MapButton");
        settings = mapPage.Q<Button>("SettingsButton");
        augmentedReality = mapPage.Q<Button>("ARButton");
        routes = mapPage.Q<Button>("MonumentButton");

        monumentButton = mapPage.Q<Button>("MonumentBtn");
        museumButton = mapPage.Q<Button>("MuseumBtn");
        squareButton = mapPage.Q<Button>("SquareBtn");
        templeButton = mapPage.Q<Button>("TempleBtn");
        playGroundButton = mapPage.Q<Button>("Playground");
        recreationGroundButton = mapPage.Q<Button>("RecreationGround");
        shoppingStreetButton = mapPage.Q<Button>("ShoppingStreet");
        culturalFacilityButton = mapPage.Q<Button>("CulturalFacility");
        FilterScrollView = mapPage.Q<ScrollView>("FilterScrollView");
        
        gpsButton = mapPage.Q<Button>("GPSIcon");
        routeButton = mapPage.Q<Button>("RouteIcon");
        retargetButton = mapPage.Q<Button>("RetargetButton");
        mapInfoButton = mapPage.Q<Button>("MapInfoButton");

        distancePage = mapPage.Q<VisualElement>("DistancePage");
        UIExtentions.Display(distancePage, false);
        distanceAcceptButton = mapPage.Q<Button>("DistanceAcceptButton");
        distanceExitButton = mapPage.Q<Button>("DistanceExitButton");
        distanceSlider = mapPage.Q<Slider>("DistanceSlider");
        distanceLabel = mapPage.Q<Label>("Kilometers");

        RecommendedRoutesPage = mapPage.Q<VisualElement>("RecommendedRoutes");
        ScrollViewRB = RecommendedRoutesPage.Q<ScrollView>("ScrollViewRB");
        RBList = RecommendedRoutesPage.Query<VisualElement>(className: ".reccomScroll").ToList();
        reccomendedScrollViewButtons = RecommendedRoutesPage.Query<Button>(className: "acceptbuttonrecommended").ToList();
        UIExtentions.Display(RecommendedRoutesPage, false);
        recommendedAcceptButton = mapPage.Q<Button>("RecommendedChooseButton");
        recommendedRoutesButtons = mapPage.Query<Button>(className: "selectbuttonrecommended").ToList();
        exitbuttonrecommended = mapPage.Query<Button>(className: "exitbuttonRecommended").ToList();

        // PopUp Initialize .......................................................
        PopUp = mapPage.Q<VisualElement>("PopUp");
        UIExtentions.Display(PopUp, false);
        exitPopUp = mapPage.Q<Button>("ExitPopUp");
        googleButton = mapPage.Q<Button>("GoogleButton");
        scrollView = mapPage.Q<ScrollView>("POIPhotoHolder");
        popUpTitle = mapPage.Q<Label>("PopUpTitle");
        popUpFilterID = mapPage.Q<Label>("PopUpFilterID");
        filterIDIconGrey = mapPage.Q<VisualElement>("FilterIDIconGrey");
        infoButton = mapPage.Q<Button>("PopUpButton");

        PointerContainer = mapPage.Q<VisualElement>("MapPointHolder");
        // .............................................................................
        // StartRoadTRip
        buttonSpawnPoint = mapPage.Q<VisualElement>("ButtonSpawner");
         _3backButton = mapPage.Q<Button>("3BackButton");

        PointerCounterHolderLabel = startRoadTripPage.Q<VisualElement>("PointerCounterHolderLabel");
        UIExtentions.Display(PointerCounterHolderLabel, false);
        pointC = PointerCounterHolderLabel.Q<Label>("PointTextC");
        pointS = PointerCounterHolderLabel.Q<Label>("PointTextS");

        /////// INFO \\\\\\\\\\\\\\\\\\\\\\\
        // Info Routes ................................................
        infoRoutesMap = mapPage.Q<VisualElement>("InfoInMap");

        UIExtentions.Display(infoRoutesMap, false);
        Fader = mapPage.Q<VisualElement>("Fader");
        UIExtentions.Display(Fader, false);
        infoAndButtonArea = startRoadTripPage.Q<ScrollView>("InfoAndButtonArea");
        buttonSpawnPoint = startRoadTripPage.Q<VisualElement>("ButtonSpawner");
        routeText = startRoadTripPage.Q<Label>("TitleText");
        pathText = startRoadTripPage.Q<Label>("IntroText");
        pointText = startRoadTripPage.Q<Label>("pointIndex");
        listView = startRoadTripPage.Q<Button>(name: "3_InfoIcon");
        UtilsHomeBar.ChangeButtonColor(listView, false);
        mapView = startRoadTripPage.Q<Button>("3_MapIcon");
        UtilsHomeBar.ChangeButtonColor(mapView, true);
        start = startRoadTripPage.Q<Button>("3_Start");
        UIExtentions.Display(start, true);
        end = startRoadTripPage.Q<Button>("3_End");
        UIExtentions.Display(end, false);
        scrollViewMap = infoRoutesMap.Q<ScrollView>("BlankSpace");
        PointerTab = infoRoutesMap.Q<VisualElement>("PointerTab");
        titleList = infoRoutesMap.Query<Label>(className: "32Routes").ToList(); //////////
        filterIconGrey = infoRoutesMap.Q<VisualElement>("FilterIconGrey");
        filtertext = infoRoutesMap.Q<Label>("FilterText");
        routeNameText = infoRoutesMap.Q<Label>("RouteNameText");
        routeIndex = infoRoutesMap.Q<Label>("IndexPOI");
        routeCounter = infoRoutesMap.Q<Label>("CounterPOI");

        back = infoRoutesMap.Q<Button>("Back");
        goTo360 = infoRoutesMap.Q<Button>("GoTo360");
        goTo360.style.unityBackgroundImageTintColor = UIExtentions.grey;
        previous = infoRoutesMap.Q<Button>("PreviousButton");
        previous.style.color = UIExtentions.grey;
        next = infoRoutesMap.Q<Button>("NextButton");
        next.style.color = UIExtentions.BlueDark;
        address = infoRoutesMap.Q<Label>("Address");
        tel = infoRoutesMap.Q<Label>("Tel");
        email = infoRoutesMap.Q<Label>("mail");
        ticket = infoRoutesMap.Q<Label>("ticket");
        descPoint = infoRoutesMap.Q<Label>("descPoint");

        websiteButton = infoRoutesMap.Q<VisualElement>("MapRoute");
        UIExtentions.Display(websiteButton, true);

        googlemap = websiteButton.Q<Button>("_32MapButton");
        
        Telephone = infoRoutesMap.Q<VisualElement>("Telephone");
        UIExtentions.Display(Telephone, false);
        
        telephone = Telephone.Q<Button>("_32TelButton");
        
        Earth = infoRoutesMap.Q<VisualElement>("Earth");
        UIExtentions.Display(Earth, false);
        
        web = Earth.Q<Button>("_32EarthButton");
        
        Ticket = infoRoutesMap.Q<VisualElement>("Ticket");
        UIExtentions.Display(Ticket, false);
        
        GooglePin = infoRoutesMap.Q<Button>("32GoogleButton");
        audio = infoRoutesMap.Q<Button>("Audio");
        audioPause = infoRoutesMap.Q<Button>("Pause");

        //// MODAL
                modal = root.Q<VisualElement>("Modal");
        UIExtentions.Display(modal, false);
        yes = modal.Q<Button>("YesButton");
        no = modal.Q<Button>("NoButton");
        exitModal = modal.Q<Button>("ExitModal");
        modalTitle = modal.Q<Label>("ModalTitle");
        modaldescription = modal.Q<Label>("ModalDesc");


        ////////INFO \\\\\\\\\\\\\\\\\\\\\\\\
        
        // FullScreen
        fullScreen = mapPage.Q<VisualElement>("PageFullScreen");
        UIExtentions.Display(fullScreen, false);
        fullScreenButton = infoRoutesMap.Q<Button>("FullScreen");
        fullScreenButtonReset = fullScreen.Q<Button>("FullScreenReset");
        fsBack = fullScreen.Q<Button>("FSback");
        fsNext = fullScreen.Q<Button>("FSnext");
        fsImage = fullScreen.Q<VisualElement>("FS_Image");
        // fsImage.style.unityBackgroundScaleMode = ScaleMode.Sca
        fsPoiIndex= fullScreen.Q<Label>("FSpoiIndex");
        fsRouteCounter= fullScreen.Q<Label>("FSrouteCounter");
        fsResetRegion = fullScreen.Q<VisualElement>("FS_ButtonHolder");
        fsNPRegion = fullScreen.Q<VisualElement>("FS_Minimazie");
        UtilsHomeBar.enableLandscapeAction += HandleLandscape;

        // 360 ...........................................................
        View360Page = mapPage.Q<VisualElement>("360ViewCore");
        UIExtentions.Display(View360Page, false);
        back360 = View360Page.Q<Button>("360BackButton");
        Title360 = View360Page.Q<Label>("360TitleText");

        scrollFilterView = mapPage.Query<VisualElement>("FilterScrollView");
        scrollContainer = scrollFilterView.Q<VisualElement>("unity-content-container");
        UIExtentions.Ignore(scrollContainer, true);
        TutorialHolder = mapPage.Q<VisualElement>("TutorialInfoHolder");

        TutorialHolder.style.unityBackgroundImageTintColor = tintBackgroundFadeOut;

        UtilsHomeBar.TriggerCompassEvent();
        ObjectSelectionManager.AudioFinished += TogglePauseUnPauseAudioIconHandler;
      
        PointOfInterest.PopUpEvent += PopUpDisplay;
        
        PointOfInterest.PopUpImages += (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            Handle32RoutesImages(numberOfSprites, sprites);
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
        
        UtilsHomeBar.swipe += (int swipeDirection) => 
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };

        UtilsHomeBar.ScrollLeftRight += ScrollRouteRightLeft;
        
        ButtonElement.buttonnotifier += ButtonToInfoBehaviour;
        ButtonElement.buttonPressedIndex += ButtonPressedFrom32Routes; 

        UtilsHomeBar.RestorePathPOI();
        UtilsHomeBar.SetActiveToFalse(false);

        UtilsHomeBar.popUpEnabled = false;
        UtilsHomeBar.isInMap = true;
        UtilsHomeBar.ar = false;
        cameFromMapView = true;
                
        UtilsCommander.UndoCommand(); 
        UtilsCommander.Clear();      
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        UtilsCommander.UndoPointer32Command();
        UtilsCommander.ClearPointer32();
        AbstractPointerBehaviour.ResetCurrentIndex();
        
        OnStartInitialize();
        startState.StartEndHandler(StartState.state.None);
        AbstractUpdateMapFromState.ResetMap();
        Debug.Log("Enter MapPage");                               
    }

    private void ScrollRouteRightLeft(int index)
    {
        curreRRIndex = Mathf.Clamp(curreRRIndex, 0, 4);
        newIndex = Mathf.Clamp(curreRRIndex + index, 0, 4);
        
        ScrollViewRB.schedule.Execute( () =>
        { 
            ScrollViewRB.ScrollTo(ScrollViewRB[newIndex]);
        }).StartingIn(50);
        
        curreRRIndex = newIndex; 
    }

    private void OnStartInitialize()
    {
        HomeBarButtons();
        StartEndWalkHandler(); 
        FullScreenButtons();
        Handle360();
        HandlePopUP();
        ListAndMapViewButtonHandler(); 
        InfoButtons32();
                    
        MapButtonInit();
        
    }
    private void UnregisterButtons()
    {
        home.UnregisterCallback<ClickEvent>(OnHomeButtonClicked);
        routes.UnregisterCallback<ClickEvent>(OnRoutesClicked);
        map.UnregisterCallback<ClickEvent>(OnMapClicked);
        settings.UnregisterCallback<ClickEvent>(OnSettingsClicked);
        augmentedReality.UnregisterCallback<ClickEvent>(OnAugmentedRealityClicked);
        start.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
        end.UnregisterCallback<ClickEvent>(OnEndButtonClicked);
        fsNext.UnregisterCallback<ClickEvent>(FullScreenButtonNextClicked);
        fsBack.UnregisterCallback<ClickEvent>(FullScreenButtonBackClicked);
        fullScreenButton.UnregisterCallback<ClickEvent>(FullScreenButtonClicked);
        goTo360.UnregisterCallback<ClickEvent>(OnGoTo360Clicked);
        back360.UnregisterCallback<ClickEvent>(OnBackFrom360Clicked);
        exitPopUp.UnregisterCallback<ClickEvent>(ExitPopUpClicked);
        googleButton.UnregisterCallback<ClickEvent>(GooglePopUpClicked);
        infoButton.UnregisterCallback<ClickEvent>(InfoPopUpClicked);
        exitModal.UnregisterCallback<ClickEvent>(OnExitModalClicked);
        no.UnregisterCallback<ClickEvent>(OnNoClicked);
        yes.UnregisterCallback<ClickEvent>(OnYesClicked);
        _3backButton.UnregisterCallback<ClickEvent>(On_3BackButtonClicked);
        listView.UnregisterCallback<ClickEvent>(OnListClicked);
        mapView.UnregisterCallback<ClickEvent>(OnMapViewClicked);
        back.UnregisterCallback<ClickEvent>(OnBackInfoButtonClicked);
        next.UnregisterCallback<ClickEvent>(OnNextButtonClicked);
        previous.UnregisterCallback<ClickEvent>(OnPreviousButtonClicked);
        audio.UnregisterCallback<ClickEvent>(OnAudioButtonClicked);
        audioPause.UnregisterCallback<ClickEvent>(OnAudioPauseButtonClicked);
        GooglePin.UnregisterCallback<ClickEvent>(OnGooglePinButtonClicked);
        web.UnregisterCallback<ClickEvent>(OnWebButtonClicked);
        telephone.UnregisterCallback<ClickEvent>(OnTelephoneButtonClicked);
        googlemap.UnregisterCallback<ClickEvent>(OnGoogleMapButtonClicked);

        routeButton.UnregisterCallback<ClickEvent>(OnRouteButtonCLicked);
        mapInfoButton.UnregisterCallback<ClickEvent>(OnMapInInfoButtonCLicked);
        retargetButton.UnregisterCallback<ClickEvent>(OnRetargetButtonCLicked);
        distanceExitButton.UnregisterCallback<ClickEvent>(OnDistanceExitButtonCLicked);
        distanceAcceptButton.UnregisterCallback<ClickEvent>(OnDistanceAcceptButtonCLicked);
        gpsButton.UnregisterCallback<ClickEvent>(OnGPSButtonCLicked);

        UnregisterButtonCallbacks();
        UnregisterFilterButtonCallbacks();
    }
    public override void Exit()
    {
        UnregisterButtons();
        UtilsHomeBar.StopAudio();

        startState.StartEndHandler(StartState.state.None);
        selectPath.SelectUnselect(SelectPath.Select.Unselected);

        PointOfInterest.PopUpEvent -= PopUpDisplay;
        
        PointOfInterest.PopUpImages -= (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            Handle32RoutesImages(numberOfSprites, sprites);
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
        HidePopUp();

        UtilsHomeBar.swipe -= (int swipeDirection) =>
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };

        BackGroundToggle(UIExtentions.cyanBlue);
        ButtonElement.buttonnotifier -= ButtonToInfoBehaviour;      
        ButtonElement.buttonPressedIndex -= ButtonPressedFrom32Routes; 

        UIExtentions.Display(mapPage, false);
        UtilsHomeBar.SetActiveToFalse(true);
        UtilsHomeBar.RestorePathPOI();
        UtilsHomeBar.DeselectPOI();

        UIExtentions.Display(Fader, false);
        UtilsHomeBar.ReduceOpacity(listView, startRoadTripPage, false, _width, _height, maximizer);
        UtilsHomeBar.ReduceOpacity(mapView, startRoadTripPage, true, _width, _height, maximizer);

        UtilsHomeBar.mapViewCheck = false;
        UtilsHomeBar.fullScreenCheck = false;
        UtilsHomeBar.isIn360 = false;
        UtilsHomeBar.infoCheck32 = false;
        UtilsHomeBar.popUpEnabled = false;
        UtilsHomeBar.isInMap = false;
        UtilsHomeBar.isInReccommendedRoutes = false;
        
        UtilsHomeBar.enableLandscapeAction -= HandleLandscape;

        homecheck = false;
        settingscheck = false;
        arCheck = false;
        infoCHeck = false;

        previousCheck = false;
        nextCheck = false;

        

        recommendedRoutesButtons.Clear();
        unseenButtons.Clear();
        seenButtons.Clear();

        ResetFilterButtons();

        ObjectSelectionManager.AudioFinished -= TogglePauseUnPauseAudioIconHandler;
        exit.Invoke();
        UtilsCommander.UndoCommand(); 
        UtilsCommander.Clear();      
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        UtilsCommander.UndoPointer32Command();
        UtilsCommander.ClearPointer32();
        AbstractPointerBehaviour.ResetCurrentIndex();
       
        foreach (bool item in mainButtonCheck)
        {
            item.Equals(false);
        }
        indexbtn = 0;
        
        UtilsHomeBar.ToggleNumberAndSpriteAction(false);
        UIExtentions.Display(PointerCounterHolderLabel, false);


        unload360Material?.Invoke();

        Debug.Log("Exit MapPage");
    }

    public override void Tick()
    {
        Debug.LogWarning(currentImageIndex);
        staticStartState = startState;
        if(UtilsHomeBar.ar == false)
        {
            // Slider
            SliderBehaviour();
            // PopUP
            PopUpUpdateTextAndIconFromSelectedPOI();
            //INFO
            UpdateInfoIconAndTextsIndex();
            UpdateWebsiteButton();
            UpdateTelButton();
            UpdateTicketButton();
            // ChangePreNextButtonColor();
            //Fullscreen
            
            
            // HandleFSReset();
UpdateFullScreenCounter();

            //360
            Update360_UI();
            //Switcher
            SwitcherOfStates();

            for (int i = 0; i < mainButtonCheck.Length; i++)
            {
                if (mainButtonCheck[i] == true)
                {
                    indexOfSumPOIS = JSONTest.Instance.globalPath[i].coordinates.Count();
                    SelectPathInitialization(i, indexOfSumPOIS.ToString());
                    mainButtonCheck[i] = false;
                }
            }
            pointC.text = seenButtons.Count().ToString();
            GeneralPreNextButtonBehaviours();
            }

    }

    #region BasicInitialize Methods
    
    #region HomeButtons
    private void HomeBarButtons()
    {
        home.RegisterCallback<ClickEvent>(OnHomeButtonClicked);
        routes.RegisterCallback<ClickEvent>(OnRoutesClicked);
        map.RegisterCallback<ClickEvent>(OnMapClicked);
        settings.RegisterCallback<ClickEvent>(OnSettingsClicked);
        augmentedReality.RegisterCallback<ClickEvent>(OnAugmentedRealityClicked);
    }
    private void OnAugmentedRealityClicked(ClickEvent evt)
    {
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
            arbtn = true;
        }
        else if(startState.isStarted == false)
        {
           arCheck = true; 
        } 
    }
    private void OnSettingsClicked(ClickEvent evt)
    {
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
            setbtn = true;
        }
        else if(startState.isStarted == false)
        {
            settingscheck = true;
        }
    }
    private void OnMapClicked(ClickEvent evt)
    {
        Debug.Log("AlreadyAtInfoState");
    }
    private void OnRoutesClicked(ClickEvent evt)
    {
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
            infobtn = true;
        }
        else if(startState.isStarted == false)
        {
            infoCHeck = true;
        }
    }
    private void OnHomeButtonClicked(ClickEvent evt)
    {
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
            hmbtn = true;
        }
        else if(startState.isStarted == false)
        {
            homecheck = true;
        }
    }
    private void SwitcherOfStates()
    {
        if (homecheck == true)
        {
            stateMachine.SwitchState(new HomePageState(stateMachine));
        }
        if (infoCHeck == true)
        {
            stateMachine.SwitchState(new InfoState(stateMachine));
        }
        if (settingscheck == true)
        {
            stateMachine.SwitchState(new SettingsState(stateMachine));
        }
        if (arCheck == true)
        {
            stateMachine.SwitchState(new ARState(stateMachine));
            // var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
            UtilsHomeBar.isInMap = false;
            // arElem.enabled = true;
            arCheck = false;
        }
    }
    #endregion HomeButtons
    // Second Page Buttons Behaviours {Back, ListView and MapView}

    #region ListAndMap
    private void ListAndMapViewButtonHandler()
    {
        exitModal.RegisterCallback<ClickEvent>(OnExitModalClicked);
        no.RegisterCallback<ClickEvent>(OnNoClicked);
        yes.RegisterCallback<ClickEvent>(OnYesClicked);
        _3backButton.RegisterCallback<ClickEvent>(On_3BackButtonClicked);
        listView.RegisterCallback<ClickEvent>(OnListClicked);
        mapView.RegisterCallback<ClickEvent>(OnMapViewClicked);
    }

    private void OnMapViewClicked(ClickEvent evt)
    {
        cameFromMapView = true;
        StartAndMapButtonBehaviour(false);
        BackGroundToggle(UIExtentions.whiteAlpha00);
        AbstractUpdateMapFromState.UpdateMapToSelectedRoute(pathIndex);
    }

    private void OnListClicked(ClickEvent evt)
    {
        cameFromMapView = false;
        StartAndMapButtonBehaviour(true);
        BackGroundToggle(UIExtentions.cyanBlue);
        HidePopUp();
    }

    private void On_3BackButtonClicked(ClickEvent evt)
    {
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
        }
        else if(startState.isStarted == false)
        {
            startState.StartEndHandler(StartState.state.None);
            UtilsCommander.UndoCommand();
            UtilsCommander.Clear();      
            UtilsCommander.UndoPointer32Command();
            UtilsCommander.ClearPointer32();

            UtilsHomeBar.RestorePathPOI();
            UtilsHomeBar.DeselectPOI();
            ResetFilterButtons();
            BackGroundToggle(UIExtentions.cyanBlue);
            UIExtentions.Display(mapViewPage, true);
            UIExtentions.Display(startRoadTripPage, false);
            UIExtentions.Display(infoAndButtonArea, true);
            UtilsHomeBar.ReduceOpacity(mapView, startRoadTripPage, false, _width, _height, maximizer);

            UIExtentions.Display(start, true);
            UIExtentions.Display(end, false);
            selectPath.SelectUnselect(SelectPath.Select.Unselected);
            
            
            if(UtilsHomeBar.popUpEnabled)
            {
                HidePopUp(); 
            }
            else
            {
                UtilsHomeBar.swipe -= GetMovePointer;
                UtilsCommander.UndoPointerCommand();
                UtilsCommander.ClearPointer();
            }
            UtilsHomeBar.SetActiveToFalse(false);
            UtilsHomeBar.ToggleNumberAndSpriteAction(false);
        }
    }

    private void OnYesClicked(ClickEvent evt)
    {
        if(googlemapenable == false)
        {
                if(hmbtn == true)
                {
                    homecheck = true;
                }
                if(setbtn == true)
                {
                    settingscheck = true;
                } 
                if( infobtn == true)
                {
                    infoCHeck = true;
                }
                if(arbtn == true)
                {
                    arCheck = true;
                }
            startState.StartEndHandler(StartState.state.None);
            UtilsCommander.UndoCommand();
            UtilsCommander.Clear();      
            UtilsCommander.UndoPointer32Command();
            UtilsCommander.ClearPointer32();
            
            UtilsHomeBar.RestorePathPOI();
            UtilsHomeBar.DeselectPOI();
            ResetFilterButtons();
            BackGroundToggle(UIExtentions.cyanBlue);
            UIExtentions.Display(mapViewPage, true);
            UIExtentions.Display(startRoadTripPage, false);
            UIExtentions.Display(infoAndButtonArea, true);
            UtilsHomeBar.ReduceOpacity(mapView, startRoadTripPage, false, _width, _height, maximizer);
            
            UIExtentions.Display(start, true);
            UIExtentions.Display(end, false);
            selectPath.SelectUnselect(SelectPath.Select.Unselected);
           
            
            if(UtilsHomeBar.popUpEnabled)
            {
                HidePopUp(); 
            }
            else
            {
                UtilsHomeBar.swipe -= GetMovePointer;
                UtilsCommander.UndoPointerCommand();
                UtilsCommander.ClearPointer();
            }
            unseenButtons.Clear();
            seenButtons.Clear();

            UIExtentions.Display(PointerCounterHolderLabel, false);
            UtilsHomeBar.ToggleNumberAndSpriteAction(false);
            UIExtentions.Display(modal, false);  
        }
        // else if(googlemapenable == true)
        // {
        //     GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
        //     googlemapenable = false;
        //     UIExtentions.Display(modal, false);  
        // }
        UtilsHomeBar.SetActiveToFalse(false);
    }
    private void OnNoClicked(ClickEvent evt)
    {
        ExitModal();
    }
    private void OnExitModalClicked(ClickEvent evt)
    {
        ExitModal();
    }
    private void ExitModal()
    {
        UIExtentions.Display(modal, false);
        if (googlemapenable == true)
            googlemapenable = false;
    }
    #endregion ListAndMap

    #region StartEnd  
    private void StartEndWalkHandler()
    {
        start.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        end.RegisterCallback<ClickEvent>(OnEndButtonClicked);
    }
    private void OnStartButtonClicked(ClickEvent evt)
    {
            UIExtentions.Display(start, false);
            UIExtentions.Display(end, true);
            startState.StartEndHandler(StartState.state.Start);
            
            var buttons = mapPage.Query<Button>(className:"CardButton").ToList();
            unseenButtons = new List<Button>(buttons);
            if (unseenButtons.Count > 0)
            {
                MakeButtonBlue(0); // Make the first button in the unseen list blue
            }

            HandleStartButtonOpacity();
            UtilsHomeBar.ToggleNumberAndSpriteAction(true);
            UtilsHomeBar.SelectPOIFromStart(0);
            UIExtentions.Display(PointerCounterHolderLabel, true);
    }
    private void OnEndButtonClicked(ClickEvent evt)
    {
        UIExtentions.Display(start, true);
        UIExtentions.Display(end, enabled:false);
        startState.StartEndHandler(StartState.state.End);
    
        if(UtilsHomeBar.popUpEnabled)
        {
            HidePopUp();
        }
        else
        {
            UtilsHomeBar.swipe -= GetMovePointer;
        }

        var buttons = mapPage.Query<Button>(className:"CardButton").ToList();
        
        foreach (var item in buttons)
        {
            item.style.backgroundImage = new StyleBackground(UtilsHomeBar.greyButton);
            item.style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
            item.Q<Label>(className: "buttonFilterText").style.color = UIExtentions.grey;
            item.Q<Label>(className: "buttonTitle").style.color = UIExtentions.grey;
            item.Q<VisualElement>(className: "buttonImage").style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
            item.Q<VisualElement>(className: "buttonIcon").style.unityBackgroundImageTintColor = UIExtentions.grey05;

        }
        previous.style.color = UIExtentions.grey;
        previous.style.opacity = 0.5f;
        next.style.color = UIExtentions.BlueDark;

        unseenButtons.Clear();
        seenButtons.Clear();

        UtilsHomeBar.DeselectPOI();
        UIExtentions.Display(PointerCounterHolderLabel, false);
        UtilsHomeBar.ToggleNumberAndSpriteAction(false);
    }
    private void StartAndMapButtonBehaviour(bool enabling) 
    {
        BackGroundToggle(Color.clear);
        // the Element that holds the POI buttons
        UIExtentions.Display(infoAndButtonArea, enabling);
        UIExtentions.Display(Fader, enabling);
        // Button Map View Visual Functionality
        UtilsHomeBar.ReduceOpacity(mapView, startRoadTripPage, !enabling, _width, _height, maximizer);
        UtilsHomeBar.ReduceOpacity(listView, startRoadTripPage, enabling, _width, _height, maximizer);
        UtilsHomeBar.mapViewCheck = !enabling;
        // IgnoreBackGroundForMapInteraction(!enabling);
    }  
    private void HandleStartButtonOpacity()
    {
        start.style.backgroundColor = UIExtentions.BlueDark;
    }

    #endregion StartEnd
    
    private void MapButtonInit()
    {
        FiltersButtonInitialize();

        #region RouteIcon

        // Reccomended Scroll View Buttons
        for (int index = 0; index < reccomendedScrollViewButtons.Count; index++)
        {
            Button button = reccomendedScrollViewButtons[index];
            int currentIndex = index;

            EventCallback<ClickEvent> callback = (ClickEvent evt) =>
            {
                UtilsHomeBar.RestorePathPOI();
                UtilsHomeBar.SendPathInt(currentIndex);
                UIExtentions.Display(RecommendedRoutesPage, false);
                UIExtentions.Display(mapViewPage, false);
                UIExtentions.Display(startRoadTripPage, true);
                UtilsHomeBar.popUpEnabled = false;
                mainButtonCheck[currentIndex] = true;
                selectPath.SelectUnselect(SelectPath.Select.Selected);
                ResetFilterButtons();
                StartAndMapButtonBehaviour(false);
                BackGroundToggle(UIExtentions.whiteAlpha00);
                AbstractUpdateMapFromState.UpdateMapToSelectedRoute(pathIndex);
                UtilsHomeBar.isInReccommendedRoutes = false;
                curreRRIndex = -1;
                newIndex = -1;
            };

            button.RegisterCallback(callback);
            buttonCallbacks[button] = callback;
        }

        // Exit Buttons
        foreach (var btn in exitbuttonrecommended)
        {
            EventCallback<ClickEvent> callback = (ClickEvent evt) =>
            {
                UIExtentions.Display(RecommendedRoutesPage, false);
                UtilsHomeBar.popUpEnabled = false;
                UtilsHomeBar.mapViewCheck = true;
                UtilsHomeBar.RestorePathPOI();
                UtilsHomeBar.SetActiveToFalse(false);
                Debug.Log("EXIT");
                UtilsHomeBar.isInReccommendedRoutes = false;
                curreRRIndex = -1;
                newIndex = -1;
            };

            btn.RegisterCallback(callback);
            buttonCallbacks[btn] = callback;
        }

        // Blue Titles Accept Buttons
        for (int index = 0; index < recommendedRoutesButtons.Count; index++)
        {
            Button button = recommendedRoutesButtons[index];
            int currentIndex = index;

            EventCallback<ClickEvent> callback = (ClickEvent evt) =>
            {
                UtilsHomeBar.RestorePathPOI();
                UtilsHomeBar.SendPathInt(currentIndex);
                UIExtentions.Display(RecommendedRoutesPage, false);
                UIExtentions.Display(mapViewPage, false);
                UIExtentions.Display(startRoadTripPage, true);
                UtilsHomeBar.popUpEnabled = false;
                mainButtonCheck[currentIndex] = true;
                selectPath.SelectUnselect(SelectPath.Select.Selected);
                ResetFilterButtons();
                StartAndMapButtonBehaviour(false);
                BackGroundToggle(UIExtentions.whiteAlpha00);
                AbstractUpdateMapFromState.UpdateMapToSelectedRoute(pathIndex);
                UtilsHomeBar.isInReccommendedRoutes = false;
                curreRRIndex = -1;
                newIndex = -1;
            };

            button.RegisterCallback(callback);
            buttonCallbacks[button] = callback;
        }

        #endregion RouteIcon
        
        routeButton.RegisterCallback<ClickEvent>(OnRouteButtonCLicked);
        mapInfoButton.RegisterCallback<ClickEvent>(OnMapInInfoButtonCLicked);
        retargetButton.RegisterCallback<ClickEvent>(OnRetargetButtonCLicked);
        distanceExitButton.RegisterCallback<ClickEvent>(OnDistanceExitButtonCLicked);
        distanceAcceptButton.RegisterCallback<ClickEvent>(OnDistanceAcceptButtonCLicked);
        gpsButton.RegisterCallback<ClickEvent>(OnGPSButtonCLicked);
    }
    private void UnregisterButtonCallbacks()
    {
        foreach (var kvp in buttonCallbacks)
        {
            kvp.Key.UnregisterCallback(kvp.Value);
        }
        buttonCallbacks.Clear();
    }

    private void OnGPSButtonCLicked(ClickEvent evt)
    {
        distancePagePressed = true;
        UIExtentions.Display(distancePage, true);

        var slider = GameObject.Find("UIDocument").GetComponent<CustomSlider>();

        slider.enabled = true;

        UtilsHomeBar.popUpEnabled = false;/////////////////////////////////
        UtilsHomeBar.mapViewCheck = false;
    }

    private void OnDistanceAcceptButtonCLicked(ClickEvent evt)
    {
        UtilsHomeBar.TriggerSendDistance(distanceSlider.value);
        previousValue = distanceSlider.value;
        currentValue = previousValue;

        distancePagePressed = false;

        UIExtentions.Display(distancePage, false);
        UtilsHomeBar.popUpEnabled = false;
        UtilsHomeBar.mapViewCheck = true;

    }

    private void OnDistanceExitButtonCLicked(ClickEvent evt)
    {
        if (distanceSlider.value != previousValue)
        {
            distanceSlider.value = previousValue;
        }
        distancePagePressed = false;

        UIExtentions.Display(distancePage, false);
        UtilsHomeBar.popUpEnabled = false;
        UtilsHomeBar.mapViewCheck = true;

    }

    private void OnRouteButtonCLicked(ClickEvent evt)
    {
        UIExtentions.Display(RecommendedRoutesPage, true);
        // UtilsHomeBar.popUpEnabled = true;
        HidePopUp();
        UtilsHomeBar.DeselectPOI();
        UtilsHomeBar.mapViewCheck = false;
        curreRRIndex = -1;
        newIndex = -1;
        UtilsHomeBar.isInReccommendedRoutes = true;
        ScrollViewRB.ScrollTo(ScrollViewRB[0]);
    }

    private void OnRetargetButtonCLicked(ClickEvent evt)
    {
        AbstractUpdateMapFromState.UpdateMapFromState();
    }
    private void OnMapInInfoButtonCLicked(ClickEvent evt)
    {
        var mapTutorial = GameObject.Find("UIDocument").GetComponent<MapTutorial>();
        UtilsHomeBar.mapViewCheck = false;
        mapTutorial.enabled = true;
        UIExtentions.Display(mapTutorial.tutorial, true);
        // tutorialMap?.Invoke();   
    }

    // Map Filter Buttons Initialize and Reset Methods
    private void FiltersButtonInitialize()
    {
        buttonFilterMap.Add(monumentButton, PointOfInterest.FilterID.Monument);
        buttonFilterMap.Add(templeButton, PointOfInterest.FilterID.Temple);
        buttonFilterMap.Add(museumButton, PointOfInterest.FilterID.Museum);
        buttonFilterMap.Add(squareButton, PointOfInterest.FilterID.Park);
        buttonFilterMap.Add(playGroundButton, PointOfInterest.FilterID.PlayGround);
        buttonFilterMap.Add(recreationGroundButton, PointOfInterest.FilterID.RecreationGround);
        buttonFilterMap.Add(shoppingStreetButton, PointOfInterest.FilterID.ShoppingStreet);
        buttonFilterMap.Add(culturalFacilityButton, PointOfInterest.FilterID.CulturalFacility);

        foreach (var pair in buttonFilterMap)
        {
            Button button = pair.Key;
            PointOfInterest.FilterID filterID = pair.Value;
            bool pressed = false;
            EventCallback<ClickEvent> callback = (ClickEvent evt) =>
            {
                pressed = !pressed;
                UtilsHomeBar.ReduceFilterOpacity(button, pressed, _height, maximizer);
                UtilsHomeBar.TriggerFilterEvent(filterID, pressed);
                FilterScrollView.ScrollTo(button);
            };

            button.RegisterCallback(callback);
            buttonCallbacks[button] = callback;
        }
    }
    private void UnregisterFilterButtonCallbacks()
    {
        foreach (var kvp in buttonCallbacks)
        {
            kvp.Key.UnregisterCallback(kvp.Value);
        }
        buttonCallbacks.Clear();
    }
    private void ResetFilterButtons()
    {
        foreach (var pair in buttonFilterMap)
        {
            bool pressed = false;

            UtilsHomeBar.ReduceFilterOpacity(pair.Key, pressed, _height, maximizer);
            UtilsHomeBar.TriggerFilterEvent(pair.Value, pressed);
        }
    }

#region InfoPage Main Route Behaviour
    private void SelectPathInitialization(int currentIndex , string txt)
    {
        UtilsCommander.ExecuteCommand(currentIndex, buttonSpawnPoint);
        routeText.text =
        (
            UIExtentions.IsEnglish()
            ? JSONTest.Instance.globalRoutes.englishTextsRoute[currentIndex].Titles[Row].ToString()
            : JSONTest.Instance.globalRoutes.greekTextsRoute[currentIndex].Titles[Row].ToString()
        );
        pathText.text =
        (
            UIExtentions.IsEnglish()
            ? pathText.text = JSONTest.Instance.globalRoutes.englishTextsRoute[currentIndex].Texts[Row].ToString()
            : pathText.text = JSONTest.Instance.globalRoutes.greekTextsRoute[currentIndex].Texts[Row].ToString()
        );
        routeNameText.text =
        (
            UIExtentions.IsEnglish()
            ? JSONTest.Instance.globalRoutes.englishTextsRoute[currentIndex].Titles[Row].ToString().ToUpper()
            : JSONTest.Instance.globalRoutes.greekTextsRoute[currentIndex].Titles[Row].ToString().ToUpper()
        );

        routeCounter.text = txt;
        pointS.text = routeCounter.text;
        pointText.text = txt;
    }

#endregion InfoPage Main Route Behaviour

#endregion BasicInitialize Methods

#region 32Routes Info
    private void InfoButtons32()
    {   
        back.RegisterCallback<ClickEvent>(OnBackInfoButtonClicked);
        next.RegisterCallback<ClickEvent>(OnNextButtonClicked);
        previous.RegisterCallback<ClickEvent>(OnPreviousButtonClicked);
        audio.RegisterCallback<ClickEvent>(OnAudioButtonClicked);
        audioPause.RegisterCallback<ClickEvent>(OnAudioPauseButtonClicked);
        GooglePin.RegisterCallback<ClickEvent>(OnGooglePinButtonClicked);
        web.RegisterCallback<ClickEvent>(OnWebButtonClicked);
        telephone.RegisterCallback<ClickEvent>(OnTelephoneButtonClicked);
        googlemap.RegisterCallback<ClickEvent>(OnGoogleMapButtonClicked);
    }

    private void OnGoogleMapButtonClicked(ClickEvent evt)
    {
        // modalTitle.text = modalTitleGoogleText.GetTranslatedText();
        // modaldescription.text = modalDescriptionGoogleText.GetTranslatedText();
        // UIExtentions.Display(modal, true);
        // // resetButtonColor?.Invoke();
        // googlemapenable = true;
        GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
    }
    private void OnTelephoneButtonClicked(ClickEvent evt)
    {
        if(ObjectSelectionManager.hasTel)
        {
            Application.OpenURL("tel:" + ObjectSelectionManager.Contact);
        }
    }
    private void OnWebButtonClicked(ClickEvent evt)
    {
        if(ObjectSelectionManager.hasWebSite)
        {
            Application.OpenURL(ObjectSelectionManager.OpenEntrance);
        }
    }
    private void OnGooglePinButtonClicked(ClickEvent evt)
    {
        // GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
        UIExtentions.Display(infoRoutesMap, false);
        resetButtonColor?.Invoke();
        if(selectPath.isStarted == true)
        {
            UIExtentions.Display(startRoadTripPage, true);
        }
        else if(selectPath.isStarted == false)
        {
            UIExtentions.Display(mapViewPage, true);
        }
        cameFromMapView = false;
        HidePopUp();
        UtilsHomeBar.infoCheck32 = false;
        StartAndMapButtonBehaviour(false);
    }
    private void OnAudioPauseButtonClicked(ClickEvent evt)
    {
        // UtilsHomeBar.StopAudio();
        switch (currentAudioState)
        {
            case AudioState.Playing:
                UtilsHomeBar.PauseAudio();
                currentAudioState = AudioState.Paused;
                audioPause.style.backgroundImage = new StyleBackground(UtilsHomeBar.resumeSpriteStatic);
                Debug.Log("PAUSE BUTOON");
                break;
            case AudioState.Paused:
                UtilsHomeBar.ResumeAudio();
                currentAudioState = AudioState.Playing;
                audioPause.style.backgroundImage = new StyleBackground(UtilsHomeBar.pauseSpriteStatic);
                Debug.Log("UNPAUSE BUTOON");
                break;
        }
    }
    private void OnAudioButtonClicked(ClickEvent evt)
    {
        UtilsHomeBar.PlayAudio();
        currentAudioState = AudioState.Playing;
        TogglePauseUnPauseAudioIconHandler(false, true);
        // ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
    }
    private void OnPreviousButtonClicked(ClickEvent evt)
    {
        previousCheck = true;
    }
    private void OnNextButtonClicked(ClickEvent evt)
    {
       nextCheck = true;
    }
    private void OnBackInfoButtonClicked(ClickEvent evt)
    {
        UtilsHomeBar.StopAudio();
        TogglePauseUnPauseAudioIconHandler(true, false);
        if(startState.stateStart == StartState.state.Start)
        {
            resetButtonColor?.Invoke();
        }
        UIExtentions.Display(infoRoutesMap, false);

        if(selectPath.stateStart == SelectPath.Select.Selected)
        {
            UIExtentions.Display(startRoadTripPage, true);
            ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
        }
        else if(selectPath.stateStart == SelectPath.Select.Unselected)
        {
            UIExtentions.Display(mapViewPage, true);
        }
                        
        UtilsHomeBar.infoCheck32 = false;
        
        if(cameFromMapView)
        {
            UtilsHomeBar.mapViewCheck = true;
        }
        else
        {
            UtilsHomeBar.mapViewCheck = false;
        }
        
        
        // UtilsCommander.UndoPointerCommand();
        // UtilsCommander.ClearPointer();
        UtilsHomeBar.DeselectPOI();
        UtilsCommander.UndoPointer32Command();
        UtilsCommander.ClearPointer32();
        

        
        HidePopUp();

        currentImageIndex = 0;
    }

    private void PreviousNextButtons(bool next)
    {
        currentImageIndex = 0;
        UtilsHomeBar.StopAudio();
        TogglePauseUnPauseAudioIconHandler(true, false);
        UtilsHomeBar.SelectNewPOI(next);
        ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
    }
    private void Handle32RoutesImages(int images, List<Sprite> sprites)
    {
        scrollViewMap.Clear();
        totalImagesCounter = images;
        fullScreenSprites = sprites;
        //  UtilsHomeBar.swipe -= GetMovePointer;
        int numSprites = Mathf.Min(images, sprites.Count);
        UtilsCommander.UndoPointer32Command();

        for (int i = 0; i < sprites.Count; i++)
        {
            VisualElement visualElement = new VisualElement();
            visualElement.style.overflow = Overflow.Hidden;
            visualElement.style.flexGrow = 1;
            visualElement.style.width = 414;
            visualElement.style.height = 304;
            visualElement.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            visualElement.style.borderTopWidth = 5;
            visualElement.style.borderLeftWidth = 5;
            visualElement.style.borderBottomWidth = 30;
            visualElement.style.borderRightWidth = 30;
            visualElement.style.borderBottomLeftRadius = 30;
            visualElement.style.borderBottomRightRadius = 30;

            visualElement.style.backgroundImage = new StyleBackground(sprites[i]);
            scrollViewMap.Add(visualElement);
        } 
        UtilsCommander.ExecutePointer32Command(sprites.Count, PointerTab);
        // AbstractPointerBehaviour.ResetCurrentIndex();       
        //  UtilsHomeBar.swipe += GetMovePointer;
    }
    private void UpdateInfoIconAndTextsIndex()
    {
        filtertext.text = ObjectSelectionManager.filterIDText;
        filterIconGrey.style.backgroundImage = new StyleBackground(ObjectSelectionManager.spriteIcon);
        routeIndex.text = ObjectSelectionManager.index.ToString();

        foreach (var item in titleList)
        {
            item.text = ObjectSelectionManager.textGlobal;
        }
        address.text = ObjectSelectionManager.Address;
        ticket.text = ObjectSelectionManager.Entry;
        email.text = ObjectSelectionManager.OpenEntrance;
        tel.text = ObjectSelectionManager.Contact;
        descPoint.text = ObjectSelectionManager.textGlobalD;
    }
    private void UpdateWebsiteButton()
    {
        if (ObjectSelectionManager.hasWebSite == true)
        {
            UIExtentions.Display(Earth, true);
        }
        else
        {
             UIExtentions.Display(Earth, false);
        }
    }
    private void UpdateTelButton()
    {
        if (ObjectSelectionManager.hasTel == true)
        {
            UIExtentions.Display(Telephone, true);
        }
        else
        {
             UIExtentions.Display(Telephone, false);
        }
    }
    private void UpdateTicketButton()
    {
        if (ObjectSelectionManager.hasTicket == true)
        {
            UIExtentions.Display(Ticket, true);
        }
        else
        {
             UIExtentions.Display(Ticket, false);
        }
    }
    private void ChangePreNextButtonColor()
    {
        if (indexbtn == 0)
        {
            previous.style.color = UIExtentions.grey;
            previous.style.opacity = 0.5f;
            next.style.color = UIExtentions.BlueDark;
            next.style.opacity = 1f;
        }
        else if (indexbtn == indexOfSumPOIS - 1)
        {
            previous.style.color = UIExtentions.BlueDark;
            previous.style.opacity = 1f;
            next.style.color = UIExtentions.grey;
            next.style.opacity = 0.5f;
        }
        else
        {
            previous.style.color = UIExtentions.BlueDark;
            next.style.color = UIExtentions.BlueDark;
            previous.style.opacity = 1f;
            next.style.opacity = 1f;
        }
    }
#endregion 32 Routes Info
    private void ButtonToInfoBehaviour(bool pressed, bool elements, bool map)
    { 
        UIExtentions.Display(infoRoutesMap, pressed);
        UIExtentions.Display(mapViewPage, !pressed);
        UIExtentions.Display(startRoadTripPage, !pressed);
        UtilsHomeBar.infoCheck32 = pressed;
        UIExtentions.Display(previous, !elements);
        UIExtentions.Display(next, !elements);    
        UIExtentions.Display(routeCounter, !elements);
        UIExtentions.Display(routeIndex, !elements);
        UIExtentions.Display(routeNameText, !elements); 
        UIExtentions.Display(infoRoutesMap.Q<Label>("From"), !elements);
        UIExtentions.Display(infoRoutesMap.Q<VisualElement>("PrevNextHolder"), !elements);
        // cameFromMapView = map;
    }   
    private void ButtonPressedFrom32Routes(int objIndex)
    {
        indexbtn = objIndex;

        if(startState.isStarted == true)
        {
            var buttons = startRoadTripPage.Query<Button>(className:"CardButton").ToList();

            if (objIndex >= 0 && objIndex < buttons.Count)
            {
                // Move the button to the seen list
                var pressedButton = buttons[objIndex];
                if (!seenButtons.Contains(pressedButton))
                {
                    seenButtons.Add(pressedButton);
                    unseenButtons.Remove(pressedButton);
                }

                // Set the current button to seen (grey)
                MakeButtonGrey(pressedButton);

                // Set the next button to blue if it exists
                if (unseenButtons.Count > 0)
                {
                    MakeButtonBlue(0); // The first button in the unseen list
                }

                currentButtonIndex = objIndex; // Update the current button index
            }  
        }
        else
        {
            return;
        }  
    } 

    #region PopUpWindow
    private void HandlePopUP()
    {
        exitPopUp.RegisterCallback<ClickEvent>(ExitPopUpClicked);
        googleButton.RegisterCallback<ClickEvent>(GooglePopUpClicked);
        infoButton.RegisterCallback<ClickEvent>(InfoPopUpClicked);
    }
    private void ExitPopUpClicked(ClickEvent evt)
    {
        UtilsHomeBar.DeselectPOI();
        UtilsCommander.UndoPointerCommand();
        HidePopUp();
        // UtilsHomeBar.swipe -= GetMovePointer;
        UtilsHomeBar.mapViewCheck = true;
    }
    private void GooglePopUpClicked(ClickEvent evt)
    {
        // modalTitle.text = modalTitleGoogleText.GetTranslatedText();
        // modaldescription.text = modalDescriptionGoogleText.GetTranslatedText();
        // UIExtentions.Display(modal, true);
        // googlemapenable = true;
        GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
    }
    private void InfoPopUpClicked(ClickEvent evt)
    {
        HidePopUp();      
        if(selectPath.stateStart == SelectPath.Select.Unselected)
        {
            ButtonToInfoBehaviour(pressed: true, true, false);
        }
        else if(selectPath.stateStart == SelectPath.Select.Selected)
        {
            if(startState.stateStart == StartState.state.Start)
            {
                ButtonToInfoBehaviour(true, false, false);
                ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
            }  
            else if(startState.stateStart == StartState.state.End || startState.stateStart == StartState.state.None)
            {
                ButtonToInfoBehaviour(true, true, false);
            }

        }
        UtilsHomeBar.infoCheck32 = true;
        UtilsHomeBar.mapViewCheck = false;
        resetButtonColor?.Invoke();
        cameFromMapView = true;
        // UtilsHomeBar.swipe -= GetMovePointer;
        
        // AbstractPointerBehaviour.ResetCurrentIndex();
        UtilsHomeBar.swipe += GetMovePointer;
    }
    private void HandlePopUpImages(int images, List<Sprite> sprites)
    {
        scrollView.Clear();
        totalImagesCounter = images;
        int numSprites = Mathf.Min(images, sprites.Count);
        UtilsCommander.UndoPointerCommand();
        for (int i = 0; i < numSprites; i++)
        {
            VisualElement visualElement = new VisualElement();
            visualElement.AddToClassList("PopUpImages");
            visualElement.style.overflow = Overflow.Hidden;
            visualElement.style.flexGrow = 1;
            visualElement.style.width = 414;
            visualElement.style.height = 218;
            visualElement.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            visualElement.style.borderTopWidth = 5;
            visualElement.style.borderLeftWidth = 5;
            visualElement.style.borderBottomWidth = 5;
            visualElement.style.borderRightWidth =5;

            visualElement.style.backgroundImage = new StyleBackground(sprites[i]);
            scrollView.Add(visualElement);
        }  

        UtilsCommander.ExecutePointerCommand(numSprites, PointerContainer);
    }
    private void PopUpDisplay()
    {
        UtilsHomeBar.swipe -= GetMovePointer;
        UIExtentions.Display(PopUp, true);
        PopUp.style.translate = new StyleTranslate(popUpVisibleTransfrom);

        TutorialHolder.style.unityBackgroundImageTintColor = tintBackgroundFadeOut;
        UtilsHomeBar.popUpEnabled = true;

        UIExtentions.Display(Fader, false);
        Physics.IgnoreLayerCollision(UIExtentions.mask, 0);

        AbstractPointerBehaviour.ResetCurrentIndex();
        UtilsHomeBar.swipe += GetMovePointer;        
    }   
    private async void HidePopUp()
    {      
        PopUp.style.translate = new StyleTranslate(popUpUnseenTransform);
        TutorialHolder.style.unityBackgroundImageTintColor = tintBackgroundFadeOut;
        await Task.Delay(500); 
        UIExtentions.Display(PopUp, false);
        UtilsHomeBar.popUpEnabled = false;  
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        AbstractPointerBehaviour.ResetCurrentIndex();
        UIExtentions.Display(Fader, false);
        UtilsHomeBar.swipe -= GetMovePointer; 
        
        scrollView.Clear();
        PointOfInterest.PopUpImages -= (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            // Handle32RoutesImages(numberOfSprites, sprites);
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
        currentImageIndex = 0;
    }
    private void PopUpUpdateTextAndIconFromSelectedPOI()
    {
        popUpTitle.text = ObjectSelectionManager.textGlobal;
        popUpFilterID.text = ObjectSelectionManager.filterIDText;
        filterIDIconGrey.style.backgroundImage = new StyleBackground(ObjectSelectionManager.spriteIcon);
    }
    
    #endregion PopUpWindow

    #region FullScreen
    private void FullScreenButtons()
    {
        fullScreenButton.RegisterCallback<ClickEvent>(FullScreenButtonClicked);        
        fsNext.RegisterCallback<ClickEvent>(FullScreenButtonNextClicked);
        fsBack.RegisterCallback<ClickEvent>(FullScreenButtonBackClicked);   
        fullScreenButtonReset.RegisterCallback<ClickEvent>(FullScreenResetClicked);         
    }
    private void FullScreenButtonClicked(ClickEvent evt)
    {
        // currentImageIndex = 0;
        UIExtentions.Display(fullScreen, true);
        back.style.display = DisplayStyle.None;
        UtilsHomeBar.fullScreenCheck = true;
        DisplayCurrentImage(currentImageIndex);
    }
    private void FullScreenResetClicked(ClickEvent evt)
    {
        if(UtilsHomeBar.fullScreenCheck)
        {
            UIExtentions.Display(fullScreen, false);
            back.style.display = DisplayStyle.Flex;
            // currentImageIndex = 0;
            UtilsHomeBar.fullScreenCheck = false;
        }
    }
    private void FullScreenButtonBackClicked(ClickEvent evt)
    {
        HandleFSImagesSwipeR(-1);
        GetMovePointer(-1);
    }
    private void FullScreenButtonNextClicked(ClickEvent evt)
    {
        HandleFSImagesSwipeL(1);
        GetMovePointer(1);
    }
    // void HandleFSReset()
    // {                
    //     if(UtilsHomeBar.fullScreenCheck)
    //     {
    //         UpdateFullScreenCounter();
    //         fullScreenButtonReset.clicked += () =>
    //         {
    //             UIExtentions.Display(fullScreen, false);
    //             back.style.display = DisplayStyle.Flex;
    //             // currentImageIndex = 0;
    //             UtilsHomeBar.fullScreenCheck = false;
    //         };
    //     }
    // }
    private void HandleFullScreenImages(int images, List<Sprite> sprites)
    {
        fullScreenSprites.Clear();
        int numSprites = Mathf.Min(images, sprites.Count);
        for (int i = 0; i < numSprites; i++)
        {
            fullScreenSprites.Add(sprites[i]);
        }
        // DisplayCurrentImage(currentImageIndex);
    }
    private void DisplayCurrentImage(int index)
    {
        // Check if the index is within bounds
        if (currentImageIndex >= 0 && currentImageIndex < fullScreenSprites.Count)
        {
            fsImage.style.backgroundImage = new StyleBackground(fullScreenSprites[index]);
        }
    }  
   private void HandleFSImagesSwipeL(int direction)
    {
        if(direction == 1)
        {
            if (currentImageIndex >= 0 && currentImageIndex < fullScreenSprites.Count - 1)
            {
                currentImageIndex ++;               
                fsImage.style.backgroundImage = new StyleBackground(fullScreenSprites[currentImageIndex]);
            }
        }else return;
    }
    private void HandleFSImagesSwipeR(int direction)
    {
        if(direction == -1)
        {
            if (currentImageIndex >= 0 && currentImageIndex < fullScreenSprites.Count)
            {             
                if (currentImageIndex > 0)
                {
                    currentImageIndex --;
                    fsImage.style.backgroundImage = new StyleBackground(fullScreenSprites[currentImageIndex]);
                }        
            }
        }else return;
    }    
    private void UpdateFullScreenCounter()
    {
        if(UtilsHomeBar.fullScreenCheck)
        {
            fsRouteCounter.text = fullScreenSprites.Count.ToString();
            fsPoiIndex.text = (currentImageIndex + 1).ToString();
        }
        else return;
    }
    void HandleLandscape(bool enabled)
    {
        UIExtentions.Display(fsResetRegion, enabled);
        UIExtentions.Display(fsNPRegion, enabled);
    }
    #endregion FullScreen

    #region _360Images
    private void Update360_UI()
    {
        Update360ButtonOpacity();
        Update360Title();
    }            
    private void Handle360()
    {
        goTo360.RegisterCallback<ClickEvent>(OnGoTo360Clicked);
        back360.RegisterCallback<ClickEvent>(OnBackFrom360Clicked);
    }

    private void OnBackFrom360Clicked(ClickEvent evt)
    {
        unload360Material?.Invoke();
        var homebar = mapPage.Q<VisualElement>("HomeBar");
        UIExtentions.Display(homebar, true);
        TransitionFromTo360(true,UIExtentions.cyanBlue, 175f, 90f);
        UtilsHomeBar.isIn360 = false;
    }

    private void OnGoTo360Clicked(ClickEvent evt)
    {
        UtilsHomeBar.isIn360 = true;
        var homebar = mapPage.Q<VisualElement>("HomeBar");
        UIExtentions.Display(homebar, false);
        load360Material?.Invoke();
        TransitionFromTo360(false,UIExtentions.whiteAlpha00, 0, 0);
    }
    private void TransitionFromTo360(bool enabling, Color color, float yPos, float xQuater)
    {
        UIExtentions.Display(View360Page, !enabling);
        UIExtentions.Display(infoRoutesMap, enabling);
        UIExtentions.Display(Fader, enabling);
        UtilsHomeBar.infoCheck32 = enabling;
        camera.transform.position = new Vector3(0, yPos, 0);
        camera.transform.rotation = Quaternion.Euler(xQuater, 0, 0);
        GameObject.Find("Camera").GetComponent<GyroscopeTest>().enabled = !enabling;
        HidePopUp();
        mapManager.gameObject.SetActive(enabling); 
    }
    private void Update360Title()
    {
        Title360.text = ObjectSelectionManager.textGlobal;
    }
    private void Update360ButtonOpacity()
    {
        if (ObjectSelectionManager.has360 == true)
        {
            goTo360.style.unityBackgroundImageTintColor = UIExtentions.white;
        }
        else
        {
            goTo360.style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha05;

        }
    }

#endregion _360Images

#region GeneralFunctionality 
    private void SliderBehaviour()
    {
        currentValue = distanceSlider.value;
        float kilometers = Mathf.Clamp(distanceSlider.value, distanceSlider.lowValue, distanceSlider.highValue) / 1000;
        distanceLabel.text = kilometers.ToString() + "Km";
    }
    void GetMovePointer(int index)
    {
        if(GameObject.Find("GameManager").GetComponent<StateTracker>().gameState == StateTracker.GameState.Map && UtilsHomeBar.isInMap)
        {
            if(UtilsHomeBar.popUpEnabled == true && UtilsHomeBar.infoCheck32 == false) 
            {
                AbstractPointerBehaviour.MovePointer(PointerContainer, index, scrollView); 
            } 
            else if(UtilsHomeBar.mapViewCheck == false && UtilsHomeBar.infoCheck32 == true)
            {
                AbstractPointerBehaviour.MovePointer(PointerTab, index, scrollViewMap); 
            }
            else 
            {
                Debug.LogWarning
                ($"Either popUP or mapView orboth were not TRUE. Pop up is "+
                $"{UtilsHomeBar.popUpEnabled} and mapView is {UtilsHomeBar.infoCheck32}");
            }     
            Debug.LogWarning(index);
        }

    } 
    void BackGroundToggle(Color color)
    {
        startRoadTripPage.style.backgroundColor = color;
    }  
    private void GeneralPreNextButtonBehaviours()
    {
        // Button Info
        if (previousCheck == true)
        {
            PreviousNextButtons(false);
            previousCheck = false;
        }
        if (nextCheck == true)
        {
            PreviousNextButtons(true);
            nextCheck = false;
        }
        ChangePreNextButtonColor();
    }
    public void TogglePauseUnPauseAudioIconHandler(bool play, bool pause)
    {
        UIExtentions.Display(audio, play);
        UIExtentions.Display(audioPause, pause);
    }
    void MakeButtonBlue(int nextButton)
    {
        if (unseenButtons.Count > 0)
        {
            var buttons = unseenButtons[nextButton];

            buttons.style.backgroundImage = new StyleBackground(UtilsHomeBar.blueButton);
            buttons.Q<Label>(className: "buttonFilterText").style.color = UIExtentions.yellow;
            buttons.Q<Label>(className: "buttonTitle").style.color = UIExtentions.yellow;
            buttons.Q<VisualElement>(className: "buttonIcon").style.unityBackgroundImageTintColor = UIExtentions.yellow;
        }
    }
    void MakeButtonGrey(Button button)
    {
        button.style.backgroundImage = new StyleBackground(UtilsHomeBar.greyButton);
        button.style.unityBackgroundImageTintColor = UIExtentions.grey08;
        
        button.Q<VisualElement>(className: "buttonImage").style.unityBackgroundImageTintColor = UIExtentions.grey05;
        button.Q<VisualElement>(className: "buttonIcon").style.unityBackgroundImageTintColor = UIExtentions.greyButtonFilter;

        button.Q<Label>(className: "buttonTitle").style.color = UIExtentions.grey05;
        button.Q<Label>(className: "buttonFilterText").style.color = UIExtentions.grey05; 
    }
#endregion GeneralFunctionality
}
