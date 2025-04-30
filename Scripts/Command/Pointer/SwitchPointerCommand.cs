using UnityEngine;
using UnityEngine.UIElements;

public class SwitchPointerCommand : ICommandPointer
{
    private readonly int routeIndex;
    // private List<PointOfInterest> poi = new List<PointOfInterest>();
    VisualElement element;
    // VisualElement element32;
    public SwitchPointerCommand(int index, VisualElement el)
    {
        this.routeIndex = index;
        this.element = el;
        // this.element32 = el32;
    }
    public void Execute()
    {
        Pointer(routeIndex, element);
    }

    public void Undo()
    {
        element.Query(className: "pointbars").ForEach(parent => parent.RemoveFromHierarchy());
    }

    private static void Pointer(int index, VisualElement elem)
    {
        for (int i = 0; i < index; i++)
        {
            var Pointer = new VisualElement();
            elem.Add(Pointer);
            Pointer.name = "Pointer";
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

            if (i != 0)
                Pointer.style.backgroundImage = new StyleBackground(UtilsHomeBar.pointer);
            else
                Pointer.style.backgroundImage = new StyleBackground(UtilsHomeBar.pointerBlue);
        }
    }
}
