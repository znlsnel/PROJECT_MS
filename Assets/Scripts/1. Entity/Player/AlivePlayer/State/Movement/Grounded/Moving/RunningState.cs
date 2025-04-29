using UnityEngine;
using UnityEngine.InputSystem;

public class RunningState : MovingState
{
    public RunningState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeed = stateMachine.Player.AlivePlayerSO.MoveSpeed;

        StartAnimation(stateMachine.Player.AnimationData.RunningParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.RunningParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return;
        }

        if(stateMachine.ReusableData.ShouldSprint)
        {
            movementStateMachine.ChangeState(movementStateMachine.SprintingState);
            return;
        }
    }

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);

        base.OnMovementCanceled(context);
    }
    #endregion
}
