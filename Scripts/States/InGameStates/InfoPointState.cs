

[System.Serializable]
public class InfoPointState
{
    public bool isAtInfoPointState;
    public enum IPState
    {
        IsAtInfo, NotAtInfo
    }
    public IPState stateInfo;
    public void InfoPointStateHandler(IPState state)
    {
        stateInfo = state;
        switch (stateInfo)
        {
            case IPState.IsAtInfo:
            isAtInfoPointState = true;
            break;
            case IPState.NotAtInfo:
            isAtInfoPointState = false;
            break;
            default:
            break;
        }
    }
}
