using UnityEngine;

public class IdlingState : GroundedState
{
    public IdlingState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeed = 0f;

        base.Enter();

        movementStateMachine.ResetPrevStates();
        
        StartAnimation(stateMachine.Player.AnimationData.IdlingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.IdlingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        OnMove();
    }
}
