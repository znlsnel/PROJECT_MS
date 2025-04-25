using UnityEngine;

public class IdlingState : GroundedState
{
    public IdlingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        base.Enter();

        stateMachine.ResetPrevStates();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
