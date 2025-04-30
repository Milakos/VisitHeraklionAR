
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingPageTutorial : MonoBehaviour
{
    public UIDocument uIDocument;
    VisualElement root;
    public ProgressBar progressBar;

    private void Awake() 
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        progressBar = root.Q<ProgressBar>();
    }
    private void Update() 
    {
        // progressBar.value = percent * 100;
    }
}
