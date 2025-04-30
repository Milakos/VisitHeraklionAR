[System.Serializable]
public class StartState
{
    public bool isStarted;
    public enum state
    {
        Start , End, None
    }
    public state stateStart;
    public void StartEndHandler(state state)
    {
        stateStart = state;
        switch (stateStart)
        {
            case state.Start:
            isStarted = true;
            break;
            case state.End:
            isStarted = false;
            break;
            case state.None:
            isStarted = false;
            break;
            default:
            break;
        }
    }
}
