using UnityEngine;

public abstract class AlivePlayerState : IState
{
    protected AlivePlayerStateMachine stateMachine;

    public AlivePlayerState(AlivePlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Update()
    {
        
    }
}
