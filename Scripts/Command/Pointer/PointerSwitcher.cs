using System.Collections.Generic;
using UnityEngine;

public class PointerSwitcher
{
    private Stack<ICommandPointer> pointerCommands = new Stack<ICommandPointer>();

    public void ExecutePointerCommand(ICommandPointer command)
    {
        pointerCommands.Push(command);
        command.Execute();
    }
    public void UndoLastPointerCommand()
    {
        if(pointerCommands.Count > 0)
        {
            ICommandPointer lastComand = pointerCommands.Pop();
            lastComand.Undo();
        }
        else
        {
            Debug.Log("Nothing to undo.");
        }
    }
        public void ClearCommand()
    {
        if (pointerCommands.Count >= 0)
        {
            pointerCommands.Clear();
        }
        else
        {
            Debug.LogError("Failed to Clear");
        }
    }
}
