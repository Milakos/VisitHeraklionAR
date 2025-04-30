using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestLoadUnload : MonoBehaviour
{

    public GameObject Instance;
    public GameObject Dominic;
    [SerializeField] AssetReferenceGameObject DominicRef;
    
    public void LoadModel()
    {
        SpawnAriadneOrDominicTask();
    }
    public void UnLoadModel()
    {
        ResetTextsAndDestoryModelTask();
    }
    private async void SpawnAriadneOrDominicTask()
    {     
        GameObject prefabToInstantiate = null;  
        await LoadModelAsync(DominicRef, result => Dominic = result);
        // Spawn the appropriate model
        prefabToInstantiate = Dominic;
        // Addressables.LoadAssetAsync<GameObject>(DominicRef);

        Instance = Instantiate(prefabToInstantiate, transform.position, transform.rotation);
        Vector3 directionToCamera = Camera.main.gameObject.transform.position - Instance.transform.position;
        // Eliminate the x and z components of the direction
        directionToCamera.y = 0;
        // Calculate the rotation to look at the camera on the y-axis only
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        // Apply the y-axis only rotation to the instance
        Instance.transform.rotation = lookRotation;             
        Instance.transform.parent = null;                    
    }
    private async Task<GameObject> LoadModelAsync(AssetReferenceGameObject reference, Action<GameObject> onLoaded)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(reference);

        await handle.Task;
        onLoaded?.Invoke(handle.Result);
        return handle.Result;
    }
    public void ResetTextsAndDestoryModelTask()
    {
        // Unload the loaded models 
        if(Instance != null)
        {
            Destroy(Instance);
        }
    }
}
