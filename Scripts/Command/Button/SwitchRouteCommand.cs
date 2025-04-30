using UnityEngine.UIElements;

public class SwitchRouteCommand : ICommand
{
    private const int RawZero = 0;
    private readonly int routeIndex;
    VisualElement buttonSpawnPoint;
    public SwitchRouteCommand(int route, VisualElement parent)
    {
        this.routeIndex = route;
        this.buttonSpawnPoint = parent;        
    }
    
    public void Execute()
    {
        var counter = JSONTest.Instance.globalPath[routeIndex].englishTextsRoute;

        if(UIExtentions.IsEnglish() == true)
        {        
            for (int i = 0; i < counter.Count; i++)
            {
                var path = JSONTest.Instance.globalPath[routeIndex].englishTextsRoute[i].Titles[RawZero];  
                
                var filter = JSONTest.Instance.globalPath[routeIndex].filterIDEng[i].ToString();
                
                var icon = JSONTest.Instance.globalPath[routeIndex].filterIDEng[i].ToString();
                
                
                if (filter.Contains("_"))
                {
                    filter = filter.Replace("_", " ");
                    // The icon string contains an underscore character
                }   
                // if (icon.Contains("_"))
                // {
                //     icon = icon.Replace("_", " ");
                //     // The icon string contains an underscore character
                // }                            

                // Debug.Log("routes = " + " " + path.ToString());

                var button = new ButtonElement(buttonSpawnPoint, i + 1 + "." + " " + path.ToString(), filter.ToString(), 
                ListPathRoutes.sprites[routeIndex][i], ListIconsFilters.AutoAssignGreyIcon(icon), i);                 
            }
            var route = JSONTest.Instance.globalRoutes.englishTextsRoute[routeIndex].Titles[RawZero];
            var routeDescription = JSONTest.Instance.globalRoutes.englishTextsRoute[routeIndex].Texts[RawZero];
            // Debug.Log("Title is " + "" + route.ToString() + " Description = " + routeDescription.ToString());
        }
        else
        {
            for (int i = 0; i < counter.Count; i++)
            {
                var path = JSONTest.Instance.globalPath[routeIndex].greekTextsRoute[i].Titles[RawZero];
                var filter = JSONTest.Instance.globalPath[routeIndex].filterIDGreek[i].ToString();               
                var icon = JSONTest.Instance.globalPath[routeIndex].filterIDGreek[i].ToString();  
                
                if (filter.Contains("_"))
                {
                    filter = filter.Replace("_", " ");
                    // The icon string contains an underscore character
                }   
                // if (icon.Contains("_"))
                // {
                //     icon = icon.Replace("_", " ");
                //     // The icon string contains an underscore character
                // }  
                // Debug.Log("routes = " + " " + path.ToString());
                var button = new ButtonElement(buttonSpawnPoint, i + 1 + "." + " " + path.ToString(), filter.ToString(), 
                ListPathRoutes.sprites[routeIndex][i], ListIconsFilters.AutoAssignGreyIcon(icon), i); 
            }
            var route = JSONTest.Instance.globalRoutes.greekTextsRoute[routeIndex].Titles[RawZero];
            var routeDescription = JSONTest.Instance.globalRoutes.greekTextsRoute[routeIndex].Texts[RawZero];
            // Debug.Log("Title is " + "" + route.ToString() + " Description = " + routeDescription.ToString());            
        }    
        var blankButton = new VisualElement();
        blankButton.AddToClassList(className: "TrashButton");
        blankButton.style.height = 100f;
        buttonSpawnPoint.Add(blankButton);
        // new ButtonElement(buttonSpawnPoint, " ", " ", null, null, -1); 
        blankButton.BringToFront();
        blankButton.style.opacity = new StyleFloat(0f);
    }
    public void Undo()
    {
        buttonSpawnPoint.Query(className: "ButtonCard").ForEach(element => element.RemoveFromHierarchy());
        buttonSpawnPoint.Query(className: "TrashButton").ForEach(element => element.RemoveFromHierarchy());
    }
}
