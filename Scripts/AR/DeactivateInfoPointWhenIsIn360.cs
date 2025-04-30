using UnityEngine;

public class DeactivateInfoPointWhenIsIn360 : MonoBehaviour
{
    ARElement aRElement;
    public GameObject child;
    public Vector3 initialScale;
    public  Transform parentTransform;
    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();
    }
    private void Start() 
    {
                // Store the initial local scale of the child object
        initialScale = transform.localScale;

        // Get the parent transform
        if (transform.parent != null)
        {
            parentTransform = transform.parent;
        }
    }
    private void OnEnable() 
    {
        aRElement.whenIn360 += WhenInfoPointIn360;
    }
    private void OnDisable() 
    {
        aRElement.whenIn360 -= WhenInfoPointIn360;
    }
    private void WhenInfoPointIn360(bool obj)
    {
       child.SetActive(obj);
    }
}
