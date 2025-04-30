using UnityEngine;
using UnityEngine.UIElements;

public class SwitchPointer32Command : ICommandPointer32
{
    private readonly int routeIndex;
    // private List<PointOfInterest> poi = new List<PointOfInterest>();
    VisualElement element;
    public SwitchPointer32Command(int index, VisualElement el)
    {
        this.routeIndex = index;
        this.element = el;
    }
    public void Execute()
    {
        CreatePointer(routeIndex, element);
        // Debug.Log(poi.Count);
    }

    public void Undo()
    {
        element.Query(className: "pointbars").ForEach(parent => parent.RemoveFromHierarchy());
    }
    private void CreatePointer(int index, VisualElement elem)
    {
        for (int i = 0; i < index; i++)
        {
            var Pointer = new VisualElement();     
            elem.Add(Pointer);
            Pointer.name = "Pointer32";
            Pointer.AddToClassList("pointbars");
            
            UIExtentions.Display(Pointer, true);

            Pointer.style.overflow = Overflow.Visible;
            Pointer.style.flexGrow = 0;
            Pointer.style.flexShrink = 1;
            Pointer.style.alignSelf = Align.Center;
            Pointer.style.width = 10;
            Pointer.style.height = 10;
            Pointer.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            Pointer.style.marginBottom = 5;
            Pointer.style.marginTop = 10;
            Pointer.style.marginLeft = 5;
            Pointer.style.marginRight = 5;

            Pointer.style.borderTopColor = Color.white;
            Pointer.style.borderLeftColor = Color.white;
            Pointer.style.borderRightColor = Color.white;
            Pointer.style.borderBottomColor = Color.white;
            Pointer.style.borderTopLeftRadius = 50;
            Pointer.style.borderTopRightRadius = 50;
            Pointer.style.borderBottomLeftRadius = 50;
            Pointer.style.borderBottomRightRadius = 50;
            Pointer.style.borderTopWidth = 1;
            Pointer.style.borderRightWidth = 1;
            Pointer.style.borderLeftWidth = 1;
            Pointer.style.borderBottomWidth = 1;

            
            if(i != 0)
                Pointer.style.backgroundImage = new StyleBackground(UtilsHomeBar.pointer); 
            else
                Pointer.style.backgroundImage = new StyleBackground(UtilsHomeBar.pointerBlue);
        }   
        
    }
}
