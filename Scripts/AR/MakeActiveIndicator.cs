using UnityEngine;

public class MakeActiveIndicator : MonoBehaviour
{
    IndicatorTest indicatorTest;
    private void Awake() 
    {
        indicatorTest = GetComponentInParent<IndicatorTest>();
    }
    public void MakeActive()
    {
        this.gameObject.SetActive(false);
        indicatorTest.GroupBubbleActive = false;
    }
    public void GroupBubbleActive()
    {
        indicatorTest.GroupBubbleActive = true;
    } 
}
