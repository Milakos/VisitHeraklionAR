using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TutorialMapData
{
    public string greekUp;
    public string engUp;
}
public class MapTutorial : MonoBehaviour
{
    Translate initPlace = new Translate(0, -80, 0);
    Translate firstPlace = new Translate(0, 0, 0);
    Translate SecondPlace = new Translate(0, 40, 0);
    
    public List<TutorialMapData> tutorialData = new List<TutorialMapData>();
    UIDocument uIDocument;
    VisualElement root;
    VisualElement map;
    VisualElement routeIcon;
    public VisualElement tutorial;
    VisualElement Holder;
    VisualElement filters;
    VisualElement RoutesTut;
    VisualElement Routes;
    VisualElement Dist;
    Button button;
    Label Up;
    public int indexCounter = 0;
    public int maxCounter;
    public bool initialized;
    private void Awake() 
    {
        uIDocument = GetComponent<UIDocument>();
        initialized = false;
        maxCounter = tutorialData.Count -1;
    }
    private void OnEnable()
    {
        root = uIDocument.rootVisualElement;
        map = root.Q<VisualElement>("MapPage");
        tutorial = map.Q<VisualElement>("TutorialMap");
        tutorial.style.backgroundColor = UIExtentions.whiteAlpha00;
        Holder = tutorial.Q<VisualElement>("TextHolder");
        Holder.style.translate = new StyleTranslate(initPlace);

        filters = tutorial.Q<VisualElement>("FiltersTut");
        UIExtentions.Display(filters, false);

        RoutesTut = tutorial.Q<VisualElement>("RoutesTut");
        UIExtentions.Display(RoutesTut, false);
        Dist = tutorial.Q<VisualElement>("DistTut");
        UIExtentions.Display(Dist, false);
        Routes = tutorial.Q<VisualElement>("RoutesTutIcon");
        UIExtentions.Display(Routes, false);

        UIExtentions.Display(tutorial, true);
        button = tutorial.Q<Button>("MapNextButton");
        Up = tutorial.Q<Label>("LabelUP");

        routeIcon = map.Q<Button>("RouteIcon");
        if(!initialized)
        {
            Initialize();
            
            initialized = true;
        }
        UpdateLabels();
    }

    public void Initialize()
    {
        UtilsHomeBar.mapViewCheck = false;
        button.clicked += OnNextButtonClick;
    }

    private void OnNextButtonClick()
    {
        if (indexCounter < maxCounter)
        {
            indexCounter++;

            if(indexCounter == 1)
            {
                UIExtentions.Display(filters, true);

                UIExtentions.Display(Routes, false);
                UIExtentions.Display(RoutesTut, false);
                UIExtentions.Display(Dist, false);
                tutorial.style.backgroundColor = UIExtentions.greyTutorial;
                Holder.style.translate = new StyleTranslate(firstPlace);
            }
            else if (indexCounter == 2)
            {
                UIExtentions.Display(filters, enabled: false);
                UIExtentions.Display(RoutesTut, true);
                
                UIExtentions.Display(Routes, true);
                
                UIExtentions.Display(Dist, false);
                tutorial.style.backgroundColor = UIExtentions.greyTutorial;
                Holder.style.translate = new StyleTranslate(SecondPlace);
            }
            else if(indexCounter == 3)
            {
                UIExtentions.Display(filters, enabled: false);
                UIExtentions.Display(RoutesTut, true);

                UIExtentions.Display(Routes, false);
                
                UIExtentions.Display(Dist, true);
                tutorial.style.backgroundColor = UIExtentions.greyTutorial;
            }
            else 
            {
                UIExtentions.Display(filters, false);
                
                UIExtentions.Display(Routes, false);
                UIExtentions.Display(RoutesTut, false);
                UIExtentions.Display(Dist, false);
                tutorial.style.backgroundColor = UIExtentions.whiteAlpha00;
                Holder.style.translate = new StyleTranslate(firstPlace);
            }

            UpdateLabels();
        }
        else
        {
            UtilsHomeBar.mapViewCheck = true;

            ResetState();
            Debug.Log("Reached the last set of tutorial data");
        }
    }

    private void ResetState()
    {
        UIExtentions.Display(tutorial, false);
        indexCounter = 0;
        UtilsHomeBar.mapViewCheck = true;
        this.enabled = false;
    }

    private void OnDisable() 
    {
        UIExtentions.Display(tutorial, false);
    }
    private void UpdateLabels()
    {
        if (UIExtentions.IsEnglish())
        {
            Up.text = tutorialData[indexCounter].engUp;
        }
        else
        {
            Up.text = tutorialData[indexCounter].greekUp;          
        }
        
    }
}
