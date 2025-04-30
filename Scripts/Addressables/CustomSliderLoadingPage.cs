using UnityEngine;
using UnityEngine.UIElements;

public class CustomSliderLoadingPage : MonoBehaviour
{
    VisualElement root;
    // public Slider Slider;
    public ProgressBar progressBar;
    // VisualElement Dragger;
    // VisualElement map;
    // VisualElement Bar;
    // VisualElement NewDragger;

    public static bool initialized = false;

    private void Start() 
    {
        initialized = false;
    }
    private void OnEnable() 
    {
        if(initialized == false)
        {
            initialized = true;
            root = GetComponent<UIDocument>().rootVisualElement;
            // map = root.Q<VisualElement>("Root");
            // Slider = map.Q<Slider>("DistanceSlider");
            // Dragger = Slider.Q<VisualElement>("unity-dragger"); 
            progressBar = root.Q<ProgressBar>();
            UIExtentions.Display(progressBar, true);

            // AddElements();
            // Slider.RegisterCallback<ChangeEvent<float>>(SliderValueChanged);
            // Slider.RegisterCallback<GeometryChangedEvent>(SliderInit);
        }
        else
        {
            Debug.Log("Slider Already Initialized");
        }
    }
    private void OnDisable() {
        initialized = true;
    }

    // private void SliderInit(GeometryChangedEvent evt)
    // {
    //     Vector2 dist = new Vector2((NewDragger.layout.width - Dragger.layout.width ) /2, (NewDragger.layout.height - Dragger.layout.height) / 2 );
    //     Vector2 pos = Dragger.parent.LocalToWorld(Dragger.transform.position);
    //     NewDragger.transform.position = NewDragger.parent.WorldToLocal(pos-dist);
    // }

    // private void SliderValueChanged(ChangeEvent<float> evt)
    // {
    //     Vector2 dist = new Vector2((NewDragger.layout.width - Dragger.layout.width ) / 2, (NewDragger.layout.height - Dragger.layout.height) / 2 );
    //     Vector2 pos = Dragger.parent.LocalToWorld(Dragger.transform.position);
    //     NewDragger.transform.position = NewDragger.parent.WorldToLocal(pos-dist);
    // }

    // void AddElements()
    // {
    //     Bar = new VisualElement();
    //     Dragger.Add(Bar);
    //     Bar.name = "Bar";
    //     Bar.AddToClassList("fillBar");

    //     NewDragger = new VisualElement();
    //     Slider.Add(NewDragger);
    //     NewDragger.name = "NewDragger";
    //     NewDragger.AddToClassList("newdragger");
    //     NewDragger.pickingMode = PickingMode.Ignore;
    // }
}
