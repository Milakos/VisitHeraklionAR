using System.Collections;
using UnityEngine;
public abstract class AbstractDisplayMessages : MonoBehaviour 
{
    private Coroutine messageCoroutine; // Reference to the current running coroutine
    private string pendingMessage; // Stores the message that is pending to be shown
    private float delay = 3f;
    ARElement aRElement;
    bool displayMessage = false;
    private void Awake() 
    {
        aRElement = FindObjectOfType<ARElement>();   
    }
    public virtual void HandleMessageChange(string newMessage)
    {
        // If the new message is the same as the pending one, no need to restart the coroutine
        if (pendingMessage == newMessage) return;

        // Update the pending message to the new one
        pendingMessage = newMessage;
        // Cancel the existing coroutine if it's running and start a new one
        CancelMessageCoroutine(displayMessage);
        messageCoroutine = StartCoroutine(DisplayMessageAfterDelay(delay));
    }
    public virtual void CancelMessageCoroutine(bool displayMessage)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
        UIExtentions.Display(aRElement.infoHolder, false);  
        displayMessage = false;
    }
    public virtual IEnumerator DisplayMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // After 3 seconds, check if the message should still be displayed
        if (!displayMessage)
        {
            displayMessage = true;
            UIExtentions.Display(aRElement.infoHolder, true);  
            
            ARBoundHandler.Instance.globalMessageString = pendingMessage; // Show the most recent message
        }
    }
}
