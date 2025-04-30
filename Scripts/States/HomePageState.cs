using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class HomePageState : PlayerBaseState
{
    public HomePageState(PlayerStateMachine stateMachine) : base(stateMachine) {} 
    //Card Button Elements
    Button buttonAR;
    Button buttonMap;
    Button buttonRoads;

    //HomeBar Buttons 
    Button home;
    Button map; 
    Button settings;
    Button augmentedReality;
    Button routes;

    // Root Elements
    VisualElement root;
    VisualElement homePage;
    SO_PageData homePageData;

    VisualElement blueLine;
    bool homecheck;
    bool mapcheck;
    bool settingscheck;
    bool infoCHeck;
    bool arCheck;
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();
    public override void Enter()
    {
        
        homePageData = stateMachine.homePageData;
        root = stateMachine.root;
        homePage = root.Q("HomePage");
        blueLine = root.Q<VisualElement>("BlueHead");
        UIExtentions.Display(blueLine, false);

        homePage.style.display = DisplayStyle.Flex;

        buttonRoads = homePage.Q<Button>("CulturalRoadsButton");
        buttonMap = homePage.Q<Button>("CityTourButton");
        buttonAR = homePage.Q<Button>("ARCardButton"); 

        home = homePage.Q<Button>("HomeButton");
        map = homePage.Q<Button>("MapButton");
        settings = homePage.Q<Button>("SettingsButton");
        augmentedReality = homePage.Q<Button>("ARButton");
        routes = homePage.Q<Button>("MonumentButton");
        
        ButtonsInit();
        
        GameObject.Find("GameManager").gameObject.GetComponent<StateTracker>().gameState = StateTracker.GameState.Home;
        Debug.Log("Enter Homepage");  

        // Resources.UnloadUnusedAssets();
        // InitializeHomePageTexts();
    }   
    public override void Exit()
    {
        UnregisterButtonCallbacks();
        UIExtentions.Display(blueLine, true);
        UtilsHomeBar.DeselectPOI();
        Debug.Log("Exit Homepage"); 
        homePage.style.display = DisplayStyle.None;        
        homecheck = false;
        mapcheck = false;
        settingscheck = false;
        infoCHeck = false;
        arCheck = false;
    }
    public override void Tick()
    {
        if(UtilsHomeBar.ar == false)
        {
            if(infoCHeck == true)
            {
                stateMachine.SwitchState(new InfoState(stateMachine));
            }
            if(settingscheck == true)
            {
                stateMachine.SwitchState(new SettingsState(stateMachine));
            }
            if(mapcheck == true)
            {
                stateMachine.SwitchState(new MapState(stateMachine));
            }
            if(arCheck == true)
            {
                stateMachine.SwitchState(new ARState(stateMachine));
                // var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
                UtilsHomeBar.ar = true;
                // arElem.enabled = true;
                arCheck = false;
            }
        }

    }
    void ButtonsInit()
    {
        RegisterButtonCallback(home, OnHomeClicked);
        RegisterButtonCallback(routes, OnRoutesClicked);
        RegisterButtonCallback(augmentedReality, OnAugmentedRealityClicked);
        RegisterButtonCallback(map, OnMapClicked);
        RegisterButtonCallback(settings, OnSettingsClicked);
        RegisterButtonCallback(buttonRoads, OnButtonRoadsClicked);
        RegisterButtonCallback(buttonMap, OnButtonMapClicked);
        RegisterButtonCallback(buttonAR, OnButtonARClicked);
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
    // Callback methods
    private void OnHomeClicked(ClickEvent evt)
    {
        Debug.Log("Already At HomePage");
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
        settingscheck = true;
    }
    private void OnButtonRoadsClicked(ClickEvent evt)
    {
        infoCHeck = true;
    }
    private void OnButtonMapClicked(ClickEvent evt)
    {
        mapcheck = true;
    }
    private void OnButtonARClicked(ClickEvent evt)
    {
        arCheck = true;
    }
    
    public void InitializeHomePageTexts()
    {
        List<Label> texts = homePage.Query<Label>(className: "homePageTitle").ToList();
        
        foreach (Label label in texts)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                label.text = homePageData.data[i].tableKeyName.GetLocalizedString();
            }            
        }
    }
}

