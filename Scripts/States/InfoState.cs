using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointsOfInterests;
using UnityEngine;
using UnityEngine.UIElements;
public class InfoState : PlayerBaseState
{
    public InfoState(PlayerStateMachine stateMachine) : base(stateMachine) {} // Class Constructor
    
    // HomeBar Buttons -------------------------------
    Button home;
    Button map;
    Button settings;
    Button augmentedReality;
    Button routes;
    //-------------------------------------------------
    Button backButton;
    Button _3backButton;
    VisualElement buttonSpawnPoint;
    List<Button> cardbuttons = new List<Button>();
    List<Label> numberofPois = new List<Label>();
    VisualElement culturalRoadsFirstCard;
    VisualElement routeRegionPage;

    // List View Visual Elements -----------------------------------------------
    Label pointText; 
    Label pathText;
    Label routeText;
    Button listView;
    Button mapView;
    Button start;
    Button end;
    VisualElement startRoadTripElement;
    VisualElement PointerCounterHolderLabel;
    Label pointC;
    Label pointS;
    ScrollView infoAndButtonArea;
    VisualElement backGround;
    VisualElement root;
    VisualElement routePage;
    VisualElement Fader;

#region PopUp Properties
    // PopUp Elements
    VisualElement popUpElement;
    Button exitPopUp;
    Button googleButton;
    ScrollView scrollView;
    Label popUpTitle;
    Label popUpFilterID;
    VisualElement filterIDIconGrey;

    Button infoButton;
    ScrollView scrollView32;
    VisualElement PointerContainer;
#endregion PopUp Properties

#region InfoRoutesAndPOIProperties
    // 3.2 Routes
    VisualElement infoRoutes;
    Button back;
    Button goTo360;
    List<Label> titleList = new List<Label>();
    VisualElement filterIconGrey;
    Label filtertext;
    Label routeNameText;
    Label routeIndex;
    Label routeCounter;

    public VisualElement PointerTab { get; private set; }

    Label address;
    Label tel;
    Label email;
    Label ticket;
    Label descPoint;

    VisualElement websiteButton;
    VisualElement Earth;
    VisualElement Telephone;
    VisualElement Ticket;

    Button web;
    Button telephone;
    Button googlemap;

    Button previous;
    Button next;
    Button fullScreenButton;
    Button fullScreenButtonReset;

    VisualElement fsResetRegion;
    VisualElement fsNPRegion;

    VisualElement fullScreen;
    Button GooglePin;
    Button audio;
    Button audioPause;
    Button fsNext;
    Button fsBack;
    VisualElement fsImage;
    Label fsPoiIndex;
    Label fsRouteCounter;
    VisualElement View360Page;
    Button back360;
    Label Title360;
#endregion InfoRoutesAndPOIProperties
    // .............................
    // General Properties ::::::::::::::::::::::::::::::::::::
    private const float _width = 65f;
    private const float _height = 50f;
    private const float maximizer = 2.5f;
    Translate popUpUnseenTransform = new Translate(0, 424, 0);
    Translate popUpVisibleTransfrom = new Translate(0, 70, 0);
    List<Sprite> fullScreenSprites = new List<Sprite>();
    int indexOfSumPOIS;
    const int Row = 0;
    int pathIndex;
    private int currentImageIndex;
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // ---------------------------AUDIO STATE ---------------------------

    public enum AudioState
    {
        Playing,
        Paused
    }

    // Maintain a variable to track the current state
    private AudioState currentAudioState;

    // :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    
    // Generall Refrences ---------------------------------------------------------------
    Camera camera = Camera.main;
    GameObject mapManager;
    // ---------------------------------------------------------------------------------
    
    // Events:::::::::::::::::
    public static Action load360Material;
    public static Action unload360Material;
    public static Action resetButtonColor;
    public static Action exit;
    //:::::::::::::::::::::::

    // Bool Checks :::::::::::::::::::::::::::::::::::::::::::::::::::::::
    bool homecheck;
    bool mapcheck;
    bool settingscheck;
    bool arCheck;
    bool[] mainButtonCheck = new bool[5];
    bool buttonNotifierCheck = false;
    bool cameFromMapView = false;

    bool previousCheck;
    bool nextCheck; 
    // :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    public StartState startState = new StartState();
    public static StartState staticState;
    public int indexbtn { get; private set; }
    public int totalImagesCounter { get; private set; }
    public int totalImages32Counter { get; private set; }


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

    private List<Button> seenButtons = new List<Button>();
    private List<Button> unseenButtons = new List<Button>();
    private int currentButtonIndex;
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();

    bool hmbtn = false;
    bool mapbtn = false;
    bool arbtn= false;
    bool setbtn= false;
    public override void Enter()
    {
        currentImageIndex = 0;
        currentButtonIndex = -1;
        indexbtn = 0;
        // Basic Init and cache Refrences
        root = stateMachine.root;
        mapManager = GameObject.Find("MapManager");
        GameObject.Find("GameManager").gameObject.GetComponent<StateTracker>().gameState = StateTracker.GameState.Routes;        
        routePage = root.Q("CulturalRoads"); // Main Info Root
        UIExtentions.Display(routePage, true);

        // Background Element
        backGround = routePage.Q<VisualElement>("BackGround"); // Main BackGround
        
        // Homebar Buttons
        home = routePage.Q<Button>("HomeButton");
        map = routePage.Q<Button>("MapButton");
        settings = routePage.Q<Button>("SettingsButton");
        augmentedReality = routePage.Q<Button>("ARButton");
        routes = routePage.Q<Button>("MonumentButton");
        
        // Main and First Card of Info Page
        culturalRoadsFirstCard = routePage.Q<VisualElement>("3_CulturalRoadsMain"); // StartPage with the 5 routes
        UIExtentions.Display(culturalRoadsFirstCard, true);
        backButton = culturalRoadsFirstCard.Q<Button>("BackButton");
        cardbuttons = culturalRoadsFirstCard.Query<Button>(className: "card").ToList();
        numberofPois = routePage.Query<Label>(className: "poicounter").ToList();
        routeRegionPage = routePage.Q<VisualElement>("3_1_RouteRegion"); // List and Map view Page
        UIExtentions.Display(routeRegionPage, false);
        _3backButton = routeRegionPage.Q<Button>("3BackButton");

        PointerCounterHolderLabel = routeRegionPage.Q<VisualElement>("PointerCounterHolderLabel");
        UIExtentions.Display(PointerCounterHolderLabel, false);
        pointC = PointerCounterHolderLabel.Q<Label>("PointTextC");
        pointS = PointerCounterHolderLabel.Q<Label>("PointTextS");
        
        //List View and Map View Basic Page
        startRoadTripElement = routeRegionPage.Q<VisualElement>("StartRoadTripPage");
        Fader = routePage.Q<VisualElement>("Fader");
        UIExtentions.Display(Fader, false);
        infoAndButtonArea = startRoadTripElement.Q<ScrollView>("InfoAndButtonArea");
        buttonSpawnPoint = routeRegionPage.Q<VisualElement>("ButtonSpawner");
        routeText = routeRegionPage.Q<Label>("TitleText");
        pathText = routeRegionPage.Q<Label>("IntroText");
        pointText = startRoadTripElement.Q<Label>("pointIndex");
        listView = routeRegionPage.Q<Button>("3_InfoIcon");
        UtilsHomeBar.ChangeButtonColor(listView, true);
        mapView = routeRegionPage.Q<Button>("3_MapIcon");
        start = startRoadTripElement.Q<Button>("3_Start");
        UIExtentions.Display(start, true);
        end = startRoadTripElement.Q<Button>("3_End");
        UIExtentions.Display(end, false);

        // PopUp Initialize
        popUpElement = startRoadTripElement.Q<VisualElement>("PopUp");
        UIExtentions.Display(popUpElement, false);
        exitPopUp = startRoadTripElement.Q<Button>("ExitPopUp");
        googleButton = startRoadTripElement.Q<Button>("GoogleButton");
        scrollView = startRoadTripElement.Q<ScrollView>("POIPhotoHolder");
        popUpTitle = startRoadTripElement.Q<Label>("PopUpTitle");
        popUpFilterID = startRoadTripElement.Q<Label>("PopUpFilterID");
        filterIDIconGrey = startRoadTripElement.Q<VisualElement>("FilterIDIconGrey");
        infoButton = startRoadTripElement.Q<Button>("InfoPageButton");
        PointerContainer = startRoadTripElement.Q<VisualElement>("MapPointHolder");
        // ................................

        // Info Routes ................................................
        infoRoutes = routePage.Q<VisualElement>("32Routes");
        UIExtentions.Display(infoRoutes, false);
        back = infoRoutes.Q<Button>("Back");
        goTo360 = infoRoutes.Q<Button>("GoTo360");
        goTo360.style.unityBackgroundImageTintColor = UIExtentions.grey;
        scrollView32 = infoRoutes.Q<ScrollView>("BlankSpace");
        titleList = infoRoutes.Query<Label>(className: "32Routes").ToList(); //////////
        filterIconGrey = infoRoutes.Q<VisualElement>("FilterIconGrey");
        filtertext = infoRoutes.Q<Label>("FilterText");
        routeNameText = infoRoutes.Q<Label>("RouteNameText");
        routeIndex = infoRoutes.Q<Label>("IndexPOI");
        routeCounter = infoRoutes.Q<Label>("CounterPOI");
        PointerTab = infoRoutes.Q<VisualElement>("PointerTab");
        address = infoRoutes.Q<Label>("Address");
        tel = infoRoutes.Q<Label>("Tel");
        email = infoRoutes.Q<Label>("mail");
        ticket = infoRoutes.Q<Label>("ticket");
        descPoint = infoRoutes.Q<Label>("descPoint");

        websiteButton = infoRoutes.Q<VisualElement>("MapRoute");
        UIExtentions.Display(websiteButton, true);
        
        googlemap = websiteButton.Q<Button>("_32MapButton");

        Telephone = infoRoutes.Q<VisualElement>("Telephone");
        UIExtentions.Display(Telephone, false);

        telephone = Telephone.Q<Button>("_32TelButton");
        
        Earth = infoRoutes.Q<VisualElement>("Earth");
        UIExtentions.Display(Earth, false);
        
        web = Earth.Q<Button>("_32EarthButton");
        
        Ticket = infoRoutes.Q<Button>("Ticket");
        UIExtentions.Display(Ticket, false);

        previous = infoRoutes.Q<Button>("PreviousButton");
        previous.style.color = UIExtentions.grey;
        next = infoRoutes.Q<Button>("NextButton");
        next.style.color = UIExtentions.BlueDark;
        GooglePin = infoRoutes.Q<Button>("32GoogleButton");
        audio = infoRoutes.Q<Button>("Audio");
        audioPause = infoRoutes.Q<Button>("Pause");

        // FullScreen
        fullScreen = routePage.Q<VisualElement>("PageFullScreen");
        UIExtentions.Display(fullScreen, false);
        fullScreenButton = infoRoutes.Q<Button>("FullScreen");
        fullScreenButtonReset = fullScreen.Q<Button>("FullScreenReset");
        fsBack = fullScreen.Q<Button>("FSback");
        fsNext = fullScreen.Q<Button>("FSnext");
        fsImage = fullScreen.Q<VisualElement>("FS_Image");
        // fsImage.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
        fsPoiIndex= fullScreen.Q<Label>("FSpoiIndex");
        fsRouteCounter= fullScreen.Q<Label>("FSrouteCounter");
        fsResetRegion = fullScreen.Q<VisualElement>("FS_ButtonHolder");
        fsNPRegion = fullScreen.Q<VisualElement>("FS_Minimazie");

        // 360 ...........................................................
        View360Page = routePage.Q<VisualElement>("360ViewPage");
        UIExtentions.Display(View360Page, false);
        back360 = View360Page.Q<Button>("360BackButton");
        Title360 = View360Page.Q<Label>("360TitleText");

        // Modal
        modal = root.Q<VisualElement>("Modal");
        UIExtentions.Display(modal, false);
        yes = modal.Q<Button>("YesButton");
        no = modal.Q<Button>("NoButton");
        exitModal = modal.Q<Button>("ExitModal");
        modalTitle = modal.Q<Label>("ModalTitle");
        modaldescription = modal.Q<Label>("ModalDesc");

        // ------------------------------- Event Subscriptions --------------------------------- \\

        // Swipe For FullScreen Events
        UtilsHomeBar.swipe += (int swipeDirection) => 
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };
     
        UtilsHomeBar.RestorePathPOI();
        // UtilsHomeBar.SetActiveToFalse(false);

        // Pop Up Element Events
       
        PointOfInterest.PopUpImages += (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            Handle32RoutesImages(numberOfSprites, sprites);
            
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
        PointOfInterest.PopUpEvent += PopUpDisplay;

        UtilsHomeBar.enableLandscapeAction += HandleLandscape;
        
        ButtonElement.buttonnotifier += ButtonToInfoBehaviour;
        ButtonElement.buttonPressedIndex += ButtonPressedFrom32Routes; 

        ObjectSelectionManager.AudioFinished += TogglePauseUnPauseAudioIconHandler;

        // Boolean Initialization
        buttonNotifierCheck = false;
        cameFromMapView = false;
        UtilsHomeBar.mapViewCheck = false;
        UtilsHomeBar.isInRoutes = true;
        UtilsHomeBar.ar = false;
        // Initialize Methods at the Start of the State
        IgnoreBackGroundForMapInteraction(false);

        OnStartInitialize();
        unseenButtons.Clear();
        seenButtons.Clear();
        startState.StartEndHandler(StartState.state.None);
        

        Debug.Log("Enter RoutePage");     
    }
    private void OnStartInitialize()
    {
        // First and Main Page
        HomeBarButtons(); // Home Bar Buttons   
        StartEndWalkHandler();     
        backButton. RegisterCallback<ClickEvent>(OnBackButtonClicked);
        FullScreenButtons();    
        Handle360();
        HandlePopUP(); // PopUp Initialization
        ListAndMapViewButtonHandler(); // Button behaviour of the Basic UI structure of map and list view elements  
        
        MainPathCardButtons(); // First Page Buttons
        // ------------------------
        MainPathCardButtonsPOITexts(); // Texts to be displayed at the path card Button. Displaying the sum of pois of each path    
        //----------------------------------------------------------------------------------------------------------------
        // Third Page of analytical info of each poi including 360 and FullScreen Behaviour. Also the change between them
        InfoPageBehaviourOfButtons();
    }   
    public override void Tick()
    {
        staticState = startState;
        if(UtilsHomeBar.ar == false)
        {
            PopUpUpdateTextAndIconFromSelectedPOI(); // Pop Up Element Updated Text and Icon
            UpdatePopUpElementVisibilityMode();

            UpdateInfoBasedOnSelectedPOI(); // 32 Routes info Page Updated text and icon        
            UpdateWebsiteButton();
            UpdateTelButton();
            UpdateTicketButton();
            // ChangePreNextButtonColor();
            
            UpdateFullScreenCounter();
            GeneralPreNextButtonBehaviours();
            
            Update360_UI();
            
            SwitcherOfStates();

            for (int i = 0; i < mainButtonCheck.Length; i++)
            {
                if (mainButtonCheck[i] == true)
                {
                    indexOfSumPOIS = JSONTest.Instance.globalPath[i].coordinates.Count();
                    SelectPathInitialization(i, indexOfSumPOIS.ToString());
                    mainButtonCheck[i] = false;
                    // UtilsHomeBar.SetActiveToFalse(false);
                }
            }
            pointC.text = seenButtons.Count().ToString();   
        }

    }   
    public override void Exit()
    {
        UnregisterButtons();

        startState.stateStart = StartState.state.None;
        // ------------------------------ Exit UnSubscription of Events --------------------------------------------- \\
        // Pop Up Events Un-Subscription
        PointOfInterest.PopUpEvent -= PopUpDisplay;

        PointOfInterest.PopUpImages -= (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            Handle32RoutesImages(numberOfSprites, sprites);
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
        // Swipe 
        UtilsHomeBar.swipe -= (int swipeDirection) =>
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };

        ButtonElement.buttonnotifier -= ButtonToInfoBehaviour;
        ButtonElement.buttonPressedIndex -= ButtonPressedFrom32Routes;

        UtilsHomeBar.swipe -= GetMovePointer;
        UtilsHomeBar.enableLandscapeAction -= HandleLandscape;
        ObjectSelectionManager.AudioFinished -= TogglePauseUnPauseAudioIconHandler;

        BackGroundToggle(UIExtentions.cyanBlue);

        HidePopUp();
        UIExtentions.Display(infoAndButtonArea, true);
        UtilsHomeBar.ReduceOpacity(listView, routeRegionPage, true, _width, _height, maximizer);
        UtilsHomeBar.ReduceOpacity(mapView, routeRegionPage, false, _width, _height, maximizer);
        UIExtentions.Display(routePage, false);
        UtilsHomeBar.DeselectPOI();

        UtilsCommander.UndoCommand();
        UtilsCommander.Clear();
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        UtilsCommander.UndoPointer32Command();
        UtilsCommander.ClearPointer32();

        UtilsHomeBar.RestorePathPOI();
        UtilsHomeBar.SetActiveToFalse(true);
        UIExtentions.Display(Fader, false);
        UtilsHomeBar.StopAudio();
        exit?.Invoke();


        AbstractPointerBehaviour.ResetCurrentIndex();

        // Boolean Reset
        UtilsHomeBar.mapViewCheck = false;
        UtilsHomeBar.fullScreenCheck = false;
        UtilsHomeBar.isIn360 = false;
        UtilsHomeBar.infoCheck32 = false;
        UtilsHomeBar.isInRoutes = false;

        hmbtn = false;
        setbtn = false;
        mapbtn = false;
        arbtn = false;
        homecheck = false;
        mapcheck = false;
        settingscheck = false;
        arCheck = false;
        buttonNotifierCheck = false;
        previousCheck = false;
        nextCheck = false;
        cardbuttons.Clear();
        unseenButtons.Clear();
        seenButtons.Clear();
        indexbtn = 0;
        currentButtonIndex = -1;

        foreach (bool item in mainButtonCheck)
        {
            item.Equals(false);
        }
        UtilsHomeBar.ToggleNumberAndSpriteAction(false);
        UIExtentions.Display(PointerCounterHolderLabel, false);
        Debug.Log("Exit RoutePage");



        unload360Material?.Invoke();      
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
        backButton.UnregisterCallback<ClickEvent>(OnBackButtonClicked);
        fsNext.UnregisterCallback<ClickEvent>(FullScreenButtonNextClicked);
        fsBack.UnregisterCallback<ClickEvent>(FullScreenButtonBackClicked);
        fullScreenButton.UnregisterCallback<ClickEvent>(FullScreenButtonClicked);
        fullScreenButtonReset.UnregisterCallback<ClickEvent>(FullScreenResetClicked);
        goTo360.UnregisterCallback<ClickEvent>(OnGoTo360Clicked);
        back360.UnregisterCallback<ClickEvent>(OnBackFrom360Clicked);
        exitPopUp.UnregisterCallback<ClickEvent>(ExitPopUpClicked);
        googleButton.UnregisterCallback<ClickEvent>(GooglePopUpClicked);
        infoButton.UnregisterCallback<ClickEvent>(InfoPopUpClicked);
        UnsubscribeAll();
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
    }

    #region BasicInitialize Methods
    
    #region HomeButtons
    /// <summary>
    /// Home bar buttons Method that trigger bool properties to true if its clicked
    /// </summary>
    void HomeBarButtons()
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
        if(startState.isStarted == true)
        {
            modalTitle.text = modalTitleText.GetTranslatedText();
            modaldescription.text = modalDescriptionText.GetTranslatedText();
            UIExtentions.Display(modal, true);
            mapbtn = true;
        }
        else if(startState.isStarted == false)
        {
            mapcheck = true;
        }
    }
    private void OnRoutesClicked(ClickEvent evt)
    {
        Debug.Log("AlreadyAtInfoState");
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
            
            routes.clicked += () => Debug.Log("Already At RoutePage");
            // stateMachine.HomeBarButtons.homeBarAR.clicked += () => { stateMachine.SwitchState(new ARState(stateMachine)); };
            
            if (mapcheck == true)
            {
                stateMachine.SwitchState(new MapState(stateMachine));
            }
            if (settingscheck == true)
            {
                stateMachine.SwitchState(new SettingsState(stateMachine));
            }
            if (arCheck == true)
            {
                stateMachine.SwitchState(new ARState(stateMachine));
                // var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
                UtilsHomeBar.isInRoutes = false;
                // arElem.enabled = true;
                arCheck = false;
            }
        }
        /// <summary>
        /// Main Path Buttons Behaviour. By which Button is clicked it triggers the propriate index to trigger
        /// the loading of each path
        /// </summary>
    #endregion HomeButtons

    #region MainButtons
    private void MainPathCardButtons()
    {
        for (int index = 0; index < cardbuttons.Count; index++)
        {
            Button button = cardbuttons[index];
            int currentIndex = index;
            pathIndex = index;
            string txt = JSONTest.Instance.globalPath[currentIndex].coordinates.Count().ToString();

            EventCallback<ClickEvent> callback = evt => OnMainButtonClicked(evt, currentIndex);
            
            // Register the callback
            button.RegisterCallback(callback);

            // Store the callback for later unsubscription
            buttonCallbacks[button] = callback;
        }
    }
    private void OnMainButtonClicked(ClickEvent evt, int currentIndex)
    {
        mainButtonCheck[currentIndex] = true;
        UtilsHomeBar.ChangeButtonColor(listView, true);
    }
    private void UnsubscribeAll()
    {
        foreach (var kvp in buttonCallbacks)
        {
            kvp.Key.UnregisterCallback(kvp.Value);
        }

        buttonCallbacks.Clear();
    }
    private void MainPathCardButtonsPOITexts()
    {
        for (int index = 0; index < numberofPois.Count; index++)
        {
            numberofPois[index].text = JSONTest.Instance.globalPath[index].coordinates.Count.ToString();
        }
    }
#endregion MainButtons
    
    #region ListAndMapViewButtons
    // Second Page Buttons Behaviours {Back, ListView and MapView}
    private void ListAndMapViewButtonHandler()
    {
        exitModal.RegisterCallback<ClickEvent>(OnExitModalClicked);
        no.RegisterCallback<ClickEvent>(OnNoClicked);
        yes.RegisterCallback<ClickEvent>(OnYesClicked);
        _3backButton.RegisterCallback<ClickEvent>(On_3BackButtonClicked);
        listView.RegisterCallback<ClickEvent>(OnListClicked);
        mapView.RegisterCallback<ClickEvent>(OnMapViewClicked);
    }
    private void ExitModal()
    {
        UIExtentions.Display(modal, false);
        hmbtn = false;
        setbtn = false;
        mapbtn = false;
        arbtn = false;
        if (googlemapenable == true)
            googlemapenable = false;
    }
    private void OnMapViewClicked(ClickEvent evt)
    {
        StartAndMapButtonBehaviour(false); 
        AbstractUpdateMapFromState.UpdateMapToSelectedRoute(pathIndex);
    }
    private void OnListClicked(ClickEvent evt)
    {
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
            UtilsCommander.UndoPointerCommand();
            UtilsCommander.ClearPointer();
            UtilsCommander.UndoPointer32Command();
            UtilsCommander.ClearPointer32();

            UtilsHomeBar.RestorePathPOI();
            UtilsHomeBar.DeselectPOI();
            BackGroundToggle(UIExtentions.cyanBlue);
            UIExtentions.Display(culturalRoadsFirstCard, true);
            UIExtentions.Display(routeRegionPage, false);
            UIExtentions.Display(infoAndButtonArea, true);
            UtilsHomeBar.ReduceOpacity(mapView, routeRegionPage, false, _width, _height, maximizer);
            IgnoreBackGroundForMapInteraction(ignored: false);
            UtilsHomeBar.mapViewCheck = false;
            UIExtentions.Display(start, true);
            UIExtentions.Display(end, false);
            HidePopUp();
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
                if(mapbtn == true)
                {
                    mapcheck = true;
                }
                if(arbtn == true)
                {
                    arCheck = true;
                }
                startState.StartEndHandler(StartState.state.None);
                UtilsHomeBar.DeselectPOI();
                UtilsHomeBar.RestorePathPOI();

                UtilsCommander.UndoCommand(); 
                UtilsCommander.Clear();      
                UtilsCommander.UndoPointerCommand();
                UtilsCommander.ClearPointer();
                UtilsCommander.UndoPointer32Command();
                UtilsCommander.ClearPointer32();
                    
                BackGroundToggle(UIExtentions.cyanBlue);
                UIExtentions.Display(culturalRoadsFirstCard, true);
                UIExtentions.Display(routeRegionPage, false);
                UIExtentions.Display(infoAndButtonArea, true);
                UtilsHomeBar.ReduceOpacity(mapView, routeRegionPage, false, _width, _height, maximizer);
                IgnoreBackGroundForMapInteraction(ignored: false);
                UtilsHomeBar.mapViewCheck = false;
                UIExtentions.Display(start, true);
                UIExtentions.Display(end, false);
                HidePopUp();
                
                UIExtentions.Display(PointerCounterHolderLabel, false);
                UtilsHomeBar.ToggleNumberAndSpriteAction(false); 
                UIExtentions.Display(modal, false);
            
        }
        // else if (googlemapenable == true)
        // {
        //     GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
        //     googlemapenable = false;
        //     UIExtentions.Display(modal, false);  
        // }
    }
    private void OnNoClicked(ClickEvent evt)
    {
        ExitModal();
    }
    private void OnExitModalClicked(ClickEvent evt)
    {
        ExitModal();
    }
    #endregion ListAndMapViewButtons
    
    //Start and End Button Behaviours by onclick events 
    #region StartEnd
    private void StartEndWalkHandler()
    {
        start.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        end.RegisterCallback<ClickEvent>(OnEndButtonClicked);
    }
    private void OnStartButtonClicked(ClickEvent evt)
    {
        // if (buttonNotifierCheck == true)
            // {
            // StartAndMapButtonBehaviour();
            UIExtentions.Display(start, false);
            UIExtentions.Display(end, true);

            startState.StartEndHandler(StartState.state.Start);

            var buttons = routePage.Query<Button>(className: "CardButton").ToList();
            unseenButtons = new List<Button>(buttons);

            if (unseenButtons.Count > 0)
            {
                MakeButtonBlue(0); // Make the first button in the unseen list blue
            }

            UtilsHomeBar.ToggleNumberAndSpriteAction(true);
            UtilsHomeBar.SelectPOIFromStart(0);
            HandleStartButtonOpacity();

            UIExtentions.Display(PointerCounterHolderLabel, true);
        // Handle the reset button click
    }
    private void OnEndButtonClicked(ClickEvent evt)
    {
        UIExtentions.Display(start, true);
        UIExtentions.Display(end, false);
        startState.StartEndHandler(StartState.state.End);
        var buttons = routePage.Query<Button>(className:"CardButton").ToList();

        HidePopUp();
        
        foreach (var item in buttons)
        {
            item.style.backgroundImage = new StyleBackground(UtilsHomeBar.greyButton);
            item.style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
            item.Q<Label>(className: "buttonFilterText").style.color = UIExtentions.grey;
            item.Q<Label>(className: "buttonTitle").style.color = UIExtentions.grey;
            item.Q<VisualElement>(className: "buttonImage").style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
            item.Q<VisualElement>(className: "buttonIcon").style.unityBackgroundImageTintColor = UIExtentions.grey05;

        }
        unseenButtons.Clear();
        seenButtons.Clear();

        UtilsHomeBar.ToggleNumberAndSpriteAction(false);
        UtilsHomeBar.DeselectPOI();
        UtilsHomeBar.swipe -= GetMovePointer;

        previous.style.color = UIExtentions.grey;
        previous.style.opacity = 0.5f;
        next.style.color = UIExtentions.BlueDark;

        UIExtentions.Display(PointerCounterHolderLabel, false);
        // ButtonElement.resetcolorevent?.Invoke();
    }
    
    private void OnBackButtonClicked(ClickEvent evt)
    {
        homecheck = true;
        UtilsHomeBar.DeselectPOI(); 
        UtilsCommander.UndoCommand();
                // UtilsCommander.UndoCommand(); 
        UtilsCommander.Clear();      
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        UtilsCommander.UndoPointer32Command();
        UtilsCommander.ClearPointer32();
        UtilsHomeBar.RestorePathPOI();
    }
    private void StartAndMapButtonBehaviour(bool enabling) 
    {
        // Color To Transparency Alpha = 0
        BackGroundToggle(Color.clear);
        // the Element that holds the POI buttons
        UIExtentions.Display(infoAndButtonArea, enabling);
        UIExtentions.Display(Fader, enabling);
        // Button Map View Visual Functionality
        UtilsHomeBar.ReduceOpacity(mapView, routeRegionPage, !enabling, _width, _height, maximizer);
        UtilsHomeBar.ReduceOpacity(listView, routeRegionPage, enabling, _width, _height, maximizer);
        UtilsHomeBar.mapViewCheck = !enabling;
        IgnoreBackGroundForMapInteraction(!enabling);
    } 
    private void HandleStartButtonOpacity()
    {
        buttonNotifierCheck = true;
        start.style.backgroundColor = UIExtentions.BlueDark;
    }

#endregion StartEnd

#endregion BasicInitialize Methods

    #region _360Images
    private void Update360_UI()
    {
        Update360ButtonOpacity();
        Update360Title();
    }
    /// <summary>
    /// Method that Handles the Transition to 360 page from Info Page and backwards
    /// </summary>
    private void Handle360()
    {
        goTo360.RegisterCallback<ClickEvent>(OnGoTo360Clicked);
        back360.RegisterCallback<ClickEvent>(OnBackFrom360Clicked);
    }

    private void OnBackFrom360Clicked(ClickEvent evt)
    {
        unload360Material?.Invoke();
        var homebar = routePage.Q<VisualElement>("HomeBar");
        UIExtentions.Display(homebar, true);
        TransitionFromTo360(true,UIExtentions.cyanBlue, 175f, 90f);
        UtilsHomeBar.isIn360 = false;
    }

    private void OnGoTo360Clicked(ClickEvent evt)
    {
        UtilsHomeBar.isIn360 = true;
        load360Material?.Invoke();
        var homebar = routePage.Q<VisualElement>("HomeBar");
        UIExtentions.Display(homebar, false);
        TransitionFromTo360(false,UIExtentions.whiteAlpha00, 0, 0);
    }

    private void TransitionFromTo360(bool enabling, Color color, float yPos, float xQuater)
    {
        UIExtentions.Display(View360Page, !enabling);
        // UIExtentions.Display(routeRegionPage, !enabling);
        UIExtentions.Display(infoRoutes, enabling);
        // UIExtentions.Display(Fader, enabling);
        UtilsHomeBar.infoCheck32 = enabling;
        backGround.style.backgroundColor = color;
        camera.transform.position = new Vector3(0, yPos, 0);
        camera.transform.rotation = Quaternion.Euler(xQuater, 0, 0);
        GameObject.Find("Camera").GetComponent<GyroscopeTest>().enabled = !enabling;
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

    private void HandleFullScreenImages(int images, List<Sprite> sprites)
    {
        fullScreenSprites.Clear();
        int numSprites = Mathf.Min(images, sprites.Count);
        for (int i = 0; i < numSprites; i++)
        {
            // fullScreenSprites = sprites;
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

    #region InfoPage Main Route Behaviour
    private void SelectPathInitialization(int currentIndex , string txt)
    {
        // button.style.unityBackgroundImageTintColor = UIExtentions.BlueDark;
        UtilsCommander.ExecuteCommand(currentIndex, buttonSpawnPoint);

        // VisualElement lastChild = infoAndButtonArea.ElementAt(infoAndButtonArea.childCount - 1);
        // lastChild.style.opacity = 0f;

        UtilsHomeBar.SendPathInt(currentIndex);
        UIExtentions.Display(culturalRoadsFirstCard, false);
        // UtilsHomeBar.ReduceOpacity(listView, routeRegionPage, true, _width, _height, maximizer);
        UIExtentions.Display(routeRegionPage, true);

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
            ? JSONTest.Instance.globalRoutes.englishTextsRoute[currentIndex].Titles[Row].ToString()
            : JSONTest.Instance.globalRoutes.greekTextsRoute[currentIndex].Titles[Row].ToString()
        );

        routeCounter.text = txt;
        pointText.text = txt;
            
            
            
        pointS.text = routeCounter.text;
    }

#endregion InfoPage Main Route Behaviour

    #region 32Routes Info
    private void InfoPageBehaviourOfButtons()
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
    private void OnBackInfoButtonClicked(ClickEvent evt)
    {
        UtilsHomeBar.StopAudio();
        TogglePauseUnPauseAudioIconHandler(true, false);

        if (cameFromMapView)
        {
            BackGroundToggle(Color.clear);
            UIExtentions.Display(routeRegionPage, true);
            UIExtentions.Display(infoRoutes, false);
            UtilsHomeBar.mapViewCheck = true;
            ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
            ButtonToInfoBehaviour(false, false, true);
        }
        else
        {
            UIExtentions.Display(infoRoutes, true);
            ButtonToInfoBehaviour(false, false, false);
            BackGroundToggle(UIExtentions.cyanBlue);
        }
        
        if(startState.stateStart == StartState.state.Start)
        {
            resetButtonColor?.Invoke();
        }

        UtilsCommander.UndoPointerCommand();
        
        UtilsHomeBar.DeselectPOI();
                 
        HidePopUp();

        currentImageIndex = 0;
    }
    private void OnNextButtonClicked(ClickEvent evt)
    {
        nextCheck = true;
    }
    private void OnPreviousButtonClicked(ClickEvent evt)
    {
        previousCheck = true;
    }
    private void OnAudioButtonClicked(ClickEvent evt)
    {
        UtilsHomeBar.PlayAudio();
        currentAudioState = AudioState.Playing;
        TogglePauseUnPauseAudioIconHandler(false, true);
    }
    private void OnAudioPauseButtonClicked(ClickEvent evt)
    {
        switch (currentAudioState)
        {
            case AudioState.Playing:
                UtilsHomeBar.PauseAudio();
                currentAudioState = AudioState.Paused;
                audioPause.style.backgroundImage = new StyleBackground(UtilsHomeBar.resumeSpriteStatic);
                Debug.Log("PAUSE BUTTON");
                break;
            case AudioState.Paused:
                UtilsHomeBar.ResumeAudio();
                currentAudioState = AudioState.Playing;
                audioPause.style.backgroundImage = new StyleBackground(UtilsHomeBar.pauseSpriteStatic);
                Debug.Log("UNPAUSE BUTTON");
                break;
        }
    }
    private void OnGooglePinButtonClicked(ClickEvent evt)
    {
        HidePopUp();
        UIExtentions.Display(routeRegionPage, true);
        UIExtentions.Display(infoRoutes, false);
        UtilsHomeBar.infoCheck32 = false;
        UtilsHomeBar.mapViewCheck = true;
        StartAndMapButtonBehaviour(false);
        resetButtonColor?.Invoke();
        currentImageIndex = 0;
    }
    private void OnWebButtonClicked(ClickEvent evt)
    {
        if (ObjectSelectionManager.hasWebSite)
        {
            Application.OpenURL(ObjectSelectionManager.OpenEntrance);
        }
    }
    private void OnTelephoneButtonClicked(ClickEvent evt)
    {
        if (ObjectSelectionManager.hasTel)
        {
            Application.OpenURL("tel:" + ObjectSelectionManager.Contact);
        }
    }
    private void OnGoogleMapButtonClicked(ClickEvent evt)
    {
        // UIExtentions.Display(modal, true);
        // googlemapenable = true;
        // modalTitle.text = modalTitleGoogleText.GetTranslatedText();
        // modaldescription.text = modalDescriptionGoogleText.GetTranslatedText();
        GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
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
        scrollView32.Clear();
        // UtilsHomeBar.swipe -= GetMovePointer;
        
        totalImages32Counter = images;
            fullScreenSprites = sprites;
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
            visualElement.style.borderBottomWidth = 5;
            visualElement.style.borderRightWidth = 5;
            visualElement.style.borderBottomLeftRadius = 30;
            visualElement.style.borderBottomRightRadius = 30;

            visualElement.style.backgroundImage = new StyleBackground(sprites[i]);
            scrollView32.Add(visualElement);
        }    
        UtilsCommander.ExecutePointer32Command(sprites.Count, PointerTab);
                AbstractPointerBehaviour.ResetCurrentIndex();
        // UtilsHomeBar.swipe += GetMovePointer;
    }
    private void UpdateInfoBasedOnSelectedPOI()
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
    private void ButtonToInfoBehaviour(bool pressed, bool elements, bool map)
    { 
        UIExtentions.Display(infoRoutes, pressed);
        UIExtentions.Display(Fader, !pressed);
        UIExtentions.Display(routeRegionPage,!pressed);
        UtilsHomeBar.infoCheck32 = pressed;
        UIExtentions.Display(previous, !elements);
        UIExtentions.Display(next, !elements);
        // UIExtentions.Display(pathText, !elements);
        UIExtentions.Display(routeCounter, !elements);
        UIExtentions.Display(routeIndex, !elements);
        UIExtentions.Display(routeNameText, !elements);    
        UIExtentions.Display(infoRoutes.Q<Label>("From"), !elements);
        UIExtentions.Display(infoRoutes.Q<VisualElement>("PrevNextHolder"), !elements);
        cameFromMapView = map;
    }    
    private void ButtonPressedFrom32Routes(int objIndex)
    {
        indexbtn = objIndex;
        
        if(startState.isStarted == true)
        {
        
            var buttons = routePage.Query<Button>(className:"CardButton").ToList();
            // Reset all buttons to default state first
            // foreach (var button in buttons)
            // {
            //     ResetButton(button);
            // }

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
            next.style.opacity = 1f;
            previous.style.opacity = 1f;
        }
    }
#endregion 32 Routes Info

    #region PopUPWindow
    /// <summary>
    /// Handles the events of a pop-up, such as exit, Google button, and info button.
    /// </summary>
    private void HandlePopUP()
    {
        exitPopUp.RegisterCallback<ClickEvent>(ExitPopUpClicked);
        googleButton.RegisterCallback<ClickEvent>(GooglePopUpClicked);
        infoButton.RegisterCallback<ClickEvent>(InfoPopUpClicked);
    }

    private void ExitPopUpClicked(ClickEvent evt)
    {
        UtilsCommander.UndoPointerCommand();
        HidePopUp();
        UtilsHomeBar.DeselectPOI();
        // UtilsHomeBar.swipe -= GetMovePointer;
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
        startRoadTripElement.style.backgroundColor = UIExtentions.cyanBlue;
        backGround.style.backgroundColor = UIExtentions.cyanBlue;
        UIExtentions.Display(infoRoutes, true);
        UIExtentions.Display(routeRegionPage, false);
        
        HidePopUp();
        UtilsHomeBar.mapViewCheck = false;
        // cameFromMapView = true;
        resetButtonColor?.Invoke();

        if(startState.stateStart == StartState.state.Start)
        {
            ButtonToInfoBehaviour(true, false, true);
            ButtonPressedFrom32Routes(ObjectSelectionManager.indexOfSelected);
        }
        else if(startState.stateStart == StartState.state.End || startState.stateStart == StartState.state.None)
        {
            ButtonToInfoBehaviour(true, true, true);
        }
        UtilsHomeBar.swipe += GetMovePointer;
    }

    /// <summary>
    /// Handles the display of images in a popup, adding VisualElements to a ScrollView.
    /// </summary>
    /// <param name="images">The number of images to display.</param>
    /// <param name="sprites">The list of Sprite images to be shown.</param>
    private void HandlePopUpImages(int images, List<Sprite> sprites)
    {
        scrollView.Clear();
        // scrollView32.Clear();
        totalImagesCounter = images;
        int numSprites = Mathf.Min(images, sprites.Count);
        UtilsCommander.UndoPointerCommand();
        for (int i = 0; i < numSprites; i++)
        {
            VisualElement visualElement = new VisualElement();
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
    /// <summary>
    /// This method Handles the first action for the Pop up Element to be visible and
    /// Retargets to the original Transform
    /// Also Notifiy every listener from the boolean check
    /// </summary>
    private void PopUpDisplay()
    {
        UtilsHomeBar.swipe -= GetMovePointer;
        UIExtentions.Display(popUpElement, true); 
        popUpElement.style.translate = new StyleTranslate(popUpVisibleTransfrom); 
        UtilsHomeBar.popUpEnabled = true;
        Physics.IgnoreLayerCollision(UIExtentions.mask, 0);
        
        AbstractPointerBehaviour.ResetCurrentIndex();
        UtilsHomeBar.swipe += GetMovePointer;
    }
    /// <summary>
    /// This method Handles the first action for the Pop up Element to be Hidden and
    /// Retargets to the Transform that it is not visible in the Game View in a specified amount of miliseconds
    /// Also Notifiy every listener from the boolean check for that action
    /// </summary>
    private async void HidePopUp()
    {
        // Handles the position when the popUpElement is no longer needed
        popUpElement.style.translate = new StyleTranslate(popUpUnseenTransform);
        //0.5 seconds delay for smooth transistion before become non-visible
        await Task.Delay(500);
        // Sets the PopUP Visual Element to non-Visible
        UIExtentions.Display(popUpElement, false);
        //Sets the the boolean check to false when the popup element is no longer needed
        UtilsHomeBar.popUpEnabled = false;
        UtilsCommander.UndoPointerCommand();
        UtilsCommander.ClearPointer();
        AbstractPointerBehaviour.ResetCurrentIndex();
        UtilsHomeBar.swipe -= GetMovePointer;

        scrollView.Clear();
        PointOfInterest.PopUpImages -= (int numberOfSprites, List<Sprite> sprites) =>
        {
            HandlePopUpImages(numberOfSprites, sprites);
            // Handle32RoutesImages(numberOfSprites, sprites);
            // HandleFullScreenImages(numberOfSprites, sprites);
        };
    }
    /// <summary>
    /// Updates The Pop Up Title, filter name and Icon
    /// </summary>
    private void PopUpUpdateTextAndIconFromSelectedPOI()
    {
        popUpTitle.text = ObjectSelectionManager.textGlobal;
        popUpFilterID.text = ObjectSelectionManager.filterIDText;
        filterIDIconGrey.style.backgroundImage = new StyleBackground(ObjectSelectionManager.spriteIcon);
    }
    /// <summary>
    /// handles The Visibility Mode of the PopUp based on boolean mapViewCheck 
    /// </summary>
    private void UpdatePopUpElementVisibilityMode()
    {
        if (UtilsHomeBar.mapViewCheck == false)
        {
            popUpElement.style.visibility = Visibility.Hidden;
        }
        else
        {
            popUpElement.style.visibility = Visibility.Visible;
        }
    }
#endregion PopUPWindow

#region GeneralFunctionality 
    void GetMovePointer(int index)
    {
        
        if(GameObject.Find("GameManager").GetComponent<StateTracker>().gameState == StateTracker.GameState.Routes && UtilsHomeBar.isInRoutes)
        {
            if(UtilsHomeBar.popUpEnabled && UtilsHomeBar.mapViewCheck) 
            {
                AbstractPointerBehaviour.MovePointer(PointerContainer, index, scrollView);   
            } 
            if(UtilsHomeBar.mapViewCheck == false && UtilsHomeBar.infoCheck32 == true)
            {
                AbstractPointerBehaviour.MovePointer(PointerTab, index, scrollView32);
            }
            else 
            {
    Debug.Log
    ($"Either popUP or mapView orboth were not TRUE. Pop up is {UtilsHomeBar.popUpEnabled} and mapView is {UtilsHomeBar.mapViewCheck}");
            }   
        }
  
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
    /// <summary>
    /// Handles the Picking Mode and Usage Hints. Prevents the touch to surpass the visual element and 
    /// comes to collision with POIs in the map
    /// </summary>
    /// <param name="ignored"></param>
    private void IgnoreBackGroundForMapInteraction(bool ignored)
    {
        List<VisualElement> visualElements = new List<VisualElement>
        {
            backGround,
            infoAndButtonArea,
            startRoadTripElement,
            culturalRoadsFirstCard
        };

        foreach (VisualElement item in visualElements)
        {
            UIExtentions.Ignore(item, ignored);
            UIExtentions.MaskIgnore(item, ignored);
        }
    }
    /// <summary>
    /// Toggles The background Color and Canvas Color From mapView to 32RoutesInfo
    /// </summary>
    /// <param name="color"></param>
    void BackGroundToggle(Color color)
    {
        startRoadTripElement.style.backgroundColor = color;
        backGround.style.backgroundColor = color;
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
    void ResetButton(Button item)
    {
        item.style.backgroundImage = new StyleBackground(UtilsHomeBar.greyButton);
        item.style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
        item.Q<Label>(className: "buttonFilterText").style.color = UIExtentions.grey;
        item.Q<Label>(className: "buttonTitle").style.color = UIExtentions.grey;
        item.Q<VisualElement>(className: "buttonImage").style.unityBackgroundImageTintColor = UIExtentions.whiteAlpha01;
        item.Q<VisualElement>(className: "buttonIcon").style.unityBackgroundImageTintColor = UIExtentions.grey05;
    }
#endregion GeneralFunctionality
}
