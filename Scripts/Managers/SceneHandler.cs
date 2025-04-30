
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// [DefaultExecutionOrder(-2)]
public class SceneHandler : MonoBehaviour
{
    // public UIDocument uiDocument;
    // VisualElement root;
    // VisualElement startPage;
    // VisualElement loadPage;
    public GameObject loadingPage;
    // SceneManager sceneManager;
    [SerializeField] private AssetReference addressableSceneCore;
    private AsyncOperationHandle<SceneInstance> handle;
    float progress;
    // ProgressBar progressBar;  
    // Label loading;
    [SerializeField] public Image panel;
    private float alphaTransitionSpeed = 0.3f;  // Speed to fade in the panel's alpha
    private float targetAlpha = 1f;
    private bool isLoadingScene = false;
    public void Awake()
    {
        // uiDocument = loadingPage.GetComponent<UIDocument>();
        // root = uiDocument.rootVisualElement;
        // // startPage = root.Q<VisualElement>("StartPageTemp");
        // // UIExtentions.Display(startPage, false);
        // loadPage = root.Q<VisualElement>("Root");
        // UIExtentions.Display(loadPage, true);
        // loading = loadPage.Q<Label>("LoadingLabel");
        // loading.text = loadingText.GetTranslatedText();

        // progressBar = loadPage.Q<ProgressBar>("ProgressBar");
        // UIExtentions.Display(progressBar, false);
        // loadingPage.SetActive(false);    
    }
    private void Update() 
    {
        if (isLoadingScene)
        {
            // Gradually increase the alpha of the panel
            var col = panel.color;
            col.a = Mathf.MoveTowards(col.a, targetAlpha, Time.deltaTime * alphaTransitionSpeed);
            panel.color = col;

            // Debugging log to verify alpha value in real-time
            Debug.Log($"Alpha: {col.a}");

            // If the handle is valid and loading is complete, we stop further updates
            if (handle.IsValid() && handle.IsDone)
            {
                Debug.Log("Scene loading completed.");
                isLoadingScene = false;
            }
        }
    }
    public void LoadNextSceneHandler()
    {
        loadingPage.SetActive(true); 
        handle = Addressables.LoadSceneAsync(addressableSceneCore, LoadSceneMode.Single);
        isLoadingScene = true;
        Resources.UnloadUnusedAssets();
    }
}
