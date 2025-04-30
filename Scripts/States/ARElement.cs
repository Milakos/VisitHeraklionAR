using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARLocation;
using PointsOfInterests;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARElement : MonoBehaviour
{
    public enum AudioState
    {
        Playing,
        Paused
    }
    public ARBoundHandler aRBoundHandler;
    private AudioState currentAudioState;
    public StateTracker state;
    public StateTracker.GameState gameState = new StateTracker.GameState();
    public StateTracker.GameState routesState = new StateTracker.GameState();
    public StateTracker.GameState mapState = new StateTracker.GameState();
    public SignArrow signArrow;
    public ImageTracker imageTracker;
    ARTrackedImageManager trackedImageManager;
    ARPlaneManager planeManager;
    ARRaycastManager raycastManager;
    List<ARPlane> trackedPlanes = new List<ARPlane>();
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    private bool planeDetected = false;
    bool planeInView = false;
    private bool completed = true;
    Vector3 placePos;
    Quaternion placeRot;

    public GameObject tap;
    public GameObject track;
    public GameObject imageTrack;

    public Light lightD;
    [SerializeField] Vector3 lightARrotation;
    [SerializeField] Vector3 lightNonArRotation;

    [SerializeField] private Sprite bluePointBar;
    [SerializeField] private Sprite point;
    public List<Sprite> Image_1 = new List<Sprite>();
    

    // UI Document ---- UI Toolkit Properties
    // Root
    UIDocument uIDocument;
    VisualElement root;
    VisualElement _ARPage;
    VisualElement ARPage;
    VisualElement ARapp;
    Button backAR;
    // Tutorial
    VisualElement Tutorial;
    Label tutorialTitle;
    VisualElement descriptionContainer;
    List<Label> description;
    Label tutorialDescriptionF;
    Label tutorialDescriptionS;
    VisualElement pointContainer ;
    Button ARNext;
    Label buttonName;
    VisualElement aRTutorialImage;

    // Core Pages
    VisualElement home;
    VisualElement settings;
    VisualElement map;
    VisualElement infoRoutes;
    public VisualElement blueHead;
    public VisualElement blueBottom;
    public VisualElement infoHolder;

    // 32 Routes Page
    #region InfoRoutesAndPOIProperties
    VisualElement _32Routes;
    Button _32Back;
    ScrollView scrollView32;
    // 3.2 Routes
    Button goTo360;
    VisualElement filterIconGrey;
    Label filtertext;
    List<Label> titleList = new List<Label>();
    public VisualElement PointerTab { get; private set; }
    public int currentImageIndex { get; private set; }
    public Action<bool> EnableFilters;

    Label address;
    Label tel;
    Label email;
    Label ticket;
    Label descPoint;

    Label messageAR;
    public string globalMessageString = " ";
    
    Button websiteButton;
    Button _mapButton;
    Button Telephone;
    VisualElement Ticket;

    VisualElement fullScreen;
    Button audioPlay;
    Button audioPause;

    // FULLSCREEN
    VisualElement fsRegionUp;
    VisualElement fsRegionDown;

    Button fullScreenButton;
    Button fullScreenButtonReset;
    Button fsNext;
    Button fsBack;
    VisualElement fsImage;
    Label fsPoiIndex;
    Label fsRouteCounter;
    
    // 360
    VisualElement View360Page;
    Button back360;
    Label Title360;
#endregion InfoRoutesAndPOIProperties
    
    // Filters
    VisualElement filterHolder;
    ScrollView FilterScrollView;
    Button squareButton;
    Button museumButton;
    Button monumentButton;
    Button templeButton;
    Button playGroundButton;
    Button recreationGroundButton;
    Button shoppingStreetButton;
    Button culturalFacilityButton;
    List<Sprite> fullScreenSprites = new List<Sprite>();
    
    // Models of Ariadne and Dominic 
    [SerializeField] AssetReferenceGameObject DominicRef;
    [SerializeField] AssetReferenceGameObject AriadneRef;
    public GameObject Dominic;
    public GameObject Ariadne;
    [HideInInspector] public GameObject Instance;
    [SerializeField]public  List<GameObject> selectedObjectInAR = new List<GameObject>();

    // Cameras
    public GameObject ARCamera;
    public GameObject Prefab;
    public GameObject cam;
    
    public int pageIndex = 0;
    private int maxIndex;
    public float DistanceFromCamera = 3.0f;

    //Boolean Checks
    [HideInInspector] public bool firstInitialized = false;
    //Actions 
    private Dictionary<Button, ARButton.FilterID> buttonFilterMap = new Dictionary<Button, ARButton.FilterID>();
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();
    List<VisualElement> pointers = new List<VisualElement>();

    public GameObject go;
    public MaterialManager materialManager;

    Toggle toggle;
    VisualElement checkmark;
    Label filtersLabel;

    public Action<bool> whenIn360;
    public Action load360Material;
    public Action unload360Material;
    bool isActivated;
    [HideInInspector] public bool initialized = false;
    public bool ARProviderInitialized = false;
    // ARLocationProvider aRLocationProvider;
    private void Awake() 
    {
        Instantiate(Prefab);
        // OnModelsLoaded();
        imageTracker = FindObjectOfType<ImageTracker>();
        ARCamera = GameObject.Find("AR Camera"); 
        ARCamera.SetActive(false);
        // pageIndex = 0;
        
        if(ARBoundHandler.Instance != null)
        {
            ARBoundHandler.Instance.OnHotspotActivated += EnableInfoPopUp;
            Debug.LogWarning("A: ARBoundHandler Exists");
        }
        else
        {
            Debug.LogError("ARBoundHandler doesnt exists");
        }
        PointOfInterest.PopUpImagesAR += (int numberOfSprites, List<Sprite> sprites) =>
        {
            Handle32RoutesImages(numberOfSprites, sprites);        
            HandleFullScreenImages(numberOfSprites, sprites);
        };
        UtilsHomeBar.swipeAR += (int swipeDirection) => 
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };
    }
    private void OnEnable()
    {     
        

        tap = GameObject.Find("Tap");
        tap.SetActive(false);
        track = GameObject.Find("Track");   
        track.SetActive(false);
        imageTrack = GameObject.Find("ImageTrackAnim");   
        imageTrack.SetActive(false);

        uIDocument = GetComponent<UIDocument>();

        imageTracker = FindObjectOfType<ImageTracker>();
        imageTracker.enabled = true;
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
        state = FindObjectOfType<StateTracker>();
        trackedImageManager.enabled = true;
        
        if(planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();  
              
        }  
        if(raycastManager == null)
        {
            raycastManager = FindObjectOfType<ARRaycastManager>();
        }
        // planeManager.planesChanged += OnPlanesChanged;  

        gameState = state.gameState;
        mapState = StateTracker.GameState.Map;
        routesState = StateTracker.GameState.Routes;

        root = uIDocument.rootVisualElement;

        home = root.Q<VisualElement>("HomePage");
        infoRoutes = root.Q<VisualElement>("CulturalRoads");
        map = root.Q<VisualElement>("MapPage");
        settings = root.Q<VisualElement>("SettingsPage");
        
        blueHead = root.Q<VisualElement>("BlueHead");
        // UIExtentions.Display(blueHead, true);
        blueBottom = root.Q<VisualElement>("BlueDown");
        // UIExtentions.Display(blueBottom, true);

        ARPage = root.Q<VisualElement>("ARPage");
        UIExtentions.Display(ARPage, true);
        ARapp = ARPage.Q<VisualElement>("ARapp");
        UIExtentions.Display(ARapp, true);  

        _ARPage = ARapp.Q<VisualElement>("_ARPage");
        _32Routes = ARapp.Q<VisualElement>("32Routes");

        backAR = _ARPage.Q<Button>("backAR");
        _32Back = _32Routes.Q<Button>("Back");


        
        Routes32();
        AudioBehaviour();
        Assign360();
        FullScreen();
        Handle360();

        messageAR = _ARPage.Q<Label>("MessageAR");
        infoHolder = _ARPage.Q<VisualElement>("InfoHolder");
        UIExtentions.Display(infoHolder, false);

        // TUTORIAL INIT //

        Tutorial = _ARPage.Q<VisualElement>("Tutorial");
        UIExtentions.Display(Tutorial, false);
        tutorialTitle = _ARPage.Q<Label>("ArTitleTutorial");
        tutorialDescriptionF = _ARPage.Q<Label>("ARDescriptionF");
        tutorialDescriptionS = _ARPage.Q<Label>("ARDescriptionS");
        ARNext = _ARPage.Q<Button>("NextButtonAR");
        buttonName = ARNext.Q<Label>("ButtonName");
        
        pointContainer = _ARPage.Q("PointIndexElement");
        descriptionContainer = _ARPage.Q("Description");
        description = descriptionContainer.Query<Label>(className: "descriptions").ToList();
        pointers = root.Query<VisualElement>(className: "PointsAR").ToList();
        aRTutorialImage = _ARPage.Q<VisualElement>("ARTutorialImage");

        // Filter Buttons
        monumentButton = _ARPage.Q<Button>("MonumentBtn");
        museumButton = _ARPage.Q<Button>("MuseumBtn");
        squareButton = _ARPage.Q<Button>("SquareBtn");
        templeButton = _ARPage.Q<Button>("TempleBtn");
        playGroundButton = _ARPage.Q<Button>("Playground");
        recreationGroundButton = _ARPage.Q<Button>("RecreationGround");
        shoppingStreetButton = _ARPage.Q<Button>("ShoppingStreet");
        culturalFacilityButton = _ARPage.Q<Button>("CulturalFacility");
        FilterScrollView = _ARPage.Q<ScrollView>("FilterScrollView");
        UIExtentions.Display(FilterScrollView, true);
        
        filterHolder = _ARPage.Q<VisualElement>("FilterHolder");

        toggle = filterHolder.Q<Toggle>();
        checkmark = toggle.Q<VisualElement>("unity-checkmark");
        filtersLabel = toggle.Q<Label>();
        // Subscribe to the toggle's change event
        toggle.RegisterValueChangedCallback(evt => OnToggleChanged(evt.newValue));

        UIExtentions.Display(filterHolder, false);     
        // Pop UP //
        _32Back.RegisterCallback<ClickEvent>(On_32BackClicked);

        UIExtentions.Display(_ARPage, true);
        UIExtentions.Display(tutorialDescriptionF, true);
        UIExtentions.Display(tutorialDescriptionS, false);
        UIExtentions.Display(_32Routes, false);

        UIExtentions.Display(Tutorial, true);
        buttonName.text = UIExtentions.IsEnglish() ? " OK " : " OK ";
        ARNext.RegisterCallback<ClickEvent>(OnARNextClicked);

        UpdateTutorial();  

        FiltersButtonInitialize();
        backAR.RegisterCallback<ClickEvent>(OnBackClicked);

        ToggleState_AR(false);
        ToggleCamera(true);
        
        UtilsHomeBar.ar = true;
        UtilsHomeBar.isInMap = false;
        UtilsHomeBar.isInRoutes = false;

        imageTracker.EnableFilters += EnableARInfoPointFilters;   
        
        // aRBoundHandler.OnHotspotActivated += EnableInfoPopUp;


        UtilsHomeBar.enableLandscapeAction += HandleLandscape;
        ObjectSelectionManager.AudioFinished += TogglePauseUnPauseAudioIconHandler;
        UtilsHomeBar.toggleFiltersOpacity += ToggleFiltersOpacity;

        if(go != null)
        {
            if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState)
            {
                go.SetActive(false);     
            }
            else
            {
                go.SetActive(true);
            }
        }
        else
        {
            Debug.Log("go is null");
        }

        if(ARState.info == true)
        {
            planeManager.enabled = false;
            trackedPlanes.Clear();
        }
    }
    private void Start() 
    {
        signArrow.enabled = true; 
        FindObjectOfType<WebMapLoader>().enabled = true;
    }
    private void OnBackClicked(ClickEvent evt)
    {
        if(imageTracker.go != null)
        {
            StartCoroutine(DestroyInfo());
            Debug.Log("ImageTracker Disabled");
        }
        else
        {
            imageTracker.enabled = false;
            Debug.Log("Do Nothing");
        }

        ResetFilterButtons();
        blueHead.style.opacity = new StyleFloat(100f);
        blueBottom.style.opacity = new StyleFloat(100f);
        UIExtentions.Display(ARPage, false);
        ToggleCamera(false);  
        UtilsHomeBar.StopAudio();
        UtilsHomeBar.DeselectPOI();
        VisualElement arChoose = root.Q<VisualElement>("ARChoose");
        UIExtentions.Display(arChoose, true);

        ARState.info = false;
        
        currentImageIndex = 0;
        if(go.activeSelf && go != null)
        {go.SetActive(false);}
        ARBoundHandler.Instance.DisableARVisibleObjects();
        this.enabled = false;
    }
    IEnumerator DestroyInfo()
    {
        imageTracker.DestoryImageObject();
        yield return new WaitForSeconds(0.5f);
        imageTracker.enabled = false;
    }
    private void On_32BackClicked(ClickEvent evt)
    {
        InfoPageDisabled(); 
    }
    //Disabling
    private void OnDestroy() 
    {
        ARBoundHandler.Instance.OnHotspotActivated -= EnableInfoPopUp;
        PointOfInterest.PopUpImagesAR -= (int numberOfSprites, List<Sprite> sprites) =>
        {
            Handle32RoutesImages(numberOfSprites, sprites);        
            HandleFullScreenImages(numberOfSprites, sprites);
        };
        UtilsHomeBar.swipeAR -= (int swipeDirection) => 
        {
            HandleFSImagesSwipeL(swipeDirection);
            HandleFSImagesSwipeR(swipeDirection);
        };
    }
    private void OnDisable() 
    {
        UnregisterButtons();
        buttonFilterMap.Clear();

        if( go != null && go.activeSelf)
            go.SetActive(false);

        UtilsHomeBar.toggleFiltersOpacity -= ToggleFiltersOpacity;

        firstInitialized = true;
        UtilsHomeBar.ar = false;
        
        UtilsHomeBar.swipeAR -= GetMovePointer;
        UtilsHomeBar.StopAudio();
        UtilsHomeBar.DeselectPOI();
        selectedObjectInAR.Clear();
        ResetFilterButtons();
        ResetTextsAndDestoryModelTask();

        tap.SetActive(true);
        tap = null;
        track.SetActive(true);
        track = null;
        imageTrack.SetActive(true);
        imageTrack = null;
        imageTracker.EnableFilters -= EnableARInfoPointFilters;
        UtilsHomeBar.enableLandscapeAction -= HandleLandscape;

        planeManager.planesChanged -= OnPlanesChanged;
        globalMessageString = "";
    }
    void UnregisterButtons()
    {
        backAR.UnregisterCallback<ClickEvent>(OnBackClicked);
        _32Back.UnregisterCallback<ClickEvent>(On_32BackClicked);
        ARNext.UnregisterCallback<ClickEvent>(OnARNextClicked);
        Telephone.UnregisterCallback<ClickEvent>(On_TelephoneClicked);
        websiteButton.UnregisterCallback<ClickEvent>(On_WebSiteClicked);
        _mapButton.UnregisterCallback<ClickEvent>(On_MapButton);
        audioPlay.UnregisterCallback<ClickEvent>(OnAudioPlay);
        audioPause.UnregisterCallback<ClickEvent>(OnAudioPause);
        goTo360.UnregisterCallback<ClickEvent>(OnGoTo360);
        back360.UnregisterCallback<ClickEvent>(OnBack360);
        fullScreenButtonReset.UnregisterCallback<ClickEvent>(OnFullScreenResetClicked);
        fullScreenButton.UnregisterCallback<ClickEvent>(OnFullScreenButtonClicked);
        fsNext.UnregisterCallback<ClickEvent>(OnFsNextClicked);
        fsBack.UnregisterCallback<ClickEvent>(OnFsBackClicked);
        UnregisterButtonCallbacks();
    }
    //Update  
    private void Update()
    {
        if(!ARBoundHandler.Instance.activated)
        {
            messageAR.text = ARBoundHandler.Instance.globalMessageString;
        }
        
        gameState = state.gameState;

        if (gameState == routesState)
        {
            UtilsHomeBar.isInRoutes = true;
        }
        else if (gameState == mapState)
        {
            UtilsHomeBar.isInMap = true;
        }
        else
        {
            Debug.Log("None of the two states");
        }

        MethodToBeUpdated();

        if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
        {    
            if (ARBoundHandler.Instance.activated == true)
            {
                if(isActivated == true)
                {
                    // planeManager.enabled = true;
                    UpdateObjectVisibility();
                    OnScreenTouch();
                }
                else if(isActivated == false && completed == true)
                {
                    UIExtentions.Display(infoHolder, false);
                }
            }
        }
    }
    private void MethodToBeUpdated()
    {
        TouchSign();
        UpdateInfoBasedOnSelectedPOI();
        UpdateWebsiteButton();
        UpdateTelButton();
        UpdateTicketButton();
        UpdateFullScreenCounter();
        Update360_UI();
    }

#region Tutorial
    // void HandleSwipe(int direction)
    // {
    //     // -1 for left swipe, 1 for right swipe
    //     if (direction == -1 && pageIndex > 0)
    //     {
    //         pageIndex--;
    //         UpdateTutorial();
    //     }
    //     else if (direction == 1 && pageIndex < maxIndex)
    //     {
    //         pageIndex++;
    //         UpdateTutorial();
    //     }
    // }
    void UpdateTutorial()
    {
        if (ARState.info == false)
        {
            // buttonName.text = UIExtentions.IsEnglish() ? " NEXT " : " ΕΠΟΜΕΝΟ ";
            UIExtentions.Display(tutorialDescriptionF, true);
            UIExtentions.Display(tutorialDescriptionS, false);
            aRTutorialImage.style.backgroundImage = new StyleBackground(Image_1[0]);
            // pointers[0].style.backgroundImage = new StyleBackground(UtilsHomeBar.pointerBlue);
            // pointers[1].style.backgroundImage = new StyleBackground(UtilsHomeBar.pointer);
        }
        else if (ARState.info == true)
        {
            // buttonName.text = UIExtentions.IsEnglish() ? " END " : " ΤΕΛΟΣ ";
            UIExtentions.Display(tutorialDescriptionF, false);
            UIExtentions.Display(tutorialDescriptionS, true);
            aRTutorialImage.style.backgroundImage = new StyleBackground(Image_1[1]);
            // pointers[1].style.backgroundImage = new StyleBackground(UtilsHomeBar.pointerBlue);
            // pointers[0].style.backgroundImage = new StyleBackground(UtilsHomeBar.pointer);
        }
        // else
        // {
        //     buttonName.text = UIExtentions.IsEnglish() ? " NEXT " : " ΕΠΟΜΕΝΟ ";
        //     UIExtentions.Display(tutorialDescriptionF, false);
        //     UIExtentions.Display(tutorialDescriptionS, true);
        //     pointers[1].BringToFront();
        // }       
    }
    // void TutorialInitialize(int index)
    // {
    //     UIExtentions.Display(Tutorial, true);
    //     aRTutorialImage.style.backgroundImage = new StyleBackground(Image_1[index]);
    //     UIExtentions.Display(tutorialDescriptionF, true);
    //     buttonName.text = UIExtentions.IsEnglish() ? " NEXT " : " ΕΠΟΜΕΝΟ ";
    //     pointers[0].style.backgroundImage = new StyleBackground(UtilsHomeBar.pointerBlue);

    //     ARNext.RegisterCallback<ClickEvent>(OnARNextClicked);

    // }
    private void OnARNextClicked(ClickEvent evt)
    {
        // UIExtentions.Display(Tutorial, false);
        // UIExtentions.Display(infoHolder, false);
        UIExtentions.Display(Tutorial, false);
        UIExtentions.Display(infoHolder, false);
        blueHead.style.opacity = new StyleFloat(0f);
        blueBottom.style.opacity = new StyleFloat(0f);
        initialized = true;
    }
#endregion Tutorial

#region 32Routes
    private void InfoPageDisabled()
    {
        UIExtentions.Display(_ARPage, true);
        UIExtentions.Display(_32Routes, false);
        blueHead.style.opacity = new StyleFloat(0f);
        blueBottom.style.opacity = new StyleFloat(0f);

        TogglePauseUnPauseAudioIconHandler(true, false);

        UtilsHomeBar.StopAudio();
        UtilsHomeBar.infoCheck32 = false;
        currentImageIndex = 0;
        // UtilsHomeBar.swipeAR -= GetMovePointer;       
    }
    public void InfoPageEnabled()
    {
        // UtilsHomeBar.StopAudio();
        // UtilsHomeBar.swipeAR += GetMovePointer;
        currentImageIndex = 0;
        UIExtentions.Display(infoHolder, false);
        UtilsHomeBar.infoCheck32 = true;
        UIExtentions.Display(_ARPage, false);
        UIExtentions.Display(_32Routes, true);
        blueHead.style.opacity = new StyleFloat(100f);
        blueBottom.style.opacity = new StyleFloat(100f);

        if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
        {
            if (Instance.GetComponent<AudioSource>().isPlaying)
            {
                Instance.GetComponent<AudioSource>().Stop();
            }
            else
            {
                Debug.Log("No audio playing");
            }
        }

        
    }
    private void Routes32()
    {    
        scrollView32 = _32Routes.Q<ScrollView>("BlankSpace");
        goTo360 = _32Routes.Q<Button>("GoTo360");
        goTo360.style.unityBackgroundImageTintColor = UIExtentions.grey;
        fullScreenButton = _32Routes.Q<Button>("FullScreen");
        PointerTab = _32Routes.Q<VisualElement>("PointerTab");

        titleList = _32Routes.Query<Label>(className: "32RoutesTitle").ToList(); //////////

        filterIconGrey = _32Routes.Q<VisualElement>("FilterIconGrey");
        filtertext = _32Routes.Q<Label>("FilterText");
        audioPlay = _32Routes.Q<Button>("Audio");
        audioPause = _32Routes.Q<Button>("Pause");   
        
        address = _32Routes.Q<Label>("Address");
        tel = _32Routes.Q<Label>("Tel");
        email = _32Routes.Q<Label>("mail");
        ticket = _32Routes.Q<Label>("ticket");
        descPoint = _32Routes.Q<Label>("descPoint");

        _mapButton = _32Routes.Q<Button>("MapRoute");
        UIExtentions.Display(_mapButton, true);
        
        _mapButton.RegisterCallback<ClickEvent>(On_MapButton);

        Telephone = _32Routes.Q<Button>("Telephone");
        UIExtentions.Display(Telephone, false);
        Telephone.RegisterCallback<ClickEvent>(On_TelephoneClicked); 

        websiteButton = _32Routes.Q<Button>("Earth");
        UIExtentions.Display(websiteButton, false);
        websiteButton.RegisterCallback<ClickEvent>(On_WebSiteClicked); 

        Ticket = _32Routes.Q<VisualElement>("Ticket");
        UIExtentions.Display(Ticket, false);     
    }
    //32 routes buttons on info ---
    private void On_WebSiteClicked(ClickEvent evt)
    {
        if(ObjectSelectionManager.hasWebSite)
        {
            Application.OpenURL(ObjectSelectionManager.OpenEntrance);
        }
    }
    private void On_TelephoneClicked(ClickEvent evt)
    {
        if(ObjectSelectionManager.hasTel)
        {
            Application.OpenURL("tel:" + ObjectSelectionManager.Contact);
        }
    }
    private void On_MapButton(ClickEvent evt)
    {
        GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().OpenGoogleMapsURL();
    }
    //32 routes buttons on info ---

    //Audio---------------------
    private void AudioBehaviour()
    {
        audioPlay.RegisterCallback<ClickEvent>(OnAudioPlay);
        audioPause.RegisterCallback<ClickEvent>(OnAudioPause);
    }
    private void OnAudioPause(ClickEvent evt)
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
    private void OnAudioPlay(ClickEvent evt)
    {
        UtilsHomeBar.PlayAudio();
        currentAudioState = AudioState.Playing;
        TogglePauseUnPauseAudioIconHandler(false, true);
    }
    public void TogglePauseUnPauseAudioIconHandler(bool play, bool pause)
    {
        UIExtentions.Display(audioPlay, play);
        UIExtentions.Display(audioPause, pause);
    }

    // 32Info Images Texts and Buttons
    private void UpdateInfoBasedOnSelectedPOI()
    {
        filterIconGrey.style.backgroundImage = new StyleBackground(ObjectSelectionManager.spriteIcon);
        filtertext.text = ObjectSelectionManager.filterIDText;

        foreach (var item in titleList)
        {
            item.text = ObjectSelectionManager.textGlobal;
        }

        address.text = ObjectSelectionManager.Address;      
        tel.text = ObjectSelectionManager.OpenEntrance;
        email.text = ObjectSelectionManager.Contact;
        ticket.text = ObjectSelectionManager.Entry;
        descPoint.text = ObjectSelectionManager.textGlobalD;
    }
    private void UpdateWebsiteButton()
    {
        if (ObjectSelectionManager.hasWebSite == true)
        {
            UIExtentions.Display(websiteButton, true);
        }
        else
        {
             UIExtentions.Display(websiteButton, false);
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
    private void Handle32RoutesImages(int images, List<Sprite> sprites)
    {
        scrollView32.Clear();
        AbstractPointerBehaviour.ResetCurrentIndex(); 

        UtilsHomeBar.swipeAR -= GetMovePointer;

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
            visualElement.style.borderRightWidth =5;
            visualElement.style.borderBottomLeftRadius = 30;
            visualElement.style.borderBottomRightRadius = 30;

            visualElement.style.backgroundImage = new StyleBackground(sprites[i]);
            scrollView32.Add(visualElement);
        } 
        UtilsCommander.ExecutePointer32Command(sprites.Count, PointerTab);

              
        UtilsHomeBar.swipeAR += GetMovePointer;
    }

#endregion 32Routes

#region FullScreen
    private void FullScreen()
    {    
        // FullScreen
        fullScreen = ARapp.Q<VisualElement>("FullScreenPage");
        UIExtentions.Display(fullScreen, false);
        fsRegionDown = fullScreen.Q<VisualElement>("FS_Minimazie");
        fsRegionUp = fullScreen.Q<VisualElement>("FS_ButtonHolder");
        
        fullScreenButtonReset = fullScreen.Q<Button>("FullScreenReset");
        fsBack = fullScreen.Q<Button>("Back");
        fsNext = fullScreen.Q<Button>("Next");
        fsImage = fullScreen.Q<VisualElement>("FS_Image");
        fsImage.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
        fsPoiIndex = fullScreen.Q<Label>("FSpoiIndex");
        fsRouteCounter= fullScreen.Q<Label>("FSrouteCounter");

        fullScreenButton.RegisterCallback<ClickEvent>(OnFullScreenButtonClicked);
        fsNext.RegisterCallback<ClickEvent>(OnFsNextClicked);
        fsBack.RegisterCallback<ClickEvent>(OnFsBackClicked);
        fullScreenButtonReset.RegisterCallback<ClickEvent>(OnFullScreenResetClicked);
    }
    private void OnFullScreenButtonClicked(ClickEvent evt)
    {
        // currentImageIndex = 0;
        UIExtentions.Display(fullScreen, true);
        _32Back.style.display = DisplayStyle.None;
        UtilsHomeBar.fullScreenCheck = true;
    }
    private void OnFsNextClicked(ClickEvent evt)
    {
        HandleFSImagesSwipeL(1);
        GetMovePointer(1);
    }
    private void OnFsBackClicked(ClickEvent evt)
    {
        HandleFSImagesSwipeR(-1);
        GetMovePointer(-1);
    }
    private void OnFullScreenResetClicked(ClickEvent evt)
    {
        if(UtilsHomeBar.fullScreenCheck)
        {
            UIExtentions.Display(fullScreen, false);
            _32Back.style.display = DisplayStyle.Flex;
            // currentImageIndex = 0;
            UtilsHomeBar.fullScreenCheck = false;
        }
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
    private void HandleFullScreenImages(int images, List<Sprite> sprites)
    {
        int numSprites = Mathf.Min(images, sprites.Count);
        for (int i = 0; i < numSprites; i++)
        {
            fullScreenSprites = sprites;
        }
        DisplayCurrentImage(currentImageIndex);
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
    private void DisplayCurrentImage(int index)
    {
        // Check if the index is within bounds
        if (currentImageIndex >= 0 && currentImageIndex < fullScreenSprites.Count)
        {
            fsImage.style.backgroundImage = new StyleBackground(fullScreenSprites[index]);;
        }
    }  

    void HandleLandscape(bool enabled)
    {
        UIExtentions.Display(fsRegionDown, enabled);
        UIExtentions.Display(fsRegionDown, enabled);
    }

#endregion FullScreen
    
#region 360
    private void Assign360()
    {
        View360Page = ARapp.Q<VisualElement>("360ViewCore");
        UIExtentions.Display(View360Page, false);
        back360 = View360Page.Q<Button>("360BackButton");
        Title360 = View360Page.Q<Label>("360TitleText");
    }
    private void Update360_UI()
    {
        Update360ButtonOpacity();
        Update360Title();
    }   
    private void Handle360()
    {
        goTo360.RegisterCallback<ClickEvent>(OnGoTo360);
        back360.RegisterCallback<ClickEvent>(OnBack360);
    }
    private void OnGoTo360(ClickEvent evt)
    {
        UtilsHomeBar.isIn360 = true;
        whenIn360?.Invoke(false);
        load360Material?.Invoke();
        ARCamera.GetComponent<Camera>().farClipPlane = 10000;
        // materialManager.LoadMaterial();
        TransitionFromTo360(false);
    }
    private void OnBack360(ClickEvent evt)
    {
        UtilsHomeBar.isIn360 = false;
        whenIn360?.Invoke(true);
        ARCamera.GetComponent<Camera>().farClipPlane = 2000;
        unload360Material?.Invoke();
        TransitionFromTo360(true);
    }
    private void TransitionFromTo360(bool enabling)
    {
        UIExtentions.Display(View360Page, !enabling);
        UIExtentions.Display(_32Routes, enabling);
        ActivateDeactivateInstance(enabling);
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
#endregion 360

#region AR
    public void EnableInfoPopUp(bool enableinfo, GameObject poi)
    {           
        if(enableinfo == true)
        {   
            planeManager.planesChanged += OnPlanesChanged;  
            isActivated = true; 
            messageAR.text = UIExtentions.IsEnglish() ? "Point the camera at the floor" 
            : "Στοχεύστε με την κάμερα στο έδαφος" ;
            UIExtentions.Display(infoHolder, enableinfo);

            if(!selectedObjectInAR.Contains(poi))
            {
                selectedObjectInAR.Add(poi);
            }            
            selectedObjectInAR[0].gameObject.GetComponent<assignNameTMP>().children.SetActive(false);
            hitResults.Clear();
            trackedPlanes.Clear();
        }
        else
        {
            planeManager.planesChanged -= OnPlanesChanged;
            
            if(selectedObjectInAR.Contains(poi))
            {
                selectedObjectInAR[0].gameObject.GetComponent<assignNameTMP>().children.SetActive(true);
                selectedObjectInAR.Remove(poi);
                UIExtentions.Display(infoHolder, enableinfo);
                ResetTextsAndDestoryModelTask();
            }
            isActivated = false;
        }   
        TogglePlaneDetection(enableinfo);
        go.SetActive(!enableinfo);   
    }
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            trackedPlanes.Add(plane);

            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                planeDetected = true;
                Debug.Log("Plane detected! Touch the screen to place an object.");
            }
        }
        foreach (var plane in args.removed)
        {
            trackedPlanes.Remove(plane);
            Destroy(plane);
        }
    } 
    public void OnScreenTouch()
    {
        if (planeDetected && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touchPosition = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPosition, hitResults, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hitResults[0].pose;
                placePos = hitPose.position;
                placeRot = hitPose.rotation;
                tap.SetActive(false);
                track.SetActive(false);

                if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
                {            
                    SpawnAriadneOrDominicTask(selectedObjectInAR[0]);
                    TogglePlaneDetection(false);
                    completed = true;
                    isActivated = false;
                    // UIExtentions.Display(infoHolder, false);
                }
            }
        }
    }
    void TogglePlaneDetection(bool value)
    {
        planeManager.enabled = value;
        
        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
            // Destroy(plane);
            // trackedPlanes.Remove(plane);
        }
    }
    void UpdateObjectVisibility()
    {
        planeInView = false;
        
        foreach (var plane in trackedPlanes)
        {
            if (IsPlaneInView(plane))
            {
                planeInView = true;
                
                messageAR.text = UIExtentions.IsEnglish() ? "Place the virtual guide by tapping the display screen" 
                : "Τοποθετήστε τον εικονικό ξεναγό στο σημείο που θέλετε, κάνοντας tap στην οθόνη" ;
                Debug.LogWarning("A: PlaceActor");
            }
            else 
            {
                planeInView = false;
                
                messageAR.text = UIExtentions.IsEnglish() ? "Point the camera at the floor" 
                : "Στοχεύστε με την κάμερα στο έδαφος" ;
                Debug.LogWarning("A: PointPhone");
            }
        }

        track.SetActive(!planeInView);
        tap.SetActive(planeInView);
    }
    bool IsPlaneInView(ARPlane plane)
    {
        Vector3 screenPoint = ARCamera.GetComponent<Camera>().WorldToViewportPoint(plane.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
                    
                    private async void SpawnAriadneOrDominicTask(GameObject point)
                    {      
                        await LoadModelsAsync();
                        // Spawn the appropriate model
                        GameObject prefabToInstantiate = null;
                        GameObject modelToActivate = null;

                        assignNameTMP.Model modelType = point.GetComponent<assignNameTMP>().model;
                        
                        if (modelType == assignNameTMP.Model.Dominic)
                        {
                            prefabToInstantiate = Dominic;
                            modelToActivate = Dominic;
                            Addressables.LoadAssetAsync<GameObject>(DominicRef);
                        }
                        else if (modelType == assignNameTMP.Model.Ariadne)
                        {
                            prefabToInstantiate = Ariadne;
                            modelToActivate = Ariadne;
                            Addressables.LoadAssetAsync<GameObject>(AriadneRef);
                        }
                        else
                        {
                            Debug.LogError("No Model Assigned");
                            return;
                        }

                        Instance = Instantiate(prefabToInstantiate, placePos, placeRot);
                        Vector3 directionToCamera = ARCamera.gameObject.transform.position - Instance.transform.position;
                        // Eliminate the x and z components of the direction
                        directionToCamera.y = 0;
                        // Calculate the rotation to look at the camera on the y-axis only
                        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
                        // Apply the y-axis only rotation to the instance
                        Instance.transform.rotation = lookRotation;             
                        Instance.transform.parent = null;                    

                        var poi = point.GetComponent<assignNameTMP>();
                        FindObjectOfType<WorldSpaceInfoCanvas>().InitializeTextsAndContent(ObjectSelectionManager.textGlobal, poi.sprite.sprite, ObjectSelectionManager.textGlobalD);
                    }
                    private async Task LoadModelsAsync()
                    {
                        await Task.WhenAll
                        (
                            LoadModelAsync(DominicRef, result => Dominic = result),
                            LoadModelAsync(AriadneRef, result => Ariadne = result)
                        );
                    }
                    private async Task<GameObject> LoadModelAsync(AssetReferenceGameObject reference, Action<GameObject> onLoaded)
                    {
                        var handle = Addressables.LoadAssetAsync<GameObject>(reference);

                        await handle.Task;
                        onLoaded?.Invoke(handle.Result);
                        return handle.Result;
                    }
                    public void ResetTextsAndDestoryModelTask()
                    {
                        // Unload the loaded models 
                        if(Instance != null)
                        {
                            Destroy(Instance);
                        }
                        completed = false;

                        planeDetected = false;
                        planeInView = false;
                        tap.SetActive(false);
                        track.SetActive(false);
                        selectedObjectInAR.Clear();
                        hitResults.Clear();
                        trackedPlanes.Clear();
                        Debug.Log($"Dominic has been Destroyed");
                    }

    public void TouchSign()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is began or moved
            if (touch.phase == TouchPhase.Began)
            {
                // Cast a ray from the touch position
                Ray ray = ARCamera.GetComponent<Camera>().ScreenPointToRay(touch.position);
                RaycastHit hitInfo;

                // Check if the ray hits an object
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
                    {
                        if(selectedObjectInAR.Count != 0)
                        {
                            // Check if the object is the one we want to track && info.style.display == DisplayStyle.None
                            if (hitInfo.collider.gameObject == selectedObjectInAR[0])
                            {
                                
                                Instance.GetComponentInChildren<WorldSpaceInfoCanvas>().quadObject.SetActive(true);
                                selectedObjectInAR[0].gameObject.GetComponent<assignNameTMP>().children.SetActive(false);
                                // UIExtentions.Display(info, true);
                                // UIExtentions.Display(infoHolder, false);
                            }
                            else
                            {
                                Debug.Log("No Object");
                            }
                        }
                    }
                }
            }
        }
    }

#endregion AR

#region Filters

    private void ToggleFiltersOpacity(bool toggled)
    {
        if(toggled)
        {
            filterHolder.style.opacity = new StyleFloat(0f);
        }
        else
        {
            filterHolder.style.opacity = new StyleFloat(100f);
        }
        
    }
    private void FiltersButtonInitialize()
    {
        buttonFilterMap.Add(monumentButton, ARButton.FilterID.Monument);
        buttonFilterMap.Add(templeButton, ARButton.FilterID.Temple);
        buttonFilterMap.Add(museumButton, ARButton.FilterID.Museum);
        buttonFilterMap.Add(squareButton, ARButton.FilterID.Park);
        buttonFilterMap.Add(playGroundButton, ARButton.FilterID.PlayGround);
        buttonFilterMap.Add(shoppingStreetButton, ARButton.FilterID.ShoppingStreet);
        buttonFilterMap.Add(recreationGroundButton, ARButton.FilterID.RecreationGround);   
        buttonFilterMap.Add(culturalFacilityButton, ARButton.FilterID.CulturalFacility);      
        
        foreach (var pair in buttonFilterMap)
        {
            bool pressed = false;
            EventCallback<ClickEvent> callback = evt =>
            {
                pressed = !pressed;
                UtilsHomeBar.ReduceFilterOpacity(pair.Key, pressed, 0, 0);
                UtilsHomeBar.TriggerFilterEventAR(pair.Value, pressed);
                FindObjectOfType<GroupInfoPoint>().FilterEventARGroup(pair.Value.GetHashCode(), pressed);
                FilterScrollView.ScrollTo(pair.Key);
            };

            RegisterButtonCallback(pair.Key, callback);
        }
    }
    private void RegisterButtonCallback(Button button, EventCallback<ClickEvent> callback)
    {
        if (!buttonCallbacks.ContainsKey(button))
        {
            buttonCallbacks.Add(button, callback);
            button.RegisterCallback(callback);
        }
    }
    private void UnregisterButtonCallbacks()
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
            UtilsHomeBar.ReduceFilterOpacity(pair.Key, pressed, 0, 0);
            UtilsHomeBar.TriggerFilterEventAR(pair.Value, pressed);
        }
    }
#endregion Filters 

#region General Functions
    public void EnableARInfoPointFilters(bool enabled)
    {
        UIExtentions.Display(filterHolder, enabled);
        UIExtentions.Display(infoHolder, !enabled);
    }
    private void ActivateDeactivateInstance(bool active)
    {
        if(Instance != null)
        {
            Instance.SetActive(active);
        }
    }
    void GetMovePointer(int index)
    {
        if(UtilsHomeBar.ar == true)
        {
            AbstractPointerBehaviour.MovePointerAR(PointerTab, index, scrollView32);   
        }
    }  
    public void ToggleState_AR(bool opacity)
    {
        if (gameState == StateTracker.GameState.Home)
        {
            UIExtentions.Display(home, opacity);
        }
        else if (gameState == routesState)
        {
            UIExtentions.Display(infoRoutes, opacity);
        }
        else if (gameState == mapState)
        {
            UIExtentions.Display(map, opacity);
        }
        else if (gameState == StateTracker.GameState.Settings)
        {
            UIExtentions.Display(settings, opacity);
        }
        else
        {
            Debug.LogError("Not Selected Map");
        }
    } 
    private void ToggleCamera(bool arEnabled)
    {
        cam.SetActive(!arEnabled);
        ARCamera.SetActive(arEnabled);
        
        if(arEnabled == true)
        {
            lightD.transform.rotation = Quaternion.Euler(lightARrotation);
            // SunPosition.UpdateSunPosition(lightD);
        }
        else if(arEnabled == false)
        {
            lightD.transform.rotation = Quaternion.Euler(lightNonArRotation);
        }   
    }
    private void OnToggleChanged(bool newValue)
    {
        if (newValue)
        {
            // Toggle is checked
            toggle.style.borderBottomColor = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
            toggle.style.borderLeftColor = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
            toggle.style.borderRightColor = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
            toggle.style.borderTopColor = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
            checkmark.style.unityBackgroundImageTintColor = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
            filtersLabel.style.color = new StyleColor(new Color(255f / 255f, 195f / 255f, 83f / 255f, 255f / 255f));
        }
        else
        {
            // Toggle is unchecked - reset to default styles if needed
            toggle.style.borderBottomColor = new StyleColor(UIExtentions.white);
            toggle.style.borderLeftColor = new StyleColor(UIExtentions.white);
            toggle.style.borderRightColor = new StyleColor(UIExtentions.white);
            toggle.style.borderTopColor = new StyleColor(UIExtentions.white);
            checkmark.style.unityBackgroundImageTintColor = new StyleColor(UIExtentions.white);
            filtersLabel.style.color = new StyleColor(UIExtentions.white);
        }
    }
#endregion General Functions
   
}
