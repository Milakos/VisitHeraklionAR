using System.Collections;
public class DisplayMessages : AbstractDisplayMessages
{
    public override void HandleMessageChange(string newMessage)
    {
        // Start a new coroutine to wait 3 seconds before displaying the message
        base.HandleMessageChange(newMessage);
    }
    public override void CancelMessageCoroutine( bool displayMessage)
    {
        base.CancelMessageCoroutine(displayMessage);
    }
    public override IEnumerator DisplayMessageAfterDelay(float delay)
    {
        return base.DisplayMessageAfterDelay(3f);
    }

}
