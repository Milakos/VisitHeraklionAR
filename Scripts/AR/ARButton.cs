using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARButton : MonoBehaviour
{
    public enum Model
    {
        Ariadne = 0, Dominic = 1, None = 2
    }
    public Model model;

    public GameObject Ariadne;
    public GameObject Dominic;
    
    public int ID;
    public int path;
    public enum FilterID { Monument , Museum, Park, Temple, PlayGround, RecreationGround, ShoppingStreet, CulturalFacility}
    [SerializeField] public FilterID filterID;
    private HashSet<FilterID> activeFilters = new HashSet<FilterID>();
    private string nameOfPOIGreek, nameOfPOIEng, globalName;
    
    // OLD InfoPoint Fields
    [SerializeField] public TMP_Text title;
    [SerializeField] public Image image;
    [SerializeField] GameObject child;
    [SerializeField] public SpriteRenderer frameColor;

    public GameObject ARCamera;
    public bool isActive = true; 
    public bool pressed;
    public bool isInGroup;

    public bool isInLeftSide;
    public bool isGroup = false;
    
    private void OnEnable()
    {   
        if(isGroup == false)
        {
            ARCamera = GameObject.Find("AR Camera");
            child.SetActive(false);
            nameOfPOIGreek = JSONTest.Instance.globalPath[path].greekTextsRoute[ID].Titles[0].ToString();
            nameOfPOIEng = JSONTest.Instance.globalPath[path].englishTextsRoute[ID].Titles[0].ToString();
            UtilsHomeBar.FilterEventAR += HandleFilterEvent;
            // UtilsHomeBar.MakeChevronsBlue += MakeButtonBlue;
            MatchPOIwithARSigns();
            CheckStringTextsLocalization();  
        }
        else
        {
            UtilsHomeBar.FilterEventAR += HandleFilterEvent;
        }  
  
    }
    private void Start()
    {
        if(Ariadne != null)
        {
            Ariadne.SetActive(false);
        }
        if(Dominic != null)
        {
            Dominic.SetActive(false);  
        }        
    }
    private void OnDisable() 
    {
        UtilsHomeBar.FilterEventAR -= HandleFilterEvent;
        // UtilsHomeBar.MakeChevronsBlue -= MakeButtonBlue;
    }
    private void Update() 
    {
        if(UtilsHomeBar.ar == true)
        {
            if (isGroup == false)
            {
                TouchSign();
            }      
        } 
    }
    private void CheckStringTextsLocalization()
    {
        globalName = UIExtentions.IsEnglish() ? nameOfPOIEng : nameOfPOIGreek;
        title.text = globalName;
    }
    // Old infoPoint method
    private void MatchPOIwithARSigns()
    {
        foreach (var item in ARBoundHandler.Instance.points)
        {
            if(this.path == item.TranlatePathToInt() && this.ID == item.routeIndex)
            {
             
                this.image.sprite = item.filterRenderer.sprite;
                this.image.color = UIExtentions.BlueDark;
            }                   
        }
    }
    public void EventButton()
    {     
        foreach (var item in ARBoundHandler.Instance.points)
        {
            if(this.path == item.TranlatePathToInt() && this.ID == item.routeIndex)
            {
                FindObjectOfType<ObjectSelectionManager>().SelectObject(item);
                item.HandlePOISelectionFromButtonOrTouch(item);

                Dominic.SetActive(false);
                Ariadne.SetActive(false);
                
                if(child.activeSelf == false)
                {
                    child.SetActive(true);
                }
                
                frameColor.color = UIExtentions.BlueDark;

                if(model == Model.Ariadne)
                {
                    Ariadne.SetActive(true);
                    FindObjectOfType<WorldSpaceInfoCanvas>().InitializeTextsAndContent(globalName
                    , item.mainRenderer.sprite
                    , ObjectSelectionManager.textGlobalD);
                    FindObjectOfType<PlayAudioAtInfoPoint>().PlayOnSpawn();

                        UtilsHomeBar.toggleFiltersOpacity?.Invoke(true);
                    
                    // UtilsHomeBar.PlayAudio();
                }
                else 
                {
                    Dominic.SetActive(true);
                    FindObjectOfType<WorldSpaceInfoCanvas>().InitializeTextsAndContent(globalName
                    , item.mainRenderer.sprite
                    , ObjectSelectionManager.textGlobalD);
                    FindObjectOfType<PlayAudioAtInfoPoint>().PlayOnSpawn();
                    // UtilsHomeBar.PlayAudio();
                    UtilsHomeBar.toggleFiltersOpacity?.Invoke(true);
                }
                // chevron.sprite = UtilsHomeBar.chevronyellow;
            }
            else
            {
                Debug.Log("WrongID");
            }
        }
    }
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
                
                gameObject.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(r => r.enabled = isActive);
                gameObject.GetComponent<BoxCollider>().enabled = isActive;

                if(isGroup)
                {
                    gameObject.GetComponentInChildren<TMP_Text>().enabled = isActive;
                }
                // col.enabled = isActive;
            }
   
    public void TouchSign()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is began or moved
            if (touch.phase == TouchPhase.Began)
            {
                // Cast a ray from the touch position
                Ray ray = ARCamera.GetComponent<Camera>().ScreenPointToRay(touch.position);
                RaycastHit hitInfo;

                // Check if the ray hits an object
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == true)
                    {
                        // Check if the object is the one we want to track
                        if (hitInfo.collider.gameObject == this.gameObject)
                        {
                            if(pressed == false)
                            {
                                FindObjectOfType<GroupInfoPoint>().MakeButtonnsBlue();
                                this.pressed = true;
                                FindObjectOfType<GroupInfoPoint>().MakeTransparentNew(40f, false);
                                EventButton();
                                
                            }

                        }
                    }
                }
            }
        }
    }
}
