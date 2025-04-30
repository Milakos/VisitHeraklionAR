using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupFilter : MonoBehaviour
{
    public enum FilterID { Monument , Museum, Park, Temple, PlayGround, RecreationGround, ShoppingStreet, CulturalFacility}
    [SerializeField] public FilterID filterID = FilterID.ShoppingStreet;
    private HashSet<FilterID> activeFilters = new HashSet<FilterID>();
    public bool isActive = true; 
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // UtilsHomeBar.FilterEventARGroup += HandleFilterEvent;
    }
    private void OnDisable() 
    {
        // UtilsHomeBar.FilterEventARGroup -= HandleFilterEvent;
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
    }
}
