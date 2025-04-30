using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListIconsFilters : MonoBehaviour
{
    public enum FilterGreyIcon
    { 
        Monument , Museum, Park, Temple ,Playground, Recreation_ground, Shopping_street, Cultural_facility
    }
    public enum FilterGreyIconGreek
    { 
        Μνημείο , Μουσείο, Πλατεία, Ναός , Παιδική_χαρά, Χώρος_άθλησης, Εμπορικός_δρόμος, Χώρος_πολιτισμού
    }
    public static Dictionary<FilterGreyIcon, int> filterSpriteIndices = new Dictionary<FilterGreyIcon, int>();
    public static Dictionary<FilterGreyIconGreek, int> filterSpriteIndicesGreek = new Dictionary<FilterGreyIconGreek, int>();
    static Sprite selectedSprite;
    private void Awake() 
    {
        filterSpriteIndices.Add(FilterGreyIcon.Monument, 0);
        filterSpriteIndices.Add(FilterGreyIcon.Museum, 1);  
        filterSpriteIndices.Add(FilterGreyIcon.Park, 2);  
        filterSpriteIndices.Add(FilterGreyIcon.Temple, 3);  
        filterSpriteIndices.Add(FilterGreyIcon.Playground, 4);  
        filterSpriteIndices.Add(FilterGreyIcon.Recreation_ground, 5);  
        filterSpriteIndices.Add(FilterGreyIcon.Shopping_street, 6);      
        filterSpriteIndices.Add(FilterGreyIcon.Cultural_facility, 7);  

        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Μνημείο, 0);
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Μουσείο, 1);  
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Πλατεία, 2);  
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Ναός, 3);  
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Παιδική_χαρά, 4);  
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Χώρος_άθλησης, 5);  
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Εμπορικός_δρόμος, 6);      
        filterSpriteIndicesGreek.Add(FilterGreyIconGreek.Χώρος_πολιτισμού, 7);  
    }

    /// <summary>
    /// Automatically assigns a grey icon sprite based on the provided filter reference.
    /// </summary>
    /// <param name="filterRef">The filter reference used to determine the grey icon.</param>
    /// <returns>The grey icon sprite associated with the provided filter reference.</returns>
    public static Sprite AutoAssignGreyIcon(string filterRef)
    {
        // Retrieve the corresponding enum value (FilterGreyIcon) for the given filter reference.
        // This code uses reflection to obtain all values of the FilterGreyIcon enum, casts them to the enum type,
        // and then searches for the first enum value whose string representation matches the provided filter reference.
        
        FilterGreyIcon filterID = Enum.GetValues(typeof(FilterGreyIcon))
                                    .Cast<FilterGreyIcon>()
                                    .FirstOrDefault(e => e.ToString() == filterRef);
        
        FilterGreyIconGreek filterIDGreek = Enum.GetValues(typeof(FilterGreyIconGreek))
                                    .Cast<FilterGreyIconGreek>()
                                    .FirstOrDefault(e => e.ToString() == filterRef);

        if(UIExtentions.IsEnglish())
        {
            if (filterID >= 0)
            {
                int spriteIndex;
                if (filterSpriteIndices.TryGetValue(filterID, out spriteIndex))
                {
                    selectedSprite = FindObjectOfType<DisplayPOIFilterHandler>().sprites[spriteIndex];
                    // Debug.Log(selectedSprite);
                }
                else
                {
                    Debug.LogError("FilterID not found in dictionary.");
                }
            }
            else
            {
                Debug.LogError("FilterID not found for EngReference: ");
            }
        }
        else 
        {
            if (filterIDGreek >= 0)
            {
                int spriteIndex;
                if (filterSpriteIndicesGreek.TryGetValue(filterIDGreek, out spriteIndex))
                {
                    selectedSprite = FindObjectOfType<DisplayPOIFilterHandler>().sprites[spriteIndex];
                    // Debug.Log(selectedSprite);
                }
                else
                {
                    Debug.LogError("FilterID not found in dictionary.");
                }
            }
            else
            {
                Debug.LogError("FilterID not found for EngReference: ");
            }
        }
        return selectedSprite;
    }
   
}
