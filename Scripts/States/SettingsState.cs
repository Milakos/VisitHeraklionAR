using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public class SettingsState : PlayerBaseState
{
    public SettingsState(PlayerStateMachine stateMachine) : base(stateMachine){}

    Button home;
    Button map;
    Button settings;
    Button augmentedReality;
    Button routes;

    Button developerButton;
    Button termsAndConditionsButton;
    Button languageButton;
    Button municipialityButton;
    Button InteractiveButton;

    Button _360;
    Button _gastronomy;
    VisualElement ScrollApps;

    VisualElement root; // Root element
    VisualElement settingsPage; // This state page-element

    // Basic Content Visual Elements
    VisualElement contentScroll;    
    VisualElement btnRegion;

    VisualElement munucupality;
    VisualElement espa;
    VisualElement Forth;

    // Settings text and Button header - Holder Properties
    Button back;
    Label descriptionTitle;
    Label title;
    VisualElement GastronomyRegion;
    VisualElement appIcon;

    TextClassTranslate settingsTextTitle = new TextClassTranslate("Ρυθμίσεις", "Settings");
    TextClassTranslate MunicipalityTitle = new TextClassTranslate("Το Έργο", "The Project");
    TextClassTranslate MunicipalityGastronomyTitle = new TextClassTranslate("Δήμου Ηρακλείου", "Municipality of Heraklion");
    TextClassTranslate settingscommentTitle = new TextClassTranslate("Πληροφορίες και Πνευματικά Δικαιώματα", "Info and Copyrights");
    TextClassTranslate HeraklionGastronomy = new TextClassTranslate("Διαδραστικές Εφαρμογές", "Interactive Applications");
   
    string Gastornomy = "Heraklion Gastronomy";
    string Title360 = "Heraklion 360";
    TextClassTranslate HeraklionGastronomyComment = new TextClassTranslate("Διαδραστική Εφαρμογή", "Interactive Application");
    
    const float maxOpacity = 100f;
    const float minOpacity = 0f;
    // ...................................................
    
    SO_PageData settingsData;

    bool eng;
    bool homecheck;
    bool infoCHeck;
    bool mapcheck;
    bool arCheck;

    bool appsState = false;

    static string urlITEGreek = "https://www.ics.forth.gr/hci/?lang=el";
    static string urlITEENglish = "https://www.ics.forth.gr/hci/?lang=en";
    TextClassTranslate urlITE = new TextClassTranslate(urlITEGreek, urlITEENglish);

    static string urltermsGreek = "https://www.heraklion.gr/static/terms-of-use.html";
    static string urltermsEnglish = "https://www.heraklion.gr/static/terms-of-use.html";
    TextClassTranslate urlterms = new TextClassTranslate(urltermsGreek, urltermsEnglish);

    Button GooglePlayButton;
    Button AppStoreButton;
    // string urlkioskGoogle = "https://play.google.com/store/apps/details?id=com.ICSFORTH.com.forth.ics.herakliongastronomy&pcampaignid=web_share";
    // string urlkioskiOS = "https://apps.apple.com/gr/app/heraklion-gastronomy/id1664900666";
    string urlkioskGoogle;
    string urlkioskiOS;
    string url360Google = "";
    string url360iOS = "";

    bool kiosk = false;
    bool _360app = false;
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();

    public override void Enter()
    {
        url360iOS = JSONTest.Instance.heraklion360IOS;
        url360Google = JSONTest.Instance.heraklion360Google;
        urlkioskGoogle = JSONTest.Instance.heraklionGastronomyGoogle;
        urlkioskiOS = JSONTest.Instance.heraklion360IOS;
        //Initialize the uxml root and the state of it
        settingsData = stateMachine.settingsData;
        root = stateMachine.root;
        settingsPage = root.Q("SettingsPage");
        UIExtentions.Display(settingsPage, true);

        home = settingsPage.Q<Button>("HomeButton");
        map = settingsPage.Q<Button>("MapButton");
        settings = settingsPage.Q<Button>("SettingsButton");
        augmentedReality = settingsPage.Q<Button>("ARButton");
        routes = settingsPage.Q<Button>("MonumentButton");

        title = settingsPage.Q<Label>("SettingsTitle");
        title.text = settingsTextTitle.GetTranslatedText();

        //Initialize Settings Button and get the name Refrences of each button in settings page
        municipialityButton = settingsPage.Q<Button>("MunicipalityButton");
        languageButton = settingsPage.Q<Button>("LanguageButton");
        termsAndConditionsButton = settingsPage.Q<Button>("TermsAndConditionsButton");
        developerButton = settingsPage.Q<Button>("DeveloperButton");
        InteractiveButton = settingsPage.Q<Button>("InteractiveApps");
        GastronomyRegion = settingsPage.Q<VisualElement>("GastronomyRegion");
        UIExtentions.Display(GastronomyRegion, false);

        _360 = settingsPage.Q<Button>("360Button");
        _gastronomy = settingsPage.Q<Button>("GastronomyButton");
        ScrollApps = settingsPage.Q<VisualElement>("ContentScrollApps");
        UIExtentions.Display(ScrollApps, false);

        appIcon = settingsPage.Q<VisualElement>("AppIcon");

        GooglePlayButton = settingsPage.Q<Button>("GooglePlayButton");
        AppStoreButton = settingsPage.Q<Button>("AppStoreButton");

        // SettingsText and Back Button
        back = settingsPage.Q<Button>("BackButton");
        back.style.opacity = 0;
        descriptionTitle = settingsPage.Q<Label>("SettingsDescription");
        descriptionTitle.style.opacity = 0;

        // Basic Region Elemenets
        btnRegion = settingsPage.Q<VisualElement>("ButtonRegion");
        UIExtentions.Display(btnRegion, true);
        contentScroll = settingsPage.Q<VisualElement>("ContentScroll");
        UIExtentions.Display(contentScroll, false);
        
        munucupality = settingsPage.Q<VisualElement>("Mayor");
        espa = settingsPage.Q<VisualElement>("Espa");
        Forth = settingsPage.Q<VisualElement>("Forth");
        //Initialize the settings page Homebar buttons
        // stateMachine.HomeBarButtons.InitializeHomeBarButtons(settingsPage); 
        HomeButtonInit();
        GameObject.Find("GameManager").gameObject.GetComponent<StateTracker>().gameState = StateTracker.GameState.Settings;
        Debug.Log("Enter Settings");

        
    }
    public override void Exit()
    {
        UnregisterButtonCallbacks();
        UtilsHomeBar.DeselectPOI();
        //Setting to a non active or non visible state the Settings state
        settingsPage.style.display = DisplayStyle.None;     
        Debug.Log("Exit Settings"); 
        title.text = settingsTextTitle.GetTranslatedText();
    }
    public override void Tick() 
    {
        if(UtilsHomeBar.ar == false)
        {
            if(UIExtentions.IsEnglish())
            {
                espa.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().espa);
                munucupality.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().mayor);
                Forth.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().Forth);
            }
            if(!UIExtentions.IsEnglish())
            {
                espa.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().espaG);
                munucupality.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().mayorG);
                Forth.style.backgroundImage = new StyleBackground(GameObject.Find("Player").GetComponent<UtilsHomeBar>().ForthG);
            }
            
            if(homecheck == true)
            {
                stateMachine.SwitchState(new HomePageState(stateMachine));
            }        
            if(infoCHeck == true)
            {
                stateMachine.SwitchState(new InfoState(stateMachine));
            }
            if(mapcheck == true)
            {
                stateMachine.SwitchState(new MapState(stateMachine));
            }
            if(arCheck == true)
            {
                stateMachine.SwitchState(new ARState(stateMachine));
                // var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
                // arElem.enabled = true;
                arCheck = false;
            }
    
        }
    }
    private void HomeButtonInit()
    {
        // Initialize buttons and their respective callbacks
        RegisterButtonCallback(home, OnHomeClicked);
        RegisterButtonCallback(routes, OnRoutesClicked);
        RegisterButtonCallback(augmentedReality, OnAugmentedRealityClicked);
        RegisterButtonCallback(map, OnMapClicked);
        RegisterButtonCallback(settings, OnSettingsClicked);

        RegisterButtonCallback(back, OnBackClicked);
        RegisterButtonCallback(_360, On360Clicked);
        RegisterButtonCallback(_gastronomy, OnGastronomyClicked);
        RegisterButtonCallback(GooglePlayButton, OnGooglePlayButtonClicked);
        RegisterButtonCallback(AppStoreButton, OnAppStoreButtonClicked);
        RegisterButtonCallback(municipialityButton, OnMunicipialityButtonClicked);
        RegisterButtonCallback(InteractiveButton, OnInteractiveButtonClicked);
        RegisterButtonCallback(languageButton, OnLanguageButtonClicked);
        RegisterButtonCallback(termsAndConditionsButton, OnTermsAndConditionsButtonClicked);
        RegisterButtonCallback(developerButton, OnDeveloperButtonClicked);
    }
    #region Buttons
    // Register Method
    private void RegisterButtonCallback(Button button, EventCallback<ClickEvent> callback)
    {
        if (!buttonCallbacks.ContainsKey(button))
        {
            buttonCallbacks.Add(button, callback);
            button.RegisterCallback(callback);
        }
    }
    //Unregister Method
    private void UnregisterButtonCallbacks()
    {
        foreach (var kvp in buttonCallbacks)
        {
            kvp.Key.UnregisterCallback(kvp.Value);
        }
        buttonCallbacks.Clear();
    }
    private void OnBackClicked(ClickEvent evt)
    {
        if (appsState == false)
        {
            UIExtentions.Display(contentScroll, false);
            UIExtentions.Display(btnRegion, true);
            UIExtentions.Display(GastronomyRegion, false);

            back.style.opacity = 0;
            descriptionTitle.style.opacity = 0;
            UIExtentions.Display(descriptionTitle, false);
            title.text = settingsTextTitle.GetTranslatedText();
        }
        else
        {
            UIExtentions.Display(ScrollApps, false);
            UIExtentions.Display(GastronomyRegion, true);
            title.text = HeraklionGastronomy.GetTranslatedText();
            descriptionTitle.text = MunicipalityGastronomyTitle.GetTranslatedText();
            appsState = false;
        }
        _360app = false;
        kiosk = false;
    }
    private void On360Clicked(ClickEvent evt)
    {
        AppsElementBehaviour(UtilsHomeBar.Kiosk);
        title.text = Title360;
        descriptionTitle.text = HeraklionGastronomyComment.GetTranslatedText();
        _360app = true;
    }
    private void OnGastronomyClicked(ClickEvent evt)
    {
        AppsElementBehaviour(UtilsHomeBar.Gastronomy);
        title.text = Gastornomy;
        descriptionTitle.text = HeraklionGastronomyComment.GetTranslatedText();
        kiosk = true;
    }
    private void OnGooglePlayButtonClicked(ClickEvent evt)
    {
        if (kiosk == true)
        {
            OpenStoreURL(urlkioskGoogle);
        }
        else if (_360app == true)
        {
            OpenStoreURL(url360Google);
        }
    }
    private void OnAppStoreButtonClicked(ClickEvent evt)
    {
        if (kiosk == true)
        {
            OpenStoreURL(urlkioskiOS);
        }
        else if (_360app == true)
        {
            OpenStoreURL(url360iOS);
        }
    }
    private void OnMunicipialityButtonClicked(ClickEvent evt)
    {
        UIExtentions.Display(btnRegion, false);
        UIExtentions.Display(contentScroll, true);
        UIExtentions.Display(descriptionTitle, true);
        back.style.opacity = 100;
        descriptionTitle.style.opacity = 100;
        title.text = MunicipalityTitle.GetTranslatedText();
        descriptionTitle.text = settingscommentTitle.GetTranslatedText();
    }
    private void OnInteractiveButtonClicked(ClickEvent evt)
    {
        UIExtentions.Display(btnRegion, false);
        UIExtentions.Display(GastronomyRegion, true);
        UIExtentions.Display(descriptionTitle, true);
        back.style.opacity = 100;
        descriptionTitle.style.opacity = 100;
        title.text = HeraklionGastronomy.GetTranslatedText();
        descriptionTitle.text = MunicipalityGastronomyTitle.GetTranslatedText();
    }
    private void OnLanguageButtonClicked(ClickEvent evt)
    {
        ChangeLangLocale();
    }
    private void OnTermsAndConditionsButtonClicked(ClickEvent evt)
    {
        Application.OpenURL(urlterms.GetTranslatedText());
    }
    private void OnDeveloperButtonClicked(ClickEvent evt)
    {
        Application.OpenURL(urlITE.GetTranslatedText());
    }
    private void OnHomeClicked(ClickEvent evt)
    {
        homecheck = true;
    }
    private void OnRoutesClicked(ClickEvent evt)
    {
        infoCHeck = true;
    }
    private void OnAugmentedRealityClicked(ClickEvent evt)
    {
        arCheck = true;
    }
    private void OnMapClicked(ClickEvent evt)
    {
        mapcheck = true;
    }
    private void OnSettingsClicked(ClickEvent evt)
    {
        Debug.Log("Already At Settings");
    }
    
    #endregion Buttons
    
    private void AppsElementBehaviour(Sprite sp)
    {
        UIExtentions.Display(ScrollApps, true);
        appIcon.style.backgroundImage = new StyleBackground(sp);
        UIExtentions.Display(GastronomyRegion, false);
        appsState = true;
    }
    private void InitializeSettingsTexts()
    {
        VisualElement Container = stateMachine.settings;
        List<Label> texts = Container.Query<Label>(className: "settingsTitle").ToList();
        
        foreach (Label label in texts)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                label.text = settingsData.data[i].tableKeyName.GetLocalizedString();
            }            
        }
    }

    // Change Language Region .........................................................................
    #region ChangeLanguageLogic 
        public void ChangeLangLocale()
        {
            stateMachine.firstInitialized = true;

            switch (UIExtentions.IsEnglish())
            {
                case true: 
                {
                    ChangeEngGreek("el");
                    eng = false;
                }
                break;
                case false:
                {
                    ChangeEngGreek("en");
                    eng = true;
                }
                break;        
            default:
            }
        }
        private void ChangeEngGreek(string language)
        {
            eng = !eng;
            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(language);
            LocalizationSettings.SelectedLocale = locale;      
        }
    #endregion ChangeLanguageLogic
    public void OpenStoreURL(string mapsURL)
    {
        Application.OpenURL(mapsURL);
    }

}
