using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AddressableAssets;
// using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace PointsOfInterests
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(PointOfInterest))]
    public class EditorEnum : Editor
    {
        private SerializedProperty has360ImageProp;
        private SerializedProperty sprite360Prop;
        private void OnEnable() 
        {
            has360ImageProp = serializedObject.FindProperty("has360Image");
            sprite360Prop = serializedObject.FindProperty("sprite360");
        }
        public override void OnInspectorGUI()
        {
            // Update the serialized object
            serializedObject.Update();

            // Draw the default inspector for the script (without the serialized properties)
            DrawDefaultInspector();

            // Conditionally show the sprite field if has360Image is true
            if (has360ImageProp.boolValue)
            {
                EditorGUILayout.PropertyField(sprite360Prop);
            }

            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }         
    }
    #endif
    
    
    [System.Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid)
        {
        }
    }
    [RequireComponent(typeof(BoxCollider))]
    public class PointOfInterest : MonoBehaviour
    {

        public enum FilterID { Monument , Museum, Park, Temple, PlayGround, RecreationGround, ShoppingStreet, CulturalFacility}
        public enum Path { CoastalRoute, VenetianWallsRoute, VenetianGatesRoute, VenetianPortRoute, DermatasCoveRoute }
        [SerializeField] public bool isInMorePaths;
        [SerializeField] public bool isInMoreThanThree;
        public bool has360Image;
        public bool hasWebSite;
        public bool hasTel;
        public bool hasTicket;
        bool spritesloaded = false;
        public enum Route 
        {
            First = 0, Second = 1, Third = 2, Forth = 3, Fifth = 4, Sixth = 5, Seventh = 6, Eighth = 7, Nineth = 8,
            Tenth = 9, Eleventh = 10, Twelve = 11 
        }
        [SerializeField] public FilterID filterID;

            private readonly Dictionary<FilterID, int> filterSpriteIndices = new Dictionary<FilterID, int>
            {
                {FilterID.Monument, 0},
                {FilterID.Museum, 1},
                {FilterID.Park, 2},
                {FilterID.Temple, 3},
                {FilterID.PlayGround, 4},
                {FilterID.RecreationGround, 5},
                {FilterID.ShoppingStreet, 6},
                {FilterID.CulturalFacility, 7},
            };

        [SerializeField] public Path pathID;
            private readonly Dictionary<Path, int> pathIndices = new Dictionary<Path, int>
            {
                {Path.CoastalRoute, 0},
                {Path.VenetianWallsRoute, 1},
                {Path.VenetianGatesRoute, 2},
                {Path.VenetianPortRoute, 3},
                {Path.DermatasCoveRoute, 4}
            };
            [HideInInspector]
            public Path AdditionalPath;
        
        [SerializeField] Route route;
        public int routeIndex;

            [HideInInspector]
            public Route AdditionalRoute;

    [SerializeField]  public string POIlocation;
    [HideInInspector] public Vector2d locationVector;
    [HideInInspector]  private Vector2d offset;
    const float offsetYMap = -0.004f;
    const float offsetYRoutes = -0.007f;
    const float offsetX = 0f;

    [HideInInspector] public string filterG;
    [HideInInspector] public string filterE;
        [HideInInspector] public string GreekRef;
        [HideInInspector] public string EnglishRef;
        [HideInInspector] public string GreekRefD;
        [HideInInspector] public string EnglishRefD;
        [HideInInspector] public string Contact;
        [HideInInspector] public string addressE;
        [HideInInspector] public string addressG;
        [HideInInspector] public string ticketE;
        [HideInInspector] public string ticketG;
        [HideInInspector] public string entranceE;
        [HideInInspector] public string entranceG;
        [SerializeField] AssetReference mainSpritePath;
        [HideInInspector] public Material sprite360;
         private AsyncOperationHandle<Material> materialHandle;
         
        // [SerializeField] private AssetReference FilterIDIconGrey;
        [HideInInspector] public Sprite filterIDIconSprite;
        [HideInInspector] public bool isPressed; 
        [HideInInspector] public bool isSelected;
        public bool isActive = true; 
        [SerializeField] public SpriteRenderer mainRenderer;
        [SerializeField] public SpriteRenderer filterRenderer;

        [SerializeField] public SpriteRenderer frame;
        [SerializeField] public SpriteRenderer filterFrame;

        [SerializeField] private AssetLabelReference spriteAssets;

        //[HideInInspector] 
        public List<Sprite> sprites;
        public int numberOfSprites {get; private set;}
        
        private const float xFrameSize = -0.21f;
        private const float yFrameSize = -0.18f;
        Collider col;
        [SerializeField] GameObject ChildPOI;
        
        [HideInInspector] public AudioSource source;
        [SerializeField] AssetReferenceAudioClip _clipE;
        [SerializeField] AssetReferenceAudioClip _clipG;
        AudioClip audioE;
        AudioClip audioG;
        [SerializeField] public TMP_Text textCounter;
        private HashSet<FilterID> activeFilters = new HashSet<FilterID>();

        public static  Action PopUpEvent;
        public static Action<int, List<Sprite>> PopUpImages; 
        public static Action<int, List<Sprite>> PopUpImagesAR; 
        public static Action<int, List<Sprite>> InfoImages; 
        VisualElement parent;
        UIDocument uiDocument;
        StateTracker stateTracker;
        private void Awake() 
        {
            CachingJsonData();
            OnAudioLoaded();
            // if(has360Image == true)  
            //     OnMaterialLoaded();
            uiDocument = FindObjectOfType<UIDocument>();
            parent = uiDocument.rootVisualElement;
            FindObjectOfType<SpawnOnMap>().spriteEvent += LoadSprites; 
            stateTracker = FindObjectOfType<StateTracker>();
            // if(this.CompareTag("Route1"))
            // {
            //     var objectList = FindObjectOfType<SpawnOnMap>();
            //     objectList._spawnedObjects.Add(this.gameObject); 
            //     objectList._spawnedObjects = objectList._spawnedObjects.OrderBy(route => route.GetComponent<PointOfInterest>().route).ToList();
            // }
            

// LoadSprites();
            // await OnRestOfSpritesLoadedAsync(); 
            routeIndex = (int)route; 
            textCounter.text = (routeIndex + 1).ToString();
            textCounter.enabled = false;
        }
        private void OnEnable() 
        {            
            FindObjectOfType<SpawnOnMap>().spriteEvent += LoadSprites; 
            
            // LoadSprites();
            // OnRestOfSpritesLoaded(); 

            
            UtilsHomeBar.FilterEvent += HandleFilterEvent;  
            UtilsHomeBar.SendPathIndex += FilterPathHandler;
            UtilsHomeBar.SendPathIndex += AssignPathPoi;
           
            UtilsHomeBar.POIselectedButton += SelectionFromInfoState;
            UtilsHomeBar.poiSelectStart += SelectFromStart;
            // UtilsHomeBar.POIselectedButton += SendImagesToInfo;
            
            UtilsHomeBar.RestorePath += RestorePOI;
            
            UtilsHomeBar.setActiveToFalse += InMorePathsToFalse;

            UtilsHomeBar.toggleNumberSprite += ToggleNumberAndSprite;
        }
        private void Start()
        {                                  
            col = GetComponent<Collider>();      
            AutoAssignFilterSprite();  
            source = GetComponent<AudioSource>();
            
            InMorePathsToFalse(false);
            // FindObjectOfType<ARElement>().intantiateLocations += AssignLocations;
            // int parentLayer = gameObject.layer;
            // SetChildrenLayerRecursively(transform, parentLayer);
            // OnRestOfSpritesLoaded();    
            CultureInfo cultureInfo = new CultureInfo("en-US");
            string[] coordinates = POIlocation.Split(',');
            if (coordinates.Length == 2)
            {
                

                // Parse the substrings into float values
                if (coordinates.Length == 2 
                && double.TryParse(coordinates[0], NumberStyles.Any, cultureInfo, out double latitude) 
                && double.TryParse(coordinates[1],  NumberStyles.Any, cultureInfo, out double longitude))
                {
                    // Create a Vector2d instance with the parsed values
                    locationVector = new Vector2d(latitude, longitude);
                    // Debug.Log("Location Vector: " + locationVector);
                }
                else
                {
                    Debug.LogError("Error parsing latitude and longitude values.");
                }
            }
            else
            {
                Debug.LogError("Invalid POIlocation string format.");
            }
        }

        private void SetChildrenLayerRecursively(Transform parent, int layer)
        {
            foreach (Transform child in parent)
            {
                // Set the layer of the child
                // child.gameObject.layer = layer;

                // Recursively set the layer for all grandchildren
                SetChildrenLayerRecursively(child, layer);
            }
        }

        private void Update()
        {
            TouchPOIHandler();

            if (UtilsHomeBar.mapViewCheck == false)
            {
                col.enabled = false;
            }
            else
            {
                if(isActive == true)
                {
                    col.enabled = true;
                }
            }
        }

        private void OnDisable() 
        {
            UtilsHomeBar.FilterEvent -= HandleFilterEvent;
            UtilsHomeBar.SendPathIndex -= FilterPathHandler; 
            UtilsHomeBar.SendPathIndex -= AssignPathPoi;
            UtilsHomeBar.poiSelectStart -= SelectFromStart;
                        //  UtilsHomeBar.RestorePath -= RestorePOI;
                                                                                // UtilsHomeBar.setActiveToFalse -= InMorePathsToFalse;
            UtilsHomeBar.POIselectedButton -= SelectionFromInfoState;
            // if(sprites != null)
            //     sprites.Clear();
            UtilsHomeBar.toggleNumberSprite -= ToggleNumberAndSprite;
        }
        private void AssignPathPoi(int obj)
        {
            if(this.gameObject.activeInHierarchy)
            {
                var selectionManager = FindObjectOfType<ObjectSelectionManager>();
                selectionManager.pathObjects.Add(this);
                selectionManager.pathObjects.Sort((a, b) => a.route.CompareTo(b.route));
                // PopUpImages?.Invoke(this.GetNumberOfSprites(), this.sprites); 
            }
        }
        private void RestorePOI()
        {
            if(!this.gameObject.activeInHierarchy)
            {
                this.gameObject.SetActive(true);
                // FindObjectOfType<ObjectSelectionManager>().pathObjects.Clear();
            }
        }
        private void SelectionFromInfoState(int obj)
        {
            if(this.gameObject.activeInHierarchy)
            {
                // FindObjectOfType<ObjectSelectionManager>().pathObjects.Add(this);
                if(TranlatePathToInt() == 0)
                {
                    var point = FindObjectOfType<SpawnOnMap>()._spawnedObjects[obj].GetComponent<PointOfInterest>();
                    // var point = FindObjectOfType<SpawnOnMap>().firtsRoutePOIs[obj].GetComponent<PointOfInterest>();
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(point);      
                    HandlePOISelectionFromButtonOrTouch(point); 
                }
                else if(TranlatePathToInt() == 1)
                {
                    var point = FindObjectOfType<SpawnOnMap>()._spawnedObjectsSecond[obj].GetComponent<PointOfInterest>();
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(point);
                    HandlePOISelectionFromButtonOrTouch(point); 
                }
                else if(TranlatePathToInt() == 2)
                {
                    var point = FindObjectOfType<SpawnOnMap>()._spawnedObjectsThird[obj].GetComponent<PointOfInterest>();
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(point);
                    HandlePOISelectionFromButtonOrTouch(point); 
                }
                else if (TranlatePathToInt() == 3)
                {
                    var point = FindObjectOfType<SpawnOnMap>()._spawnedObjectsForth[obj].GetComponent<PointOfInterest>();
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(point);
                    HandlePOISelectionFromButtonOrTouch(point); 
                }
                else if(TranlatePathToInt() == 4)
                {
                    var point = FindObjectOfType<SpawnOnMap>()._spawnedObjectsFifth[obj].GetComponent<PointOfInterest>();
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(point);
                    HandlePOISelectionFromButtonOrTouch(point); 
                }                               
            }
        }
        private void SelectFromStart(int intV)
        {
            if(this.gameObject.activeInHierarchy && route == 0)
            {
                if(UtilsHomeBar.mapViewCheck == false)
                {
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(this);
                }
                else if(UtilsHomeBar.mapViewCheck == true)
                {
                    FindObjectOfType<ObjectSelectionManager>().SelectObject(this);
                    HandlePOISelectionFromButtonOrTouch(this);
                }
                
            } 
            // if(UtilsHomeBar.mapViewCheck == true)
            // {
            //     HandlePOISelectionFromButtonOrTouch(this);
            // }
            
        }
        /// <summary>
        /// used forresetting the activation of the object when leave the page
        /// </summary>
        /// <param name="check"></param>
        private void InMorePathsToFalse(bool check)
        {           
            if(isInMorePaths)
            {
                this.gameObject.SetActive(check);
            }  
        }
        private void FilterPathHandler(int pathIndex)
        {
            if(pathIndex != TranlatePathToInt())
            {
                this.gameObject.SetActive(false);
            }   
        }

    #region HandleFilterFunctionality
            /// <summary>
            /// Handles the event when a filter is activated or deactivated.
            /// </summary>
            /// <param name="filter">The filter ID.</param>
            /// <param name="active">True if the filter is activated, false if deactivated.</param>
            private void HandleFilterEvent(FilterID filter, bool active)
            {           
                if (active)
                {
                    activeFilters.Add(filter);
                    
                }
                else
                {
                    activeFilters.Remove(filter);
                }
                
                HandleFilterLogic();
            }
            /// <summary>
            /// Handles the logic for applying filters and updating object visibility.
            /// </summary>
            private void HandleFilterLogic()
            {
                if (activeFilters.Count > 0)
                {
                    // If any filters are active, show only objects with those filters
                    isActive = activeFilters.Contains(filterID);
                    
                }
                else
                {
                    // If no filters are active, show all objects
                    isActive = true;
                }
                ChildPOI.SetActive(isActive);
                col.enabled = isActive;
            }
    #endregion HandleFilterFunctionality
        private void LoadSprites()
        {
            Addressables.LoadAssetAsync<Sprite>(mainSpritePath).Completed += OnMainSpriteLoaded;          
        }
        private async Task OnRestOfSpritesLoadedAsync()
        {
            // sprites = new List<Sprite>();
            var handle = Addressables.LoadAssetsAsync<Sprite>(spriteAssets, (sprite) => 
            {
                sprites.Add(sprite);
                SetNUmberOfSprites(sprites.Count);
            });

            await handle.Task;

            // Release the handle after all sprites have been loaded
            Addressables.Release(handle);
            spritesloaded = true;
        }
        public async Task OnMaterialLoaded()
        {
            materialHandle = Addressables.LoadAssetAsync<Material>(spriteAssets);
            await materialHandle.Task;

            if (materialHandle.Status == AsyncOperationStatus.Succeeded)
            {
                sprite360 = materialHandle.Result;
            }
            else
            {
                Debug.LogError("Failed to load material");
            }
        }
        public void OnMaterialUnLoaded()
        {
            // Release the handle
            if (materialHandle.IsValid())
            {
                Addressables.Release(materialHandle);
                sprite360 = null;
            }
        }
        private void OnAudioLoaded()
        {
            Addressables.LoadAssetAsync<AudioClip>(_clipE).Completed += handle =>
            {
                audioE = handle.Result;
                // Addressables.Release(handle);
            };
            Addressables.LoadAssetAsync<AudioClip>(_clipG).Completed += handle =>
            {
                audioG = handle.Result;
                // Addressables.Release(handle);
            };
        }
        public void SetNUmberOfSprites(int value)
        {
            numberOfSprites = value;
        }
        public int GetNumberOfSprites()
        {
            return numberOfSprites;
        }
        private void OnMainSpriteLoaded(AsyncOperationHandle<Sprite> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                mainRenderer.sprite = obj.Result;
                mainRenderer.size = new Vector2(xFrameSize, yFrameSize);
            }
            else
            {
                Debug.LogWarning($"Failed to load main sprite at path: {mainSpritePath}. Error: {obj.OperationException}");
            }
            Addressables.Release(obj);
        }
        
        /// <summary>
        /// This Method is responsible for detection of a finger above POI so to return a boolean value
        /// This will Triggers some events TODO//////////
        /// </summary>
        private void TouchPOIHandler()
        {
            if(UtilsHomeBar.ar == false)
            {
            if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (
                                Physics.Raycast(ray, out hit) && hit.collider == col && UtilsHomeBar.popUpEnabled == false
                                || Physics.Raycast(ray, out hit) && hit.collider == col && UtilsHomeBar.mapViewCheck == true
                            )
                            {
                                HandlePOISelectionFromButtonOrTouch(this);
                                FindObjectOfType<ObjectSelectionManager>().SelectObject(this);
                                Debug.Log("Button Pressed!");
                                isPressed = true; 
                                if(stateTracker.gameState == StateTracker.GameState.Map)
                                {
                                    offset = new Vector2d(offsetYMap, offsetX);
                                    FindObjectOfType<AbstractMap>().UpdateMap(locationVector + offset);
                                }
                                else if(stateTracker.gameState == StateTracker.GameState.Routes)
                                {
                                    offset = new Vector2d(offsetYRoutes, offsetX);
                                    FindObjectOfType<AbstractMap>().UpdateMap(locationVector + offset);
                                }
                                
                            }
                            break;
                        case TouchPhase.Ended:
                            // Reset button state when touch is released
                            if (isPressed)
                            {
                                isPressed = false;
                            }
                            break;
                    }
                }
            }
            
        }
        public async void HandlePOISelectionFromButtonOrTouch(PointOfInterest point)
        {   
            AssignAudio();  
            //this change
            if(this.spritesloaded == false)
            {
                await OnRestOfSpritesLoadedAsync(); 
            }
            
            if(UtilsHomeBar.ar)
            {
                if(PopUpImagesAR != null)
                {
                    PopUpImagesAR?.Invoke(this.GetNumberOfSprites(), point.sprites);
                }
                else
                {
                    Debug.LogError("A: Images Not Listeners");
                }
            }
            else
            {
                PopUpImages?.Invoke(this.GetNumberOfSprites(), point.sprites); 
                PopUpEvent?.Invoke();   
            }
                
        }
        public void SendImagesToInfo(PointOfInterest point)
        {   
            InfoImages?.Invoke(this.GetNumberOfSprites(), this.sprites);            
        }
        /// <summary>
        /// A Method that each object that helds this componenet e.g. PointFfInterest.cs take the
        /// Coordinate from Json depending on which path it belongs, and which route POI it is
        /// </summary>
        private void CachingJsonData()
        {   
            int result = TranlatePathToInt();
            // JSONTest.Instance.globalPath = new GlobalPath[5]; 
            
            if(JSONTest.Instance.globalPath[result] != null)
            {
                if (pathID == Path.CoastalRoute || pathID == Path.VenetianWallsRoute 
                    || pathID == Path.VenetianGatesRoute 
                    || pathID == Path.VenetianPortRoute || pathID == Path.DermatasCoveRoute)
                {
                    int routeIndex = (int)route; 
                    int poiIndex = (int)pathID;
                    
                    if (routeIndex >= 0 && routeIndex < JSONTest.Instance.globalPath[result].coordinates.Count)
                    {
                        POIlocation = JSONTest.Instance.globalPath[result].coordinates[routeIndex].ToString();
                        
                        GreekRef = JSONTest.Instance.globalPath[result].greekTextsRoute[routeIndex].Titles[0].ToString();
                        EnglishRef = JSONTest.Instance.globalPath[result].englishTextsRoute[routeIndex].Titles[0].ToString();
                        GreekRefD = JSONTest.Instance.globalPath[result].greekTextsRoute[routeIndex].Texts[0].ToString();
                        EnglishRefD = JSONTest.Instance.globalPath[result].englishTextsRoute[routeIndex].Texts[0].ToString();

                        var filter = JSONTest.Instance.globalPath[result].filterIDGreek[routeIndex].ToString();
                        
                        if (filter.Contains("_"))
                        {
                            filter = filter.Replace("_", " ");
                            
                            filterG = filter;
                        }   
                        else
                        {
                            filterG = filter;
                        }
                        

                        var filter_ = JSONTest.Instance.globalPath[result].filterIDEng[routeIndex].ToString();
                        if (filter_.Contains("_"))
                        {
                            filter_ = filter_.Replace("_", " ");
                            
                            filterE = filter_;
                        }  else
                        {
                            filterE = filter_;
                        }
                        

                        Contact = JSONTest.Instance.globalPath[result].contact[routeIndex].ToString();
                        addressE = JSONTest.Instance.globalPath[result].addressesE[routeIndex].ToString();
                        addressG = JSONTest.Instance.globalPath[result].addressesG[routeIndex].ToString();
                        entranceE = JSONTest.Instance.globalPath[result].openTimeEng[routeIndex].ToString();
                        entranceG = JSONTest.Instance.globalPath[result].openTime[routeIndex].ToString();
                        ticketE = JSONTest.Instance.globalPath[result].EntryE[routeIndex].ToString();
                        ticketG = JSONTest.Instance.globalPath[result].EntryG[routeIndex].ToString();
                    }
                    else
                    {
                        Debug.LogError($"Invalid route index: {routeIndex}");
                    }
                }  
            }
            else
            {
                Debug.LogError($"Invalid path ID: {pathID}");
            }
            
        }
        /// <summary>
        /// This Method Auto Assigns the FIlter Sprite like monument temple etc to each specific POI depend on the ID
        /// </summary>
        private void AutoAssignFilterSprite()
        {
            switch (filterID)
            {
                case FilterID.Monument:
                case FilterID.Museum:
                case FilterID.Park:
                case FilterID.Temple:
                case FilterID.PlayGround:
                case FilterID.RecreationGround:
                case FilterID.ShoppingStreet:
                case FilterID.CulturalFacility:
                    int spriteIndex;
                    if (filterSpriteIndices.TryGetValue(filterID, out spriteIndex))
                    {
                        filterRenderer.sprite = FindObjectOfType<DisplayPOIFilterHandler>().sprites[spriteIndex];
                        filterRenderer.color = UIExtentions.BlueDark;
                        filterIDIconSprite = FindObjectOfType<DisplayPOIFilterHandler>().icons[spriteIndex];
                    }
                    else
                    {
                        Debug.LogError("FilterID not found in dictionary.");
                    }
                    break;
                default:
                    Debug.LogError("Unhandled FilterID case.");
                    break;
            }
        }
        /// <summary>
        /// A pure Function that Returns the index of path based on the PathID
        /// </summary>
        /// <returns></returns>
        public int TranlatePathToInt()
        {
            int pathIndex = 0;
            switch (pathID)
            {
                case Path.CoastalRoute:
                case Path.VenetianWallsRoute:
                case Path.VenetianGatesRoute:
                case Path.VenetianPortRoute:
                case Path.DermatasCoveRoute:
                    
                    if(pathIndices.TryGetValue(pathID, out pathIndex))
                    {
                        return pathIndex;
                    }
                    else
                    {
                        Debug.LogError("PathID not found in dictionary.");
                    }
                    break;
                default:
                    Debug.LogError("Unhandled PathID case.");
                    break;
            }
            return pathIndex;
        }

#region  Audio
        private void AssignAudio()
        {
            if (UIExtentions.IsEnglish())
            {
                source.clip = audioE;
            }
            else
            {
                source.clip = audioG;
            }
            Debug.Log("ASSIGNAUDIO");
        }

#endregion Audio

        public void ToggleNumberAndSprite(bool toggle)
        {
            filterRenderer.enabled = !toggle;
            
            // if(toggle == true)
            // {
            //     filterFrame.color = UIExtentions.BlueDark;
            // }
            // else
            // {
            //     filterFrame.color = UIExtentions.white;
            // }
            
            textCounter.enabled = toggle;
        }
    }
}  