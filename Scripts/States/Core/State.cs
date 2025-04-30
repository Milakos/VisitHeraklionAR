public abstract class State
{
    protected StateMachine stateMachine;
    public virtual void SetStateMachine(StateMachine machine)
    {
        stateMachine = machine;
    }
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
    
    
}
