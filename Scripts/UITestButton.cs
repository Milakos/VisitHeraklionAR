using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TutorialData
{
    public Sprite icon;
    public string TitleTextE;
    public string TitleTextG;
    public string descriptionTextE;
    public string descriptionTextG;
    public Vector2 dimensions;
    
}
public class UITestButton : MonoBehaviour
{
    [SerializeField] private TutorialData[] firstData = new TutorialData[5];
    [SerializeField] private Sprite rectPointBar;
    [SerializeField] private Sprite point;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip click; 
    public UIDocument ui;
    private VisualElement root;
    VisualElement img;
     public int pageIndex = 1;
    public InputIntro inputManager;
    SwipeIntro swipeDetection;
    private Button nextButton;
    Button skipLabel;

    TextClassTranslate skipText = new TextClassTranslate("ΠΑΡΑΛΕΙΨΗ", "SKIP");

    public Sprite espaG;
    public Sprite espaE;
    VisualElement espa;



    public UIDocument uiDocument;
    VisualElement root2;
    VisualElement loadPage;
    public GameObject loadingPage;
    ProgressBar progressBar;  
    Label loading;
    TextClassTranslate loadingText = new TextClassTranslate("Φόρτωση...", "Loading...");

    private void Awake() 
    {
        inputManager = FindObjectOfType<InputIntro>();
        swipeDetection = FindObjectOfType<SwipeIntro>();
    }
    private void OnEnable()
    {
        ui = GetComponent<UIDocument>();

        root = ui.rootVisualElement;
        img = root.Q<VisualElement>("CentralImage");
        skipLabel = root.Q<Button>("SkipButton");
        espa = root.Q<VisualElement>("Espa");
        Sprite esp = UIExtentions.IsEnglish() ? espaE : espaG;
        
        espa.style.backgroundImage = new StyleBackground(esp);

        skipLabel.text = skipText.GetTranslatedText();

        //Subscribers...........................................

        swipeDetection.SwipeRightAction += NextButtonHandler;
        swipeDetection.SwipeLeftAction += NextButtonHandler;

        //Overisght Button
        SkipButtonInit();

        // Next Button
        NextButtonInit();
        //.......................................................

        root2 = uiDocument.rootVisualElement;
        loadPage = root2.Query<VisualElement>("Root");
        UIExtentions.Display(loadPage, true);
        loading = loadPage.Query<Label>("LoadingLabel");
        loading.text = loadingText.GetTranslatedText();

        progressBar = loadPage.Q<ProgressBar>();
        UIExtentions.Display(progressBar, false);
        
    }
    private void Start() 
    {
            NextButtonHandler(0) ;
            loadingPage.SetActive(false);  
    }
    public void SkipButtonInit()
    {
        Button skipbutn = root.Query<Button>("SkipButton").First();

        skipbutn.clicked += () =>
        {
            SkipButtonHandler();
        };
    }

    public void NextButtonInit()
    {
        nextButton = root.Query<Button>("NextButton");
        nextButton.clicked += () =>
        {
            if (inputManager.pressed == false)
            {
                if (pageIndex <= firstData.Length - 1)
                {
                    pageIndex++;
                    NextButtonHandler(pageIndex);
                    ReduceOpacity("NextButton");
                    source.PlayOneShot(click);
                    Debug.Log("Tutorial");
                }
            }
        };
    }

    private void SkipButtonHandler()
    {
        NextSceneLoad();
    }
    public void NextButtonHandler(int index) 
    {
        if(index == firstData.Length )
        {           
            NextSceneLoad();
        }
        else if(index < 0)
        {
            pageIndex = 0;
            index = 0;
        }
        else if(index < firstData.Length)
        {        
            // Image Content ................................
            //  = root.Q<VisualElement>("CentralImage");
            img.style.backgroundImage = firstData[index].icon.texture;
            // img.style.width = firstData[index].dimensions.x;
            // img.style.height = firstData[index].dimensions.y;

            // Text Content ...................................
            
            VisualElement TitleContainer = root.Q("TitleElement");
            VisualElement descriptionContainer = root.Q("DescriptionElement");
            List<Label> title = TitleContainer.Query<Label>(className: "titles").ToList();
            List<Label> description = descriptionContainer.Query<Label>(className: "descriptions").ToList();           
            title[index].style.display = DisplayStyle.Flex;
            description[index].style.display = DisplayStyle.Flex;
            
            if(UIExtentions.IsEnglish())
            {
                title[index].text = firstData[index].TitleTextE;
                description[index].text = firstData[index].descriptionTextE;
            }
            else
            {
                title[index].text = firstData[index].TitleTextG;
                description[index].text = firstData[index].descriptionTextG;                
            }


            if(index > 0)
            {
                title[index - 1].style.display = DisplayStyle.None;
                description[index - 1].style.display = DisplayStyle.None;
            }
            if(index < pageIndex)
            {
                title[index + 1].style.display = DisplayStyle.None;
                description[index + 1].style.display = DisplayStyle.None;
            }
            
            // Point Bar Content
            // Point Indicator Bar
            VisualElement pointContainer = root.Q("PointIndexElement");
            List<VisualElement> points = pointContainer.Query(className: "points").ToList();
            points[index].style.backgroundImage = rectPointBar.texture;
            points[index].style.width = 40;
            
            if(index > 0)
            {
                points[index - 1].style.backgroundImage = point.texture;
                points[index -1].style.width = 10;
            }
            if(index < pageIndex)
            {
                points[index + 1].style.backgroundImage = point.texture;
                points[index + 1].style.width = 10;
            } 
            print("Next Page");
        }
        pageIndex = index;            
    }
    void NextSceneLoad() 
    {
        FindObjectOfType<SceneHandler>().LoadNextSceneHandler();
    }
    void ReduceOpacity(string buttonName) 
    {
        Button btn = root.Query<Button>(buttonName);
        
        btn.style.unityBackgroundImageTintColor = new Color(0, 148, 216);
        btn.style.width = 60;
        btn.style.height = 60;
        // Schedule a delayed action to restore opacity
        root.schedule.Execute(() => 
        {
            btn.style.unityBackgroundImageTintColor = Color.white;
            btn.style.width = 70;
            btn.style.height = 70;
        }).StartingIn(100);
    }
}
