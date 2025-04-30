using System.Collections.Generic;
using UnityEngine;

#region classObjects

[System.Serializable] // Contrains the Text Data of the Points of Interests in both Greek and English. Also the coordinates
public class GlobalPath
{
    public List<DictionaryLanguages> greekTextsRoute = new List<DictionaryLanguages>();
    public List<DictionaryLanguages> englishTextsRoute = new List<DictionaryLanguages>();
    public List<string> coordinates = new List<string>();
    public List<string> filterIDGreek = new List<string>();
    public List<string> filterIDEng = new List<string>();
    public List<string> contact = new List<string>();
    public List<string> addressesE = new List<string>();
    public List<string> addressesG = new List<string>();
    public List<string> openTime = new List<string>();
    public List<string> openTimeEng = new List<string>();
    public List<string> EntryE = new List<string>();
    public List<string> EntryG = new List<string>();
}

[System.Serializable] // Contains the Text Data Titles and Description, in both greek and english of the five Routes
public class GlobalRoutes
{
    public List<DictionaryLanguages> greekTextsRoute = new List<DictionaryLanguages>();
    public List<DictionaryLanguages> englishTextsRoute = new List<DictionaryLanguages>();
    public List<string> keys = new List<string>();
}

[System.Serializable]
public class CulturalRoutes
{
    public List<Route> Paths;
}

[System.Serializable]
public class DictionaryLanguages
{
    [SerializeField]
    public List<string> Titles = new List<string>();

    [SerializeField]
    public List<string> Texts = new List<string>();
   
    public Dictionary <string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int i = 0; i < Titles.Count; i++)
        {
            dictionary[Titles[i]] = Texts[i];
        }
        return dictionary;
    }
}

[System.Serializable] 
public class Route
{
    public string key;
    public string greekText;
    public string greekTitle;
    public string englishText;
    public string englishTitle;
    public List<PointsOfInterest> coastRoute;
    public List<PointsOfInterest> wallRoute;
    public List<PointsOfInterest> gateRoute;
    public List<PointsOfInterest> portRoute;
    public List<PointsOfInterest> coveRoute;
}

[System.Serializable]
public class PointsOfInterest
{
    public string key;
    public string englishTitle;
    public string englishText;
    public string greekTitle;
    public string greekText;
    public string coordinates;
    public string filterGreek;
    public string filterEng;

    public string addressG;
    public string addressE;
    public string contact;
    public string openTimeEng;
    public string openTime;
    public string EntryE;
    public string EntryG;
}

#endregion classObjects

[System.Serializable]
public class AppLinks
{
    public string google;
    public string iOS;
}

[System.Serializable]
public class AppLinksContainer
{
    public AppLinks[] LinkHeraklionGastronomy;
    public AppLinks[] LinkHeraklion360;
}

[System.Serializable]
public class JSONTest : Singleton<JSONTest>
{
    [SerializeField] private TextAsset jsonFile;
    [HideInInspector] public string heraklionGastronomyGoogle;
    [HideInInspector] public string heraklionGastronomyIOS;
    [HideInInspector] public string heraklion360IOS;
    [HideInInspector] public string heraklion360Google;
    [SerializeField] private UnityEngine.TextAsset jsonFilePath;
    public GlobalRoutes globalRoutes;
    public GlobalPath[] globalPath;
    public object LoadAssetMain { get; private set; }

    public override void Awake() 
    {
        base.Awake();
        LoadJSONTextsAndCoordinates();
        LoadJSonLinks();
    }
    void LoadJSonLinks()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not assigned in the inspector.");
            return;
        }

        // Load JSON data from the TextAsset
        string json = jsonFile.text;

        // Deserialize the JSON string to an object
        AppLinksContainer appLinksContainer = JsonUtility.FromJson<AppLinksContainer>(json);

        // Extract and store the links in strings
        heraklionGastronomyIOS = appLinksContainer.LinkHeraklionGastronomy[0].iOS;
        heraklion360Google = appLinksContainer.LinkHeraklion360[0].google;
        heraklion360IOS = appLinksContainer.LinkHeraklion360[0].iOS;
        heraklionGastronomyGoogle = appLinksContainer.LinkHeraklionGastronomy[0].google;
    }
    void LoadJSONTextsAndCoordinates()
    {
        if(jsonFilePath != null)
        {
            // Load JSON data from the file
            string jsonText = jsonFilePath.text; 
            CulturalRoutes routeData = JsonUtility.FromJson<CulturalRoutes>(jsonText);
            globalPath = new GlobalPath[routeData.Paths.Count];

            // globalRoutes = new GlobalRoutes;
            
            for (int i = 0; i < routeData.Paths.Count; i++)
            {
                var path = routeData.Paths[i];
                
                // Create a new GlobalPath instance for the current path
                GlobalPath pathData = new GlobalPath();
                GlobalRoutes routePath = new GlobalRoutes();
                // Iterate through each route in the current path
                AddRoutesData(path.coastRoute, pathData);
                AddRoutesData(path.wallRoute, pathData);
                AddRoutesData(path.gateRoute, pathData);
                AddRoutesData(path.portRoute, pathData);
                AddRoutesData(path.coveRoute, pathData);

                AddPathsData(routeData.Paths, routePath);
                
                // Assign the pathData to the globalPath array
                globalPath[i] = pathData;
                globalRoutes = routePath;
            }
        }
        else
        {
            Debug.LogError("Failed to load JSON data.");
        }
    }
    DictionaryLanguages GetRoutetexts(Route route,bool isGreek)
    {
        DictionaryLanguages texts = new DictionaryLanguages();

        texts.Titles.Add(isGreek ? route.greekTitle : route.englishTitle);
        texts.Texts.Add(isGreek ? route.greekText : route.englishText);
        return texts;
    }
    private void AddPathsData(List<Route> routes, GlobalRoutes routeData)
    {
        for (int j = 0; j < routes.Count; j++)
        {
            var route = routes[j];

            // Add coordinates to the pathData
            routeData.keys.Add(route.key);
            // Add Greek and English texts to the pathData
            routeData.greekTextsRoute.Add(GetRoutetexts(route, true));
            routeData.englishTextsRoute.Add(GetRoutetexts(route, false));
        }
    }
    public DictionaryLanguages GetTexts(PointsOfInterest point, bool isGreek)
    {
        DictionaryLanguages texts = new DictionaryLanguages();

        texts.Titles.Add(isGreek ? point.greekTitle : point.englishTitle);
        texts.Texts.Add(isGreek ? point.greekText : point.englishText);

        return texts;
    }
    private void AddRoutesData(List<PointsOfInterest> routes, GlobalPath pathData)
    {
        for (int j = 0; j < routes.Count; j++)
        {
            var route = routes[j];          
            // Add coordinates to the pathData
            pathData.coordinates.Add(route.coordinates);
            pathData.filterIDEng.Add(route.filterEng);
            pathData.filterIDGreek.Add(route.filterGreek);

            pathData.contact.Add(route.contact);
            pathData.addressesE.Add(route.addressE);
            pathData.addressesG.Add(route.addressG);
            pathData.EntryE.Add(route.EntryE);
            pathData.EntryG.Add(route.EntryG);
            pathData.openTime.Add(route.openTime);
            pathData.openTimeEng.Add(route.openTimeEng);

            // Add Greek and English texts to the pathData
            pathData.greekTextsRoute.Add(GetTexts(route, true));
            pathData.englishTextsRoute.Add(GetTexts(route, false));
        }
    }
}
