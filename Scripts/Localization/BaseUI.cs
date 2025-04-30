using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseUI : MonoBehaviour
{
    protected UIDocument uIDocument;

    private void OnEnable() 
    {
        uIDocument = GetComponent<UIDocument>();
        InitControls();    
    }
    public abstract void InitControls();
}
