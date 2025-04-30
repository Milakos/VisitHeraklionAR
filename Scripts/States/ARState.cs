using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class ARState : PlayerBaseState
{
    public ARState(PlayerStateMachine stateMachine) : base(stateMachine){}
    ARSession arSession;
    StateTracker stateTracker;
    GameObject gameManager;
    VisualElement root;
    // ARChoose
    VisualElement arChoose;
    Button arBackChoose;
    Button arNavigationButton;
    Button arInfoPointButton;
    public static bool info = false;

    public override void Enter()
    {
        gameManager = GameObject.Find("GameManager");
        arSession = GameObject.Find("AR(Clone)").GetComponentInChildren<ARSession>();

        stateTracker = gameManager.GetComponent<StateTracker>();
        UtilsHomeBar.stateAction += SwitcherOfStates;

        // GameObject.Find("360ImageManager").GetComponent<MaterialManager>().UnloadMaterial();
        
        root = stateMachine.root;
        arChoose = root.Q<VisualElement>("ARChoose");
        UIExtentions.Display(arChoose, true);
        arBackChoose = arChoose.Q<Button>("BackChoose");
        arBackChoose.RegisterCallback<ClickEvent>(OnARBackChooseClicked);
        arNavigationButton = arChoose.Q<Button>("NavigationButton");
        arNavigationButton.RegisterCallback<ClickEvent>(OnARInfoPointClicked);
        arInfoPointButton = arChoose.Q<Button>("InfoPointButton");
        arInfoPointButton.RegisterCallback<ClickEvent>(OnARNavigationClicked);
    }

    private void OnARInfoPointClicked(ClickEvent evt)
    {
        ARBoundHandler.Instance.infoPointState.InfoPointStateHandler(InfoPointState.IPState.IsAtInfo);
        info = true;
        var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
        UIExtentions.Display(arChoose, false); 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        arSession.Reset();
        
        arElem.enabled = true;
        arElem.pageIndex = 1;
        OpacityBlueBorders(100f);
    }

    private void OnARNavigationClicked(ClickEvent evt)
    {
        ARBoundHandler.Instance.infoPointState.InfoPointStateHandler(InfoPointState.IPState.NotAtInfo);
        info = false;
        var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
        UIExtentions.Display(arChoose, false);
        arSession.Reset();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        arElem.enabled = true;
        arElem.pageIndex = 0;
        OpacityBlueBorders(100f);  
    }

    private void OnARBackChooseClicked(ClickEvent evt)
    {
        UIExtentions.Display(arChoose, false);
        var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
        
        if(arElem.state.gameState != StateTracker.GameState.Home)
        {
            UIExtentions.Display(arElem.blueHead, true);
        }
        else
        {
            UIExtentions.Display(arElem.blueHead, false);
        }
        
        // OpacityBlueBorders(100f);

        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        // arElem.ToggleState_AR(true);
        UtilsHomeBar.ar = false;
        info = false;
        SwitcherOfStates();
    }

    public override void Exit()
    {
        GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().DeselectAllObjects();
        arBackChoose.UnregisterCallback<ClickEvent>(OnARBackChooseClicked);
        arNavigationButton.UnregisterCallback<ClickEvent>(OnARNavigationClicked);
        arInfoPointButton.UnregisterCallback<ClickEvent>(OnARInfoPointClicked);
        UtilsHomeBar.DeselectPOI();
        // UtilsHomeBar.stateAction -= SwitcherOfStates;
    }
    public override void Tick()
    {
        // Debug.Log(GameObject.Find("JsonHandler").GetComponent<ObjectSelectionManager>().selectedObjects[0].name);
    }
    private void SwitcherOfStates()
    {   
        if (stateTracker.gameState == StateTracker.GameState.Home)
        {
            stateMachine.SwitchState(new HomePageState(stateMachine));
        }
        if (stateTracker.gameState == StateTracker.GameState.Routes)
        {
            stateMachine.SwitchState(new InfoState(stateMachine));
        }
        if (stateTracker.gameState == StateTracker.GameState.Map)
        {
            stateMachine.SwitchState(new MapState(stateMachine));
        }
        if (stateTracker.gameState == StateTracker.GameState.Settings)
        {
            stateMachine.SwitchState(new SettingsState(stateMachine));
        }
    }

    public void OpacityBlueBorders(float opacity)
    {
        var arElem = GameObject.Find("UIDocument").gameObject.GetComponent<ARElement>();
        arElem.blueHead.style.opacity = new StyleFloat(opacity);
        arElem.blueBottom.style.opacity = new StyleFloat(opacity);
    }
}
