
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public UIDocument uIDocument {get; private set;}
    public SO_PageData settingsData;
    public SO_PageData homePageData;
    public SO_PageData mapPageData;
    public VisualElement root;
    public VisualElement home;
    public VisualElement map;
    public VisualElement settings;
    VisualElement augmentedReality;
    VisualElement routes;
    List<VisualElement> pages = new List<VisualElement>();


    public bool firstInitialized = false;
    private void Start() 
    {
        StateInitialization();
    }
    public void StateInitialization()
    {
        root = uIDocument.rootVisualElement;

        home = root.Q("HomePage");
        pages.Add(home);
        settings = root.Q("SettingsPage");
        pages.Add(settings);
        map = root.Q("MapPage");
        pages.Add(map);
        routes = root.Q("CulturalRoads");
        pages.Add(routes);

        foreach(VisualElement element in pages)
        {
            element.style.display = DisplayStyle.None;
        }
        if(firstInitialized == false) 
            SwitchState(new HomePageState(this));
        if(firstInitialized == true)
        {
            SwitchState(new SettingsState(this));   
        }
    }    
    
}
