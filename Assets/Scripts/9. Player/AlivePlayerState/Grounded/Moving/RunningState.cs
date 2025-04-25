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

        stateMachine.ReusableData.MovementSpeedModifier = stateMachine.Player.AlivePlayerSO.MoveSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    #region Input Methods
    protected override void OnMovementStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdlingState);

        base.OnMovementCanceled(context);
    }
    #endregion
}
