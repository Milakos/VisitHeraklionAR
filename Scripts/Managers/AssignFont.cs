using UnityEngine;
using UnityEngine.UIElements;

public class AssignFont : MonoBehaviour
{
    public UIDocument uIDocument;
    public void Assign() 
    {
        var texts = uIDocument.rootVisualElement.Query<Label>().ToList();  
        var childs = uIDocument.rootVisualElement.Children(); 
        // childs.AddToClassList(className: "segoeFone");
        foreach (var item in childs)
        {
            item.AddToClassList(className: "segoeFont");
        } 
    }
}
