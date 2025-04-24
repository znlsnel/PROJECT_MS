using UnityEngine;

public class IdlingState : AlivePlayerState
{
    public IdlingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ResetPrevStates();
    }
}
