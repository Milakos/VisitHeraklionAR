using UnityEngine;
using UnityEngine.UIElements;

public class MaterialManager : MonoBehaviour
{
    MeshRenderer meshRenderer;
    ObjectSelectionManager objectSelectionManager;
    Material mat;
    public ARElement aRElement;
    private const int RawZero = 0;
    public UIDocument uIDocument;
    VisualElement visualElement;
    private void Awake() 
    {
        meshRenderer = GetComponent<MeshRenderer>();  
        // aRElement = FindObjectOfType<ARElement>();
        objectSelectionManager = FindObjectOfType<ObjectSelectionManager>();  
    }
    private void Start() 
    {
        var vs = uIDocument.rootVisualElement;
        visualElement = vs.Q<VisualElement>("Loading360"); 
    }
    private void OnEnable() 
    {
        InfoState.load360Material += LoadMaterial;
        MapState.load360Material += LoadMaterial;
        aRElement.load360Material += LoadMaterial;

        MapState.unload360Material += UnloadMaterial;
        InfoState.unload360Material += UnloadMaterial;
        aRElement.unload360Material += UnloadMaterial;
    }

    public async void LoadMaterial()
    {
        UIExtentions.Display(visualElement, true);
        await objectSelectionManager.selectedObjects[RawZero].OnMaterialLoaded();
        meshRenderer.material = objectSelectionManager.selectedObjects[RawZero].sprite360;
        UIExtentions.Display(visualElement, false);
        Debug.Log("MaterialLoaded");
    }
    public void UnloadMaterial()
    {
        if(objectSelectionManager.selectedObjects.Count > 0)
        {
            objectSelectionManager.selectedObjects[RawZero].OnMaterialUnLoaded(); 
        }    
        meshRenderer.material = null;
        Debug.Log("MaterialUnLoaded");
    }

    private void OnDisable() 
    {
        InfoState.load360Material -= LoadMaterial;
        InfoState.unload360Material -= UnloadMaterial;
        
        MapState.load360Material -= LoadMaterial;
        MapState.unload360Material -= UnloadMaterial;
        
        // aRElement.load360Material -= UnloadMaterial;
        // aRElement.unload360Material -= LoadMaterial;
    }
}
