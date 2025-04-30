using System.Collections.Generic;
using UnityEngine;
public class Pointer32Switcher
{
    private Stack<ICommandPointer32> pointer32Commands = new Stack<ICommandPointer32>();

    public void ExecutePointer32Command(ICommandPointer32 command)
    {
        pointer32Commands.Push(command);
        command.Execute();
    }
    public void UndoLastPointer32Command()
    {
        if(pointer32Commands.Count > 0)
        {
            ICommandPointer32 lastComand = pointer32Commands.Pop();
            lastComand.Undo();
        }
        else
        {
            Debug.Log("Nothing to undo.");
        }
    }
    public void ClearCommand()
    {
        if (pointer32Commands.Count >= 0)
        {
            pointer32Commands.Clear();
        }
        else
        {
            Debug.LogError("Failed to Clear");
        }
    }
}
