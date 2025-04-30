public class SelectPath
{
 public bool isStarted;
    public enum Select
    {
        Selected, Unselected
    }
    public Select stateStart;
    public void SelectUnselect(Select state)
    {
        stateStart = state;
        switch (stateStart)
        {
            case Select.Selected:
            isStarted = true;
            break;
            case Select.Unselected:
            isStarted = false;
            break;
            default:
            break;
        }
    }
}
