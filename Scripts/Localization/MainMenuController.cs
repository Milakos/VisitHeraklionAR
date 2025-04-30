using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
[RequireComponent( typeof(UIDocumentLocalization) )]
public class MainMenuController : MonoBehaviour
{      
    void OnEnable () => GetComponent<UIDocumentLocalization>().onCompleted += Bind;
    void OnDisable () => GetComponent<UIDocumentLocalization>().onCompleted -= Bind;
    
    void Bind ( VisualElement root )
    {

        if(GetComponent<UITestButton>() == null) 
        {
            print("noUITestButton");
        }
        else if(GetComponent<UITestButton>() != null)
        {
            GetComponent<UITestButton>().NextButtonHandler(GetComponent<UITestButton>().pageIndex);
            GetComponent<UITestButton>().NextButtonInit();
            GetComponent<UITestButton>().SkipButtonInit();
        }
        
        GetComponent<AssignFont>().Assign();
        // FindObjectOfType<JsonLocalization>().CallJSONMethods();
        // FindObjectOfType<PointOfInterest>().GetLocalizedAudio();
        FindObjectOfType<PlayerStateMachine>().StateInitialization();
        // FindObjectOfType<JsonLocalization>().EnableJsonLocalization(root);
        print("BIND");
        /* bind here */
    }
}
