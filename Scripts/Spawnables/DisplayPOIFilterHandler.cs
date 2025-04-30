using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DisplayPOIFilterHandler : MonoBehaviour
{
    public List<AssetReference> assetReferences = new List<AssetReference>();
    public List<AssetReference> filterIconsReferences = new List<AssetReference>();

    [HideInInspector] public List<Sprite> sprites = new List<Sprite>();
    [HideInInspector] public List<Sprite> icons = new List<Sprite>();
    private void Awake() 
    {
        foreach(AssetReference reference in assetReferences)
        { 
            Addressables.LoadAssetAsync<Sprite>(reference).Completed += OnFilterSpriteLoaded;
        }
        foreach(AssetReference refe in filterIconsReferences)
        {
            Addressables.LoadAssetAsync<Sprite>(refe).Completed += OnGreyFilterSpriteLoaded;
        }
    }

    private void OnGreyFilterSpriteLoaded(AsyncOperationHandle<Sprite> icon)
    {
        if (icon.Status == AsyncOperationStatus.Succeeded)
        {
            icons.Add(icon.Result);
        }
        else
        {
            Debug.LogError($"Failed to load grey sprite {icon.OperationException}");
        }
    }

    private void OnFilterSpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            sprites.Add(obj.Result);
        }
        else
        {
            Debug.LogError($"Failed to load filter sprite {obj.OperationException}");
        }
    }
}
