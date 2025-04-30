using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


// [DefaultExecutionOrder(-10)]
public class AddressablesDownLoader : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement root;
    VisualElement loadingPage;
    VisualElement startPage;
    VisualElement fill;

    Label title;
    TextClassTranslate titletext = new TextClassTranslate("Πολιτιστικές Διαδρομές", "Cultural Routes");
    Label desc;
    TextClassTranslate descText = new TextClassTranslate("Ιστορικά Μνημεία Ηρακλείου", " Historical Monuments of Heraklion");


    Label loading;
    TextClassTranslate loadingText = new TextClassTranslate("Φόρτωση...", "Loading");
    UnityEngine.UIElements.Button startButton;
    bool start = false;
    public GameObject MessagesPanel;
    public TMP_Text messageText;
    TextClassTranslate enableText = new TextClassTranslate("Δεν υπάρχει σύνδεση στο διαδίκτυο, παρακαλώ ελέγξτε τη σύνδεσή σας και προσπαθήστε ξανά"
    , "No internet connection, please check your internet connection and try again");
    [SerializeField] private AssetReference addressableSceneCore;
    // [SerializeField] private AssetReference tutorial;
    [SerializeField] AsyncOperationHandle<SceneInstance> handle;

    AsyncOperationHandle initializationOperation;
    AsyncOperationHandle downloadOperation;
    bool isInCache;

    public bool firstInitialized;
    public Action<bool> OnNetworkReachabilityChanged;
    public float time = 3f;
    private NetworkReachability previousNetworkReachability;
    
    public Sprite espaG;
    public Sprite espaE;
    VisualElement espa;

    private const string AddressableVersionKey = "AddressableCacheVersion";
    public int currentAppVersion = 0; // Increment for each app update
    
    void InitializeUIDocument()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        loadingPage = root.Q<VisualElement>("Root");
        UIExtentions.Display(loadingPage, false);
        startPage = root.Q<VisualElement>("StartPageTemp");
        UIExtentions.Display(startPage, true);

        title = startPage.Q<Label>("LoadTitle");
        title.text = titletext.GetTranslatedText();
        desc = startPage.Q<Label>("LoadDesc");
        desc.text = descText.GetTranslatedText(); 

        loading = loadingPage.Q<Label>("LoadingLabel");

        espa = startPage.Q<VisualElement>("Espa");
        Sprite esp = UIExtentions.IsEnglish() ? espaE : espaG;
        
        espa.style.backgroundImage = new StyleBackground(esp);
        // fill = startPage.Q<VisualElement>("fill");
        // UIExtentions.Display(fill, true);
        // fill.style.opacity = 100;
    }
    public  void Awake() 
    {
        InitializeUIDocument();
        messageText.text = enableText.GetTranslatedText();
        this.gameObject.SetActive(true);
        MessagesPanel.SetActive(false);
        loading.text = loadingText.GetTranslatedText();
        
        CheckAndUpdateCache();
    }
    private void Start()
    {   
        // Initialize the previous network reachability status
        previousNetworkReachability = Application.internetReachability;
        StartCoroutine(SplashScreen(time));
    }
    IEnumerator SplashScreen(float sec)
    {
        UIExtentions.Display(loadingPage, true);
        yield return new WaitForSeconds(sec);
        UIExtentions.Display(startPage, false);   
        FindObjectOfType<Canvas>().sortingOrder = 1;
        CheckNetworkReachability();
    }
    private void Update() 
    {    
        HandleProgressBars();
        if (Application.internetReachability != previousNetworkReachability)
        {
            // Update the previous network reachability status
            previousNetworkReachability = Application.internetReachability;

            CheckNetworkReachability();
        }
    }
    private void HandleProgressBars()
    {
        if (initializationOperation.IsValid())
        {
            UpdateProgress(initializationOperation.PercentComplete, "Initializing");
        }
        if (downloadOperation.IsValid())
        {
            UpdateProgress(downloadOperation.GetDownloadStatus().Percent, "Downloading");
        }
        if (handle.IsValid())
        {
            UpdateProgress(handle.PercentComplete, "Loading Scene");
        }
    }
    private void UpdateProgress(float percent, string operation)
    {
        GetComponent<CustomSliderLoadingPage>().progressBar.lowValue = 0;
        int progressValue = Mathf.RoundToInt(percent * 100);
        GetComponent<CustomSliderLoadingPage>().progressBar.lowValue = progressValue;
        GetComponent<CustomSliderLoadingPage>().progressBar.title = $"{operation}: {progressValue} %";
    }
    private void CheckNetworkReachability()
    {
        bool isNetworkReachable = previousNetworkReachability != NetworkReachability.NotReachable;

        if (isNetworkReachable)
        {
            // If network is reachable, hide the message panel
            MessagesPanel.SetActive(false);
            StartCoroutine(LoadAddressableScene(addressableSceneCore));
        }
        else
        {
            // If network is not reachable, display the message panel
            MessagesPanel.SetActive(true);
        }
    }
    private void CheckAndUpdateCache()
    {
        int savedVersion = PlayerPrefs.GetInt(AddressableVersionKey, 0);

        if (currentAppVersion > savedVersion)
        {
            Debug.Log("AD: New content version detected. Clearing cache.");
            Caching.ClearCache();
            PlayerPrefs.SetInt(AddressableVersionKey, currentAppVersion);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("AD: Cache is up-to-date.");
        }
    }
    IEnumerator LoadAddressableScene(AssetReference addressableScene)
    {      
        // Check network connectivity
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("AD: No internet access detected. Please enable data or connect to a network.");
            // yield break; // Exit the coroutine if there's no internet access
        }

        initializationOperation = Addressables.InitializeAsync();
        yield return initializationOperation;             
        
        // Check if the scene is already cached
        var downloadSize = Addressables.GetDownloadSizeAsync(addressableScene);        
        yield return downloadSize;

        isInCache = downloadSize.Result == 0;

        if (isInCache)
        {
            // Scene is already cached, load it directly
            handle = Addressables.LoadSceneAsync(addressableScene, LoadSceneMode.Single);
            Resources.UnloadUnusedAssets();
            Debug.Log("AD: Loading");
            yield return handle;
        }
        else
        {
            // Scene is not cached, download it first
            downloadOperation = Addressables.DownloadDependenciesAsync(addressableScene, true);            
            yield return downloadOperation;

            // Load the scene after downloading
            handle = Addressables.LoadSceneAsync(addressableScene, LoadSceneMode.Single);
            Resources.UnloadUnusedAssets();
            Debug.Log("AD: DownLoading");
            yield return handle;

            // Release the handle after loading
            // Addressables.Release(handle);
        }
        Addressables.Release(handle);
        this.gameObject.SetActive(false);
        // Now the scene is loaded and you can proceed to the next scene or perform other actions
    }
}
