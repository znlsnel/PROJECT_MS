using UnityEngine;
using UnityEngine.InputSystem;

public class AlivePlayerRunningState : AlivePlayerMovingState
{
    public AlivePlayerRunningState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
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

        if(stateMachine.ReusableData.ShouldSprint && stateMachine.Player.Stamina.Current.Value > stateMachine.Player.Stamina.Maximum.Value * 0.1f)
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
