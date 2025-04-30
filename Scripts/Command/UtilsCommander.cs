using UnityEngine.UIElements;

public class UtilsCommander
{
    private static RouteSwitcher routeSwitch;
    private static PointerSwitcher pointerSwitch;
    private static Pointer32Switcher pointer32Switch;
    public static void ExecuteCommand(int routeIndex, VisualElement parent)
    {
        ICommand switchCommand = new SwitchRouteCommand(routeIndex, parent);
        RouteSwitcher routeSwitcher = new RouteSwitcher();
        routeSwitch = routeSwitcher;
        routeSwitcher.ExecuteCommand(switchCommand);
    }
    public static void ExecutePointerCommand(int index,  VisualElement parent)
    {
        ICommandPointer switchCommand = new SwitchPointerCommand(index, parent);
        PointerSwitcher pointerSwitcher = new PointerSwitcher();
        pointerSwitch = pointerSwitcher;
        pointerSwitcher.ExecutePointerCommand(switchCommand);

    }
    public static void ExecutePointer32Command(int index,  VisualElement parent)
    {
        ICommandPointer32 switchCommand = new SwitchPointer32Command(index, parent);
        Pointer32Switcher pointer32Switcher = new Pointer32Switcher();
        pointer32Switch = pointer32Switcher;
        pointer32Switcher.ExecutePointer32Command(switchCommand);

    }
    public static void UndoCommand()
    {
        if(routeSwitch != null)
        // RouteSwitcher routeSwitcher = new RouteSwitcher();
            routeSwitch.UndoLastCommand();
    }
    public static void UndoPointerCommand()
    {
        if(pointerSwitch != null)
            pointerSwitch.UndoLastPointerCommand();
    }
    public static void UndoPointer32Command()
    {
        if(pointer32Switch != null)
            pointer32Switch.UndoLastPointer32Command();
    }

    public static void Clear()
    {
        if(routeSwitch != null)
            routeSwitch.ClearCommand();
    }
    public static void ClearPointer()
    {
        if(pointerSwitch != null)
            pointerSwitch.ClearCommand();
    }
    public static void ClearPointer32()
    {
        if(pointer32Switch != null)
            pointer32Switch.ClearCommand();
    }
}
