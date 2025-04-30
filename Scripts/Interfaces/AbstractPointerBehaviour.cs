using UnityEngine;
using UnityEngine.UIElements;

public static class AbstractPointerBehaviour
{
    static int currentIndex = 0;
    public static void MovePointer(VisualElement PointerContainer, int additive, ScrollView scrollView)    
    {
        // currentIndex = 0;
        currentIndex = Mathf.Clamp(currentIndex, 0, 6);
        // Calculate the new index based on the direction and ensure it's within bounds
        int newIndex = Mathf.Clamp(currentIndex + additive, 0, PointerContainer.childCount - 1);
        // Get the child element to be moved
        // VisualElement childElement = 
        PointerContainer.Query(className: "pointbars").ToList();
        VisualElement childElement = PointerContainer.ElementAt(currentIndex);
        
        // Remove the child element from its current position
        PointerContainer.Remove(childElement);
        scrollView.ScrollTo(scrollView[newIndex]);  
        // Insert the child element at the new index
        PointerContainer.Insert(newIndex, childElement);
        currentIndex = newIndex;   
    }
    public static void ResetCurrentIndex()
    {
        currentIndex = 0;
    }
    public static void MovePointerAR(VisualElement PointerContainer, int additive, ScrollView scrollView)    
    {
        // currentIndex = 0;
        currentIndex = Mathf.Clamp(currentIndex, 0, 6);
        // Calculate the new index based on the direction and ensure it's within bounds
        int newIndex = Mathf.Clamp(currentIndex + additive, 0, PointerContainer.childCount - 1);
        // Get the child element to be moved
        // VisualElement childElement = 
        PointerContainer.Query(className: "pointbars").ToList();
        VisualElement childElement = PointerContainer.ElementAt(currentIndex);
        
        // Remove the child element from its current position
        PointerContainer.Remove(childElement);
        scrollView.ScrollTo(scrollView[newIndex]);  
        // Insert the child element at the new index
        PointerContainer.Insert(newIndex, childElement);
        currentIndex = newIndex;   
    }
    public static void ResetCurrentIndexAR()
    {
        currentIndex = 0;
    }
}
