using System.Collections.Generic;
using UnityEngine;

public class RouteSwitcher
{
    private Stack<ICommand> routeCommands = new Stack<ICommand>();
    // Start is called before the first frame update
    public void ExecuteCommand(ICommand command)
    {
        routeCommands.Push(command);
        command.Execute();      
    }

    public void UndoLastCommand()
    {
        if (routeCommands.Count > 0)
        {
            ICommand lastCommand = routeCommands.Pop();
            lastCommand.Undo();
        }
        else
        {
            Debug.Log("Nothing to undo.");
        }
    }
    public void ClearCommand()
    {
        if (routeCommands.Count >= 0)
        {
            routeCommands.Clear();
        }
        else
        {
            Debug.LogError("Failed to Clear");
        }
    }
}
