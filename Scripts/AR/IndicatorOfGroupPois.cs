using System.Globalization;
using TMPro;
using UnityEngine;

public class IndicatorOfGroupPois : MonoBehaviour
{
    Camera arCamera;
    public float scale_factor;
    private float timer;
    private Vector3 temp_scale;
    bool hasReachedDestination;
    public GameObject child;
    [SerializeField] TMP_Text Pointname;
    [SerializeField] TMP_Text Minutes;
    [SerializeField] TMP_Text Meters;
    ARElement aRElement;
    public int numberOfPois;
    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();
    }
    private void OnEnable() 
    {
        aRElement.whenIn360 += WhenIndicatorIn360;
    }
    private void Update() 
    {
        AdjustIndicatorScale();
        Pointname.text =  numberOfPois + " " + TextLibrary.pointTextTranslate.GetTranslatedText();
        Minutes.text = CalculateNearestDistanceToMinutes().ToString();
        Meters.text = UserToPointDistance().ToString("0", CultureInfo.InvariantCulture) + "m";
    }
    private void OnDisable() 
    {
        aRElement.whenIn360 -= WhenIndicatorIn360;
    }
    private void WhenIndicatorIn360(bool obj)
    {
        child.SetActive(obj);
    }
    public void AdjustIndicatorScale()
    {
        gameObject.transform.LookAt(arCamera.transform);

        if (timer > 1)
        {
            timer = 0;
            
            temp_scale = new Vector3(UserToPointDistance() * scale_factor, UserToPointDistance() * scale_factor, 1);
            
            if (Vector3.Distance(gameObject.transform.localScale, temp_scale) < 0.01f) // Check if the scale has almost reached the target
            {
                hasReachedDestination = true;
                child.SetActive(true); // Make the object visible when it reaches the destination
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, temp_scale, Time.deltaTime * 1.2f);
    }
    private float UserToPointDistance()
    {
        Vector3 userPosition = arCamera.transform.position;
        Vector3 pointPosition = gameObject.transform.position;
        float distance = Vector3.Distance(userPosition, pointPosition);
        return distance;
    }
    private string CalculateNearestDistanceToMinutes()
    {
        int averageWalkSpeed = 2;
        float distance = UserToPointDistance();
        return Mathf.Round((distance / averageWalkSpeed) / 60).ToString() + " " + TextLibrary.minutesTextTranslate.GetTranslatedText();
    }
}
