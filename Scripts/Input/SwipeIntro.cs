using UnityEngine;

public class SwipeIntro : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    public delegate void SwipeEvents(int index); 
    public SwipeEvents SwipeRightAction;
    public SwipeEvents SwipeLeftAction;
    int index = 1;

    private void Update() 
    {
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startPosition = Input.GetTouch(0).position;
        }  
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPosition = Input.GetTouch(0).position;

            if(endPosition.x < startPosition.x)
            {
                //NextPage
                SwipeRightAction(FindObjectOfType<UITestButton>().pageIndex + index);
            }
            if(endPosition.x > startPosition.x)
            {
                //previous
                SwipeLeftAction(FindObjectOfType<UITestButton>().pageIndex - index);
            }
        }     
    }
}
