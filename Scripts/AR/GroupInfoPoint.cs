using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class GroupInfoPoint : MonoBehaviour
{
    public float transparent = 20f;
    [SerializeField] List<GameObject> Groups = new List<GameObject>();
    [SerializeField] List<GameObject> GroupPoints = new List<GameObject>();
    [SerializeField] List<GameObject> steadyPoints = new List<GameObject>();
    
    [SerializeField] public List<GameObject> InGroupPoints = new List<GameObject>();
    public enum FilterID { Monument , Museum, Park, Temple, PlayGround, RecreationGround, ShoppingStreet, CulturalFacility}
    // public FilterID filterID;
    [SerializeField] List<GameObject> exits = new List<GameObject>();
    public List<GameObject> taggedChildren = new List<GameObject>();  
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    GameObject ARCamera;
    bool groupHitted = false;
    bool isActive = false;
    private void OnEnable() 
    {
        ARCamera = GameObject.Find("AR Camera");
        UtilsHomeBar.MakeChevronsBlue += MakeButtonnsBlue;
        // UtilsHomeBar.FilterEventAR += FilterEventARGroup;
    }
    public void FilterEventARGroup(int iD, bool isSelected)
    {
        if (iD == FilterID.Monument.GetHashCode())
        {
            HandleFilterSelection(isSelected,  
                new int[] { 2, 8, 9 }, 
                new int[] { 2, 0, 0 });
        }
        else if (iD == FilterID.Museum.GetHashCode())
        {
            HandleFilterSelection(isSelected, 
                new int[] { 5, 7 }, 
                new int[] { 3, 1 });
        }
        else if (iD == FilterID.Park.GetHashCode())
        {
            HandleFilterSelection(isSelected, 
                new int[] { 3, 6 }, 
                new int[] { 2, 1 });
        }
        else if (iD == FilterID.Temple.GetHashCode())
        {
            HandleFilterSelection(isSelected,  
                new int[] { 0, 1, 4, 10, 11 }, 
                new int[] { 4, 4, 3, 0, 0 });
        }
        else
        {
            Debug.LogWarning("No Filter POI Existed");
        }
    }

    private void HandleFilterSelection(bool isSelected,  int[] inGroupIndices, int[] groupIndices)
    {
        if (isSelected)
        {

            foreach (var index in inGroupIndices)
            {
                InGroupPoints[index].transform.parent = this.transform;
            }
        }
        else
        {
            for (int i = 0; i < inGroupIndices.Length; i++)
            {
                InGroupPoints[inGroupIndices[i]].transform.parent = GroupPoints[groupIndices[i]].transform;
            }
        }
    }
    // public void FilterEventARGroup(int iD, bool arg2)
    // {
    //     if (arg2 == true)
    //     {
    //         if(iD == FilterID.Monument.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Monument Selected");
    //             InGroupPoints[2].transform.parent = this.transform;
    //             InGroupPoints[8].transform.parent = this.transform;
    //             InGroupPoints[9].transform.parent = this.transform;
    //         }
    //         else if(iD == FilterID.Museum.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Museum Selected");
    //             InGroupPoints[5].transform.parent = this.transform;
    //             InGroupPoints[7].transform.parent = this.transform;  
    //         }
    //         else if(iD == FilterID.Park.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Park Selected"); 
    //             InGroupPoints[3].transform.parent = this.transform;
    //             InGroupPoints[6].transform.parent = this.transform;  
    //         }
    //         else if(iD == FilterID.Temple.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Temple Selected");
    //             InGroupPoints[0].transform.parent = this.transform;
    //             InGroupPoints[4].transform.parent = this.transform;
    //             InGroupPoints[1].transform.parent = this.transform;
    //             InGroupPoints[10].transform.parent = this.transform;
    //             InGroupPoints[11].transform.parent = this.transform;
    //         }
    //         else
    //         {
    //             Debug.LogWarning("No Filter Poi Existed");
    //         }
    //     }
    //     else
    //     {
    //         if(iD == FilterID.Monument.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Monument UnSelected");
    //             InGroupPoints[2].transform.parent = GroupPoints[2].transform;
    //             InGroupPoints[8].transform.parent = GroupPoints[0].transform;
    //             InGroupPoints[9].transform.parent = GroupPoints[0].transform;
    //         }
    //         else if(iD == FilterID.Museum.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Museum UnSelected");
    //             InGroupPoints[5].transform.parent =  GroupPoints[3].transform;
    //             InGroupPoints[7].transform.parent =  GroupPoints[1].transform;  
    //         }
    //         else if(iD == FilterID.Park.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Park UnSelected"); 
    //             InGroupPoints[3].transform.parent =  GroupPoints[2].transform;
    //             InGroupPoints[6].transform.parent =  GroupPoints[1].transform;     
    //         }
    //         else if(iD == FilterID.Temple.GetHashCode())
    //         {
    //             Debug.LogWarning("Filter Temple UnSelected");
    //             InGroupPoints[0].transform.parent = GroupPoints[4].transform;
    //             InGroupPoints[1].transform.parent = GroupPoints[4].transform;
    //             InGroupPoints[4].transform.parent = GroupPoints[3].transform;
    //             InGroupPoints[10].transform.parent = GroupPoints[0].transform;
    //             InGroupPoints[11].transform.parent = GroupPoints[0].transform;
    //         }
    //         else
    //         {
    //             Debug.LogWarning("No Filter Poi Existed");
    //         }
    //     }
    // }

    private void OnDisable() {
        UtilsHomeBar.MakeChevronsBlue -= MakeButtonnsBlue;
    }
    public void MakeButtonnsBlue()
    {
        foreach (var item in sprites)
        {
            if(groupHitted == false)
            {
                item.color = UIExtentions.cyanBlue;
            }
            
            item.gameObject.GetComponent<ARButton>().pressed = false;
            
            if(item.gameObject.GetComponent<ARButton>().isInGroup == true && item.gameObject.activeInHierarchy)
            {
                item.color = UIExtentions.cyanBlue;
            }
        }
        // Do something with the tagged children
        foreach (var taggedChild in taggedChildren)
        {
            taggedChild.SetActive(false);
        }
    }
    public void MakeTransparentNew(float trans, bool col)
    {
        MakeTransparent(Groups, trans);
        
        GameObject go = GameObject.Find("GroupOfSteadyPoints");
        List<SpriteRenderer> rends = new List<SpriteRenderer>();
        
        List<SpriteRenderer> newRends = new List<SpriteRenderer>();
        
        var btns = go.GetComponentsInChildren<ARButton>().ToList();
        
        foreach (var item in btns)
        {
            item.gameObject.GetComponent<BoxCollider>().enabled = col; 

            if(item.pressed == false)
            {
                var spr = item.GetComponent<SpriteRenderer>();
                var sprs = item.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
                
                foreach (var sprite in sprs)
                {
                    if(!rends.Contains(sprite))
                    {
                        rends.Add(sprite);
                    }
                }

                if(!newRends.Contains(spr))
                {
                    newRends.Add(spr);
                } 
                  
            } 
            else
            {
                var spr = item.GetComponent<SpriteRenderer>();

                if(newRends.Contains(spr))
                {
                    newRends.Remove(spr);
                } 
                var sprs = item.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList(); 
                foreach (var sprite in sprs)
                {
                    if(rends.Contains(sprite))
                    {
                        rends.Remove(sprite);
                    }
                }
            }     
            if(groupHitted == true)
            {
                for (int i = 0; i < Groups.Count; i++)
                {
                    GroupPoints[i].SetActive(false);
                    Groups[i].GetComponent<BoxCollider>().enabled = true;
                    groupHitted = false;
                }
            }
        }
        foreach (var r in newRends)
        {          
            r.color = new Color(r.color.r, r.color.g, r.color.b, trans/255f);
        }
        foreach(var r in rends)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, trans/255f);
        }
    }
    public void MakeTransparent(List<GameObject> objects, float transparent)
    {
        List<SpriteRenderer> spriteRend = new List<SpriteRenderer>();
        
        foreach (var item in objects)
        {
            if(item.CompareTag("AR Group"))
            {
                var sr = item.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, transparent/255f);
            }


            var spritesRend = item.GetComponentsInChildren<SpriteRenderer>().ToList();   
            
            for (int i = 0; i < spritesRend.Count; i++)
            {
                spriteRend.Add(spritesRend[i]);    
            }   
            if(item.CompareTag("AR Group"))
            {
                var text = item.gameObject.GetComponentInChildren<TMP_Text>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, transparent/255f); 
            }   
            // spritesRend.Clear();
            
        }
        foreach (var sr in spriteRend)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, transparent/255f);            
        } 
        // spriteRend.Clear();
    }
    
    // exit groups
    public void ResetAll()
    {
        foreach (var item in Groups)
        {
            item.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255/255f);
            Image[] images;
            TMP_Text[] text; 
            images = item.GetComponentsInChildren<Image>();           
            text = item.gameObject.GetComponentsInChildren<TMP_Text>();

            foreach (var img in images)
            {
                img.color = new Color(255f, 255f, 255f, 040/255f);
            }
            foreach (var txt in text)
            {
               txt.color = new Color(17/255f, 57/255f, 132/255f, 40/255f); 
            }
            item.GetComponent<Collider>().enabled = false;
        }
    }
    
    private void Update() 
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Check if the touch phase is began or moved
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.GetComponent<Camera>().ScreenPointToRay(touch.position);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.collider.gameObject.tag == "AR Group")
                    {
                        int activeIndex = -1;

                        // Determine which group was hit
                        for (int i = 0; i < Groups.Count; i++)
                        {
                            if (hitInfo.collider.gameObject == Groups[i])
                            {
                                activeIndex = i;
                                break;
                            }
                        }
                        foreach (var item in taggedChildren)
                        {
                            item.SetActive(false);
                            item.transform.parent.GetComponent<BoxCollider>().enabled = false;
                        }
                        foreach (var item in Groups)
                        {
                            item.GetComponent<BoxCollider>().enabled = false;
                        }

                        // Update the active state of the group points
                        for (int i = 0; i < GroupPoints.Count; i++)
                        {
                            GroupPoints[i].SetActive(i == activeIndex);
                            var colliders = GroupPoints[i].GetComponentsInChildren<BoxCollider>(); 
                            foreach (var col in colliders)
                            {
                                col.enabled = true;
                            }
                            MakeTransparent(Groups, transparent);                 
                        }
                        UtilsHomeBar.DeselectPOI();
                        MakeTransparent(steadyPoints, transparent);
                        var worldSpaceInfoCanvases = FindObjectsOfType<WorldSpaceInfoCanvas>(true);
                        foreach (var item in worldSpaceInfoCanvases)
                        {
                            var model = item.model;
                            if(model != null)
                            {
                                model.SetActive(false);
                            }
                        }
                        groupHitted = true;
                    }
                    else if(hitInfo.collider.gameObject.tag == "ExitButton")
                    {
                        int activeIndexExit = -1;
                        
                        for (int i = 0; i < Groups.Count; i++)
                        {
                            if (hitInfo.collider.gameObject == exits[i])
                            {
                                activeIndexExit = i;
                                break;
                            }
                        }
                        // Update the active state of the group points
                        for (int i = 0; i < exits.Count; i++)
                        {   
                            MakeTransparent(Groups, 255f);
                            GroupPoints[i].SetActive(false);
                            MakeTransparent(steadyPoints, 255f);
                        }
                        foreach (var item in taggedChildren)
                        {
                            item.transform.parent.GetComponent<BoxCollider>().enabled = true;
                        }
                        foreach (var item in Groups)
                        {
                            item.GetComponent<BoxCollider>().enabled = true;
                        }
                        groupHitted = false;
                        
                        MakeButtonnsBlue();
                        
                        var worldSpaceInfoCanvases = FindObjectsOfType<WorldSpaceInfoCanvas>(true);
                        foreach (var item in worldSpaceInfoCanvases)
                        {
                            var model = item.model;
                            if(model != null)
                            {
                                model.SetActive(false);
                            }
                        }
                                               
                        UtilsHomeBar.DeselectPOI();         
                    }

                }
            }
        }
    
    }
}
