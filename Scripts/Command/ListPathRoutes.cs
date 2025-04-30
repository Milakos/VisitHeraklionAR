using System.Collections.Generic;
using UnityEngine;

public class ListPathRoutes : MonoBehaviour
{
    public List<Sprite> CoastalRouteSprites = new List<Sprite>();
    public List<Sprite> WallsRouteSprites = new List<Sprite>();
    public List<Sprite> GateRouteSprites = new List<Sprite>();
    public List<Sprite> CoveRouteSprites = new List<Sprite>();
    public List<Sprite> DermatasRouteSprites = new List<Sprite>();
    public static List<List<Sprite>> sprites = new List<List<Sprite>>();
    

    void Start()
    {
        AddSpritesToList(CoastalRouteSprites);
        AddSpritesToList(WallsRouteSprites);
        AddSpritesToList(GateRouteSprites);
        AddSpritesToList(CoveRouteSprites);
        AddSpritesToList(DermatasRouteSprites);
    }
    private void OnDisable() 
    {
        CoastalRouteSprites.Clear();
        WallsRouteSprites.Clear();
        GateRouteSprites.Clear();
        CoveRouteSprites.Clear();
        DermatasRouteSprites.Clear();
        sprites.Clear();    
    }
    void AddSpritesToList(List<Sprite> routeSprites)
    {
        List<Sprite> staticSprites = new List<Sprite>(routeSprites);
        sprites.Add(staticSprites);
    }
}

